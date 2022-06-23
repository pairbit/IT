using System;

namespace IT.Security.Cryptography.Models;

public record Issuer
{
    public String Name { get; set; }

    public String Organization { get; set; }

    public String Location { get; set; }

    public String CertificateNumber { get; set; }

    public String OGRN { get; set; }

    public String INN { get; set; }

    public String Email { get; set; }
}