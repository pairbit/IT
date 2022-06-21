using System;
using System.Text;

namespace IT.Serialization;

public abstract class TextSerializer<T> : ITextSerializer<T>, ISerializer<T>
{
    private readonly Encoding _encoding;

    public TextSerializer(Encoding encoding)
    {
        _encoding = encoding;
    }

    public TextSerializer() : this(Encoding.UTF8) { }

    #region ISerializer

    T? ISerializer<T>.Deserialize(ReadOnlySpan<Byte> value)
    {
        var chars = new Span<Char>();
        _encoding.GetChars(value, chars);
        return Deserialize(chars);
    }

    Byte[]? ISerializer<T>.Serialize(T? value) => _encoding.GetBytes(Serialize(value));

    #endregion ISerializer

    #region ITextSerializer

    public abstract T? Deserialize(ReadOnlySpan<Char> value);

    public abstract String? Serialize(T? value);

    #endregion ITextSerializer
}