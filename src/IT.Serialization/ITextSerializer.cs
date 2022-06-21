using System;

namespace IT.Serialization;

public interface ITextSerializer
{
    String Serialize<T>(T value);

    String Serialize(Object value);

    Object? Deserialize(ReadOnlySpan<Char> value, Type type);

    T? Deserialize<T>(ReadOnlySpan<Char> value);
}