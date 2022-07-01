using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer;

using Internal;
using Models;
using Options;

public class SigningService : IHasher, ISigner
{
    private static readonly String[] _algs = new[] { "1.2.643.2.2.9", "1.2.643.7.1.1.2.2", "1.2.643.7.1.1.2.3" };

    private readonly ICryptoInformer _cryptoInformer;
    private readonly ILogger? _logger;
    private readonly JinnServerService _service;
    private readonly SigningOptions _options;

    public SigningService(
        Func<SigningOptions> getOptions,
        ICryptoInformer cryptoInformer,
        ILogger<SigningService>? logger = null)
    {
        if (getOptions is null) throw new ArgumentNullException(nameof(getOptions));

        var options = getOptions();

        _cryptoInformer = cryptoInformer;
        _logger = logger;
        _service = new JinnServerService(options.SigningUrl, logger);
        _options = options;
    }

    #region IHasher

    public IReadOnlyCollection<String> Algs => _algs;

    public Byte[] Hash(String alg, Stream data)
    {
        var oid = _cryptoInformer.GetOid(alg) ?? alg;

        var partInBytesValue = _options.PartInBytesValue;

        var parts = data.GetParts(partInBytesValue);

        if (parts > 1)
        {
            _logger?.LogInformation("Parts: {parts}", parts);

            _logger?.LogInformation("Read {part} part", 0);

            var bytesBase64 = data.ReadPartBytes(partInBytesValue, 0).ToBase64();

            var response = _service.GetResponseText(Soap.Request.GetDigest(bytesBase64, oid), Soap.Actions.Digest);

            var state = ParseState(response);

            parts--;

            if (parts > 1)
            {
                for (int part = 1; part < parts; part++)
                {
                    _logger?.LogInformation("Read {part} part", part);

                    bytesBase64 = data.ReadPartBytes(partInBytesValue, part).ToBase64();

                    response = _service.GetResponseText(Soap.Request.GetDigest(bytesBase64, oid, state), Soap.Actions.Digest);

                    state = ParseState(response);
                }
            }

            bytesBase64 = data.ReadPartBytes(partInBytesValue, parts).ToBase64();

            response = _service.GetResponseText(Soap.Request.GetDigest(bytesBase64, oid, state), Soap.Actions.Digest);

            return ParseDigest(response);
        }
        else
        {
            var bytesBase64 = data.ReadBytes().ToBase64();

            var response = _service.GetResponseText(Soap.Request.GetDigest(bytesBase64, oid), Soap.Actions.Digest);

            return ParseDigest(response);
        }
    }

    public Byte[] Hash(String alg, ReadOnlySpan<Byte> data)
    {
        var oid = _cryptoInformer.GetOid(alg) ?? alg;

        //TODO: ReadOnlySpan
        var dataBase64 = data.ToArray().ToBase64();

        var request = Soap.Request.GetDigest(dataBase64, oid);

        var response = _service.GetResponseText(request, Soap.Actions.Digest);

        return ParseDigest(response);
    }

    public async Task<Byte[]> HashAsync(String alg, Stream data, CancellationToken cancellationToken = default)
    {
        var oid = _cryptoInformer.GetOid(alg) ?? alg;

        var partInBytesValue = _options.PartInBytesValue;

        var parts = data.GetParts(partInBytesValue);

        if (parts > 1)
        {
            _logger?.LogInformation("Parts: {parts}", parts);

            _logger?.LogInformation("Read {part} part", 0);

            var bytesBase64 = (await data.ReadPartBytesAsync(partInBytesValue, 0).ConfigureAwait(false)).ToBase64();

            var response = await _service.GetResponseTextAsync(Soap.Request.GetDigest(bytesBase64, oid), Soap.Actions.Digest).ConfigureAwait(false);

            var state = ParseState(response);

            parts--;

            if (parts > 1)
            {
                for (int part = 1; part < parts; part++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    _logger?.LogInformation("Read {part} part", part);

                    bytesBase64 = (await data.ReadPartBytesAsync(partInBytesValue, part).ConfigureAwait(false)).ToBase64();

                    response = await _service.GetResponseTextAsync(Soap.Request.GetDigest(bytesBase64, oid, state), Soap.Actions.Digest).ConfigureAwait(false);

                    state = ParseState(response);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            _logger?.LogInformation("Read {part} part", parts);

            bytesBase64 = (await data.ReadPartBytesAsync(partInBytesValue, parts).ConfigureAwait(false)).ToBase64();

            response = await _service.GetResponseTextAsync(Soap.Request.GetDigest(bytesBase64, oid, state), Soap.Actions.Digest).ConfigureAwait(false);

            return ParseDigest(response);
        }
        else
        {
            var bytesBase64 = (await data.ReadBytesAsync().ConfigureAwait(false)).ToBase64();

            var response = await _service.GetResponseTextAsync(Soap.Request.GetDigest(bytesBase64, oid), Soap.Actions.Digest).ConfigureAwait(false);

            return ParseDigest(response);
        }
    }

    private ReadOnlySpan<Char> ParseDigestResponseType(ReadOnlySpan<Char> response)
    {
        response = _service.ParseEnvelope(response);

        var range = TagFinder.Outer(response, "DigestResponseType".AsSpan(), StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default)) throw new InvalidOperationException($"'DigestResponseType' is null");

        return response[range];
    }

    private Byte[] ParseDigest(String response)
    {
        var span = response.AsSpan();
        span = ParseDigestResponseType(span);

        var range = TagFinder.Inner(span, "Digest".AsSpan(), StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default)) throw new InvalidOperationException("'DigestResponse.Digest' is null");

        var digest = span[range].ToString();

        return Convert.FromBase64String(digest);
    }

    private String ParseState(String response)
    {
        var span = response.AsSpan();

        span = ParseDigestResponseType(span);

        var range = TagFinder.Inner(span, "State".AsSpan(), StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default)) throw new InvalidOperationException("'DigestResponse.State' is null");

        var state = span[range].ToString();

        return state;
    }

    #endregion IHasher

    #region ISigner

    public String Sign(String alg, String data, SignFormat format, Boolean detached)
    {
        if (!detached) data = String.Format(Soap.Request.SignMessage, data).ToBase64();

        var request = String.Format(Soap.Request.Sign, data, format.GetCode(), alg);

        var response = _service.GetResponse(request, Soap.Actions.Sign);

        var signingResponseType = response.SigningResponseType;

        if (signingResponseType is null || signingResponseType.Length == 0)
            throw new InvalidOperationException($"{nameof(Body.SigningResponseType)} is null");

        return signingResponseType;
    }

    public async Task<String> SignAsync(String alg, String data, SignFormat format, Boolean detached, CancellationToken cancellationToken)
    {
        if (!detached) data = String.Format(Soap.Request.SignMessage, data).ToBase64();

        var request = String.Format(Soap.Request.Sign, data, format.GetCode(), alg);

        var response = await _service.GetResponseAsync(request, Soap.Actions.Sign).ConfigureAwait(false);

        var signingResponseType = response.SigningResponseType;

        if (signingResponseType is null || signingResponseType.Length == 0)
            throw new InvalidOperationException($"{nameof(Body.SigningResponseType)} is null");

        return signingResponseType;
    }

    #endregion ISigner
}