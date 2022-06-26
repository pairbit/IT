using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataReaderDeleter<TId, TValue> : IReadOnlyId<TId>
{
    Task<TValue?> GetDeleteAsync();
}