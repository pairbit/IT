namespace System;

public readonly partial struct Id
{
    private String ToBase64Url()
    {
        var result = new string((char)0, 16);
        var table = Base64.tableUrl;

        unsafe
        {
            fixed (char* resultP = result)
            fixed (char* base64 = &table[0])
            {
                var byte0 = (byte)(_timestamp >> 24);
                var byte1 = (byte)(_timestamp >> 16);
                var byte2 = (byte)(_timestamp >> 8);

                resultP[0] = base64[(byte0 & 0xfc) >> 2];
                resultP[1] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
                resultP[2] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
                resultP[3] = base64[byte2 & 0x3f];

                var byte3 = (byte)(_timestamp);
                var byte4 = (byte)(_b >> 24);
                var byte5 = (byte)(_b >> 16);

                resultP[4] = base64[(byte3 & 0xfc) >> 2];
                resultP[5] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
                resultP[6] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
                resultP[7] = base64[byte5 & 0x3f];

                var byte6 = (byte)(_b >> 8);
                var byte7 = (byte)(_b);
                var byte8 = (byte)(_c >> 24);

                resultP[8] = base64[(byte6 & 0xfc) >> 2];
                resultP[9] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
                resultP[10] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
                resultP[11] = base64[byte8 & 0x3f];

                var byte9 = (byte)(_c >> 16);
                var byte10 = (byte)(_c >> 8);
                var byte11 = (byte)(_c);

                resultP[12] = base64[(byte9 & 0xfc) >> 2];
                resultP[13] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
                resultP[14] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
                resultP[15] = base64[byte11 & 0x3f];
            }
        }
        return result;
    }

    private String ToBase64()
    {
        var result = new string((char)0, 16);
        var table = Base64.table;

        unsafe
        {
            fixed (char* resultP = result)
            fixed (char* base64 = &table[0])
            {
                var byte0 = (byte)(_timestamp >> 24);
                var byte1 = (byte)(_timestamp >> 16);
                var byte2 = (byte)(_timestamp >> 8);

                resultP[0] = base64[(byte0 & 0xfc) >> 2];
                resultP[1] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
                resultP[2] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
                resultP[3] = base64[byte2 & 0x3f];

                var byte3 = (byte)(_timestamp);
                var byte4 = (byte)(_b >> 24);
                var byte5 = (byte)(_b >> 16);

                resultP[4] = base64[(byte3 & 0xfc) >> 2];
                resultP[5] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
                resultP[6] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
                resultP[7] = base64[byte5 & 0x3f];

                var byte6 = (byte)(_b >> 8);
                var byte7 = (byte)(_b);
                var byte8 = (byte)(_c >> 24);

                resultP[8] = base64[(byte6 & 0xfc) >> 2];
                resultP[9] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
                resultP[10] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
                resultP[11] = base64[byte8 & 0x3f];

                var byte9 = (byte)(_c >> 16);
                var byte10 = (byte)(_c >> 8);
                var byte11 = (byte)(_c);

                resultP[12] = base64[(byte9 & 0xfc) >> 2];
                resultP[13] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
                resultP[14] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
                resultP[15] = base64[byte11 & 0x3f];
            }
        }
        return result;
    }

    private unsafe void ToBase64(Span<Char> destination, Char[] table)
    {
        fixed (char* resultP = destination)
        fixed (char* base64 = &table[0])
        {
            var byte0 = (byte)(_timestamp >> 24);
            var byte1 = (byte)(_timestamp >> 16);
            var byte2 = (byte)(_timestamp >> 8);

            resultP[0] = base64[(byte0 & 0xfc) >> 2];
            resultP[1] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
            resultP[2] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
            resultP[3] = base64[byte2 & 0x3f];

            var byte3 = (byte)(_timestamp);
            var byte4 = (byte)(_b >> 24);
            var byte5 = (byte)(_b >> 16);

            resultP[4] = base64[(byte3 & 0xfc) >> 2];
            resultP[5] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
            resultP[6] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
            resultP[7] = base64[byte5 & 0x3f];

            var byte6 = (byte)(_b >> 8);
            var byte7 = (byte)(_b);
            var byte8 = (byte)(_c >> 24);

            resultP[8] = base64[(byte6 & 0xfc) >> 2];
            resultP[9] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
            resultP[10] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
            resultP[11] = base64[byte8 & 0x3f];

            var byte9 = (byte)(_c >> 16);
            var byte10 = (byte)(_c >> 8);
            var byte11 = (byte)(_c);

            resultP[12] = base64[(byte9 & 0xfc) >> 2];
            resultP[13] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
            resultP[14] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
            resultP[15] = base64[byte11 & 0x3f];
        }
    }

    private unsafe void ToBase64(Span<Byte> destination, Byte[] table)
    {
        fixed (byte* resultP = destination)
        fixed (byte* base64 = &table[0])
        {
            var byte0 = (byte)(_timestamp >> 24);
            var byte1 = (byte)(_timestamp >> 16);
            var byte2 = (byte)(_timestamp >> 8);

            resultP[0] = base64[(byte0 & 0xfc) >> 2];
            resultP[1] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
            resultP[2] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
            resultP[3] = base64[byte2 & 0x3f];

            var byte3 = (byte)(_timestamp);
            var byte4 = (byte)(_b >> 24);
            var byte5 = (byte)(_b >> 16);

            resultP[4] = base64[(byte3 & 0xfc) >> 2];
            resultP[5] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
            resultP[6] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
            resultP[7] = base64[byte5 & 0x3f];

            var byte6 = (byte)(_b >> 8);
            var byte7 = (byte)(_b);
            var byte8 = (byte)(_c >> 24);

            resultP[8] = base64[(byte6 & 0xfc) >> 2];
            resultP[9] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
            resultP[10] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
            resultP[11] = base64[byte8 & 0x3f];

            var byte9 = (byte)(_c >> 16);
            var byte10 = (byte)(_c >> 8);
            var byte11 = (byte)(_c);

            resultP[12] = base64[(byte9 & 0xfc) >> 2];
            resultP[13] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
            resultP[14] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
            resultP[15] = base64[byte11 & 0x3f];
        }
    }

    private static Id ParseBase64(ReadOnlySpan<Char> value)
    {
        if (value.Length != 16) throw new ArgumentException("String must be 16 characters long", nameof(value));

        Span<Byte> bytes = stackalloc Byte[12];

        Base64.Parse(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }

    private static Id ParseBase64(ReadOnlySpan<Byte> value)
    {
        if (value.Length != 16) throw new ArgumentException("Id must be 16 bytes long", nameof(value));

        Span<Byte> bytes = stackalloc Byte[12];

        Base64.Parse(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }
}