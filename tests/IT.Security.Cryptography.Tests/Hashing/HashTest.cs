using System.Text;

namespace IT.Security.Cryptography.Tests.Hashing;

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
    public async Task StreamTest()
    {
        //using var stream = File.OpenRead(@"S:\Videos\grip_legend\Vano_RT.mp4");//1.6MB - 3parts
        using var stream = File.OpenRead(@"S:\Videos\grip_legend\2parts");//1MB - 2part
        //using var stream = File.OpenRead(@"S:\Videos\grip_legend\schwartz.mp4");//534KB - 1part

        var hashes = new Dictionary<String, String>();

        foreach (String alg in _hasher.Algs)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var hashBytes = _hasher.Hash(alg, stream);
            var hash = Convert.ToHexString(hashBytes);
            hashes.Add(alg, hash);
        }

        foreach (String alg in _hasher.Algs)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var hashBytes = await _hasher.HashAsync(alg, stream).ConfigureAwait(false);
            var hash = Convert.ToHexString(hashBytes);

            if (!hashes[alg].Equals(hash))
                throw new InvalidOperationException();
        }

        var comparer = StringComparer.OrdinalIgnoreCase;

        foreach (var item in hashes.OrderBy(x => x.Key))
        {
            var alg = item.Key;
            var hash = item.Value;

            var oid = _cryptoInformer.GetOid(alg) ?? alg;

            Console.WriteLine($"{oid,-26} | {alg,20} | {hash}");
        }
    }

    [Test]
    public void BytesTest()
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