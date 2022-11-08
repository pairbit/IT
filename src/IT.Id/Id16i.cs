using System.Runtime.InteropServices;

namespace System;

/// <summary>
/// 16 bytes
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 16)]
internal readonly struct Id16i
{
    [FieldOffset(0)]
    private readonly Id _id;

    [FieldOffset(12)]
    private readonly UInt16 _type;

    [FieldOffset(14)]
    private readonly UInt16 _index;

    public static readonly Int32 MinIndex = 0;
    public static readonly Int32 MaxIndex = 65535;//3 bytes
    public static readonly Id16i Empty = default;
    public static readonly Id16i MinValue = new(Id.MinValue, Id16.MinType, MinIndex);
    public static readonly Id16i MaxValue = new(Id.MaxValue, Id16.MaxType, MaxIndex);

    public Id Id => _id;

    public UInt16 Type => _type;

    public Int32 Index => _index;

    public Id16i(Id id, UInt16 type, Int32 index)
    {
        if (type > 4095) throw new ArgumentOutOfRangeException(nameof(type), "The type value cannot be greater than 4095");
        if ((index & 0xff000000) != 0) throw new ArgumentOutOfRangeException(nameof(index), "The index value must be between 0 and 65535 (it must fit in 2 bytes).");

        _id = id;
        _type = type;
        _index = (UInt16)index;
    }
}
