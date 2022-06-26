using IT.Serialization;
using System;

namespace IT.Data;

public interface IDataRepositoryFactory
{
    IDataRepository<TId, TValue>? GetRepository<TId, TValue>(String name, String? connectionString = null,
        Func<TId>? newId = null, ISerializer<TId>? idSerializer = null, ISerializer<TValue>? valueSerializer = null);

    IAsyncDataRepository<TId, TValue>? GetAsyncRepository<TId, TValue>(String name, String? connectionString = null,
        Func<TId>? newId = null, ISerializer<TId>? idSerializer = null, ISerializer<TValue>? valueSerializer = null);
}