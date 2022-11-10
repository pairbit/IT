using System.Runtime.InteropServices;

namespace System;

/// <summary>
/// 13 bytes and 17 chars
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 13)]
internal readonly struct Id6
{
    [FieldOffset(0)]
    private readonly Id _id;

    [FieldOffset(12)]
    private readonly Byte _value;

    public static readonly Byte MinValue = 0;
    public static readonly Byte MaxValue = 63;
    public static readonly Id6 Empty = default;
    public static readonly Id6 Min = new(Id.Min, MinValue);
    public static readonly Id6 Max = new(Id.Max, MaxValue);

    /// <summary>
    /// 12 bytes and 16 chars
    /// </summary>
    public Id Id => _id;

    /// <summary>
    /// 6 bits and 1 char
    /// </summary>
    public Byte Value => _value;

    /// <param name="id">12 bytes</param>
    /// <param name="value">6 bits</param>
    /// <exception cref="ArgumentOutOfRangeException">The value cannot be greater than 63 (it must fit in 6 bits)</exception>
    public Id6(Id id, Byte value)
    {
        if (value > 63) throw new ArgumentOutOfRangeException(nameof(value), "The value cannot be greater than 63 (it must fit in 6 bits)");

        _id = id;
        _value = value;
    }
}