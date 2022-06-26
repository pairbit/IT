using System;

namespace IT.Data;

public interface IDataRepositoryCreator<TId, TValue>
{
    Boolean CreateById(TValue value, TId id);

    TId Create(TValue value);
}