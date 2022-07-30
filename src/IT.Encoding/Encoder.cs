using IT.Encoding.Impl;
using System;
using System.Buffers;

namespace IT.Encoding;

public abstract class Encoder : IEncoder
{
    public abstract Int32 MaxDataLength { get; }

    public abstract Int32 GetMaxEncodedLength(Int32 dataLength);

    public virtual Int32 GetEncodedLength(ReadOnlySpan<Byte> data)
    {
        var len = data.Length;

        if (len > MaxDataLength) throw new ArgumentOutOfRangeException(nameof(data), $"Length > {MaxDataLength}");

        return GetMaxEncodedLength(len);
    }

    public abstract OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    public virtual OperationStatus Encode(Span<Byte> buffer, Int32 dataLength, out Int32 written) => throw new NotImplementedException();

    public virtual Byte[] Encode(ReadOnlySpan<Byte> data) => this.EncodeImpl(data);
}