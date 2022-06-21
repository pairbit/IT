using System;
using System.IO;
using System.Threading.Tasks;

namespace IT.Serialization;

public interface IAsyncSerializer
{
    Task SerializeAsync<T>(Stream stream, T? value);

    Task SerializeAsync(Stream stream, Object value);

    Task<Object> DeserializeAsync(Stream value, Type type);

    Task<T?> DeserializeAsync<T>(Stream value);
}