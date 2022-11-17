using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System;

internal static class Hex
{
    //https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
    private static readonly uint[] _lowerLookup32Unsafe = CreateLookup32Unsafe("x2");
    private static readonly uint[] _upperLookup32Unsafe = CreateLookup32Unsafe("X2");

    private static readonly ushort[] _lowerLookup16Unsafe = CreateLookup16Unsafe("x2");
    private static readonly ushort[] _upperLookup16Unsafe = CreateLookup16Unsafe("X2");

    internal static readonly unsafe uint* _lowerLookup32UnsafeP = (uint*)GCHandle.Alloc(_lowerLookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();
    internal static readonly unsafe uint* _upperLookup32UnsafeP = (uint*)GCHandle.Alloc(_upperLookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();

    internal static readonly unsafe ushort* _lowerLookup16UnsafeP = (ushort*)GCHandle.Alloc(_lowerLookup16Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();
    internal static readonly unsafe ushort* _upperLookup16UnsafeP = (ushort*)GCHandle.Alloc(_upperLookup16Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();

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

    private static ushort[] CreateLookup16Unsafe(string format)
    {
        var result = new ushort[256];
        for (int i = 0; i < 256; i++)
        {
            string s = i.ToString(format);
            if (BitConverter.IsLittleEndian)
                result[i] = (ushort)(s[0] + (s[1] << 8));
            else
                result[i] = (ushort)(s[1] + (s[0] << 8));
        }
        return result;
    }

    public static void Decode(ReadOnlySpan<char> chars, Span<byte> bytes)
    {
        bytes[0] = (byte)((CharToHex[chars[0]] << 4) | CharToHex[chars[1]]);
        bytes[1] = (byte)((CharToHex[chars[2]] << 4) | CharToHex[chars[3]]);
        bytes[2] = (byte)((CharToHex[chars[4]] << 4) | CharToHex[chars[5]]);
        bytes[3] = (byte)((CharToHex[chars[6]] << 4) | CharToHex[chars[7]]);
        bytes[4] = (byte)((CharToHex[chars[8]] << 4) | CharToHex[chars[9]]);
        bytes[5] = (byte)((CharToHex[chars[10]] << 4) | CharToHex[chars[11]]);
        bytes[6] = (byte)((CharToHex[chars[12]] << 4) | CharToHex[chars[13]]);
        bytes[7] = (byte)((CharToHex[chars[14]] << 4) | CharToHex[chars[15]]);
        bytes[8] = (byte)((CharToHex[chars[16]] << 4) | CharToHex[chars[17]]);
        bytes[9] = (byte)((CharToHex[chars[18]] << 4) | CharToHex[chars[19]]);
        bytes[10] = (byte)((CharToHex[chars[20]] << 4) | CharToHex[chars[21]]);
        bytes[11] = (byte)((CharToHex[chars[22]] << 4) | CharToHex[chars[23]]);
    }

    public static void Decode(ReadOnlySpan<byte> chars, Span<byte> bytes)
    {
        bytes[0] = (byte)((CharToHex[chars[0]] << 4) | CharToHex[chars[1]]);
        bytes[1] = (byte)((CharToHex[chars[2]] << 4) | CharToHex[chars[3]]);
        bytes[2] = (byte)((CharToHex[chars[4]] << 4) | CharToHex[chars[5]]);
        bytes[3] = (byte)((CharToHex[chars[6]] << 4) | CharToHex[chars[7]]);
        bytes[4] = (byte)((CharToHex[chars[8]] << 4) | CharToHex[chars[9]]);
        bytes[5] = (byte)((CharToHex[chars[10]] << 4) | CharToHex[chars[11]]);
        bytes[6] = (byte)((CharToHex[chars[12]] << 4) | CharToHex[chars[13]]);
        bytes[7] = (byte)((CharToHex[chars[14]] << 4) | CharToHex[chars[15]]);
        bytes[8] = (byte)((CharToHex[chars[16]] << 4) | CharToHex[chars[17]]);
        bytes[9] = (byte)((CharToHex[chars[18]] << 4) | CharToHex[chars[19]]);
        bytes[10] = (byte)((CharToHex[chars[20]] << 4) | CharToHex[chars[21]]);
        bytes[11] = (byte)((CharToHex[chars[22]] << 4) | CharToHex[chars[23]]);
    }

    public static bool TryDecodeFromUtf16(ReadOnlySpan<char> chars, Span<byte> bytes)
    {
        var byteHi = FromChar(chars[0]);
        var byteLo = FromChar(chars[1]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[0] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[2]);
        byteLo = FromChar(chars[3]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[1] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[4]);
        byteLo = FromChar(chars[5]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[2] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[6]);
        byteLo = FromChar(chars[7]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[3] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[8]);
        byteLo = FromChar(chars[9]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[4] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[10]);
        byteLo = FromChar(chars[11]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[5] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[12]);
        byteLo = FromChar(chars[13]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[6] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[14]);
        byteLo = FromChar(chars[15]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[7] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[16]);
        byteLo = FromChar(chars[17]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[8] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[18]);
        byteLo = FromChar(chars[19]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[9] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[20]);
        byteLo = FromChar(chars[21]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[10] = (byte)((byteHi << 4) | byteLo);

        byteHi = FromChar(chars[22]);
        byteLo = FromChar(chars[23]);

        if ((byteLo | byteHi) == 0xFF) return false;

        bytes[11] = (byte)((byteHi << 4) | byteLo);

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FromChar(int c)
    {
        return c >= CharToHex.Length ? 0xFF : CharToHex[c];
    }

    /// <summary>Map from an ASCII char to its hex value, e.g. arr['b'] == 11. 0xFF means it's not a hex digit.</summary>
    private static ReadOnlySpan<byte> CharToHex => new byte[]
    {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 15
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 31
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 47
            0x0,  0x1,  0x2,  0x3,  0x4,  0x5,  0x6,  0x7,  0x8,  0x9,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 63
            0xFF, 0xA,  0xB,  0xC,  0xD,  0xE,  0xF,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 79
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 95
            0xFF, 0xa,  0xb,  0xc,  0xd,  0xe,  0xf,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 111
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 127
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 143
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 159
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 175
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 191
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 207
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 223
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 239
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF  // 255
    };
}