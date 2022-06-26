namespace IT.Data;

public interface IAsyncEntityRepository<TId, TEntity> :
    IAsyncCreator<TId, TEntity>,
    IAsyncFieldCreator<TId>,
    IAsyncReader<TId, TEntity>,
    IAsyncUpdater<TId, TEntity>,
    IAsyncFieldUpdater<TId>,
    //IAsyncUpdaterConditional<TId, TEntity>,
    IAsyncReaderUpdater<TId, TEntity>,
    //IAsyncReaderUpdaterConditional<TId, TEntity>,
    IAsyncDeleter<TId>,
    //IAsyncDeleterConditional<TId, TEntity>,
    IAsyncReaderDeleter<TId, TEntity>
    //IAsyncReaderDeleterConditional<TId, TEntity>
    where TEntity : new()
{ }