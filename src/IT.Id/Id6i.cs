namespace System;

/// <summary>
/// 16 bytes and from 19 to 22 chars
/// </summary>
internal readonly struct Id6i
{
    private readonly Id _id;
    private readonly UInt16 _index1;
    private readonly Byte _index2;
    private readonly Byte _value;

    public static readonly Int32 MinIndex = 0;
    public static readonly Int32 MaxIndex = 16777215;//3 bytes
    public static readonly Id6i Empty = default;
    public static readonly Id6i Min = new(Id.Min, Id6.MinValue, MinIndex);
    public static readonly Id6i Max = new(Id.Max, Id6.MaxValue, MaxIndex);

    /// <summary>
    /// 12 bytes and 16 chars
    /// </summary>
    public Id Id => _id;

    /// <summary>
    /// 6 bits and 1 char
    /// </summary>
    public Byte Value => _value;

    /// <summary>
    /// 3 bytes and from 1 to 4 chars
    /// </summary>
    public Int32 Index => (_index1 << 8) + _index2;

    /// <param name="id">12 bytes</param>
    /// <param name="value">6 bits</param>
    /// <param name="index">3 bytes</param>
    /// <exception cref="ArgumentOutOfRangeException">The value cannot be greater than 63 (it must fit in 6 bits)</exception>
    /// <exception cref="ArgumentOutOfRangeException">The index value must be between 0 and 16777215 (it must fit in 3 bytes)</exception>
    public Id6i(Id id, Byte value, Int32 index)
    {
        if (value > 63) throw new ArgumentOutOfRangeException(nameof(value), "The value cannot be greater than 63 (it must fit in 6 bits)");
        if ((index & 0xff000000) != 0) throw new ArgumentOutOfRangeException(nameof(index), "The index value must be between 0 and 16777215 (it must fit in 3 bytes)");

        _id = id;
        _value = value;
        _index1 = (ushort)(index >> 8);//first 2 bytes
        _index2 = (byte)index;//3rd byte
    }
}