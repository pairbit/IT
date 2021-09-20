using System;

namespace IT.Security.Cryptography.JinnServer.Internal.Services
{
    internal class SimpleValidationResponse
    {
        public String GmtDateTime { get; set; }

        public Byte[] Advanced { get; set; }

        public GlobalStatus GlobalStatus { get; set; }

        public SimpleSignatureInfos SignatureInfos { get; set; }
    }
}