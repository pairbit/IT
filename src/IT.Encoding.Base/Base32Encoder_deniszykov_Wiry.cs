using System;
using System.Text;

namespace IT.Encoding.Base;

public class Base32Encoder_deniszykov_Wiry : TextEncoder
{
    private readonly deniszykov.BaseN.BaseNEncoding _baseNEncoding;
    private readonly Wiry.Base32.Base32Encoding _base;

    public Base32Encoder_deniszykov_Wiry(Boolean isZ32 = false)
    {
        _baseNEncoding = isZ32 ? deniszykov.BaseN.BaseNEncoding.ZBase32 : deniszykov.BaseN.BaseNEncoding.Base32;
        _base = isZ32 ? Wiry.Base32.Base32Encoding.ZBase32 : Wiry.Base32.Base32Encoding.Standard;
    }

    #region Encoder

    private const Int32 _MaxDataLength = Int32.MaxValue / 8 * 5; // 1 342 177 275

    public override Int32 MaxDataLength => _MaxDataLength;

    public override Int32 GetMaxEncodedLength(Int32 dataLength) => _baseNEncoding.GetMaxCharCount(dataLength);

    public override Int32 GetMaxDecodedLength(Int32 encodedLength) => _baseNEncoding.GetMaxByteCount(encodedLength);

    #endregion Encoder

    public override String EncodeToText(ReadOnlySpan<Byte> data) => _baseNEncoding.GetString(data);

    public override Byte[] Decode(ReadOnlySpan<Char> encoded) => _base.ToBytes(encoded);
}