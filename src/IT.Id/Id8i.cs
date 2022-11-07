namespace System;

/// <summary>
/// 16 bytes
/// </summary>
public readonly struct Id8i
{
    private readonly Id _id;
    private readonly UInt16 _index1;
    private readonly Byte _index2;
    private readonly Byte _type;

    public static readonly Byte MinType = 0;
    public static readonly Byte MaxType = 63;
    public static readonly Int32 MinIndex = 0;
    public static readonly Int32 MaxIndex = 16777215;//3 bytes
    public static readonly Id8i Empty = default;
    public static readonly Id8i MinValue = new(Id.MinValue, MinType, MinIndex);
    public static readonly Id8i MaxValue = new(Id.MaxValue, MaxType, MaxIndex);

    public Id Id => _id;

    public Byte Type => _type;

    public Int32 Index => (_index1 << 8) + _index2;

    public Id8i(Id id, Byte type, Int32 index)
    {
        if (type > 63) throw new ArgumentOutOfRangeException(nameof(type), "The type value cannot be greater than 63");
        if ((index & 0xff000000) != 0) throw new ArgumentOutOfRangeException(nameof(index), "The index value must be between 0 and 16777215 (it must fit in 3 bytes).");

        _id = id;
        _type = type;
        _index1 = (ushort)(index >> 8);//first 2 bytes
        _index2 = (byte)index;//3rd byte
    }
}