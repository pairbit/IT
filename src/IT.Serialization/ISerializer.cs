using System;

namespace IT.Serialization;

public interface ISerializer
{
    Byte[]? Serialize<T>(T? value);

    Byte[]? Serialize(Object? value);

    Object? Deserialize(ReadOnlySpan<Byte> value, Type type);

    T? Deserialize<T>(ReadOnlySpan<Byte> value);
}