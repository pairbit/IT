using System.Text;

namespace System;

internal class Base32
{
    private static readonly char[] Base32Text = "0123456789ABCDEFGHJKMNPQRSTVWXYZ".ToCharArray();
    private static readonly byte[] Base32Bytes = Encoding.UTF8.GetBytes(Base32Text);
    private static readonly byte[] CharToBase32 = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 255, 255, 255, 255, 255, 255, 255, 10, 11, 12, 13, 14, 15, 16, 17, 255, 18, 19, 255, 20, 21, 255, 22, 23, 24, 25, 26, 255, 27, 28, 29, 30, 31, 255, 255, 255, 255, 255, 255, 10, 11, 12, 13, 14, 15, 16, 17, 255, 18, 19, 255, 20, 21, 255, 22, 23, 24, 25, 26, 255, 27, 28, 29, 30, 31 };

    public static String Encode(ReadOnlySpan<byte> bytes)
    {
        return Wiry.Base32.Base32Encoding.Base32.GetString(bytes);
    }

    public static void Encode(ReadOnlySpan<byte> bytes, Span<char> span)
    {
        throw new InvalidOperationException();
    }

    public static void Decode(ReadOnlySpan<char> base32, Span<byte> bytes)
    {
        
    }

    /*

     From Ulid not correct

    public static String Encode(ReadOnlySpan<byte> bytes)
    {
        var result = new string((char)0, 20);

        var byte0 = bytes[0];
        var byte1 = bytes[1];
        var byte2 = bytes[2];
        var byte3 = bytes[3];
        var byte4 = bytes[4];
        var byte5 = bytes[5];
        var byte6 = bytes[6];
        var byte7 = bytes[7];
        var byte8 = bytes[8];
        var byte9 = bytes[9];
        var byte10 = bytes[10];
        var byte11 = bytes[11];
        var byte12 = 0;

        unsafe
        {
            fixed (char* resultP = result)
            {
                resultP[0] = Base32Text[(byte0 & 224) >> 5];
                resultP[1] = Base32Text[byte0 & 31];
                resultP[2] = Base32Text[(byte1 & 248) >> 3];
                resultP[3] = Base32Text[((byte1 & 7) << 2) | ((byte2 & 192) >> 6)];
                resultP[4] = Base32Text[(byte2 & 62) >> 1];
                resultP[5] = Base32Text[((byte2 & 1) << 4) | ((byte3 & 240) >> 4)];
                resultP[6] = Base32Text[((byte3 & 15) << 1) | ((byte4 & 128) >> 7)];
                resultP[7] = Base32Text[(byte4 & 124) >> 2];
                resultP[8] = Base32Text[((byte4 & 3) << 3) | ((byte5 & 224) >> 5)];
                resultP[9] = Base32Text[byte5 & 31];
                resultP[10] = Base32Text[(byte6 & 248) >> 3];
                resultP[11] = Base32Text[((byte6 & 7) << 2) | ((byte7 & 192) >> 6)];
                resultP[12] = Base32Text[(byte7 & 62) >> 1];
                resultP[13] = Base32Text[((byte7 & 1) << 4) | ((byte8 & 240) >> 4)];
                resultP[14] = Base32Text[((byte8 & 15) << 1) | ((byte9 & 128) >> 7)];
                resultP[15] = Base32Text[(byte9 & 124) >> 2];
                resultP[16] = Base32Text[((byte9 & 3) << 3) | ((byte10 & 224) >> 5)];
                resultP[17] = Base32Text[byte10 & 31];
                resultP[18] = Base32Text[(byte11 & 248) >> 3];
                resultP[19] = Base32Text[((byte11 & 7) << 2) | ((byte12 & 192) >> 6)];
            } 
        }

        return result;
    }

    public static void Encode(ReadOnlySpan<byte> bytes, Span<char> span)
    {
        var byte0 = bytes[0];
        var byte1 = bytes[1];
        var byte2 = bytes[2];
        var byte3 = bytes[3];
        var byte4 = bytes[4];
        var byte5 = bytes[5];
        var byte6 = bytes[6];
        var byte7 = bytes[7];
        var byte8 = bytes[8];
        var byte9 = bytes[9];
        var byte10 = bytes[10];
        var byte11 = bytes[11];
        var byte12 = 0;

        span[0] = Base32Text[(byte0 & 224) >> 5];
        span[1] = Base32Text[byte0 & 31];
        span[2] = Base32Text[(byte1 & 248) >> 3];
        span[3] = Base32Text[((byte1 & 7) << 2) | ((byte2 & 192) >> 6)];
        span[4] = Base32Text[(byte2 & 62) >> 1];
        span[5] = Base32Text[((byte2 & 1) << 4) | ((byte3 & 240) >> 4)];
        span[6] = Base32Text[((byte3 & 15) << 1) | ((byte4 & 128) >> 7)];
        span[7] = Base32Text[(byte4 & 124) >> 2];
        span[8] = Base32Text[((byte4 & 3) << 3) | ((byte5 & 224) >> 5)];
        span[9] = Base32Text[byte5 & 31];
        span[10] = Base32Text[(byte6 & 248) >> 3];
        span[11] = Base32Text[((byte6 & 7) << 2) | ((byte7 & 192) >> 6)];
        span[12] = Base32Text[(byte7 & 62) >> 1];
        span[13] = Base32Text[((byte7 & 1) << 4) | ((byte8 & 240) >> 4)];
        span[14] = Base32Text[((byte8 & 15) << 1) | ((byte9 & 128) >> 7)];
        span[15] = Base32Text[(byte9 & 124) >> 2];
        span[16] = Base32Text[((byte9 & 3) << 3) | ((byte10 & 224) >> 5)];
        span[17] = Base32Text[byte10 & 31];
        span[18] = Base32Text[(byte11 & 248) >> 3];
        span[19] = Base32Text[((byte11 & 7) << 2) | ((byte12 & 192) >> 6)];

        //span[20] = Base32Text[(randomness6 & 62) >> 1];
        //span[21] = Base32Text[((randomness6 & 1) << 4) | ((randomness7 & 240) >> 4)];
        //span[22] = Base32Text[((randomness7 & 15) << 1) | ((randomness8 & 128) >> 7)];
        //span[23] = Base32Text[(randomness8 & 124) >> 2];
        //span[24] = Base32Text[((randomness8 & 3) << 3) | ((randomness9 & 224) >> 5)];
        //span[25] = Base32Text[randomness9 & 31]; // eliminate bounds-check of span
    }

    public static void Decode(ReadOnlySpan<char> base32, Span<byte> bytes)
    {
        bytes[0] = (byte)((CharToBase32[base32[0]] << 5) | CharToBase32[base32[1]]);
        bytes[1] = (byte)((CharToBase32[base32[2]] << 3) | (CharToBase32[base32[3]] >> 2));
        bytes[2] = (byte)((CharToBase32[base32[3]] << 6) | (CharToBase32[base32[4]] << 1) | (CharToBase32[base32[5]] >> 4));
        bytes[3] = (byte)((CharToBase32[base32[5]] << 4) | (CharToBase32[base32[6]] >> 1));
        bytes[4] = (byte)((CharToBase32[base32[6]] << 7) | (CharToBase32[base32[7]] << 2) | (CharToBase32[base32[8]] >> 3));
        bytes[5] = (byte)((CharToBase32[base32[8]] << 5) | CharToBase32[base32[9]]);

        bytes[6] = (byte)((CharToBase32[base32[10]] << 3) | (CharToBase32[base32[11]] >> 2));
        bytes[7] = (byte)((CharToBase32[base32[11]] << 6) | (CharToBase32[base32[12]] << 1) | (CharToBase32[base32[13]] >> 4));
        bytes[8] = (byte)((CharToBase32[base32[13]] << 4) | (CharToBase32[base32[14]] >> 1));
        bytes[9] = (byte)((CharToBase32[base32[14]] << 7) | (CharToBase32[base32[15]] << 2) | (CharToBase32[base32[16]] >> 3));
        bytes[10] = (byte)((CharToBase32[base32[16]] << 5) | CharToBase32[base32[17]]);
        bytes[11] = (byte)((CharToBase32[base32[18]] << 3) | CharToBase32[base32[19]] >> 2);

        //randomness6 = (byte)((CharToBase32[base32[19]] << 6) | (CharToBase32[base32[20]] << 1) | (CharToBase32[base32[21]] >> 4));
        //randomness7 = (byte)((CharToBase32[base32[21]] << 4) | (CharToBase32[base32[22]] >> 1));
        //randomness8 = (byte)((CharToBase32[base32[22]] << 7) | (CharToBase32[base32[23]] << 2) | (CharToBase32[base32[24]] >> 3));
        //randomness9 = (byte)((CharToBase32[base32[24]] << 5) | CharToBase32[base32[25]]); // eliminate bounds-check of span
    }

    */
}