using System;

namespace IT.Security.Cryptography;

public interface IRawSignatureVerifier
{
    Boolean Verify(Byte[] rawSignature, Byte[] hash, Byte[] certificate);
}