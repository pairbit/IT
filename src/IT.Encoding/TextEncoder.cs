using System;
using System.Buffers;

namespace IT.Encoding;

public abstract class TextEncoder : Encoder, ITextEncoder
{
    public abstract Int32 GetDecodedLength(ReadOnlySpan<Char> encoded);

    public abstract OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    public abstract OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    public abstract String Encode(ReadOnlySpan<Byte> data);

    public abstract Byte[] Decode(ReadOnlySpan<Char> encoded);
}