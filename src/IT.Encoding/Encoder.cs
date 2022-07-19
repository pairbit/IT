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

    public virtual OperationStatus Encode(Span<Byte> buffer, Int32 dataLength, out Int32 written)
        => throw new NotImplementedException();

    public virtual OperationStatus Decode(Span<Byte> buffer, out Int32 written)
        => throw new NotImplementedException();

    public virtual Byte[] Encode(ReadOnlySpan<Byte> data)
    {
        if (data.IsEmpty) return Array.Empty<Byte>();

        var encodedLength = GetEncodedLength(data);

#if NET6_0
        var encodedArray = GC.AllocateUninitializedArray<Byte>(encodedLength);
#else
        var encodedArray = new Byte[encodedLength];
#endif

        var encoded = encodedArray.AsSpan();

        var status = Encode(data, encoded, out var consumed, out var written);

        if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

        if (consumed != data.Length) throw new InvalidOperationException($"(consumed == {consumed}) != (data.Length == {data.Length})");

        return written < encoded.Length ? encoded[..written].ToArray() : encodedArray;
    }

    public virtual Byte[] Decode(ReadOnlySpan<Byte> encoded)
    {
        if (encoded.IsEmpty) return Array.Empty<Byte>();

        var decodedLength = GetDecodedLength(encoded);

#if NET6_0
        var dataArray = GC.AllocateUninitializedArray<Byte>(decodedLength);
#else
        var dataArray = new Byte[decodedLength];
#endif

        var data = dataArray.AsSpan();

        var status = Decode(encoded, data, out var consumed, out var written);

        if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

        if (consumed != encoded.Length) throw new InvalidOperationException($"(consumed == {consumed}) != (encoded.Length == {encoded.Length})");

        return written < data.Length ? data[..written].ToArray() : dataArray;
    }
}