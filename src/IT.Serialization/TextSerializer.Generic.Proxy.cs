using System;
using System.Threading;

namespace IT.Serialization;

public class TextSerializerProxy<T> : SerializerProxy<T>, ITextSerializer<T>
{
    private readonly ITextSerializer _textSerializer;

    public TextSerializerProxy(ITextSerializer textSerializer) : base(textSerializer)
    {
        _textSerializer = textSerializer;
    }

    #region ITextSerializer

    //public virtual void Serialize(IBufferWriter<Char> writer, T value, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    public String SerializeToText(T value, CancellationToken cancellationToken)
        => _textSerializer.SerializeToText(value, cancellationToken);

    public T? Deserialize(ReadOnlyMemory<Char> memory, CancellationToken cancellationToken)
        => _textSerializer.Deserialize<T>(memory, cancellationToken);

    //public virtual T? Deserialize(in ReadOnlySequence<Char> sequence, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    #endregion ITextSerializer
}