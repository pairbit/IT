using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataRepositoryCreator<TId, TValue>
{
    Task<Boolean> CreateByIdAsync(TValue value, TId id);

    Task<TId> CreateAsync(TValue value);
}