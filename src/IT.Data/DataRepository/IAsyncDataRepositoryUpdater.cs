using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataRepositoryUpdater<TId, TValue>
{
    Task<Boolean> UpdateByIdAsync(TValue value, TId id);

    Task<Boolean> UpdateByIdIfExistsAsync(TValue value, TId id);

    Task UpdateByIdsAsync(params IdValue<TId, TValue>[] data);

    Task UpdateByIdsIfExistsAsync(params IdValue<TId, TValue>[] data);
}