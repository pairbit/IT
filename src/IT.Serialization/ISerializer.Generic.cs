using System;

namespace IT.Serialization;

public interface ISerializer<T>
{
    Byte[] Serialize(T value);

    T? Deserialize(ReadOnlySpan<Byte> value);
}