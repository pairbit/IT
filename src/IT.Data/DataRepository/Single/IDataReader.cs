using System;

namespace IT.Data;

public interface IDataReader<TId, TValue> : IReadOnlyId<TId>
{
    Boolean Exists();

    Int64 GetSize();

    TValue? Get();
}