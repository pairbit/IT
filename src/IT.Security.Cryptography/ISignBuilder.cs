using System;
using System.Text;

namespace IT.Security.Cryptography;

public interface ISignBuilder
{
    void ReplaceSignatureMethod(StringBuilder mask, String signatureMethod);

    void ReplaceDigestMethod(StringBuilder mask, String digestMethod);

    void RemoveTransformEnveloped(StringBuilder mask);

    void PrepareViaHash(StringBuilder mask, String certificate);

    void PrepareViaData(StringBuilder mask);

    void ReplaceId(StringBuilder mask, String id);

    void ReplaceTransform(StringBuilder mask, String transform);

    String Build(ReadOnlySpan<Char> mask, String data);

    Byte[] Hash(out String mask, IHasher hasher);

    void ReplaceSignatureValue(String mask, ReadOnlySpan<Byte> signatureValue);

    void Detach(String sign, out String data, out String signDetached);
}