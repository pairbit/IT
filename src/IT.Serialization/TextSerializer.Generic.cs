﻿using System;
using System.Buffers;
using System.Text;
using System.Threading;

namespace IT.Serialization;

public abstract class TextSerializer<T> : Serializer<T>, ITextSerializer<T>
{
    private readonly Encoding _encoding;

    public TextSerializer(Encoding? encoding = null)
    {
        _encoding = encoding ?? Encoding.UTF8;
    }

    #region ISerializer

    public override byte[] Serialize(T value, CancellationToken cancellationToken)
        => _encoding.GetBytes(SerializeToText(value, cancellationToken));

    public override T? Deserialize(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken)
    {
        var span = memory.Span;

        var count = _encoding.GetCharCount(span);

        var pool = ArrayPool<Char>.Shared;

        var rented = pool.Rent(count);

        var rentedMemory = rented.AsMemory();

        try
        {
            _encoding.GetChars(span, rentedMemory.Span);

            return Deserialize(rentedMemory, cancellationToken);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    #endregion ISerializer

    #region ITextSerializer

    //public virtual void Serialize(IBufferWriter<Char> writer, T value, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    public abstract String SerializeToText(T value, CancellationToken cancellationToken);

    public abstract T? Deserialize(ReadOnlyMemory<Char> memory, CancellationToken cancellationToken);

    //public virtual T? Deserialize(in ReadOnlySequence<Char> sequence, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    #endregion ITextSerializer
}