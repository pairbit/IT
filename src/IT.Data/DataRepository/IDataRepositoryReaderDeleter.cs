namespace IT.Data;

public interface IDataRepositoryReaderDeleter<TId, TValue>
{
    TValue? GetDeleteById(TId id);
}