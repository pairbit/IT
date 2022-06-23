using System;
using System.Security.Cryptography;

namespace IT.Security.Cryptography;

public class HashAlgorithmAdapter : HashAlgorithm
{
    private readonly Action _reset;
    private readonly Action<byte[], int, int> _update;
    private readonly Func<byte[]> _digest;

    public HashAlgorithmAdapter(
        int hashSize, Action reset, Action<byte[], int, int> update, Func<byte[]> digest)
    {
        _reset = reset;
        _update = update;
        _digest = digest;
        HashSize = hashSize;
    }

    public override int HashSize { get; }

    public override byte[] Hash => _digest();

    protected override void HashCore(byte[] array, int ibStart, int cbSize) =>
        _update(array, ibStart, cbSize);

    protected override byte[] HashFinal() =>
        _digest();

    public override void Initialize() =>
        _reset();
}