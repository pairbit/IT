using System;

namespace IT.Security.Cryptography.JinnServer;

public record ValidationOptions
{
    /// <example>http://195.230.101.103:8080/tccs/SignatureValidationService</example>
    public String ValidationUrl { get; set; }
}