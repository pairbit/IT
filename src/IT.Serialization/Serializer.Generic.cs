using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public abstract class Serializer<T> : ISerializer<T>
{
    #region IAsyncSerializer

    public virtual Task SerializeAsync(Stream stream, T value, CancellationToken cancellationToken = default)
    {
        var bytes = Serialize(value, cancellationToken);
        return stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
    }

    public virtual async ValueTask<T?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        //TODO: ArrayPool<Byte>.Shared.Rent(stream.Length);
        var bytes = new Byte[stream.Length];
        var len = await stream.ReadAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
        return Deserialize(bytes, cancellationToken);
    }

    #endregion IAsyncSerializer

    #region ISerializer

    public virtual void Serialize(IBufferWriter<Byte> writer, T value, CancellationToken cancellationToken = default)
        => writer.Write(Serialize(value, cancellationToken));

    public virtual void Serialize(Stream stream, T value, CancellationToken cancellationToken = default)
    {
        var bytes = Serialize(value, cancellationToken);
        stream.Write(bytes, 0, bytes.Length);
    }

    public abstract Byte[] Serialize(T value, CancellationToken cancellationToken = default);

    public abstract T? Deserialize(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken = default);

    public virtual T? Deserialize(in ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken = default)
    {
        if (!sequence.IsSingleSegment) throw new NotImplementedException();
        return Deserialize(sequence.First, cancellationToken);
    }

    public virtual T? Deserialize(Stream stream, CancellationToken cancellationToken = default)
    {
        //TODO: ArrayPool<Byte>.Shared.Rent(stream.Length);
        var bytes = new Byte[stream.Length];
        var len = stream.Read(bytes, 0, bytes.Length);
        return Deserialize(bytes, cancellationToken);
    }

    #endregion ISerializer
}