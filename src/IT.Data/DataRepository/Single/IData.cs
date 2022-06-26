namespace IT.Data;

public interface IData<TId, TValue> :
    IDataReader<TId, TValue>,
    IDataUpdater<TId, TValue>,
    IDataUpdaterConditional<TId, TValue>,
    IDataReaderUpdater<TId, TValue>,
    IDataReaderUpdaterConditional<TId, TValue>,
    IDataDeleter<TId>,
    IDataDeleterConditional<TId, TValue>,
    IDataReaderDeleter<TId, TValue>,
    IDataReaderDeleterConditional<TId, TValue>
{ }