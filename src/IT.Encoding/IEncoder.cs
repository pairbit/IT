using System;
using System.Buffers;

namespace IT.Encoding;

public interface IEncoder
{
    Int32 MaxDataLength { get; }

    Int32 GetMaxEncodedLength(Int32 dataLength);

    Int32 GetEncodedLength(ReadOnlySpan<Byte> data);

    OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Encode(Span<Byte> buffer, Int32 dataLength, out Int32 written);

    Byte[] Encode(ReadOnlySpan<Byte> data);
}