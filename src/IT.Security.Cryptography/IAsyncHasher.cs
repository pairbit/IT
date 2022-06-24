using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography;

public interface IAsyncHasher
{
    IReadOnlyCollection<String> Algs { get; }

    Task<Byte[]> HashAsync(String alg, Stream data, CancellationToken cancellationToken = default);

    //Task<Byte[]> HashAsync(String alg, ReadOnlyMemory<Byte> data, CancellationToken cancellationToken = default);
}