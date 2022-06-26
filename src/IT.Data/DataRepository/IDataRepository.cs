namespace IT.Data;

public interface IDataRepository<TId, TValue> :
    IDataRepositoryCreator<TId, TValue>,
    IDataRepositoryReader<TId, TValue>,
    IDataRepositoryUpdater<TId, TValue>,
    IDataRepositoryUpdaterConditional<TId, TValue>,
    IDataRepositoryReaderUpdater<TId, TValue>,
    IDataRepositoryReaderUpdaterConditional<TId, TValue>,
    IDataRepositoryDeleter<TId>,
    IDataRepositoryDeleterConditional<TId, TValue>,
    IDataRepositoryReaderDeleter<TId, TValue>,
    IDataRepositoryReaderDeleterConditional<TId, TValue>
{ }