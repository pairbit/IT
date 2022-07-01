using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography;

public interface IAsyncSigner
{
    IReadOnlyCollection<String> Algs { get; }

    IReadOnlyCollection<String> Formats { get; }

    Task<String> SignAsync(String alg, String data, String format, Boolean detached, CancellationToken cancellationToken);
}