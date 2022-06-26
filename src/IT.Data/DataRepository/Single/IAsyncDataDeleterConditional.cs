using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataDeleterConditional<TId, TValue> : IReadOnlyId<TId>
{
    Task<Boolean> DeleteIfEqualAsync(TValue equalValue);

    Task<Boolean> DeleteIfNotEqualAsync(TValue notEqualValue);

    //Task<Boolean> DeleteByIdIfContainsAsync(TId id, params TValue[] contains);

    //Task<Boolean> DeleteByIdIfNotContainsAsync(TId id, params TValue[] contains);
}