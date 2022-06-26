using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncDataReaderDeleterConditional<TId, TValue>
{
    Task<TValue?> GetDeleteIfNotEqualAsync(TValue notEqualValue);

    //Task<TValue?> GetDeleteByIdIfContainsAsync(TId id, params TValue[] contains);

    //Task<TValue?> GetDeleteByIdIfNotContainsAsync(TId id, params TValue[] contains);
}