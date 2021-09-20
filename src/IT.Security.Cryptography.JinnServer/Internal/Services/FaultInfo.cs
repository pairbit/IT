using System;

namespace IT.Security.Cryptography.JinnServer.Internal.Services
{
    internal class FaultInfo
    {
        //FaultInfoType
        public String Type { get; set; }

        public String Comment { get; set; }

        public override String ToString() => $"[{Type}]: {Comment}";
    }
}