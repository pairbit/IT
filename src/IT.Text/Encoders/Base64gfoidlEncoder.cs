using System;
using System.Buffers.Text;

namespace IT.Text.Encoders;

public class Base64gfoidlEncoder : IEncoder
{
    private gfoidl.Base64.IBase64 _base64;

    public Base64gfoidlEncoder(Boolean isUrl = false)
    {
        _base64 = isUrl ? gfoidl.Base64.Base64.Url : gfoidl.Base64.Base64.Default;
    }

    public int GetMaxEncodedLength(ReadOnlySpan<byte> bytes)
        => Base64.GetMaxEncodedToUtf8Length(bytes.Length);

    public int Encode(ReadOnlySpan<byte> bytes, Span<char> chars)
    {
        _base64.Encode(bytes, chars, out _, out var written);
        return written;
    }

    public string Encode(ReadOnlySpan<byte> bytes)
        => _base64.Encode(bytes);

    public int GetMaxDecodedLength(ReadOnlySpan<char> chars)
        => Base64.GetMaxDecodedFromUtf8Length(chars.Length);

    public int Decode(ReadOnlySpan<char> chars, Span<byte> bytes)
    {
        _base64.Decode(chars, bytes, out _, out var written);
        return written;
    }

    public byte[] Decode(ReadOnlySpan<char> chars)
        => _base64.Decode(chars);
}