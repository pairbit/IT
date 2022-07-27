using System;
using Wiry.Base32;

namespace IT.Encoding.Base;

public class Base32Encoder_Wiry : TextEncoding
{
    private readonly Base32Encoding _base;

    public Base32Encoder_Wiry(Boolean isZ32 = false)
    {
        _base = isZ32 ? Base32Encoding.ZBase32 : Base32Encoding.Standard;
    }

    #region Encoder

    private const Int32 _MaxDataLength = Int32.MaxValue / 8 * 5; // 1 342 177 275

    public override Int32 MaxDataLength => _MaxDataLength;

    public override Int32 GetMaxEncodedLength(Int32 dataLength) => Base32Encoding.GetSymbolsCount(dataLength);

    public override Int32 GetMaxDecodedLength(Int32 encodedLength) => Base32Encoding.GetBytesCount(encodedLength);

    #endregion Encoder

    public override String EncodeToText(ReadOnlySpan<Byte> data) => _base.GetString(data);

    public override Byte[] Decode(ReadOnlySpan<Char> encoded) => _base.ToBytes(encoded);
}