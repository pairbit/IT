using System;

namespace IT.Encoding;

public interface ITextEncoder : IEncoder
{
    String Encode(ReadOnlySpan<Byte> data);

    Byte[] Decode(ReadOnlySpan<Char> encoded);
}