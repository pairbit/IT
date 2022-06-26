namespace IT.Data;

public interface IDataReaderDeleterConditional<TId, TValue> : IReadOnlyId<TId>
{
    TValue? GetDeleteIfNotEqual(TValue notEqualValue);

    //TValue? GetDeleteIfContains(params TValue[] contains);

    //TValue? GetDeleteIfNotContains(params TValue[] contains);
}