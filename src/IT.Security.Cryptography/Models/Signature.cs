using System;

namespace IT.Security.Cryptography.Models;

public record Signature
{
    public Int32 Number { get; set; }

    public String Status { get; set; }

    public String StatusComment { get; set; }

    public String StatusType { get; set; }

    public Certificate Certificate { get; set; }

    public String Alg { get; set; }

    public String Value { get; set; }
}