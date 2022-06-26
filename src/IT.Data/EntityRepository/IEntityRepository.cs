namespace IT.Data;

public interface IEntityRepository<TId, TEntity> :
    IEntityRepositoryCreator<TId, TEntity>,
    IEntityRepositoryReader<TId, TEntity>,
    IEntityRepositoryUpdater<TId, TEntity>,
    //IUpdaterConditional<TId, TEntity>,
    IDataRepositoryReaderUpdater<TId, TEntity>,
    //IReaderUpdaterConditional<TId, TEntity>,
    IEntityRepositoryDeleter<TId>,
    //IDeleterConditional<TId, TEntity>,
    IDataRepositoryReaderDeleter<TId, TEntity>
    //IReaderDeleterConditional<TId, TEntity>
    where TEntity : new()
{ }