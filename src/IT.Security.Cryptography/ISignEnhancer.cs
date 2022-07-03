using IT.Security.Cryptography.Models;
using System;

namespace IT.Security.Cryptography;

public interface ISignEnhancer : IAsyncSignEnhancer
{
    String Enhance(String signature, String format, String? detachedData = null);

    EnhancedSignatures EnhanceDetail(String signature, String format, String? detachedData = null);
}