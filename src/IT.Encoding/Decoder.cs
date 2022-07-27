using IT.Encoding.Internal;
using System;
using System.Buffers;

namespace IT.Encoding;

public abstract class Decoder : IDecoder
{
    public abstract Int32 GetMaxDecodedLength(Int32 encodedLength);

    public virtual Int32 GetDecodedLength(ReadOnlySpan<Byte> encoded)
    {
        return GetMaxDecodedLength(encoded.Length);
    }

    public abstract OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true);

    public virtual OperationStatus Decode(Span<Byte> buffer, out Int32 written)
        => throw new NotImplementedException();

    public virtual Byte[] Decode(ReadOnlySpan<Byte> encoded) => this.DecodeImpl(encoded);
}