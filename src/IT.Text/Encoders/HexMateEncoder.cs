using System;

namespace IT.Text.Encoders;

public class HexMateEncoder : IEncoder
{
    private readonly HexMate.HexFormattingOptions _options;

    public HexMateEncoder(Boolean isUpper = true)
    {
        _options = isUpper ? HexMate.HexFormattingOptions.None : HexMate.HexFormattingOptions.Lowercase;
    }

    public int GetMaxEncodedLength(ReadOnlySpan<byte> bytes)
        => bytes.Length * 2;

    public int Encode(ReadOnlySpan<byte> bytes, Span<char> chars)
        => HexMate.Convert.TryToHexChars(bytes, chars, out var written, _options) ? written : 0;

    public string Encode(ReadOnlySpan<byte> bytes)
        => HexMate.Convert.ToHexString(bytes, _options);

    public int GetMaxDecodedLength(ReadOnlySpan<char> chars)
    {
        var len = chars.Length;
        if ((len & 0x01) != 0) throw new ArgumentException("Even number of digits expected");
        return len / 2;
    }

    public int Decode(ReadOnlySpan<char> chars, Span<byte> bytes)
        => HexMate.Convert.TryFromHexChars(chars, bytes, out var written) ? written : 0;

    public byte[] Decode(ReadOnlySpan<char> chars)
    {
        var bytes = new byte[GetMaxDecodedLength(chars)];
        Decode(chars, bytes);
        return bytes;
    }
}