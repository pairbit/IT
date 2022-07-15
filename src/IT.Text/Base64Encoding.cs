using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text;

namespace IT.Text;

public class Base64Encoding : IEncoding
{
    public Int32 GetByteCount(ReadOnlySpan<Char> chars)
        => Base64.GetMaxDecodedFromUtf8Length(chars.Length);

    public Int32 GetBytes(ReadOnlySpan<Char> chars, Span<Byte> bytes)
    {
        var utf8len = chars.Length;

        var len = Base64.GetMaxDecodedFromUtf8Length(utf8len);

        var pool = ArrayPool<byte>.Shared;

        var rented = pool.Rent(len);

        Span<byte> utf8Span = rented;

        try
        {
            var count = Encoding.ASCII.GetBytes(chars, utf8Span);

            if (count != utf8len) throw new InvalidOperationException();

            var status = Base64.DecodeFromUtf8(utf8Span, bytes, out var consumed, out var written);

            if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

            if (consumed != len) throw new InvalidOperationException();

            if (written != utf8len) throw new InvalidOperationException();

            return count;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public Int32 GetCharCount(ReadOnlySpan<Byte> bytes)
        => Base64.GetMaxEncodedToUtf8Length(bytes.Length);

    public Int32 GetChars(ReadOnlySpan<Byte> bytes, Span<Char> chars)
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

    public String GetString(ReadOnlySpan<Byte> bytes)
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
}