using System;

namespace IT.Data;

public interface IEntityRepositoryUpdater<TId, TEntity> : IDataRepositoryUpdater<TId, TEntity>
{
    Boolean UpdateFieldById<TField>(String fieldName, TField fieldValue, TId id);

    Boolean UpdateViewById<TView>(TView value, TId id);

    Boolean UpdateViewById<TView>(TView value, TId id, params String[] fieldNames);

    Boolean UpdateById(TEntity value, TId id, params String[] fieldNames);
}