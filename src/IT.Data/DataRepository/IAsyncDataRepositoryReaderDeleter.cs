using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataRepositoryReaderDeleter<TId, TValue>
{
    Task<TValue?> GetDeleteByIdAsync(TId id);
}