using IT.Security.Cryptography;
using Org.BouncyCastle.Security;
using System.Text;

namespace IT.Tests.Security.Cryptography.Hasher;

public class BouncyCastleTest
{
    private const String Empty = "Empty";
    private readonly IHasher _hasher;
    private readonly byte[] _data = Encoding.UTF8.GetBytes(Empty);

    public BouncyCastleTest()
    {
        _hasher = new IT.Security.Cryptography.BouncyCastle.Hasher();
    }

    [Test]
    public void Test1()
    {
        var hashes = new Dictionary<String, String>();

        var comparer = StringComparer.OrdinalIgnoreCase;

        foreach (String algo in DigestUtilities.Algorithms)
        {
            var hashBytes = _hasher.Hash(algo, _data);
            var hash = Convert.ToHexString(hashBytes);

            var oid = DigestUtilities.GetObjectIdentifier(algo).ToString();
            var oid2 = Hash.GetOid(algo);

            if (!comparer.Equals(oid, oid2)) throw new InvalidOperationException($"{oid} != {oid2}");

            var hash2 = Hash.GetEmptyByOid(oid);

            if (!comparer.Equals(hash, hash2)) throw new InvalidOperationException($"{hash} != {hash2}");

            hashes.Add(algo, hash);
        }

        foreach (var item in hashes.OrderBy(x => x.Key))
        {
            var oid = DigestUtilities.GetObjectIdentifier(item.Key);
            Console.WriteLine($"{oid,-26} | {item.Key,17} | {item.Value}");
        }
    }
}