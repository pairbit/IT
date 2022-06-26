namespace IT.Data;

public interface IAsyncDataRepository<TId, TValue> :
    IAsyncDataRepositoryCreator<TId, TValue>,
    IAsyncDataRepositoryReader<TId, TValue>,
    IAsyncDataRepositoryUpdater<TId, TValue>,
    IAsyncDataRepositoryUpdaterConditional<TId, TValue>,
    IAsyncDataRepositoryReaderUpdater<TId, TValue>,
    IAsyncDataRepositoryReaderUpdaterConditional<TId, TValue>,
    IAsyncDataRepositoryDeleter<TId>,
    IAsyncDataRepositoryDeleterConditional<TId, TValue>,
    IAsyncDataRepositoryReaderDeleter<TId, TValue>,
    IAsyncDataRepositoryReaderDeleterConditional<TId, TValue>
{ }