using System;

namespace StackExchange.Redis;

public static class xRedisValue
{
    public static RedisValue ToRedisValue<T>(this T value, Func<T, Byte[]>? serialize = null)
    {
        if (value is String str) return str;
        if (value is Char @char) return (UInt64)@char;
        if (value is Boolean boolean) return boolean;
        if (value is SByte @sbyte) return (UInt64)@sbyte;
        if (value is Byte @byte) return (Int64)@byte;
        if (value is Byte[] bytes) return bytes;
        if (value is ReadOnlyMemory<Byte> readOnlyMemory) return readOnlyMemory;
        if (value is Memory<Byte> memory) return (ReadOnlyMemory<Byte>)memory;
        if (value is Int16 int16) return int16;
        if (value is Int32 int32) return int32;
        if (value is Int64 int64) return int64;
        if (value is UInt16 uint16) return (UInt64)uint16;
        if (value is UInt32 uint32) return uint32;
        if (value is UInt64 uint64) return uint64;
        if (value is Single single) return single;
        if (value is Double @double) return @double;
        if (value is Decimal @decimal) return (Double)@decimal;//SUPPORT??
        if (value is Guid guid) return guid.ToByteArray();
        if (value is DateTime dateTime) return dateTime.Ticks;
        if (value is TimeSpan timeSpan) return timeSpan.Ticks;

        if (serialize is null) throw new NotSupportedException("serialize is null");

        return serialize(value);
    }

    public static T? TryTo<T>(this RedisValue value, Func<ReadOnlyMemory<Byte>, T?>? deserialize = null)
    {
        if (value.IsNull) return default;

        var val = (T?)value.TryToDynamic(typeof(T));

        if (val is null)
        {
            if (deserialize is null) throw new NotSupportedException("deserialize is null");

            return deserialize(value);
        }

        return val;
    }

    public static T To<T>(this RedisValue value, Func<ReadOnlyMemory<Byte>, T?>? deserialize = null)
    {
        if (value.IsNull) throw new ArgumentNullException(nameof(value));

        var val = (T?)value.TryToDynamic(typeof(T));

        if (val is null)
        {
            if (deserialize is null) throw new NotSupportedException();

            val = deserialize(value);
        }

        if (val is null) throw new InvalidOperationException("val is null");

        return val!;
    }

    private static dynamic? Null() => null;

    private static dynamic? TryToDynamic(this RedisValue value, Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.Boolean => (Boolean)value,
        TypeCode.Byte => (Byte)(UInt64)value,
        TypeCode.Char => (Char)(Int64)value,
        TypeCode.DateTime => new DateTime((Int64)value),
        TypeCode.DBNull => throw new InvalidOperationException(),
        TypeCode.Decimal => new Decimal((Double)value),
        TypeCode.Double => (Double)value,
        TypeCode.Empty => throw new InvalidOperationException(),
        TypeCode.Int16 => (Int16)value,
        TypeCode.Int32 => (Int32)value,
        TypeCode.Int64 => (Int64)value,
        TypeCode.Object => value.TryToObject(type),
        TypeCode.SByte => (SByte)value,
        TypeCode.Single => (Single)value,
        TypeCode.String => (String?)value,
        TypeCode.UInt16 => (UInt16)(UInt64)value,
        TypeCode.UInt32 => (UInt32)value,
        TypeCode.UInt64 => (UInt64)value,
        _ => throw new InvalidOperationException()
    };

    private static dynamic? TryToObject(this RedisValue value, Type type)
    {
        if (type == typeof(Guid)) return new Guid((Byte[])value);
        if (type == typeof(TimeSpan)) return new TimeSpan((Int64)value);
        if (type == typeof(Byte[])) return (Byte[])value;
        if (type == typeof(ReadOnlyMemory<Byte>)) return (ReadOnlyMemory<byte>)value;

        return null;
    }
}