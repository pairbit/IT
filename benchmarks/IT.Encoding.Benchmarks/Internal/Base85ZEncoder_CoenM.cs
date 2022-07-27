using CoenM.Encoding;
using System;
using System.Buffers;

namespace IT.Encoding.Base;

public class Base85ZEncoder_CoenM : TextEncoding
{
    #region Encoder

    private const Int32 _MaxDataLength = (Int32.MaxValue / 5 * 4) + 1; // 1 717 986 917

    public override Int32 MaxDataLength => _MaxDataLength;

    public override Int32 GetMaxEncodedLength(Int32 dataLength) => Z85.GetEncodedSize(dataLength);

    public override Int32 GetMaxDecodedLength(Int32 encodedLength) => Z85.GetDecodedSize(encodedLength);

    #endregion Encoder

    #region TextEncoder

    public override OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => Z85.Encode(data, encoded, out consumed, out written, isFinal);

    public override OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => Z85.Decode(encoded, data, out consumed, out written, isFinal);

    #endregion TextEncoder
}