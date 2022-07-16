using System;

namespace IT.Text;

public interface IDecoder
{
    Int32 GetCharCount(ReadOnlySpan<Byte> bytes);

    Int32 GetChars(ReadOnlySpan<Byte> bytes, Span<Char> chars);

    String GetString(ReadOnlySpan<Byte> bytes);
}