using System;

namespace IT.Text;

public interface IEncoding
{
    Int32 GetByteCount(ReadOnlySpan<Char> chars);

    Int32 GetBytes(ReadOnlySpan<Char> chars, Span<Byte> bytes);

    Int32 GetCharCount(ReadOnlySpan<Byte> bytes);

    Int32 GetChars(ReadOnlySpan<Byte> bytes, Span<Char> chars);

    String GetString(ReadOnlySpan<Byte> bytes);
}