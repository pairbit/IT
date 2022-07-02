using System;

namespace IT.Security.Cryptography.Models;

public record CertificateExtension
{
    public String? Oid { get; set; }

    public Boolean? IsCritical { get; set; }

    public String? Value { get; set; }
}