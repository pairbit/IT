using System;

namespace IT.Security.Cryptography.Models;

public record VerifySignatureResult
{
    public Boolean? IsVerify { get; set; }

    public String ResultStatus { get; set; }

    public Signature[] Signatures { get; set; }

    public Int32 Count { get; set; }

    public String Error { get; set; }

    public String ValidationStatus { get; set; }
}