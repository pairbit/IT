using System;

namespace IT.Security.Cryptography.Models;

public record Signature
{
    public String? Reference { get; set; }

    public SignatureStatus? Status { get; set; }

    public SignatureFaultType? FaultType { get; set; }

    public String? FaultComment { get; set; }

    public Certificate? Certificate { get; set; }

    public DateTime? ValidationDate { get; set; }

    public DateTime? FirstTimeStamp { get; set; }
}