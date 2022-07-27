using System;
using System.Buffers;

namespace IT.Encoding;

public interface ITextDecoder : IDecoder
{
    Int32 GetDecodedLength(ReadOnlySpan<Char> encoded);

    OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Char> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Char> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    Byte[] Decode(ReadOnlySpan<Char> encoded);

    String DecodeToText(ReadOnlySpan<Byte> encoded);

    String DecodeToText(ReadOnlySpan<Char> encoded);
}