using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography;

using Models;

public interface IAsyncSignatureVerifier
{
    Task<Boolean> VerifyAsync(String signature, String? detachedData = null, CancellationToken cancellationToken = default);

    Task<Signatures> VerifyDetailAsync(String signature, String? detachedData = null, CancellationToken cancellationToken = default);
}