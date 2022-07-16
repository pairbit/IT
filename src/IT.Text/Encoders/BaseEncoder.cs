using System;
using System.Text;

namespace IT.Text.Encoders;

public class BaseEncoder : IEncoder
{
    private Encoding _encoding;

    public BaseEncoder(Encoding encoding)
    {
        _encoding = encoding;
    }

    public int GetMaxEncodedLength(ReadOnlySpan<byte> bytes)
        => _encoding.GetCharCount(bytes);

    public int Encode(ReadOnlySpan<byte> bytes, Span<char> chars)
        => _encoding.GetChars(bytes, chars);

    public string Encode(ReadOnlySpan<byte> bytes)
        => _encoding.GetString(bytes);

    public int GetMaxDecodedLength(ReadOnlySpan<char> chars)
    => _encoding.GetByteCount(chars);

    public int Decode(ReadOnlySpan<char> chars, Span<byte> bytes)
         => _encoding.GetBytes(chars, bytes);

    public byte[] Decode(ReadOnlySpan<char> chars)
    {
        var bytes = new byte[_encoding.GetByteCount(chars)];
        _encoding.GetBytes(chars, bytes);
        return bytes;
    }
}