using System;
using System.Buffers;

namespace IT.Encoding;

public interface ITextEncoder : IEncoder
{
    Int32 GetEncodedLength(ReadOnlySpan<Char> data);

    Int32 GetDecodedLength(ReadOnlySpan<Char> encoded);

    #region ByteToText

    OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    String EncodeToText(ReadOnlySpan<Byte> data);

    Byte[] Decode(ReadOnlySpan<Char> encoded);

    #endregion ByteToText

    #region TextToByte

    OperationStatus Encode(ReadOnlySpan<Char> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Char> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    Byte[] Encode(ReadOnlySpan<Char> data);

    String DecodeToText(ReadOnlySpan<Byte> encoded);

    #endregion TextToByte

    #region TextToText

    OperationStatus Encode(ReadOnlySpan<Char> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Char> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    String EncodeToText(ReadOnlySpan<Char> data);

    String DecodeToText(ReadOnlySpan<Char> encoded);

    #endregion TextToText
}