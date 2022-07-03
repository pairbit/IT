using System;

namespace IT.Security.Cryptography.Models;

public record EnhancedSignatures
{
    public String? Enhanced { get; set; }

    public Signatures? Signatures { get; set; }
}