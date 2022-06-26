using System;

namespace IT.Data;

public interface IDataRepositoryFactory
{
    IDataRepository<TId, TValue>? Get<TId, TValue>(String name,
        Func<TId>? newId = null,
        Func<TId, Byte[]>? idSerialize = null,
        Func<ReadOnlyMemory<Byte>, TId?>? idDeserialize = null,
        Func<TValue, Byte[]>? valueSerialize = null,
        Func<ReadOnlyMemory<Byte>, TValue?>? valueDeserialize = null);

    IAsyncDataRepository<TId, TValue>? GetAsync<TId, TValue>(String name,
        Func<TId>? newId = null,
        Func<TId, Byte[]>? idSerialize = null,
        Func<ReadOnlyMemory<Byte>, TId?>? idDeserialize = null,
        Func<TValue, Byte[]>? valueSerialize = null,
        Func<ReadOnlyMemory<Byte>, TValue?>? valueDeserialize = null);
}