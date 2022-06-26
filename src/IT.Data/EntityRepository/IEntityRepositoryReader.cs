using System;

namespace IT.Data;

public interface IEntityRepositoryReader<TId, TEntity> : IDataRepositoryReader<TId, TEntity>
{
    //Проверять на null???
    Boolean ExistsFieldById(TId id, String fieldName);

    TField? GetFieldById<TField>(TId id, String fieldName);

    TField?[] GetFieldsById<TField>(TId id, params String[] fieldName);

    TView? GetViewById<TView>(TId id);

    TView? GetViewById<TView>(TId id, params String[] fieldNames);

    TEntity? GetById(TId id, params String[] fieldNames);
}