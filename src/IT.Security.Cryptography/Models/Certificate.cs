using System;

namespace IT.Security.Cryptography.Models;

public record Certificate
{
    public String SignatureAlg { get; set; }

    public String SerialNumber { get; set; }

    public DateTime ValidityDateFrom { get; set; }

    public DateTime ValidityDateTo { get; set; }

    public Owner Owner { get; set; }

    public Issuer Issuer { get; set; }
}