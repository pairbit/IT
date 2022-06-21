using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace IT.Serialization.Json;

public class Serializer : ISerializer, IAsyncSerializer, ITextSerializer
{
    private readonly IOptions<JsonSerializerOptions> _options;

    public Serializer(IOptions<JsonSerializerOptions> options)
    {
        _options = options;
    }

    #region ISerializer

    public Byte[] Serialize<T>(T value) => JsonSerializer.SerializeToUtf8Bytes(value, _options.Value);

    public Byte[] Serialize(Object value) => JsonSerializer.SerializeToUtf8Bytes(value, value.GetType(), _options.Value);

    public Object? Deserialize(ReadOnlySpan<Byte> value, Type type) => JsonSerializer.Deserialize(value, type, _options.Value);

    public T? Deserialize<T>(ReadOnlySpan<Byte> value) => JsonSerializer.Deserialize<T>(value, _options.Value);

    #endregion ISerializer

    #region ITextSerializer

    String ITextSerializer.Serialize<T>(T value) => JsonSerializer.Serialize(value, _options.Value);

    String ITextSerializer.Serialize(Object value) => JsonSerializer.Serialize(value, value.GetType(), _options.Value);

    public Object? Deserialize(ReadOnlySpan<Char> value, Type type) => JsonSerializer.Deserialize(value, type, _options.Value);

    public T? Deserialize<T>(ReadOnlySpan<Char> value) => JsonSerializer.Deserialize<T>(value, _options.Value);

    #endregion ITextSerializer

    #region ISerializerAsync

    public Task SerializeAsync<T>(Stream stream, T value) => JsonSerializer.SerializeAsync(stream, value, _options.Value);

    public Task SerializeAsync(Stream stream, Object value) => JsonSerializer.SerializeAsync(stream, value, value.GetType(), _options.Value);

    public Task<Object?> DeserializeAsync(Stream value, Type type) => JsonSerializer.DeserializeAsync(value, type, _options.Value).AsTask();

    public Task<T?> DeserializeAsync<T>(Stream value) => JsonSerializer.DeserializeAsync<T>(value, _options.Value).AsTask();

    #endregion ISerializerAsync
}