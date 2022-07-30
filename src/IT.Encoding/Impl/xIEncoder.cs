using System;
using System.Buffers;

namespace IT.Encoding.Impl;

public static class xIEncoder
{
    public static Byte[] EncodeImpl(this IEncoder encoder, ReadOnlySpan<Byte> data)
    {
        if (data.IsEmpty) return Array.Empty<Byte>();

        var encodedLength = encoder.GetEncodedLength(data);

#if NET6_0
        var encodedArray = GC.AllocateUninitializedArray<Byte>(encodedLength);
#else
        var encodedArray = new Byte[encodedLength];
#endif

        var encoded = encodedArray.AsSpan();

        var status = encoder.Encode(data, encoded, out var consumed, out var written);

        if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

        if (consumed != data.Length) throw new InvalidOperationException($"(consumed == {consumed}) != (data.Length == {data.Length})");

        return written < encoded.Length ? encoded[..written].ToArray() : encodedArray;
    }
}