namespace System;

public readonly struct Id8
{
    private readonly Id _id;
    private readonly Byte _type;

    public static readonly Id8 Empty = default;

    public Id Id => _id;

    public Byte Type => _type;

    public Id8(Id id, Byte type) 
    {
        if (type > 63) throw new ArgumentOutOfRangeException(nameof(type), "Value cannot be greater than 63");

        _id = id;
        _type = type;
    }
}