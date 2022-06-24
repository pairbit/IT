using IT.Security.Cryptography;
using System.Text;

namespace IT.Tests.Security.Cryptography.Hashing;

public abstract class HashTest
{
    private const String Empty = "Empty";
    private static readonly byte[] _data = Encoding.UTF8.GetBytes(Empty);

    private readonly IHasher _hasher;
    private readonly ICryptoInformer _cryptoInformer;

    public HashTest(IHasher hasher, ICryptoInformer? cryptoInformer = null)
    {
        _hasher = hasher;
        _cryptoInformer = cryptoInformer ?? new CryptoInformer();
    }

    [Test]
    public void TestHash()
    {
        var hashes = new Dictionary<String, String>();

        foreach (String alg in _hasher.Algs)
        {
            var hashBytes = _hasher.Hash(alg, _data);
            var hash = Convert.ToHexString(hashBytes);
            hashes.Add(alg, hash);
        }

        var comparer = StringComparer.OrdinalIgnoreCase;

        foreach (var item in hashes.OrderBy(x => x.Key))
        {
            var alg = item.Key;
            var hash = item.Value;

            var oid = _cryptoInformer.GetOid(alg) ?? alg;

            var hash2 = Hash.GetEmptyByOid(oid);

            if (!comparer.Equals(hash, hash2)) 
                throw new InvalidOperationException($"{hash} != {hash2} ({alg} | {oid})");

            Console.WriteLine($"{oid,-26} | {alg,20} | {hash}");
        }
    }
}