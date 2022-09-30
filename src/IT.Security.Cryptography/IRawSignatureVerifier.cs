using System;

namespace IT.Security.Cryptography;

public interface IRawSignatureVerifier
{
    Boolean Verify(Byte[] signature, Byte[] hash, Byte[] certificate);
}