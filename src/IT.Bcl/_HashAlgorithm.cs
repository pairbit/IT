#if NETSTANDARD2_0 || NETSTANDARD2_1

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography
{
    public static class _HashAlgorithm
    {
        public static async Task<byte[]> ComputeHashAsync(this HashAlgorithm alg, Stream stream, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (alg == null) throw new ArgumentNullException(nameof(alg));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) > 0)
                alg.TransformBlock(buffer, 0, bytesRead, buffer, 0);

            alg.TransformFinalBlock(buffer, 0, bytesRead);

            return alg.Hash;
        }
    }
}

#endif