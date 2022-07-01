using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer;

using Internal;
using Options;

public class SigningService : IHasher, ISigner
{
    //private static readonly String[] _signAlgs = new[] { "1.2.643.7.1.1.3.2", "1.2.643.7.1.1.3.3", "1.2.643.2.2.3" };
    private static readonly String[] _hashAlgs = new[] { "1.2.643.7.1.1.2.2", "1.2.643.7.1.1.2.3", "1.2.643.2.2.9" };
    private static readonly String[] _signAlgs = new[] { "1.2.643.7.1.1.1.1", "1.2.643.7.1.1.1.2", "1.2.643.2.2.19" };
    private static readonly String[] _formats = new[] {
        "cms", "xmldsig", "wssecurity", 
        //должна быть сформирована под-пись в формате CMS-SignedData,
        //усиленная в необходимом объеме (допол-ненная атрибутами) в соответствии с ETSI TS 101 733 (CAdES)
        "cades-bes", "cades-c", "cades-t", "cades-a",
        //должна быть сформирована подпись в формате XMLDsig,
        //усиленная в необходимом объеме (дополненная атрибутами) в соответствии с ETSI TS 101 903 (XadES)
        "xades-bes", "xades-c", "xades-t", "xades-a",
        //должна быть сформирована подпись в формате WS-Security,
        //усиленная в необходимом объеме (дополненная атрибутами) в соответствии с ETSI TS 101 903 (XadES)
        "wssec-bes", "wssec-c", "wssec-t", "wssec-a"
    };

    private readonly ICryptoInformer _cryptoInformer;
    private readonly ILogger? _logger;
    private readonly JinnServerService _service;
    private readonly SigningOptions _options;

    public IReadOnlyCollection<String> Formats => _formats;

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

    IReadOnlyCollection<String> IAsyncHasher.Algs => _hashAlgs;

    public Byte[] Hash(String alg, Stream data)
    {
        var oid = _cryptoInformer.GetOid(alg) ?? alg;

        var partInBytesValue = _options.PartInBytesValue;

        var parts = data.GetParts(partInBytesValue);

        if (parts > 1)
        {
            var logger = _logger;
            var canInfo = logger is not null && logger.IsEnabled(LogLevel.Information);

            if (canInfo)
            {
                logger!.LogInformation("Parts: {parts}", parts);

                logger!.LogInformation("Read {part} part", 0);
            }
            var bytesBase64 = data.ReadPartBytes(partInBytesValue, 0).ToBase64();

            var response = _service.GetResponseText(Soap.Request.Digest(bytesBase64, oid), Soap.Actions.Digest);

            var state = ParseState(response);

            parts--;

            if (parts > 1)
            {
                for (int part = 1; part < parts; part++)
                {
                    if (canInfo) logger!.LogInformation("Read {part} part", part);

                    bytesBase64 = data.ReadPartBytes(partInBytesValue, part).ToBase64();

                    response = _service.GetResponseText(Soap.Request.Digest(bytesBase64, oid, state), Soap.Actions.Digest);

                    state = ParseState(response);
                }
            }

            if (canInfo) logger!.LogInformation("Read {part} part", parts);

            bytesBase64 = data.ReadPartBytes(partInBytesValue, parts).ToBase64();

            response = _service.GetResponseText(Soap.Request.Digest(bytesBase64, oid, state), Soap.Actions.Digest);

            return ParseDigest(response);
        }
        else
        {
            var bytesBase64 = data.ReadBytes().ToBase64();

            var response = _service.GetResponseText(Soap.Request.Digest(bytesBase64, oid), Soap.Actions.Digest);

            return ParseDigest(response);
        }
    }

    public Byte[] Hash(String alg, ReadOnlySpan<Byte> data)
    {
        var oid = _cryptoInformer.GetOid(alg) ?? alg;

        //TODO: ReadOnlySpan
        var dataBase64 = data.ToArray().ToBase64();

        var request = Soap.Request.Digest(dataBase64, oid);

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
            var logger = _logger;
            var canInfo = logger is not null && logger.IsEnabled(LogLevel.Information);

            if (canInfo)
            {
                logger!.LogInformation("Parts: {parts}", parts);

                logger!.LogInformation("Read {part} part", 0);
            }
            var bytesBase64 = (await data.ReadPartBytesAsync(partInBytesValue, 0, cancellationToken: cancellationToken).ConfigureAwait(false)).ToBase64();

            var response = await _service.GetResponseTextAsync(Soap.Request.Digest(bytesBase64, oid), Soap.Actions.Digest).ConfigureAwait(false);

            var state = ParseState(response);

            parts--;

            if (parts > 1)
            {
                for (int part = 1; part < parts; part++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (canInfo) logger!.LogInformation("Read {part} part", part);

                    bytesBase64 = (await data.ReadPartBytesAsync(partInBytesValue, part, cancellationToken: cancellationToken).ConfigureAwait(false)).ToBase64();

                    response = await _service.GetResponseTextAsync(Soap.Request.Digest(bytesBase64, oid, state), Soap.Actions.Digest).ConfigureAwait(false);

                    state = ParseState(response);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (canInfo) logger!.LogInformation("Read {part} part", parts);

            bytesBase64 = (await data.ReadPartBytesAsync(partInBytesValue, parts, cancellationToken: cancellationToken).ConfigureAwait(false)).ToBase64();

            response = await _service.GetResponseTextAsync(Soap.Request.Digest(bytesBase64, oid, state), Soap.Actions.Digest).ConfigureAwait(false);

            return ParseDigest(response);
        }
        else
        {
            var bytesBase64 = (await data.ReadBytesAsync(cancellationToken: cancellationToken).ConfigureAwait(false)).ToBase64();

            var response = await _service.GetResponseTextAsync(Soap.Request.Digest(bytesBase64, oid), Soap.Actions.Digest).ConfigureAwait(false);

            return ParseDigest(response);
        }
    }

    private ReadOnlySpan<Char> ParseDigestResponseType(ReadOnlySpan<Char> response)
    {
        var range = TagFinder.Outer(response, "DigestResponseType".AsSpan(), StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default) && JinnServerService.NotFound(response)) throw new InvalidOperationException("'DigestResponseType' not found");

        return response[range];
    }

    private Byte[] ParseDigest(String response)
    {
        var span = response.AsSpan();

        span = ParseDigestResponseType(span);

        var range = TagFinder.Inner(span, "Digest".AsSpan(), StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default) && JinnServerService.NotFound(span)) throw new InvalidOperationException("'DigestResponseType.Digest' not found");

        var digest = span[range].ToString();

        return Convert.FromBase64String(digest);
    }

    private String ParseState(String response)
    {
        var span = response.AsSpan();

        span = ParseDigestResponseType(span);

        var range = TagFinder.Inner(span, "State".AsSpan(), StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default) && JinnServerService.NotFound(span)) throw new InvalidOperationException("'DigestResponseType.State' not found");

        var state = span[range].ToString();

        return state;
    }

    #endregion IHasher

    #region ISigner

    IReadOnlyCollection<String> IAsyncSigner.Algs => _signAlgs;

    public String Sign(String alg, String data, String format, Boolean detached)
    {
        //if (!detached) data = String.Format(Soap.Request.SignMessage, data).ToBase64();

        var request = detached ? Soap.Request.SigningDetached(alg, data.TryToBase64(), format) 
                               : Soap.Request.Signing(alg, data.TryToBase64(), format);

        var response = _service.GetResponseText(request, Soap.Actions.Sign);

        return ParseSigningResponseType(response);
    }

    public async Task<String> SignAsync(String alg, String data, String format, Boolean detached, CancellationToken cancellationToken)
    {
        //if (!detached) data = String.Format(Soap.Request.SignMessage, data).ToBase64();

        //actor = http://smev.gosuslugi.ru/actors/smev

        var request = detached ? Soap.Request.SigningDetached(alg, data.TryToBase64(), format)
                               : Soap.Request.Signing(alg, data.TryToBase64(), format);

        var response = await _service.GetResponseTextAsync(request, Soap.Actions.Sign).ConfigureAwait(false);

        return ParseSigningResponseType(response);
    }

    private String ParseSigningResponseType(String response)
    {
        var span = response.AsSpan();

        var range = TagFinder.Inner(span, "SigningResponseType".AsSpan(), StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default) && JinnServerService.NotFound(span)) throw new InvalidOperationException("'SigningResponseType' not found");

        return response[range].ToString();
    }

    #endregion ISigner
}