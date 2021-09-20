using System;
using System.IO;

namespace IT.Security.Cryptography.JinnServer
{
    public interface ISigningService : ISigningServiceAsync
    {
        String Sign(String data, String alg, SignFormat format, Boolean detached);

        String Digest(Stream data, String alg);
    }
}