using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer
{
    public interface IValidationServiceAsync
    {
        Task<VerifySignatureResult> ValidateAsync(String sign, String alg, String detachedData = null, CancellationToken cancellationToken = default);

        Task<String> EnhanceAsync(String sign, String alg, String detachedData = null, CancellationToken cancellationToken = default);
    }
}