using System;
using System.Buffers;

namespace IT.Encoding;

public abstract class Encoder : IEncoder
{
    public abstract Int32 MaxDataLength { get; }

    public abstract Int32 GetMaxEncodedLength(Int32 dataLength);

    public abstract Int32 GetMaxDecodedLength(Int32 encodedLength);

    public virtual Int32 GetEncodedLength(ReadOnlySpan<Byte> data)
    {
        var len = data.Length;

        if (len > MaxDataLength) throw new ArgumentOutOfRangeException(nameof(data), $"Length > {MaxDataLength}");

        return GetMaxEncodedLength(len);
    }

    public virtual Int32 GetDecodedLength(ReadOnlySpan<Byte> encoded)
    {
        return GetMaxDecodedLength(encoded.Length);
    }

    public abstract OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    public abstract OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    public abstract OperationStatus Encode(Span<Byte> buffer, Int32 dataLength, out Int32 written);

    public abstract OperationStatus Decode(Span<Byte> buffer, out Int32 written);
}