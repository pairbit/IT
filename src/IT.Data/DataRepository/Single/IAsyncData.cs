namespace IT.Data;

public interface IAsyncData<TId, TValue> :
    IAsyncDataReader<TId, TValue>,
    IAsyncDataUpdater<TId, TValue>,
    IAsyncDataUpdaterConditional<TId, TValue>,
    IAsyncDataReaderUpdater<TId, TValue>,
    IAsyncDataReaderUpdaterConditional<TId, TValue>,
    IAsyncDataDeleter<TId>,
    IAsyncDataDeleterConditional<TId, TValue>,
    IAsyncDataReaderDeleter<TId, TValue>,
    IAsyncDataReaderDeleterConditional<TId, TValue>
{ }