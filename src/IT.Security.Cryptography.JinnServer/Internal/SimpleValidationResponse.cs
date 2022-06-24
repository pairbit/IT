using System;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal class SimpleValidationResponse
{
    public String GmtDateTime { get; set; }

    public Byte[] Advanced { get; set; }

    public GlobalStatus GlobalStatus { get; set; }

    public SimpleSignatureInfos SignatureInfos { get; set; }
}