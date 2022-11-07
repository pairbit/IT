using System.Runtime.InteropServices;

namespace System;

/// <summary>
/// 14 bytes
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 14)]
public readonly struct Id16
{
    [FieldOffset(0)]
    private readonly Id _id;

    [FieldOffset(12)]
    private readonly UInt16 _type;

    public static readonly UInt16 MinType = 0;
    public static readonly UInt16 MaxType = 4095;
    public static readonly Id16 Empty = default;
    public static readonly Id16 MinValue = new(Id.MinValue, MinType);
    public static readonly Id16 MaxValue = new(Id.MaxValue, MaxType);

    public Id Id => _id;

    public UInt16 Type => _type;

    public Id16(Id id, UInt16 type)
    {
        if (type > 4095) throw new ArgumentOutOfRangeException(nameof(type), "The type value cannot be greater than 4095");

        _id = id;
        _type = type;
    }
}