using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataRepositoryReader<TId, TValue>
{
    Task<Boolean> ExistsByIdAsync(TId id);

    Task<Int64> GetSizeByIdAsync(TId id);

    Task<TValue?> GetByIdAsync(TId id);

    Task<TValue?[]> GetByIdsAsync(params TId[] ids);

    Task<Int64> CountAsync();

    Task<IdValue<TId, TValue>[]> GetAllAsync();

    Task<TId[]> GetIdsAsync();

    Task<TValue[]> GetValuesAsync();
}