using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataRepositoryDeleter<TId>
{
    Task<Boolean> DeleteByIdAsync(TId id);

    Task<Int64> DeleteByIdsAsync(params TId[] ids);

    Task<Boolean> DeleteAllAsync();
}