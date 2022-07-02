using System;

namespace IT.Security.Cryptography.Models;

public record CertificateIssuer
{
    /// <summary>
    /// 1.2.643.3.131.1.1
    /// </summary>
    public String? INN { get; set; }

    /// <summary>
    /// 1.2.643.100.1
    /// </summary>
    public String? OGRN { get; set; }

    /// <summary>
    /// 1.2.840.113549.1.9.1
    /// </summary>
    public String? Email { get; set; }

    /// <summary>
    /// 1.2.840.113549.1.9.2
    /// </summary>
    public String? UnstructuredName { get; set; }

    /// <summary>
    /// 2.5.4.3
    /// </summary>
    public String? CommonName { get; set; }

    /// <summary>
    /// 2.5.4.6
    /// </summary>
    public String? Country { get; set; }

    /// <summary>
    /// 2.5.4.7
    /// </summary>
    public String? City { get; set; }

    /// <summary>
    /// 2.5.4.8
    /// </summary>
    public String? Region { get; set; }

    /// <summary>
    /// 2.5.4.9
    /// </summary>
    public String? Street { get; set; }

    /// <summary>
    /// 2.5.4.10
    /// </summary>
    public String? Organization { get; set; }

    /// <summary>
    /// 2.5.4.11
    /// </summary>
    public String? OrganizationalUnit { get; set; }
}