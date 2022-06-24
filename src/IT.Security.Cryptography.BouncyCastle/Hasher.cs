using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.BouncyCastle;

public class Hasher : IHasher
{
    private static readonly String[] _algs = DigestUtilities.Algorithms.Cast<String>().ToArray();

    public IReadOnlyCollection<String> Algs => _algs;

    public Byte[] Hash(String alg, Stream data)
    {
        var bytes = new Byte[data.Length];
        data.Read(bytes, 0, bytes.Length);
        return DigestUtilities.CalculateDigest(alg, bytes);
    }

    public Byte[] Hash(String alg, ReadOnlySpan<Byte> data)
        => DigestUtilities.CalculateDigest(alg, data.ToArray());

    public async Task<Byte[]> HashAsync(String alg, Stream data, CancellationToken cancellationToken = default)
    {
        var bytes = new Byte[data.Length];
        await data.ReadAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
        return DigestUtilities.CalculateDigest(alg, bytes);
    }
}