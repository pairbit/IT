using System;

namespace IT.Security.Cryptography;

public interface ISignEnhancer : IAsyncSignEnhancer
{
    String Enhance(String signature, String? detachedData = null);
}