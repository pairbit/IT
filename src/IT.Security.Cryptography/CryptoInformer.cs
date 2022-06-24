using System;

namespace IT.Security.Cryptography;

public class CryptoInformer : ICryptoInformer
{
    public String GetOid(String alg) => Hash.GetOid(alg);
}