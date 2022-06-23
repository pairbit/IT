using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.Impl;

public class Hasher : IHasher
{
    public virtual Byte[] Hash(String algName, Stream data)
    {
        using var alg = Cryptography.Hash.Create(algName);
        return alg.ComputeHash(data);
    }

    public virtual Byte[] Hash(String algName, ReadOnlySpan<Byte> data)
    {
        if (algName.Equals("XXH32", StringComparison.OrdinalIgnoreCase)) return BitConverter.GetBytes(XXH32.DigestOf(data));
        if (algName.Equals("XXH64", StringComparison.OrdinalIgnoreCase)) return BitConverter.GetBytes(XXH64.DigestOf(data));

        using var alg = Cryptography.Hash.Create(algName);
        
        //TODO: ReadOnlySpan<Byte>
        return alg.ComputeHash(data.ToArray());
    }

    public virtual async Task<Byte[]> HashAsync(String algName, Stream data, CancellationToken cancellationToken = default)
    {
        using var alg = Cryptography.Hash.Create(algName);
        return await alg.ComputeHashAsync(data, cancellationToken).ConfigureAwait(false);
    }

    //public virtual async Task<Byte[]> HashAsync(String algName, ReadOnlyMemory<Byte> data, CancellationToken cancellationToken = default)
    //{
    //    using var alg = Cryptography.Hash.Create(algName);
    //    return await alg.ComputeHashAsync(data, cancellationToken).ConfigureAwait(false);
    //}
}