using IT.Serialization;
using System;

namespace IT.Data;

public interface IDataRepositoryFactory<TId, TValue>
{
    IDataRepository<TId, TValue>? GetRepository(String name, String? connectionString = null,
        Func<TId>? newId = null, ISerializer<TId>? idSerializer = null, ISerializer<TValue>? valueSerializer = null);

    IAsyncDataRepository<TId, TValue>? GetAsyncRepository(String name, String? connectionString = null,
        Func<TId>? newId = null, ISerializer<TId>? idSerializer = null, ISerializer<TValue>? valueSerializer = null);
}