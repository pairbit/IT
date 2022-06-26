using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataDeleter<TId> : IReadOnlyId<TId>
{
    Task<Boolean> DeleteAsync();
}