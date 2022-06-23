using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography;

public static class xHashAlgorithm
{
#if NETSTANDARD2_0 || NETSTANDARD2_1

    public static async Task<byte[]> ComputeHashAsync(this HashAlgorithm alg, Stream inputStream, CancellationToken cancellationToken = default)
    {
        if (alg == null) throw new ArgumentNullException(nameof(alg));
        if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));

        var buffer = new byte[4096];
        int bytesRead;

        while ((bytesRead = await inputStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) > 0)
            alg.TransformBlock(buffer, 0, bytesRead, buffer, 0);

        alg.TransformFinalBlock(buffer, 0, bytesRead);

        return alg.Hash;
    }

#endif
}