using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text;

namespace IT.Text.Encoders;

public class Base64Utf8Encoder : IEncoder
{
    public int GetMaxEncodedLength(ReadOnlySpan<byte> bytes)
        => Base64.GetMaxEncodedToUtf8Length(bytes.Length);

    public int Encode(ReadOnlySpan<byte> bytes, Span<char> chars)
    {
        var len = bytes.Length;

        var utf8len = Base64.GetMaxEncodedToUtf8Length(len);

        var pool = ArrayPool<byte>.Shared;

        var rented = pool.Rent(utf8len);

        Span<byte> utf8 = rented;

        try
        {
            var status = Base64.EncodeToUtf8(bytes, utf8, out var consumed, out var written);

            if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

            if (consumed != len) throw new InvalidOperationException();

            if (written != utf8len) throw new InvalidOperationException();

            var count = Encoding.ASCII.GetChars(utf8[..utf8len], chars);

            if (count != utf8len) throw new InvalidOperationException();

            return count;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public string Encode(ReadOnlySpan<byte> bytes)
    {
        var len = bytes.Length;

        var utf8len = Base64.GetMaxEncodedToUtf8Length(len);

        var pool = ArrayPool<byte>.Shared;

        var rented = pool.Rent(utf8len);

        Span<byte> utf8 = rented;

        try
        {
            var status = Base64.EncodeToUtf8(bytes, utf8, out var consumed, out var written);

            if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

            if (consumed != len) throw new InvalidOperationException();

            if (written != utf8len) throw new InvalidOperationException();

            return Encoding.ASCII.GetString(utf8[..utf8len]);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public int GetMaxDecodedLength(ReadOnlySpan<char> chars)
        => Base64.GetMaxDecodedFromUtf8Length(chars.Length);

    public int Decode(ReadOnlySpan<char> chars, Span<byte> bytes)
    {
        var utf8len = chars.Length;

        var pool = ArrayPool<byte>.Shared;

        var rented = pool.Rent(utf8len);

        Span<byte> utf8Span = rented;

        try
        {
            var count = Encoding.ASCII.GetBytes(chars, utf8Span);

            if (count != utf8len) throw new InvalidOperationException();

            var status = Base64.DecodeFromUtf8(utf8Span[..utf8len], bytes, out var consumed, out var written);

            if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

            if (consumed != utf8len) throw new InvalidOperationException();

            bytes = bytes[..written];

            return written;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public byte[] Decode(ReadOnlySpan<char> chars)
    {
        var bytes = new byte[Base64.GetMaxDecodedFromUtf8Length(chars.Length)];
        Decode(chars, bytes);
        return bytes;
    }
}