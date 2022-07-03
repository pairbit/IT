using System;

namespace IT.Security.Cryptography;

using Models;

public interface ISignVerifier : IAsyncSignVerifier
{
    Boolean Verify(String signature, String? detachedData = null);

    Signatures VerifyDetail(String signature, String? detachedData = null);
}