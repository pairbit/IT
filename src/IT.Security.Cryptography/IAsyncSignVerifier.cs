using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography;

using Models;

public interface IAsyncSignVerifier
{
    Task<Boolean> IsVerifiedAsync(String signature, String? detachedData = null, CancellationToken cancellationToken = default);

    Task<Signatures> VerifyAsync(String signature, String? detachedData = null, CancellationToken cancellationToken = default);
}