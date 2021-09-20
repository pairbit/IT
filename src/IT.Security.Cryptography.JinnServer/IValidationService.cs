using System;

namespace IT.Security.Cryptography.JinnServer
{
    public interface IValidationService : IValidationServiceAsync
    {
        VerifySignatureResult Validate(String sign, String alg, String detachedData = null);

        String Enhance(String sign, String alg, String detachedData = null);
    }
}