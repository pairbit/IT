using System.Runtime.InteropServices;

namespace System;

/// <summary>
/// 16 bytes and from 20 to 22 chars
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 16)]
public readonly struct Id12i
{
    [FieldOffset(0)]
    private readonly Id _id;

    [FieldOffset(12)]
    private readonly UInt16 _value;

    [FieldOffset(14)]
    private readonly UInt16 _index;

    public static readonly UInt16 MinIndex = 0;
    public static readonly UInt16 MaxIndex = 65535;//2 bytes
    public static readonly Id12i Empty = default;
    public static readonly Id12i Min = new(Id.Min, Id12.MinValue, MinIndex);
    public static readonly Id12i Max = new(Id.Max, Id12.MaxValue, MaxIndex);

    /// <summary>
    /// 12 bytes and 16 chars
    /// </summary>
    public Id Id => _id;

    /// <summary>
    /// 12 bits and 2 chars
    /// </summary>
    public UInt16 Value => _value;

    /// <summary>
    /// 2 bytes and from 1 to 3 chars
    /// </summary>
    public Int32 Index => _index;

    /// <param name="id">12 bytes</param>
    /// <param name="value">12 bits</param>
    /// <param name="index">2 bytes</param>
    /// <exception cref="ArgumentOutOfRangeException">The value cannot be greater than 4095 (it must fit in 12 bits)</exception>
    /// <exception cref="ArgumentOutOfRangeException">The index value must be between 0 and 65535 (it must fit in 2 bytes)</exception>
    public Id12i(Id id, UInt16 value, Int32 index)
    {
        if (value > 4095) throw new ArgumentOutOfRangeException(nameof(value), "The value cannot be greater than 4095 (it must fit in 12 bits)");
        if ((index & 0xff000000) != 0) throw new ArgumentOutOfRangeException(nameof(index), "The index value must be between 0 and 65535 (it must fit in 2 bytes)");

        _id = id;
        _value = value;
        _index = (UInt16)index;
    }

    public Id12i(Id id, UInt16 value, UInt16 index)
    {
        if (value > 4095) throw new ArgumentOutOfRangeException(nameof(value), "The value cannot be greater than 4095 (it must fit in 12 bits)");

        _id = id;
        _value = value;
        _index = index;
    }
}