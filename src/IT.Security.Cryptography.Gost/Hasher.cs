using GostCryptography.Asn1.Gost.Gost_R3410_2012_256;
using GostCryptography.Asn1.Gost.Gost_R3410_2012_512;
using GostCryptography.Asn1.Gost.Gost_R3410_94;
using GostCryptography.Config;
using GostCryptography.Gost_R3411;

namespace IT.Security.Cryptography.Gost;

public class Hasher : Cryptography.Hasher
{
    static Hasher()
    {
        GostCryptoConfig.Initialize();

        Cryptography.Hash.Add<Gost_R3411_94_HashAlgorithm>(Gost_R3410_94_Constants.HashAlgorithm.Value, Cryptography.Hash.Name.GOST_R3411_94);
        Cryptography.Hash.Add<Gost_R3411_2012_256_HashAlgorithm>(Gost_R3410_2012_256_Constants.HashAlgorithm.Value, Cryptography.Hash.Name.GOST_R3411_2012_256);
        Cryptography.Hash.Add<Gost_R3411_2012_512_HashAlgorithm>(Gost_R3410_2012_512_Constants.HashAlgorithm.Value, Cryptography.Hash.Name.GOST_R3411_2012_512);
    }

    public Hasher()
    {
        _algs.Add(Cryptography.Hash.Name.GOST_R3411_94);
        _algs.Add(Cryptography.Hash.Name.GOST_R3411_2012_256);
        _algs.Add(Cryptography.Hash.Name.GOST_R3411_2012_512);
    }
}