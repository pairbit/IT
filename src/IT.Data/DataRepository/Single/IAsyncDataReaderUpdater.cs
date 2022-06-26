using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataReaderUpdater<TId, TValue> : IReadOnlyId<TId>
{
    Task<TValue?> GetUpdateAsync(TValue value);

    Task<TValue?> GetUpdateIfExistsAsync(TValue value);
}