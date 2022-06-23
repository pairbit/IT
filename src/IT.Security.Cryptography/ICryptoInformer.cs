using System;

namespace IT.Security.Cryptography;

public interface ICryptoInformer
{
    String GetOid(String alg);
}