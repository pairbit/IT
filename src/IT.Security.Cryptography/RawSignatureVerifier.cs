//using System;
//using System.Security.Cryptography;

//namespace IT.Security.Cryptography;

//public class RawSignatureVerifier : ISignRawVerifier
//{
//    public Boolean Verify(Byte[] signature, Byte[] hash, Byte[] certificate)
//    {
//        var signatureMethod = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256";

//        var signatureDescription = Crypto.CreateSignatureDescription(signatureMethod)!;

//        var key = (AsymmetricAlgorithm)CryptoConfig.CreateFromName(signatureDescription.KeyAlgorithm!)!;

//        var deformatter = signatureDescription.CreateDeformatter(key);
//        deformatter.SetHashAlgorithm(signatureDescription.DigestAlgorithm);

//        return deformatter.VerifySignature(hash, certificate);
//    }
//}