namespace System;

public readonly struct Id16
{
    private readonly Id _id;
    private readonly UInt16 _type;

    public static readonly Id16 Empty = default;

    public Id Id => _id;

    public UInt16 Type => _type;

    public Id16(Id id, UInt16 type)
    {
        if (type > 4095) throw new ArgumentOutOfRangeException(nameof(type), "Value cannot be greater than 4095");

        _id = id;
        _type = type;
    }
}