namespace System;

internal static class Base58
{
    public static String Encode(ReadOnlySpan<byte> bytes)
    {
        return SimpleBase.Base58.Bitcoin.Encode(bytes);
    }

    public static void Decode(ReadOnlySpan<char> encoded, Span<byte> output)
    {
        SimpleBase.Base58.Bitcoin.TryDecode(encoded, output, out _);
    }
}