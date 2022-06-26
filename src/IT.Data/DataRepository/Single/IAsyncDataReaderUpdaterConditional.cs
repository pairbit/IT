using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataReaderUpdaterConditional<TId, TValue> : IReadOnlyId<TId>
{
    Task<TValue?> GetUpdateIfNotEqualAsync(TValue value, TValue notEqualValue);

    //Task<TValue?> GetUpdateByIdIfContainsAsync(TValue value, TId id, params TValue[] contains);

    //Task<TValue?> GetUpdateByIdIfNotContainsAsync(TValue value, TId id, params TValue[] contains);
}