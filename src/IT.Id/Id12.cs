using System.Runtime.InteropServices;

namespace System;

/// <summary>
/// 14 bytes and 18 chars
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 14)]
public readonly struct Id12
{
    [FieldOffset(0)]
    private readonly Id _id;

    [FieldOffset(12)]
    private readonly UInt16 _value;

    public static readonly UInt16 MinValue = 0;
    public static readonly UInt16 MaxValue = 4095;//12 bits
    public static readonly Id12 Empty = default;
    public static readonly Id12 Min = new(Id.Min, MinValue);
    public static readonly Id12 Max = new(Id.Max, MaxValue);

    /// <summary>
    /// 12 bytes and 16 chars
    /// </summary>
    public Id Id => _id;

    /// <summary>
    /// 12 bits and 2 chars
    /// </summary>
    public UInt16 Value => _value;

    /// <param name="id">12 bytes</param>
    /// <param name="value">12 bits</param>
    /// <exception cref="ArgumentOutOfRangeException">The value cannot be greater than 4095 (it must fit in 12 bits)</exception>
    public Id12(Id id, UInt16 value)
    {
        if (value > 4095) throw new ArgumentOutOfRangeException(nameof(value), "The value cannot be greater than 4095 (it must fit in 12 bits)");

        _id = id;
        _value = value;
    }
}