using System;
using System.Collections.Generic;

namespace IT.Security.Cryptography.Models;

public record Certificate
{
    public String? Version { get; set; }

    public String? SerialNumber { get; set; }

    public String? SignatureAlg { get; set; }

    public CertificateIssuer? Issuer { get; set; }

    public DateTime? ValidityFrom { get; set; }

    public DateTime? ValidityTo { get; set; }

    public CertificateSubject? Subject { get; set; }

    public String? SubjectPublicKey { get; set; }

    public String? SubjectPublicKeyAlgorithm { get; set; }

    public List<CertificateExtension>? Extensions { get; set; }

    public String? AlgorithmIdentifier { get; set; }

    public String? Signature { get; set; }
}