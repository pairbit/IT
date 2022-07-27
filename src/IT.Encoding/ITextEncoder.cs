using System;
using System.Buffers;

namespace IT.Encoding;

public interface ITextEncoder : IEncoder
{
    Int32 GetEncodedLength(ReadOnlySpan<Char> data);

    OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Encode(ReadOnlySpan<Char> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Encode(ReadOnlySpan<Char> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    Byte[] Encode(ReadOnlySpan<Char> data);

    String EncodeToText(ReadOnlySpan<Byte> data);

    String EncodeToText(ReadOnlySpan<Char> data);
}