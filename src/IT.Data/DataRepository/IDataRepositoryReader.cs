using System;

namespace IT.Data;

public interface IDataRepositoryReader<TId, TValue>
{
    Boolean ExistsById(TId id);

    Int64 GetSizeById(TId id);

    TValue? GetById(TId id);

    TValue?[] GetByIds(params TId[] ids);

    Int64 Count();

    IdValue<TId, TValue>[] GetAll();

    TId[] GetIds();

    TValue[] GetValues();
}