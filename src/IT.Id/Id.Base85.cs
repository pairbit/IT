namespace System;

public readonly partial struct Id
{
    private String ToBase85()
    {
        var result = new string((char)0, 15);
        unsafe
        {
            var source = ToByteArray();
            fixed (byte* sourceP = source)
            fixed (char* targetP = result)
                Base85.Encode(sourceP, targetP);
        }

        return result;
    }

    private void ToBase85(Span<Char> destination)
    {
        Base85.Encode(ToByteArray(), destination);
    }

    private static Id ParseBase85(ReadOnlySpan<Char> value)
    {
        if (value.Length != 15) throw new ArgumentException("String must be 15 characters long", nameof(value));

        Span<Byte> bytes = stackalloc Byte[12];

        Base85.Decode(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }

    private static Id ParseBase85(ReadOnlySpan<Byte> value)
    {
        if (value.Length != 15) throw new ArgumentException("String must be 15 characters long", nameof(value));

        Span<Byte> bytes = stackalloc Byte[12];

        Base85.Decode(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }
}