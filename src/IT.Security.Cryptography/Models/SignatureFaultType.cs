namespace IT.Security.Cryptography.Models;

public enum SignatureFaultType
{
    unknownDigestAlgorithm,
    unknownSignatureAlgorithm,
    signerCertificateNotFound,
    signerCertificateIssuerNotFound,
    signerCertificateSignatureInvalid,
    signerCertificateCRLNotFound,
    signerCertificateExpired,
    signerCertificateRevoked,
    invalidDigestValue,
    invalidSignatureValue,
    invalidSignatureTimeStamp,
}