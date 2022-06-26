namespace IT.Data;

public interface IDataRepositoryReaderUpdaterConditional<TId, TValue>
{
    TValue? GetUpdateByIdIfNotEqual(TValue value, TId id, TValue notEqualValue);

    //TValue? GetUpdateByIdIfContains(TValue value, TId id, params TValue[] contains);

    //TValue? GetUpdateByIdIfNotContains(TValue value, TId id, params TValue[] contains);
}