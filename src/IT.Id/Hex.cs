using System.Runtime.InteropServices;

namespace System;

internal static class Hex
{
    //https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
    private static readonly uint[] _lowerLookup32Unsafe = CreateLookup32Unsafe("x2");
    private static readonly uint[] _upperLookup32Unsafe = CreateLookup32Unsafe("X2");

    internal static readonly unsafe uint* _lowerLookup32UnsafeP = (uint*)GCHandle.Alloc(_lowerLookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();
    internal static readonly unsafe uint* _upperLookup32UnsafeP = (uint*)GCHandle.Alloc(_upperLookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();

    private static uint[] CreateLookup32Unsafe(string format)
    {
        var result = new uint[256];
        for (int i = 0; i < 256; i++)
        {
            string s = i.ToString(format);
            if (BitConverter.IsLittleEndian)
                result[i] = s[0] + ((uint)s[1] << 16);
            else
                result[i] = s[1] + ((uint)s[0] << 16);
        }
        return result;
    }

    public static byte[] ParseHexString(string hex)
    {
        if (hex == null) throw new ArgumentNullException(nameof(hex));

        return TryParseHexString(hex, out byte[]? bytes) ? bytes! : throw new FormatException("String should contain only hexadecimal digits.");
    }

    public static bool TryParseHexString(string hex, out byte[]? bytes)
    {
        bytes = null;

        if (hex == null) return false;

        var buffer = new byte[(hex.Length + 1) / 2];

        var i = 0;
        var j = 0;

        if (hex.Length % 2 == 1)
        {
            // if s has an odd length assume an implied leading "0"
            int y;
            if (!TryParseHexChar(hex[i++], out y))
            {
                return false;
            }
            buffer[j++] = (byte)y;
        }

        while (i < hex.Length)
        {
            int x, y;
            if (!TryParseHexChar(hex[i++], out x))
            {
                return false;
            }
            if (!TryParseHexChar(hex[i++], out y))
            {
                return false;
            }
            buffer[j++] = (byte)(x << 4 | y);
        }

        bytes = buffer;
        return true;
    }

    private static bool TryParseHexChar(char c, out int value)
    {
        if (c >= '0' && c <= '9')
        {
            value = c - '0';
            return true;
        }

        if (c >= 'a' && c <= 'f')
        {
            value = 10 + (c - 'a');
            return true;
        }

        if (c >= 'A' && c <= 'F')
        {
            value = 10 + (c - 'A');
            return true;
        }

        value = 0;
        return false;
    }
}