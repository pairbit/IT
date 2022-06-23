using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography;

public interface IAsyncSignEnhancer
{
    Task<String> EnhanceAsync(String signature, String? detachedData = null, CancellationToken cancellationToken = default);
}