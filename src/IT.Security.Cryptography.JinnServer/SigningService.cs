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

        string? digest = null;

        var partInBytesValue = _options.PartInBytesValue;

        var parts = data.GetParts(partInBytesValue);

        var moreOne = parts > 1 && _logger is not null;

        if (moreOne) _logger!.LogInformation("Parts: {parts}", parts);

        string? state = null;

        for (int part = 0; part < parts; part++)
        {
            if (moreOne) _logger!.LogInformation("Read {part} part", part);

            var bytes = data.ReadPartBytes(partInBytesValue, part);

            var dataBase64 = bytes.ToBase64();

            var request = state is null ? Soap.Request.GetDigest(dataBase64, oid) : Soap.Request.GetDigest(dataBase64, oid, state);

            var response = _service.GetResponse(request, Soap.Actions.Digest);

            var digestResponse = response?.DigestResponseType;

            if (digestResponse is null) throw new InvalidOperationException($"'{nameof(Body.DigestResponseType)}' is null");

            digest = digestResponse.Digest;

            state = digestResponse.State;
        }

        if (digest is null) throw new InvalidOperationException($"'{nameof(DigestResponse.Digest)}' is null");

        return Convert.FromBase64String(digest);
    }

    public Byte[] Hash(String alg, ReadOnlySpan<Byte> data)
    {
        var oid = _cryptoInformer.GetOid(alg) ?? alg;

        //TODO: ReadOnlySpan
        var dataBase64 = data.ToArray().ToBase64();

        var request = Soap.Request.GetDigest(dataBase64, oid);

        var response = _service.GetResponse(request, Soap.Actions.Digest);

        var digestResponse = response?.DigestResponseType;

        if (digestResponse is null) throw new InvalidOperationException($"'{nameof(Body.DigestResponseType)}' is null");

        var digest = digestResponse.Digest;

        if (digest is null) throw new InvalidOperationException($"'{nameof(DigestResponse.Digest)}' is null");

        return Convert.FromBase64String(digest);
    }

    public async Task<Byte[]> HashAsync(String alg, Stream data, CancellationToken cancellationToken = default)
    {
        var oid = _cryptoInformer.GetOid(alg) ?? alg;

        string? digest = null;

        var partInBytesValue = _options.PartInBytesValue;

        var parts = data.GetParts(partInBytesValue);

        var moreOne = parts > 1 && _logger is not null;

        if (moreOne) _logger!.LogInformation("Parts: {parts}", parts);

        string? state = null;

        for (int part = 0; part < parts; part++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (moreOne) _logger!.LogInformation("Read {part} part", part);

            var bytes = await data.ReadPartBytesAsync(partInBytesValue, part).ConfigureAwait(false);

            var dataBase64 = bytes.ToBase64();

            var request = state is null ? Soap.Request.GetDigest(dataBase64, oid) : Soap.Request.GetDigest(dataBase64, oid, state);

            cancellationToken.ThrowIfCancellationRequested();

            var response = await _service.GetResponseAsync(request, Soap.Actions.Digest).ConfigureAwait(false);

            var digestResponse = response.DigestResponseType;

            if (digestResponse is null) throw new InvalidOperationException($"{nameof(response.DigestResponseType)} is null");

            digest = digestResponse.Digest;

            state = digestResponse.State;
        }

        if (digest is null) throw new InvalidOperationException($"'{nameof(DigestResponse.Digest)}' is null");

        return Convert.FromBase64String(digest);
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