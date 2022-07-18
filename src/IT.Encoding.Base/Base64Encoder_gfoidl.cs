using System;
using System.Buffers;

namespace IT.Encoding.Base;

public class Base64Encoder_gfoidl : TextEncoder
{
    private gfoidl.Base64.IBase64 _base64;

    public Base64Encoder_gfoidl(Boolean isUrl = false)
    {
        _base64 = isUrl ? gfoidl.Base64.Base64.Url : gfoidl.Base64.Base64.Default;
    }

    #region Encoder

    private const Int32 _MaxDataLength = Int32.MaxValue / 4 * 3; // 1610612733

    public override Int32 MaxDataLength => _MaxDataLength;

    public override Int32 GetMaxEncodedLength(Int32 dataLength) => _base64.GetEncodedLength(dataLength);

    public override Int32 GetMaxDecodedLength(Int32 encodedLength) => _base64.GetMaxDecodedLength(encodedLength);

    public override Int32 GetEncodedLength(ReadOnlySpan<Byte> data) => _base64.GetEncodedLength(data.Length);

    public override Int32 GetDecodedLength(ReadOnlySpan<Byte> encoded) => _base64.GetDecodedLength(encoded);

    public override OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => _base64.Encode(data, encoded, out consumed, out written, isFinal);

    public override OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => _base64.Decode(encoded, data, out consumed, out written, isFinal);

    public override OperationStatus Encode(Span<Byte> buffer, Int32 dataLength, out Int32 written) => throw new NotImplementedException();

    public override OperationStatus Decode(Span<Byte> buffer, out Int32 written) => throw new NotImplementedException();

    #endregion Encoder

    #region TextEncoder

    public override Int32 GetDecodedLength(ReadOnlySpan<Char> encoded) => _base64.GetDecodedLength(encoded);

    public override OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => _base64.Encode(data, encoded, out consumed, out written, isFinal);

    public override OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => _base64.Decode(encoded, data, out consumed, out written, isFinal);

    public override String Encode(ReadOnlySpan<Byte> data) => _base64.Encode(data);

    public override Byte[] Decode(ReadOnlySpan<Char> encoded) => _base64.Decode(encoded);

    #endregion TextEncoder
}