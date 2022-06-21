using System.IO;
using System.Threading.Tasks;

namespace IT.Serialization;

public interface IAsyncSerializer<T>
{
    Task SerializeAsync(Stream stream, T? value);

    Task<T?> DeserializeAsync(Stream value);
}