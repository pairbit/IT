using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataRepositoryReaderDeleterConditional<TId, TValue>
{
    Task<TValue?> GetDeleteByIdIfNotEqualAsync(TId id, TValue notEqualValue);

    //Task<TValue?> GetDeleteByIdIfContainsAsync(TId id, params TValue[] contains);

    //Task<TValue?> GetDeleteByIdIfNotContainsAsync(TId id, params TValue[] contains);
}