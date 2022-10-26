using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System;

internal static class Base64
{
    internal static readonly Char[] table = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
                                              'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
                                              'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
                                              't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
                                              '8', '9', '+', '/', '=' };

    internal static readonly Char[] tableUrl = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
                                                 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
                                                 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
                                                 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
                                                 '8', '9', '-', '_', '=' };

    public static void ParsePath3(ReadOnlySpan<char> utf16, Span<byte> bytes)
    {
        ref char srcChars = ref MemoryMarshal.GetReference(utf16);
        ref byte destBytes = ref MemoryMarshal.GetReference(bytes);
        ref sbyte decodingMap = ref MemoryMarshal.GetReference(DecodingMap);

        int r = DecodeReverse(ref Unsafe.Add(ref srcChars, 15), ref decodingMap);

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 0), r);

        r = DecodeReverse(ref Unsafe.Add(ref srcChars, 11), ref decodingMap);

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 3), r);

        r = DecodeReverse(ref Unsafe.Add(ref srcChars, 7), ref decodingMap);

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 6), r);

        int i0 = Unsafe.Add(ref srcChars, 6);
        int i1 = Unsafe.Add(ref srcChars, 4);
        int i2 = Unsafe.Add(ref srcChars, 2);
        int i3 = Unsafe.Add(ref srcChars, 0);

        if (((i0 | i1 | i2 | i3) & 0xffffff00) != 0)
            throw new FormatException();

        i0 = Unsafe.Add(ref decodingMap, i0);
        i1 = Unsafe.Add(ref decodingMap, i1);
        i2 = Unsafe.Add(ref decodingMap, i2);
        i3 = Unsafe.Add(ref decodingMap, i3);

        i0 <<= 18;
        i1 <<= 12;
        i2 <<= 6;

        i0 |= i1;
        i0 |= i3;
        i0 |= i2;

        if (i0 < 0) throw new FormatException();

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 9), i0);
    }

    public static void ParsePath2(ReadOnlySpan<char> utf16, Span<byte> bytes)
    {
        ref char srcChars = ref MemoryMarshal.GetReference(utf16);
        ref byte destBytes = ref MemoryMarshal.GetReference(bytes);
        ref sbyte decodingMap = ref MemoryMarshal.GetReference(DecodingMap);

        int r = DecodeReverse(ref Unsafe.Add(ref srcChars, 14), ref decodingMap);

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 0), r);

        r = DecodeReverse(ref Unsafe.Add(ref srcChars, 10), ref decodingMap);

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 3), r);

        r = DecodeReverse(ref Unsafe.Add(ref srcChars, 6), ref decodingMap);

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 6), r);

        int i0 = Unsafe.Add(ref srcChars, 5);
        int i1 = Unsafe.Add(ref srcChars, 4);
        int i2 = Unsafe.Add(ref srcChars, 2);
        int i3 = Unsafe.Add(ref srcChars, 0);

        if (((i0 | i1 | i2 | i3) & 0xffffff00) != 0)
            throw new FormatException();

        i0 = Unsafe.Add(ref decodingMap, i0);
        i1 = Unsafe.Add(ref decodingMap, i1);
        i2 = Unsafe.Add(ref decodingMap, i2);
        i3 = Unsafe.Add(ref decodingMap, i3);

        i0 <<= 18;
        i1 <<= 12;
        i2 <<= 6;

        i0 |= i1;
        i0 |= i3;
        i0 |= i2;

        if (i0 < 0) throw new FormatException();

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 9), i0);
    }

    public static void Parse(ReadOnlySpan<char> utf16, Span<byte> bytes)
    {
        ref char srcChars = ref MemoryMarshal.GetReference(utf16);
        ref byte destBytes = ref MemoryMarshal.GetReference(bytes);
        ref sbyte decodingMap = ref MemoryMarshal.GetReference(DecodingMap);

        int r = Decode(ref Unsafe.Add(ref srcChars, 0), ref decodingMap);

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 0), r);

        r = Decode(ref Unsafe.Add(ref srcChars, 4), ref decodingMap);

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 3), r);

        r = Decode(ref Unsafe.Add(ref srcChars, 8), ref decodingMap);

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 6), r);

        int i0 = Unsafe.Add(ref srcChars, 12);
        int i1 = Unsafe.Add(ref srcChars, 13);
        int i2 = Unsafe.Add(ref srcChars, 14);
        int i3 = Unsafe.Add(ref srcChars, 15);

        if (((i0 | i1 | i2 | i3) & 0xffffff00) != 0)
            throw new FormatException();

        i0 = Unsafe.Add(ref decodingMap, i0);
        i1 = Unsafe.Add(ref decodingMap, i1);
        i2 = Unsafe.Add(ref decodingMap, i2);
        i3 = Unsafe.Add(ref decodingMap, i3);

        i0 <<= 18;
        i1 <<= 12;
        i2 <<= 6;

        i0 |= i1;
        i0 |= i3;
        i0 |= i2;

        if (i0 < 0) throw new FormatException();

        WriteThreeLowOrderBytes(ref Unsafe.Add(ref destBytes, 9), i0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Decode(ref char encodedChars, ref sbyte decodingMap)
    {
        int i0 = encodedChars;
        int i1 = Unsafe.Add(ref encodedChars, 1);
        int i2 = Unsafe.Add(ref encodedChars, 2);
        int i3 = Unsafe.Add(ref encodedChars, 3);

        if (((i0 | i1 | i2 | i3) & 0xffffff00) != 0)
            return -1; // One or more chars falls outside the 00..ff range. This cannot be a valid Base64 character.

        i0 = Unsafe.Add(ref decodingMap, i0);
        i1 = Unsafe.Add(ref decodingMap, i1);
        i2 = Unsafe.Add(ref decodingMap, i2);
        i3 = Unsafe.Add(ref decodingMap, i3);

        i0 <<= 18;
        i1 <<= 12;
        i2 <<= 6;

        i0 |= i3;
        i1 |= i2;
        i0 |= i1;

        if (i0 < 0) throw new FormatException();

        return i0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int DecodeReverse(ref char encodedChars, ref sbyte decodingMap)
    {
        int i3 = encodedChars;
        int i2 = Unsafe.Add(ref encodedChars, 1);
        int i1 = Unsafe.Add(ref encodedChars, 2);
        int i0 = Unsafe.Add(ref encodedChars, 3);

        if (((i0 | i1 | i2 | i3) & 0xffffff00) != 0)
            return -1; // One or more chars falls outside the 00..ff range. This cannot be a valid Base64 character.

        i0 = Unsafe.Add(ref decodingMap, i0);
        i1 = Unsafe.Add(ref decodingMap, i1);
        i2 = Unsafe.Add(ref decodingMap, i2);
        i3 = Unsafe.Add(ref decodingMap, i3);

        i0 <<= 18;
        i1 <<= 12;
        i2 <<= 6;

        i0 |= i3;
        i1 |= i2;
        i0 |= i1;

        if (i0 < 0) throw new FormatException();

        return i0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteThreeLowOrderBytes(ref byte destination, int value)
    {
        destination = (byte)(value >> 16);
        Unsafe.Add(ref destination, 1) = (byte)(value >> 8);
        Unsafe.Add(ref destination, 2) = (byte)value;
    }

    // Pre-computing this table using a custom string(s_characters) and GenerateDecodingMapAndVerify (found in tests)
    private static ReadOnlySpan<sbyte> DecodingMap => new sbyte[] // rely on C# compiler optimization to reference static data
    {
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, 62, -1, 63,         // 62 is placed at index 43 (for +) and 45 (for -), 63 at index 47 (for /)
            52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1,         // 52-61 are placed at index 48-57 (for 0-9), 64 at index 61 (for =)
            -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
            15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, 63,         // 0-25 are placed at index 65-90 (for A-Z), 63 at index 95 (for _)
            -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1,         // 26-51 are placed at index 97-122 (for a-z)
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,         // Bytes over 122 ('z') are invalid and cannot be decoded
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,         // Hence, padding the map with 255, which indicates invalid input
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    };
}