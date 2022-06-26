using System;

namespace IT.Data;

public interface IDataRepositoryDeleter<TId>
{
    Boolean DeleteById(TId id);

    Int64 DeleteByIds(params TId[] ids);

    Boolean DeleteAll();
}