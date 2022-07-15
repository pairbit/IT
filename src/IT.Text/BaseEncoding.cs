using System;
using System.Text;

namespace IT.Text;

public class BaseEncoding : IEncoding
{
    private Encoding _encoding;

    public BaseEncoding(Encoding encoding)
    {
        _encoding = encoding;
    }

    public Int32 GetByteCount(ReadOnlySpan<Char> chars)
        => _encoding.GetByteCount(chars);

    public Int32 GetBytes(ReadOnlySpan<Char> chars, Span<Byte> bytes)
         => _encoding.GetBytes(chars, bytes);

    public Int32 GetCharCount(ReadOnlySpan<Byte> bytes)
        => _encoding.GetCharCount(bytes);

    public Int32 GetChars(ReadOnlySpan<Byte> bytes, Span<Char> chars)
        => _encoding.GetChars(bytes, chars);

    public String GetString(ReadOnlySpan<Byte> bytes)
        => _encoding.GetString(bytes);
}