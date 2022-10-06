using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using System;

namespace IT.Security.Cryptography.BouncyCastle;

public class RawSignatureVerifier : IRawSignatureVerifier
{
    public Boolean Verify(Byte[] rawSignature, Byte[] hash, Byte[] certificate)
    {
        var x509CertificateParser = new X509CertificateParser();
        var x509Certificate = x509CertificateParser.ReadCertificate(certificate);
        var publicKey = x509Certificate.GetPublicKey();

        var dsa = GetDsa(publicKey);
        dsa.Init(false, publicKey);

        var halfSize = rawSignature.Length / 2;
        var r = new BigInteger(1, rawSignature, halfSize, halfSize);
        var s = new BigInteger(1, rawSignature, 0, halfSize);

        return dsa.VerifySignature(hash, r, s);
    }

    private static IDsa GetDsa(AsymmetricKeyParameter key)
    {
        //SignerUtilities.GetSigner

        if (key is ECKeyParameters eckey)
        {
            if (eckey.Parameters is ECGost3410Parameters) return new ECGost3410Signer();
        }

        throw new NotSupportedException();
    }
}