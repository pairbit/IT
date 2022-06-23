using System;
using System.IO;

namespace IT.Security.Cryptography;

public interface IHasher : IAsyncHasher
{
    Byte[] Hash(String alg, Stream data);

    Byte[] Hash(String alg, ReadOnlySpan<Byte> data);
}