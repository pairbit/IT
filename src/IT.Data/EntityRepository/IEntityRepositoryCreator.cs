using System;

namespace IT.Data;

public interface IEntityRepositoryCreator<TId, TEntity> : IDataRepositoryCreator<TId, TEntity>
{
    Boolean CreateFieldById<TField>(String fieldName, TField fieldValue, TId id);

    TId CreateField<TField>(String fieldName, TField fieldValue);

    Boolean CreateViewById<TView>(TView value, TId id);

    TId CreateView<TView>(TView value);
}