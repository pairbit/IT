using System;

namespace IT.Data;

public interface IDataDeleter<TId> : IReadOnlyId<TId>
{
    Boolean Delete();
}