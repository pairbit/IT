using System;

namespace IT.Security.Cryptography;

using Models;

public interface ISignatureVerifier : IAsyncSignatureVerifier
{
    Boolean Verify(String signature, String? detachedData = null);

    Signatures VerifyDetail(String signature, String? detachedData = null);
}