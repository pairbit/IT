namespace System;

public readonly partial struct Id
{
    private String ToBase58()
    {
        return Base58.Encode(ToByteArray());
    }

    private static Id ParseBase58(ReadOnlySpan<Char> value)
    {
        var len = value.Length;
        if (len < 12 || len > 17) throw new ArgumentOutOfRangeException(nameof(value), len, "String must be 12 to 17 characters long");

        Span<Byte> bytes = stackalloc Byte[18];

        Base58.Decode(value, bytes, out var written);

        bytes = bytes.Slice(written - 12, 12);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }


}