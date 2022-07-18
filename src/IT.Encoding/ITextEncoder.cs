using System;
using System.Buffers;

namespace IT.Encoding;

public interface ITextEncoder : IEncoder
{
    Int32 GetDecodedLength(ReadOnlySpan<Char> encoded);

    OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    String Encode(ReadOnlySpan<Byte> data);

    Byte[] Decode(ReadOnlySpan<Char> encoded);
}