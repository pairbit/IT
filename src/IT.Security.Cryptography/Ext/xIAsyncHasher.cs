using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography;

//public static class xIAsyncHasher
//{
//    public static Task<Byte[]> HashAsync(this IAsyncHasher hasher, String alg, Byte[] buffer, CancellationToken cancellationToken = default)
//    {
//        using var stream = new MemoryStream(buffer);
//        return hasher.HashAsync(alg, stream, cancellationToken);
//    }
//}