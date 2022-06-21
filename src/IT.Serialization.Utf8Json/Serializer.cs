using System;
using System.IO;
using System.Threading.Tasks;
using Utf8Json;

namespace IT.Serialization.Utf8Json;

public class Serializer : ISerializer, IAsyncSerializer, ITextSerializer
{
    private readonly IJsonFormatterResolver _resolver;

    public Serializer(IJsonFormatterResolver resolver)
    {
        _resolver = resolver;
    }

    #region ISerializer

    Byte[] ISerializer.Serialize<T>(T value) => JsonSerializer.Serialize(value, _resolver);

    Byte[] ISerializer.Serialize(Object value) => JsonSerializer.NonGeneric.Serialize(value, _resolver);

    Object? ISerializer.Deserialize(ReadOnlySpan<Byte> value, Type type) => JsonSerializer.NonGeneric.Deserialize(type, value.ToArray(), _resolver);

    public T? Deserialize<T>(ReadOnlySpan<Byte> value) => JsonSerializer.Deserialize<T?>(value.ToArray(), _resolver);

    #endregion ISerializer

    #region ISerializerAsync

    Task IAsyncSerializer.SerializeAsync<T>(Stream stream, T value) => JsonSerializer.SerializeAsync(stream, value, _resolver);

    Task IAsyncSerializer.SerializeAsync(Stream stream, Object value) => JsonSerializer.SerializeAsync(stream, value, _resolver);

    Task<Object?> IAsyncSerializer.DeserializeAsync(Stream value, Type type) => JsonSerializer.NonGeneric.DeserializeAsync(type, value, _resolver);

    public Task<T?> DeserializeAsync<T>(Stream value) => JsonSerializer.DeserializeAsync<T?>(value, _resolver);

    #endregion ISerializerAsync

    #region ITextSerializer

    String ITextSerializer.Serialize<T>(T value) => JsonSerializer.ToJsonString(value, _resolver);

    String ITextSerializer.Serialize(Object value) => JsonSerializer.NonGeneric.ToJsonString(value, _resolver);

    Object? ITextSerializer.Deserialize(ReadOnlySpan<Char> value, Type type) => JsonSerializer.NonGeneric.Deserialize(type, value.ToString(), _resolver);

    public T? Deserialize<T>(ReadOnlySpan<Char> value) => JsonSerializer.Deserialize<T?>(value.ToString(), _resolver);

    #endregion ITextSerializer
}