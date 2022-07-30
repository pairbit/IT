using System;
using System.Buffers;
using System.Text;

namespace IT.Encoding.Impl;

public static class xITextDecoder
{
    public static OperationStatus DecodeImpl(this ITextDecoder textDecoder, ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal, System.Text.Encoding encoding)
    {
        var encodedLength = encoded.Length;

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(encodedLength);

        Span<Byte> encodedBytes = rented;

        try
        {
            var count = encoding.GetBytes(encoded, encodedBytes);

            if (count != encodedLength) throw new InvalidOperationException();

            return textDecoder.Decode(encodedBytes[..encodedLength], data, out consumed, out written);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public static Byte[] DecodeImpl(this ITextDecoder textDecoder, ReadOnlySpan<Char> encoded)
    {
        if (encoded.IsEmpty) return Array.Empty<Byte>();

        var decodedLength = textDecoder.GetDecodedLength(encoded);

#if NET6_0
        var dataArray = GC.AllocateUninitializedArray<Byte>(decodedLength);
#else
        var dataArray = new Byte[decodedLength];
#endif

        var data = dataArray.AsSpan();

        var status = textDecoder.Decode(encoded, data, out var consumed, out var written);

        if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

        if (consumed != encoded.Length) throw new InvalidOperationException($"(consumed == {consumed}) != (encoded.Length == {encoded.Length})");

        return written < data.Length ? data[..written].ToArray() : dataArray;
    }
}