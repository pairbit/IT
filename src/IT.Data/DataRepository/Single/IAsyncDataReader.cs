using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataReader<TId, TValue> : IReadOnlyId<TId>
{
    Task<Boolean> ExistsAsync();

    Task<Int64> GetSizeAsync();

    Task<TValue?> GetAsync();
}