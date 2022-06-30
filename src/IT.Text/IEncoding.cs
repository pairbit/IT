using System;

namespace IT.Text;

public interface IEncoding
{
    int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars);

    string GetString(ReadOnlySpan<byte> bytes);

    int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes);

    byte[] GetBytes(ReadOnlySpan<char> chars);
}