using System;

namespace IT.Security.Cryptography;

public interface ISigner : IAsyncSigner
{
    String Sign(String alg, String data, String format, Boolean detached);
}