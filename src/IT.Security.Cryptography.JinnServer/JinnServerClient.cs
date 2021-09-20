using System;

namespace IT.Security.Cryptography.JinnServer
{
    using Internal;

    public class JinnServerClient
    {
        public ISigningService SigningService { get; }

        public IValidationService ValidationService { get; }

        public JinnServerClient(String signingServiceUrl, String validationServiceUrl)
        {
            SigningService = GetSigningService(signingServiceUrl);
            ValidationService = GetValidationService(validationServiceUrl);
        }

        public static ISigningService GetSigningService(String url) => new SigningService { Url = url };

        public static IValidationService GetValidationService(String url) => new ValidationService { Url = url };
    }
}