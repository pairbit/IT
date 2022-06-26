namespace IT.Data;

public interface IDataReaderUpdaterConditional<TId, TValue> : IReadOnlyId<TId>
{
    TValue? GetUpdateIfNotEqual(TValue value, TValue notEqualValue);

    //TValue? GetUpdateByIdIfContains(TValue value, TId id, params TValue[] contains);

    //TValue? GetUpdateByIdIfNotContains(TValue value, TId id, params TValue[] contains);
}