using System;
using System.Buffers;

namespace IT.Encoding;

public interface IEncoder
{
    Int32 MaxDataLength { get; }

    Int32 GetMaxEncodedLength(Int32 dataLength);

    Int32 GetMaxDecodedLength(Int32 encodedLength);

    Int32 GetEncodedLength(ReadOnlySpan<Byte> data);

    Int32 GetDecodedLength(ReadOnlySpan<Byte> encoded);

    OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    OperationStatus Encode(Span<Byte> buffer, Int32 dataLength, out Int32 written);

    OperationStatus Decode(Span<Byte> buffer, out Int32 written);
}