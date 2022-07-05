using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization;

public interface ISerializer<T> : IAsyncSerializer<T>
{
    void Serialize(IBufferWriter<Byte> writer, T value, CancellationToken cancellationToken = default);

    void Serialize(Stream stream, T value, CancellationToken cancellationToken = default);

    Byte[] Serialize(T value, CancellationToken cancellationToken = default);

    T? Deserialize(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken = default);

    T? Deserialize(in ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken = default);

    T? Deserialize(Stream stream, CancellationToken cancellationToken = default);
}