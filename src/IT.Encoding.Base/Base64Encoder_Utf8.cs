using System;
using System.Buffers;
using System.Buffers.Text;

namespace IT.Encoding.Base;

public class Base64Encoder_Utf8 : TextEncoder
{
    #region Encoder

    private const Int32 _MaxDataLength = Int32.MaxValue / 4 * 3; // 1610612733

    public override Int32 MaxDataLength => _MaxDataLength;

    public override Int32 GetMaxEncodedLength(Int32 dataLength) => Base64.GetMaxEncodedToUtf8Length(dataLength);

    public override Int32 GetMaxDecodedLength(Int32 encodedLength) => Base64.GetMaxDecodedFromUtf8Length(encodedLength);

    public override OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => Base64.EncodeToUtf8(data, encoded, out consumed, out written, isFinal);

    public override OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => Base64.DecodeFromUtf8(encoded, data, out consumed, out written, isFinal);

    public override OperationStatus Encode(Span<Byte> buffer, Int32 dataLength, out Int32 written)
        => Base64.EncodeToUtf8InPlace(buffer, dataLength, out written);

    public override OperationStatus Decode(Span<Byte> buffer, out Int32 written)
        => Base64.DecodeFromUtf8InPlace(buffer, out written);

    #endregion Encoder

    public override String EncodeToText(ReadOnlySpan<Byte> data) => EncodeToTextFromBytes(data);
}