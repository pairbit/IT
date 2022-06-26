namespace IT.Data;

public interface IDataRepositoryReaderUpdater<TId, TValue>
{
    TValue? GetUpdateById(TValue value, TId id);

    TValue? GetUpdateByIdIfExists(TValue value, TId id);
}