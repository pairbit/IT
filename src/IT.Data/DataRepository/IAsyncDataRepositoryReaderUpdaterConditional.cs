using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataRepositoryReaderUpdaterConditional<TId, TValue>
{
    Task<TValue?> GetUpdateByIdIfNotEqualAsync(TValue value, TId id, TValue notEqualValue);

    //Task<TValue?> GetUpdateByIdIfContainsAsync(TValue value, TId id, params TValue[] contains);

    //Task<TValue?> GetUpdateByIdIfNotContainsAsync(TValue value, TId id, params TValue[] contains);
}