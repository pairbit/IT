using System;

namespace IT.Text;

public interface IEncoder
{
    Int32 GetMaxEncodedLength(ReadOnlySpan<Byte> bytes);

    Int32 Encode(ReadOnlySpan<Byte> bytes, Span<Char> chars);

    String Encode(ReadOnlySpan<Byte> bytes);

    Int32 GetMaxDecodedLength(ReadOnlySpan<Char> chars);

    Int32 Decode(ReadOnlySpan<Char> chars, Span<Byte> bytes);

    Byte[] Decode(ReadOnlySpan<Char> chars);
}