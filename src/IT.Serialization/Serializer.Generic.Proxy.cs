using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public class SerializerProxy<T> : ISerializer<T>
{
    private readonly ISerializer _serializer;

    public SerializerProxy(ISerializer serializer)
    {
        _serializer = serializer;
    }

    #region IAsyncSerializer

    public Task SerializeAsync(Stream stream, T value, CancellationToken cancellationToken = default)
        => _serializer.SerializeAsync(stream, value, cancellationToken);

    public ValueTask<T?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
        => _serializer.DeserializeAsync<T>(stream, cancellationToken);

    #endregion IAsyncSerializer

    #region ISerializer

    public void Serialize(IBufferWriter<Byte> writer, T value, CancellationToken cancellationToken = default)
        => _serializer.Serialize(writer, value, cancellationToken);

    public void Serialize(Stream stream, T value, CancellationToken cancellationToken = default)
        => _serializer.Serialize(stream, value, cancellationToken);

    public Byte[] Serialize(T value, CancellationToken cancellationToken = default)
         => _serializer.Serialize(value, cancellationToken);

    public T? Deserialize(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken = default)
        => _serializer.Deserialize<T>(memory, cancellationToken);

    public T? Deserialize(in ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken = default)
        => _serializer.Deserialize<T>(sequence, cancellationToken);

    public T? Deserialize(Stream stream, CancellationToken cancellationToken = default)
        => _serializer.Deserialize<T>(stream, cancellationToken);

    #endregion ISerializer
}