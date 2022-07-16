using System;

namespace IT.Text;

public interface IEncoder
{
    Int32 GetByteCount(ReadOnlySpan<Char> chars);

    Int32 GetBytes(ReadOnlySpan<Char> chars, Span<Byte> bytes);
}