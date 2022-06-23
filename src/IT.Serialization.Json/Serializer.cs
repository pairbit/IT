using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace IT.Serialization.Json;

public class Serializer : ISerializer, IAsyncSerializer, ITextSerializer
{
    private readonly Func<JsonSerializerOptions>? _getOptions;

    public Serializer(Func<JsonSerializerOptions>? getOptions = null)
    {
        _getOptions = getOptions;
    }

    #region ISerializer

    public Byte[] Serialize<T>(T value) => JsonSerializer.SerializeToUtf8Bytes(value, _getOptions?.Invoke());

    public Byte[] Serialize(Object value) => JsonSerializer.SerializeToUtf8Bytes(value, value.GetType(), _getOptions?.Invoke());

    public Object? Deserialize(ReadOnlySpan<Byte> value, Type type) => JsonSerializer.Deserialize(value, type, _getOptions?.Invoke());

    public T? Deserialize<T>(ReadOnlySpan<Byte> value) => JsonSerializer.Deserialize<T>(value, _getOptions?.Invoke());

    #endregion ISerializer

    #region ITextSerializer

    String ITextSerializer.Serialize<T>(T value) => JsonSerializer.Serialize(value, _getOptions?.Invoke());

    String ITextSerializer.Serialize(Object value) => JsonSerializer.Serialize(value, value.GetType(), _getOptions?.Invoke());

    public Object? Deserialize(ReadOnlySpan<Char> value, Type type) => JsonSerializer.Deserialize(value, type, _getOptions?.Invoke());

    public T? Deserialize<T>(ReadOnlySpan<Char> value) => JsonSerializer.Deserialize<T>(value, _getOptions?.Invoke());

    #endregion ITextSerializer

    #region ISerializerAsync

    public Task SerializeAsync<T>(Stream stream, T value) => JsonSerializer.SerializeAsync(stream, value, _getOptions?.Invoke());

    public Task SerializeAsync(Stream stream, Object value) => JsonSerializer.SerializeAsync(stream, value, value.GetType(), _getOptions?.Invoke());

    public Task<Object?> DeserializeAsync(Stream value, Type type) => JsonSerializer.DeserializeAsync(value, type, _getOptions?.Invoke()).AsTask();

    public Task<T?> DeserializeAsync<T>(Stream value) => JsonSerializer.DeserializeAsync<T>(value, _getOptions?.Invoke()).AsTask();

    #endregion ISerializerAsync
}