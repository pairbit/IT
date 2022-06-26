using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataRepositoryReaderUpdater<TId, TValue>
{
    Task<TValue?> GetUpdateByIdAsync(TValue value, TId id);

    Task<TValue?> GetUpdateByIdIfExistsAsync(TValue value, TId id);
}