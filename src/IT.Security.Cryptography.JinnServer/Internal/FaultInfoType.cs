namespace IT.Security.Cryptography.JinnServer.Internal;

internal enum FaultInfoType
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
    invalidRequestDataFormat,
}