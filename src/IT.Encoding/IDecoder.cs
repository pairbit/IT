using System;
using System.Buffers;

namespace IT.Encoding;

public interface IDecoder
{
    Int32 GetMaxDecodedLength(Int32 encodedLength);

    Int32 GetDecodedLength(ReadOnlySpan<Byte> encoded);

    OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Decode(Span<Byte> buffer, out Int32 written);

    Byte[] Decode(ReadOnlySpan<Byte> encoded);
}