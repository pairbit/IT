using System;

namespace IT.Data;

public interface IEntityRepositoryDeleter<TId> : IDataRepositoryDeleter<TId>
{
    Boolean DeleteFieldById(TId id, String fieldName);

    Boolean DeleteFieldsById(TId id, params String[] fieldNames);
}