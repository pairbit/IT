using IT.Security.Cryptography.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography;

public interface IAsyncSignEnhancer
{
    IReadOnlyCollection<String> Formats { get; }

    Task<String> EnhanceAsync(String signature, String format, String? detachedData = null, CancellationToken cancellationToken = default);

    Task<EnhancedSignatures> EnhanceDetailAsync(String signature, String format, String? detachedData = null, CancellationToken cancellationToken = default);
}