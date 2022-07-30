using System;
using System.Buffers;

namespace IT.Encoding.Impl;

public static class xIDecoder
{
    public static Byte[] DecodeImpl(this IDecoder decoder, ReadOnlySpan<Byte> encoded)
    {
        if (encoded.IsEmpty) return Array.Empty<Byte>();

        var decodedLength = decoder.GetDecodedLength(encoded);

#if NET6_0
        var dataArray = GC.AllocateUninitializedArray<Byte>(decodedLength);
#else
        var dataArray = new Byte[decodedLength];
#endif

        var data = dataArray.AsSpan();

        var status = decoder.Decode(encoded, data, out var consumed, out var written);

        if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

        if (consumed != encoded.Length) throw new InvalidOperationException($"(consumed == {consumed}) != (encoded.Length == {encoded.Length})");

        return written < data.Length ? data[..written].ToArray() : dataArray;
    }
}