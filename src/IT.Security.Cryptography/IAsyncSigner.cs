using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography;

using Models;

public interface IAsyncSigner
{
    Task<String> SignAsync(String alg, String data, SignFormat format = SignFormat.XadesBES, Boolean detached = true, CancellationToken cancellationToken = default);
}