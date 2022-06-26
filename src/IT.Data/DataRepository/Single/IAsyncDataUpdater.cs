using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataUpdater<TId, TValue> : IReadOnlyId<TId>
{
    Task<Boolean> UpdateAsync(TValue value);

    Task<Boolean> UpdateIfExistsAsync(TValue value);
}