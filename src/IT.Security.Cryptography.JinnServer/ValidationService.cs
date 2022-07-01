using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer;

using Internal;
using Models;

public class ValidationService : ISignEnhancer, ISignVerifier
{
    //private static readonly String[] _signAlgs = new[] { "1.2.643.7.1.1.3.2", "1.2.643.7.1.1.3.3", "1.2.643.2.2.3" };
    private static readonly String[] _formats = new[] { 
        "cades-t", "xades-t", "wssec-t",
        "cades-c", "xades-c", "wssec-c",
        "cades-a", "xades-a", "wssec-a"
    };
    private readonly JinnServerService _service;
    private readonly ValidationOptions _options;
    private readonly ILogger? _logger;

    public IReadOnlyCollection<String> Formats => _formats;

    public ValidationService(Func<ValidationOptions> getOptions, ILogger<ValidationService>? logger = null)
    {
        if (getOptions is null) throw new ArgumentNullException(nameof(getOptions));

        var options = getOptions();

        _logger = logger;
        _service = new JinnServerService(options.ValidationUrl, logger);
        _options = options;
    }

    #region ISignVerifier

    public Boolean IsVerified(String signature, String? detachedData)
        => IsVerified(_service.GetResponseText(GetRequestValidation(signature, detachedData), Soap.Actions.Validate));

    public VerifySignatureResult Verify(String signature, String? detachedData)
        => GetVerifySignatureResult(_service.GetResponseText(GetRequestValidation(signature, detachedData), Soap.Actions.Validate));

    public async Task<Boolean> IsVerifiedAsync(String signature, String? detachedData, CancellationToken cancellationToken)
        => IsVerified(await _service.GetResponseTextAsync(GetRequestValidation(signature, detachedData), Soap.Actions.Validate).ConfigureAwait(false));

    public async Task<VerifySignatureResult> VerifyAsync(String signature, String? detachedData, CancellationToken cancellationToken)
        => GetVerifySignatureResult(await _service.GetResponseTextAsync(GetRequestValidation(signature, detachedData), Soap.Actions.Validate).ConfigureAwait(false));

    private static Boolean IsVerified(String response)
    {
        var span = response.AsSpan();

        span = ParseValidationResponseType(span);

        var range = TagFinder.Inner(span, "globalStatus".AsSpan(), StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default)) throw new InvalidOperationException("'ValidationResponseType.globalStatus' not found");

        return span[range].SequenceEqual("valid".AsSpan());
    }

    private VerifySignatureResult GetVerifySignatureResult(String response)
    {
        var result = _service.ParseResponseValidation(response);

        return result.ToVerifySignatureResult();
    }

    #endregion ISignVerifier

    #region ISignEnhancer

    public String Enhance(String signature, String format, String? detachedData)
        => GetEnhanced(_service.GetResponseText(GetRequestEnhance(signature, format, detachedData), Soap.Actions.Validate));

    public async Task<String> EnhanceAsync(String signature, String format, String? detachedData, CancellationToken cancellationToken)
        => GetEnhanced(await _service.GetResponseTextAsync(GetRequestEnhance(signature, format, detachedData), Soap.Actions.Validate).ConfigureAwait(false));

    private static String GetEnhanced(String response)
    {
        var span = response.AsSpan();

        span = ParseValidationResponseType(span);

        var range = TagFinder.Inner(span, "advanced".AsSpan(), StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default)) throw new InvalidOperationException("'ValidationResponseType.advanced' not found");

        return span[range].ToString();
    }

    #endregion ISignEnhancer

    #region Private Methods

    private static ReadOnlySpan<Char> ParseValidationResponseType(ReadOnlySpan<Char> response)
    {
        var range = TagFinder.Outer(response, "ValidationResponseType".AsSpan(), StringComparison.OrdinalIgnoreCase);

        if (range.Equals(default) && JinnServerService.NotFound(response)) throw new InvalidOperationException("'ValidationResponseType' not found");

        return response[range];
    }

    private String GetRequestValidation(String sign, String? detachedData)
    {
        return detachedData is null || detachedData.Length == 0
            ? Soap.Request.Validation(sign.TryToBase64())
            : Soap.Request.Validation(sign.TryToBase64(), detachedData.TryToBase64());
    }

    private String GetRequestEnhance(String sign, String format, String? detachedData)
    {
        return detachedData is null || detachedData.Length == 0
            ? Soap.Request.Enhance(sign.TryToBase64(), format)
            : Soap.Request.Enhance(sign.TryToBase64(), format, detachedData.TryToBase64());
    }

    private void CheckStatus(GlobalStatus status, SimpleSignatureInfos signatureInfos)
    {
        var infos = signatureInfos?.SignatureInfo;

        Exception? inner = null;

        if (infos is null || infos.Length == 0)
        {
            if (status == GlobalStatus.valid) return;
        }
        else
        {
            var inners = new List<Exception>();

            foreach (var info in infos!)
            {
                if (info.FailInfo is null) continue;

                var innerException = new InvalidOperationException(info.ToString());

                innerException.Data.Add(nameof(info.Status), info.Status);

                inners.Add(innerException);
            }

            if (inners.Count > 0)
                inner = inners.Count > 1 ? new AggregateException(inners) : inners.FirstOrDefault();
        }

        if (inner == null && status == GlobalStatus.valid) return;

        var exception = new InvalidOperationException(Res.NoEnhanced + " " + status.Localize(), inner);

        exception.Data.Add(nameof(GlobalStatus), status);

        throw exception;
    }

    private String GetEnhanced(Body body)
    {
        var data = body.ValidationResponseType;

        if (data is null) throw new InvalidOperationException($"{nameof(Body.ValidationResponseType)} is null");

        CheckStatus(data.GlobalStatus, data.SignatureInfos);

        var advanced = data.Advanced;

        if (advanced is null || advanced.Length == 0) throw new InvalidOperationException("Internal Error JinnServer");

        return advanced.ToBase64();
    }

    #endregion Private Methods
}