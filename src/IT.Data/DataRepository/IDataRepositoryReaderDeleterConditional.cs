namespace IT.Data;

public interface IDataRepositoryReaderDeleterConditional<TId, TValue>
{
    TValue? GetDeleteByIdIfNotEqual(TId id, TValue notEqualValue);

    //TValue? GetDeleteByIdIfContains(TId id, params TValue[] contains);

    //TValue? GetDeleteByIdIfNotContains(TId id, params TValue[] contains);
}