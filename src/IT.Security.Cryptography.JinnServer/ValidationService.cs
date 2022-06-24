using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer;

using Internal;
using Models;
using Options;

public class ValidationService : ISignEnhancer, ISignVerifier
{
    private readonly JinnServerService _service;
    private readonly ValidationOptions _options;
    private readonly ILogger _logger;

    public ValidationService(Func<ValidationOptions> getOptions, ILogger<ValidationService> logger)
    {
        if (getOptions is null) throw new ArgumentNullException(nameof(getOptions));

        var options = getOptions();

        _logger = logger;
        _service = new JinnServerService(options.ValidationUrl, logger);
        _options = options;
    }

    #region ISignVerifier

    public Boolean IsVerified(String signature, String? detachedData)
    {
        String? alg = null;//TODO: Зачем?
        return IsVerified(_service.GetResponseValidation(GetRequest(signature, alg, detachedData, false), Soap.Actions.Validate));
    }

    public VerifySignatureResult Verify(String signature, String? detachedData)
    {
        String? alg = null;//TODO: Зачем?
        return GetVerifySignatureResult(_service.GetResponseValidation(GetRequest(signature, alg, detachedData, false), Soap.Actions.Validate));
    }

    public async Task<Boolean> IsVerifiedAsync(String signature, String? detachedData, CancellationToken cancellationToken)
    {
        String? alg = null;//TODO: Зачем?
        return IsVerified(await _service.GetResponseValidationAsync(GetRequest(signature, alg, detachedData, false), Soap.Actions.Validate).ConfigureAwait(false));
    }

    public async Task<VerifySignatureResult> VerifyAsync(String signature, String? detachedData, CancellationToken cancellationToken)
    {
        String? alg = null;//TODO: Зачем?
        var response = await _service.GetResponseValidationAsync(GetRequest(signature, alg, detachedData, false), Soap.Actions.Validate).ConfigureAwait(false);
        return GetVerifySignatureResult(response);
    }

    #endregion ISignVerifier

    #region ISignEnhancer

    public String Enhance(String signature, String? detachedData)
    {
        String? alg = null;//TODO: Зачем?
        return GetEnhanced(_service.GetResponse(GetRequest(signature, alg, detachedData, true), Soap.Actions.Validate));
    }

    public async Task<String> EnhanceAsync(String signature, String? detachedData, CancellationToken cancellationToken)
    {
        String? alg = null;//TODO: Зачем?
        var response = await _service.GetResponseAsync(GetRequest(signature, alg, detachedData, true), Soap.Actions.Validate).ConfigureAwait(false);
        return GetEnhanced(response);
    }

    #endregion ISignEnhancer

    #region Private Methods

    private String GetRequest(String sign, String? alg, String? detachedData, Boolean enhance)
    {
        if (sign is null) throw new ArgumentNullException(nameof(sign));

        sign = sign.TryToBase64();

        var enhanceLower = enhance.ToString().ToLower();

        if (alg is null)
        {
            return detachedData is null || detachedData.Length == 0
                ? String.Format(Soap.Request.ValidationWithoutAlg, sign, enhanceLower)
                : String.Format(Soap.Request.ValidationWithDetachedWithoutAlg, sign, detachedData!.TryToBase64(), enhanceLower);
        }

        return detachedData is null || detachedData.Length == 0
            ? String.Format(Soap.Request.Validation, sign, enhanceLower, alg)
            : String.Format(Soap.Request.ValidationWithDetached, sign, detachedData!.TryToBase64(), enhanceLower, alg);
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

    private Boolean IsVerified(ValidationResponseType response)
    {
        //CheckStatus(response.GlobalStatus, response.SignatureInfos);

        return response.GlobalStatus == GlobalStatus.valid;
    }

    private VerifySignatureResult GetVerifySignatureResult(ValidationResponseType response)
    {
        //CheckStatus(response.GlobalStatus, response.SignatureInfos);

        return response.ToVerifySignatureResult();
    }

    #endregion Private Methods
}