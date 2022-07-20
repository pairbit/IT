using System;
using System.Buffers;
using K4os.Text.BaseX;

namespace IT.Encoding.Base;

public class Base85Encoder_K4os : TextEncoder
{
    private readonly BaseXCodec _codec;

    public Base85Encoder_K4os(Boolean isZ85 = false)
    {
        _codec = isZ85 ? Base85.Z85 : Base85.Default;
    }

    #region Encoder

    private const Int32 _MaxDataLength = (Int32.MaxValue / 5 * 4) + 1; // 1 717 986 917

    public override Int32 MaxDataLength => _MaxDataLength;

    public override Int32 GetMaxEncodedLength(Int32 dataLength) => _codec.MaximumEncodedLength(dataLength);

    public override Int32 GetMaxDecodedLength(Int32 encodedLength) => _codec.MaximumDecodedLength(encodedLength);

    public override Int32 GetEncodedLength(ReadOnlySpan<Byte> data) => _codec.EncodedLength(data);

    #endregion Encoder

    #region TextEncoder

    public override Int32 GetDecodedLength(ReadOnlySpan<Char> encoded) => _codec.DecodedLength(encoded);

    public override OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        written = _codec.Encode(data, encoded);
        consumed = data.Length;
        return OperationStatus.Done;
    }

    public override OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        written = _codec.Decode(encoded, data);
        consumed = encoded.Length;
        return OperationStatus.Done;
    }

    public override String EncodeToText(ReadOnlySpan<Byte> data) => EncodeToTextFromCharsVarLen(data);

    #endregion TextEncoder
}