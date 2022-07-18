using System;
using System.Buffers;
using System.Text;

namespace IT.Encoding;

public abstract class TextEncoder : Encoder, ITextEncoder
{
    public virtual Int32 GetDecodedLength(ReadOnlySpan<Char> encoded)
        => GetMaxDecodedLength(encoded.Length);

    public virtual OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        var encodedLength = GetEncodedLength(data);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(encodedLength);

        Span<Byte> encodedBytes = rented;

        try
        {
            var status = Encode(data, encodedBytes, out consumed, out written);

            if (status == OperationStatus.Done || status == OperationStatus.NeedMoreData)
            {
                System.Text.Encoding.ASCII.GetChars(encodedBytes[..consumed], encoded);
            }

            return status;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public virtual OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        var encodedLength = encoded.Length;

        var pool = ArrayPool<byte>.Shared;

        var rented = pool.Rent(encodedLength);

        Span<byte> encodedBytes = rented;

        try
        {
            var count = System.Text.Encoding.ASCII.GetBytes(encoded, encodedBytes);

            if (count != encodedLength) throw new InvalidOperationException();

            return Decode(encodedBytes[..encodedLength], data, out consumed, out written);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public virtual String Encode(ReadOnlySpan<Byte> data)
    {
        var encodedLength = GetEncodedLength(data);

        var pool = ArrayPool<byte>.Shared;

        var rented = pool.Rent(encodedLength);

        Span<byte> encoded = rented;

        try
        {
            var status = Encode(data, encoded, out var consumed, out var written);

            if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

            if (consumed != data.Length) throw new InvalidOperationException();

            if (written != encodedLength) throw new InvalidOperationException();

            return System.Text.Encoding.ASCII.GetString(encoded[..encodedLength]);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public virtual Byte[] Decode(ReadOnlySpan<Char> encoded)
    {
        var decodedLength = GetDecodedLength(encoded);

        var bytes = new byte[decodedLength];

        var data = bytes.AsSpan();

        var status = Decode(encoded, data, out var consumed, out var written);

        if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

        if (consumed != encoded.Length) throw new InvalidOperationException();

        return written != decodedLength ? data[..written].ToArray() : bytes;
    }
}