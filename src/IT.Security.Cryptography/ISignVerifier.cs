using System;

namespace IT.Security.Cryptography;

using Models;

public interface ISignVerifier : IAsyncSignVerifier
{
    Boolean IsVerified(String signature, String? detachedData = null);

    VerifySignatureResult Verify(String signature, String? detachedData = null);
}