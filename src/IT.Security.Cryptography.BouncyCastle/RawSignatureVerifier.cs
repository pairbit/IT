using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;

namespace IT.Security.Cryptography.BouncyCastle;

public class RawSignatureVerifier : IRawSignatureVerifier
{
    public Boolean Verify(Byte[] signature, Byte[] hash, Byte[] certificate)
    {
        var x509CertificateParser = new X509CertificateParser();
        var x509Certificate = x509CertificateParser.ReadCertificate(certificate);
        var publicKey = x509Certificate.GetPublicKey();

        var dsa = GetDsa(publicKey, out var size);
        dsa.Init(false, publicKey);

        var r = new BigInteger(1, signature, size, size);
        var s = new BigInteger(1, signature, 0, size);

        return dsa.VerifySignature(hash, r, s);
    }

    private static IDsa GetDsa(AsymmetricKeyParameter key, out Int32 size)
    {
        //SignerUtilities.GetSigner

        if (key is ECKeyParameters eckey)
        {
            if (eckey.Parameters is ECGost3410Parameters)
            {
                size = 32;
                return new ECGost3410Signer();
            }
        }

        throw new NotSupportedException();
    }
}