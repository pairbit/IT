using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer
{
    public interface ISigningServiceAsync
    {
        Task<String> SignAsync(String data, String alg, SignFormat format, Boolean detached, CancellationToken cancellationToken = default);

        Task<String> DigestAsync(Stream data, String alg, CancellationToken cancellationToken = default);
    }
}