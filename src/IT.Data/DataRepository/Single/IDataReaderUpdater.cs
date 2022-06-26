namespace IT.Data;

public interface IDataReaderUpdater<TId, TValue> : IReadOnlyId<TId>
{
    TValue? GetUpdate(TValue value);

    TValue? GetUpdateIfExists(TValue value);
}