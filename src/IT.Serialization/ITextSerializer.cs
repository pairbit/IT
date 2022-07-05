using System;
using System.Buffers;
using System.Threading;

namespace IT.Serialization;

public interface ITextSerializer : ISerializer
{
    #region Generic

    void Serialize<T>(IBufferWriter<Char> writer, T value, CancellationToken cancellationToken = default);

    String SerializeToText<T>(T value, CancellationToken cancellationToken = default);

    T? Deserialize<T>(ReadOnlyMemory<Char> memory, CancellationToken cancellationToken = default);

    T? Deserialize<T>(in ReadOnlySequence<Char> sequence, CancellationToken cancellationToken = default);

    #endregion Generic

    #region NonGeneric

    void Serialize(Type type, IBufferWriter<Char> writer, Object value, CancellationToken cancellationToken = default);

    String SerializeToText(Type type, Object value, CancellationToken cancellationToken = default);

    Object? Deserialize(Type type, ReadOnlyMemory<Char> memory, CancellationToken cancellationToken = default);

    Object? Deserialize(Type type, ReadOnlySequence<Char> sequence, CancellationToken cancellationToken = default);

    #endregion NonGeneric
}