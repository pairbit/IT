namespace System;

public readonly partial struct Id
{
    private String ToBase32()
    {
        return Base32.Encode(ToByteArray());
    }

    private void ToBase32(Span<Char> destination)
    {
        Base32.Encode(ToByteArray(), destination);
    }

    private static Id ParseBase32(ReadOnlySpan<Char> value)
    {
        if (value.Length != 20) throw new ArgumentException("String must be 20 characters long", nameof(value));

        Span<Byte> bytes = stackalloc Byte[12];

        Base32.Decode(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }
}