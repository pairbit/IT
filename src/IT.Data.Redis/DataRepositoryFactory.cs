using StackExchange.Redis;
using System;

namespace IT.Data.Redis;

public class DataRepositoryFactory : IDataRepositoryFactory
{
    protected readonly IDatabase _db;
    protected readonly Func<Options>? _getOptions;

    public DataRepositoryFactory(IDatabase db, Func<Options>? getOptions = null)
    {
        _db = db;
        _getOptions = getOptions;
    }

    #region IRepositoryFactory

    public IAsyncDataRepository<TId, TValue>? GetAsync<TId, TValue>(String name,
        Func<TId>? newId = null,
        Func<TId, Byte[]>? idSerialize = null,
        Func<ReadOnlyMemory<Byte>, TId?>? idDeserialize = null,
        Func<TValue, Byte[]>? valueSerialize = null,
        Func<ReadOnlyMemory<Byte>, TValue?>? valueDeserialize = null)
        => GetHashRepository(name, newId, idSerialize, idDeserialize, valueSerialize, valueDeserialize);

    public IDataRepository<TId, TValue>? Get<TId, TValue>(String name,
        Func<TId>? newId = null,
        Func<TId, Byte[]>? idSerialize = null,
        Func<ReadOnlyMemory<Byte>, TId?>? idDeserialize = null,
        Func<TValue, Byte[]>? valueSerialize = null,
        Func<ReadOnlyMemory<Byte>, TValue?>? valueDeserialize = null)
        => GetHashRepository(name, newId, idSerialize, idDeserialize, valueSerialize, valueDeserialize);

    #endregion IRepositoryFactory

    protected HashRepository<TId, TValue>? GetHashRepository<TId, TValue>(String name,
        Func<TId>? newId = null,
        Func<TId, Byte[]>? idSerialize = null,
        Func<ReadOnlyMemory<Byte>, TId?>? idDeserialize = null,
        Func<TValue, Byte[]>? valueSerialize = null,
        Func<ReadOnlyMemory<Byte>, TValue?>? valueDeserialize = null)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (name.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(name));

        return new HashRepository<TId, TValue>(_db, GetHashName(name), newId, idSerialize, idDeserialize, valueSerialize, valueDeserialize);
    }

    protected String GetHashName(String key)
    {
        var options = _getOptions?.Invoke();
        var prefix = options?.Prefix;
        return prefix is null || prefix.Length == 0 ? key : prefix + ":" + key;
    }
}