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

        var charCount = _encoding.GetCharCount(span);

        //TODO: ArrayPool<Char>.Shared.Rent(charCount);
        Memory<Char> memChars = new Char[charCount];

        _encoding.GetChars(span, memChars.Span);

        return Deserialize(memChars, cancellationToken);
    }

    #endregion ISerializer

    #region ITextSerializer

    public virtual void Serialize(IBufferWriter<Char> writer, T value, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public abstract String SerializeToText(T value, CancellationToken cancellationToken);

    public abstract T? Deserialize(ReadOnlyMemory<Char> memory, CancellationToken cancellationToken);

    public virtual T? Deserialize(in ReadOnlySequence<Char> sequence, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    #endregion ITextSerializer
}