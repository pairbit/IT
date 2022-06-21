using System;

namespace IT.Serialization;

public interface ITextSerializer<T>
{
    String Serialize(T value);

    T? Deserialize(ReadOnlySpan<Char> value);
}