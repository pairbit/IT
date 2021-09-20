using IT.Ext;
using IT.Resources;
using IT.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer.Internal
{
    using Services;

    internal class ValidationService : JinnServerService, IValidationService
    {
        #region IValidationService

        public VerifySignatureResult Validate(String sign, String alg, String detachedData = null)
        {
            return GetResponseValidation(GetRequest(sign, alg, detachedData, false), Soap.Actions.Validate);
        }

        public String Enhance(String sign, String alg, String detachedData = null)
        {
            return GetEnhanced(GetResponse(GetRequest(sign, alg, detachedData, true), Soap.Actions.Validate));
        }

        #endregion IValidationService

        #region IValidationServiceAsync

        public Task<VerifySignatureResult> ValidateAsync(String sign, String alg, String detachedData = null, CancellationToken cancellationToken = default)
        {
            return GetResponseValidationAsync(GetRequest(sign, alg, detachedData, false), Soap.Actions.Validate);
        }

        public async Task<String> EnhanceAsync(String sign, String alg, String detachedData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetResponseAsync(GetRequest(sign, alg, detachedData, true), Soap.Actions.Validate).CA();
            return GetEnhanced(response);
        }

        #endregion IValidationServiceAsync

        #region Private Methods

        private String GetRequest(String sign, String alg, String detachedData, Boolean enhance)
        {
            Arg.NotNull(sign, nameof(sign));

            sign = sign.TryToBase64();

            var enhanceLower = enhance.ToString().ToLower();

            if (alg == null)
            {
                return detachedData.IsEmpty()
                    ? Soap.Request.ValidationWithoutAlg.Format(sign, enhanceLower)
                    : Soap.Request.ValidationWithDetachedWithoutAlg.Format(sign, detachedData.TryToBase64(), enhanceLower);
            }

            return detachedData.IsEmpty()
                ? Soap.Request.Validation.Format(sign, enhanceLower, alg)
                : Soap.Request.ValidationWithDetached.Format(sign, detachedData.TryToBase64(), enhanceLower, alg);
        }

        private void CheckStatus(GlobalStatus status, SimpleSignatureInfos signatureInfos)
        {
            if (status == GlobalStatus.valid) return;
            var innerExceptions = new List<Exception>();
            if (signatureInfos != null)
            {
                foreach (var signatureInfo in signatureInfos.SignatureInfo)
                {
                    if (signatureInfo.Status == SignatureStatus.valid) continue;
                    var innerException = new InvalidOperationException(signatureInfo.ToString());
                    innerException.Data.Add(nameof(signatureInfo.Status), signatureInfo.Status);
                    innerExceptions.Add(innerException);
                }
            }
            var msg = Res.Get("NoEnhanced") + " " + Res.Get(status);
            var inner = innerExceptions.Count > 1 ? new AggregateException(innerExceptions) : innerExceptions.FirstOrDefault();
            var ex = new InvalidOperationException(msg, inner);
            ex.Data.Add(nameof(GlobalStatus), status);
            throw ex;
        }

        private String GetEnhanced(Body body)
        {
            var data = body.ValidationResponseType;

            CheckStatus(data.GlobalStatus, data.SignatureInfos);

            return data.Advanced.ToBase64();
        }

        #endregion Private Methods
    }
}