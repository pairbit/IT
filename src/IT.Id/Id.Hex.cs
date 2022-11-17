namespace System;

public readonly partial struct Id
{
    private String ToHexLower()
    {
        var result = new string((char)0, 24);
        unsafe
        {
            var lookupP = Hex._lowerLookup32UnsafeP;
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;
                resultP2[0] = lookupP[(byte)(_timestamp >> 24)];
                resultP2[1] = lookupP[(byte)(_timestamp >> 16)];
                resultP2[2] = lookupP[(byte)(_timestamp >> 8)];
                resultP2[3] = lookupP[(byte)(_timestamp)];
                resultP2[4] = lookupP[(byte)(_b >> 24)];
                resultP2[5] = lookupP[(byte)(_b >> 16)];
                resultP2[6] = lookupP[(byte)(_b >> 8)];
                resultP2[7] = lookupP[(byte)(_b)];
                resultP2[8] = lookupP[(byte)(_c >> 24)];
                resultP2[9] = lookupP[(byte)(_c >> 16)];
                resultP2[10] = lookupP[(byte)(_c >> 8)];
                resultP2[11] = lookupP[(byte)(_c)];
            }
        }
        return result;
    }

    private String ToHexUpper()
    {
        var result = new string((char)0, 24);
        unsafe
        {
            var lookupP = Hex._upperLookup32UnsafeP;
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;
                resultP2[0] = lookupP[(byte)(_timestamp >> 24)];
                resultP2[1] = lookupP[(byte)(_timestamp >> 16)];
                resultP2[2] = lookupP[(byte)(_timestamp >> 8)];
                resultP2[3] = lookupP[(byte)(_timestamp)];
                resultP2[4] = lookupP[(byte)(_b >> 24)];
                resultP2[5] = lookupP[(byte)(_b >> 16)];
                resultP2[6] = lookupP[(byte)(_b >> 8)];
                resultP2[7] = lookupP[(byte)(_b)];
                resultP2[8] = lookupP[(byte)(_c >> 24)];
                resultP2[9] = lookupP[(byte)(_c >> 16)];
                resultP2[10] = lookupP[(byte)(_c >> 8)];
                resultP2[11] = lookupP[(byte)(_c)];
            }
        }
        return result;
    }

    private unsafe void ToHex(Span<Char> destination, uint* lookupP)
    {
        fixed (char* resultP = destination)
        {
            uint* resultP2 = (uint*)resultP;
            resultP2[0] = lookupP[(byte)(_timestamp >> 24)];
            resultP2[1] = lookupP[(byte)(_timestamp >> 16)];
            resultP2[2] = lookupP[(byte)(_timestamp >> 8)];
            resultP2[3] = lookupP[(byte)(_timestamp)];
            resultP2[4] = lookupP[(byte)(_b >> 24)];
            resultP2[5] = lookupP[(byte)(_b >> 16)];
            resultP2[6] = lookupP[(byte)(_b >> 8)];
            resultP2[7] = lookupP[(byte)(_b)];
            resultP2[8] = lookupP[(byte)(_c >> 24)];
            resultP2[9] = lookupP[(byte)(_c >> 16)];
            resultP2[10] = lookupP[(byte)(_c >> 8)];
            resultP2[11] = lookupP[(byte)(_c)];
        }
    }

    private unsafe void ToHex(Span<Byte> destination, ushort* lookupP)
    {
        fixed (byte* resultP = destination)
        {
            ushort* resultP2 = (ushort*)resultP;
            resultP2[0] = lookupP[(byte)(_timestamp >> 24)];
            resultP2[1] = lookupP[(byte)(_timestamp >> 16)];
            resultP2[2] = lookupP[(byte)(_timestamp >> 8)];
            resultP2[3] = lookupP[(byte)(_timestamp)];
            resultP2[4] = lookupP[(byte)(_b >> 24)];
            resultP2[5] = lookupP[(byte)(_b >> 16)];
            resultP2[6] = lookupP[(byte)(_b >> 8)];
            resultP2[7] = lookupP[(byte)(_b)];
            resultP2[8] = lookupP[(byte)(_c >> 24)];
            resultP2[9] = lookupP[(byte)(_c >> 16)];
            resultP2[10] = lookupP[(byte)(_c >> 8)];
            resultP2[11] = lookupP[(byte)(_c)];
        }
    }

    private static Id ParseHex(ReadOnlySpan<Char> value)
    {
        if (value.Length != 24) throw new ArgumentException("String must be 24 characters long", nameof(value));

        Span<Byte> bytes = stackalloc Byte[12];

        Hex.Decode(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }

    private static Id ParseHex(ReadOnlySpan<Byte> value)
    {
        if (value.Length != 24) throw new ArgumentException("String must be 24 characters long", nameof(value));

        Span<Byte> bytes = stackalloc Byte[12];

        Hex.Decode(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }
}