using System;

namespace IT.Security.Cryptography.JinnServer.Internal.Services
{
    internal class SimpleSignatureInfo
    {
        public SignatureStatus Status { get; set; }

        public FaultInfo FailInfo { get; set; }

        public String ValidationDate { get; set; }

        public override String ToString() => $"[{Status}]{FailInfo}";
    }
}