using System.Linq;

namespace System;

internal static class Base32
{
    private const int AlphabetLength = 32;
    private const int LookupTableNullItem = -1;
    private const Int32 LowCode = 48;
    private const Int32 LookupSize = 43;

    private static readonly int[] _lookupValues;
    private static readonly string ALPHABET = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

    static Base32()
    {
        int[] codes = ALPHABET.Select(ch => (int)ch).ToArray();
        int min = codes.Min();
        int max = codes.Max();
        int size = max - min + 1;
        var table = new int[size];

        for (int i = 0; i < table.Length; i++)
            table[i] = LookupTableNullItem;

        foreach (int code in codes)
            table[code - min] = ALPHABET.IndexOf((char)code);

        if (min != LowCode) throw new InvalidOperationException();

        if (table.Length != LookupSize) throw new InvalidOperationException();

        _lookupValues = table;
    }

    public static String Encode(ReadOnlySpan<byte> bytes)
    {
#if NETSTANDARD2_0
        var result = new string((char)0, 20);

        unsafe
        {
            fixed (byte* pInput = bytes)
            fixed (char* pOutput = result)
            fixed (char* pAlphabet = ALPHABET)
            {
                ToBase32GroupsUnsafe(pInput, pOutput, pAlphabet);
            }
        }

        return result;
#else
        unsafe
        {
            fixed (byte* dataPtr = bytes)
            {
                return String.Create(20, (IntPtr)dataPtr, (encoded, state) =>
                {
                    Encode(new ReadOnlySpan<Byte>((Byte*)state, 12), encoded);
                });
            }
        }
#endif
    }

    public static unsafe void Encode(ReadOnlySpan<byte> input, Span<char> output)
    {
        fixed (byte* pInput = input)
        fixed (char* pOutput = output)
        fixed (char* pAlphabet = ALPHABET)
        {
            ToBase32GroupsUnsafe(pInput, pOutput, pAlphabet);
        }
    }

    private static unsafe void ToBase32GroupsUnsafe(byte* pInput, char* pOutput, char* pAlphabet)
    {
        ulong value = *pInput++;
        value = (value << 8) | (*pInput++);
        value = (value << 8) | (*pInput++);
        value = (value << 8) | (*pInput++);
        value = (value << 8) | (*pInput++);

        *pOutput++ = pAlphabet[value >> 35];
        *pOutput++ = pAlphabet[(value >> 30) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 25) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 20) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 15) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 10) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 5) & 0x1F];
        *pOutput++ = pAlphabet[value & 0x1F];

        value = *pInput++;
        value = (value << 8) | (*pInput++);
        value = (value << 8) | (*pInput++);
        value = (value << 8) | (*pInput++);
        value = (value << 8) | (*pInput++);

        *pOutput++ = pAlphabet[value >> 35];
        *pOutput++ = pAlphabet[(value >> 30) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 25) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 20) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 15) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 10) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 5) & 0x1F];
        *pOutput++ = pAlphabet[value & 0x1F];

        value = (((ulong)(*pInput++) << 8) | *pInput) << 4;

        *pOutput++ = pAlphabet[value >> 15];
        *pOutput++ = pAlphabet[(value >> 10) & 0x1F];
        *pOutput++ = pAlphabet[(value >> 5) & 0x1F];
        *pOutput = pAlphabet[value & 0x1F];
    }

    public static unsafe void Decode(ReadOnlySpan<char> encoded, Span<byte> output)
    {
        fixed (char* pEncoded = encoded)
        fixed (byte* pOutput = output)
        {
            ToBytesGroupsUnsafe(pEncoded, pOutput);
        }
    }

    private static unsafe void ToBytesGroupsUnsafe(char* pEncoded, byte* pOutput)
    {
        ulong value = GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);

        *pOutput++ = (byte)(value >> 32);
        *pOutput++ = (byte)(value >> 24);
        *pOutput++ = (byte)(value >> 16);
        *pOutput++ = (byte)(value >> 8);
        *pOutput++ = (byte)value;

        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);

        *pOutput++ = (byte)(value >> 32);
        *pOutput++ = (byte)(value >> 24);
        *pOutput++ = (byte)(value >> 16);
        *pOutput++ = (byte)(value >> 8);
        *pOutput++ = (byte)value;

        value = GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded++ - LowCode);
        value = (value << 5) | GetByte(*pEncoded - LowCode);

        *pOutput++ = (byte)(value >> 12);
        *pOutput = (byte)(value >> 4);
    }

    private static Byte GetByte(int lookupIndex)
    {
        if (lookupIndex < 0 || lookupIndex >= LookupSize)
            throw new FormatException();

        //int item = *(pLookup + lookupIndex);

        var item = _lookupValues[lookupIndex];

        if (item == LookupTableNullItem)
            throw new FormatException();

        return (byte)item;
    }
}