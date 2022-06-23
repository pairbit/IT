using System;

namespace IT.Security.Cryptography;

using Models;

public interface ISigner : IAsyncSigner
{
    String Sign(String alg, String data, SignFormat format = SignFormat.XadesBES, Boolean detached = true);
}