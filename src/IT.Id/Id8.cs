namespace System;

/// <summary>
/// 13 bytes
/// </summary>
public readonly struct Id8
{
    private readonly Id _id;
    private readonly Byte _type;

    public static readonly Byte MinType = 0;
    public static readonly Byte MaxType = 63;
    public static readonly Id8 Empty = default;
    public static readonly Id8 MinValue = new(Id.MinValue, MinType);
    public static readonly Id8 MaxValue = new(Id.MaxValue, MaxType);

    public Id Id => _id;

    public Byte Type => _type;

    public Id8(Id id, Byte type) 
    {
        if (type > 63) throw new ArgumentOutOfRangeException(nameof(type), "The type value cannot be greater than 63");

        _id = id;
        _type = type;
    }
}