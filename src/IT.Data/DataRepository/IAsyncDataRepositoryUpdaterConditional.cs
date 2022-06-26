using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataRepositoryUpdaterConditional<TId, TValue>
{
    Task<Boolean> UpdateByIdIfEqualAsync(TValue value, TId id, TValue equalValue);

    Task<Boolean> UpdateByIdIfNotEqualAsync(TValue value, TId id, TValue notEqualValue);

    //Task<Boolean> UpdateByIdIfContainsAsync(TValue value, TId id, params TValue[] contains);

    //Task<Boolean> UpdateByIdIfNotContainsAsync(TValue value, TId id, params TValue[] contains);
}