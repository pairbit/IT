using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataUpdaterConditional<TId, TValue> : IReadOnlyId<TId>
{
    Task<Boolean> UpdateIfEqualAsync(TValue value, TValue equalValue);

    Task<Boolean> UpdateIfNotEqualAsync(TValue value, TValue notEqualValue);

    //Task<Boolean> UpdateByIdIfContainsAsync(TValue value, TId id, params TValue[] contains);

    //Task<Boolean> UpdateByIdIfNotContainsAsync(TValue value, TId id, params TValue[] contains);
}