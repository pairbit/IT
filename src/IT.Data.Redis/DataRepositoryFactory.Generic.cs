using StackExchange.Redis;
using System;

namespace IT.Data.Redis;

public class DataRepositoryFactory<TId, TValue>
    : DataRepositoryFactory, IDataRepositoryFactory<TId, TValue>
{
    public DataRepositoryFactory(IDatabase db, Func<Options>? getOptions = null)
        : base(db, getOptions)
    {
    }

    #region IRepositoryFactory

    public IAsyncDataRepository<TId, TValue>? GetAsync(String name,
        Func<TId>? newId = null,
        Func<TId, Byte[]>? idSerialize = null,
        Func<ReadOnlyMemory<Byte>, TId?>? idDeserialize = null,
        Func<TValue, Byte[]>? valueSerialize = null,
        Func<ReadOnlyMemory<Byte>, TValue?>? valueDeserialize = null)
        => GetHashRepository(name, newId, idSerialize, idDeserialize, valueSerialize, valueDeserialize);

    public IDataRepository<TId, TValue>? Get(String name,
        Func<TId>? newId = null,
        Func<TId, Byte[]>? idSerialize = null,
        Func<ReadOnlyMemory<Byte>, TId?>? idDeserialize = null,
        Func<TValue, Byte[]>? valueSerialize = null,
        Func<ReadOnlyMemory<Byte>, TValue?>? valueDeserialize = null)
        => GetHashRepository(name, newId, idSerialize, idDeserialize, valueSerialize, valueDeserialize);

    #endregion IRepositoryFactory
}