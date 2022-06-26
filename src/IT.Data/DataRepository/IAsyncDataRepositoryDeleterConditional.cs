using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataRepositoryDeleterConditional<TId, TValue>
{
    Task<Boolean> DeleteByIdIfEqualAsync(TId id, TValue equalValue);

    Task<Boolean> DeleteByIdIfNotEqualAsync(TId id, TValue notEqualValue);

    //Task<Boolean> DeleteByIdIfContainsAsync(TId id, params TValue[] contains);

    //Task<Boolean> DeleteByIdIfNotContainsAsync(TId id, params TValue[] contains);
}