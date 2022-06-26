using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace IT.Data.Redis;

public class HashRepository<TId, TValue> : HashRepository,
    IDataRepository<TId, TValue>, IAsyncDataRepository<TId, TValue>
{
    private readonly Func<TId>? _newId;
    private readonly Func<TId, Byte[]>? _idSerialize;
    private readonly Func<ReadOnlyMemory<Byte>, TId?>? _idDeserialize;
    private readonly Func<TValue, Byte[]>? _valueSerialize;
    private readonly Func<ReadOnlyMemory<Byte>, TValue?>? _valueDeserialize;

    public HashRepository(IDatabase db, String hash,
        Func<TId>? newId = null,
        Func<TId, Byte[]>? idSerialize = null,
        Func<ReadOnlyMemory<Byte>, TId?>? idDeserialize = null,
        Func<TValue, Byte[]>? valueSerialize = null,
        Func<ReadOnlyMemory<Byte>, TValue?>? valueDeserialize = null)
        : base(db, hash)
    {
        _newId = newId;

        _idSerialize = idSerialize;
        _idDeserialize = idDeserialize;

        _valueSerialize = valueSerialize;
        _valueDeserialize = valueDeserialize;
    }

    #region IAsyncCreator

    public Task<Boolean> CreateByIdAsync(TValue value, TId id)
    {
        return _db.HashSetAsync(_hash, IdToRedis(id), ValueToRedis(value), When.NotExists);
    }

    public async Task<TId> CreateAsync(TValue value)
    {
        if (_newId is null) throw new NotSupportedException();

        TId id = _newId.Invoke();

        var created = await _db.HashSetAsync(_hash, IdToRedis(id), ValueToRedis(value), When.NotExists).ConfigureAwait(false);

        if (!created) throw new InvalidOperationException();

        return id;
    }

    #endregion IAsyncCreator

    #region ICreator

    public Boolean CreateById(TValue value, TId id)
    {
        return _db.HashSet(_hash, IdToRedis(id), ValueToRedis(value), When.NotExists);
    }

    public TId Create(TValue value)
    {
        if (_newId is null) throw new NotSupportedException();

        TId id = _newId.Invoke();

        var created = _db.HashSet(_hash, IdToRedis(id), ValueToRedis(value), When.NotExists);

        if (!created) throw new InvalidOperationException();

        return id;
    }

    #endregion ICreator

    #region IAsyncReader

    public Task<Boolean> ExistsByIdAsync(TId id)
    {
        return _db.HashExistsAsync(_hash, IdToRedis(id));
    }

    public Task<Int64> GetSizeByIdAsync(TId id) => _db.HashStringLengthAsync(_hash, IdToRedis(id));

    public async Task<TValue?> GetByIdAsync(TId id)
    {
        return ValueFromRedis(await _db.HashGetAsync(_hash, IdToRedis(id)).ConfigureAwait(false));
    }

    public async Task<TValue?[]> GetByIdsAsync(params TId[] ids)
    {
        if (ids is null) throw new ArgumentNullException(nameof(ids));
        if (ids.Length == 0) throw new ArgumentException("is empty", nameof(ids));

        return (await _db.HashGetAsync(_hash, ids.To(IdToRedis)).ConfigureAwait(false)).To(ValueFromRedis);
    }

    public Task<Int64> CountAsync()
    {
        return _db.HashLengthAsync(_hash);
    }

    public async Task<IdValue<TId, TValue>[]> GetAllAsync()
    {
        return (await _db.HashGetAllAsync(_hash).ConfigureAwait(false)).To(FromRedis);
    }

    public async Task<TId[]> GetIdsAsync()
    {
        return (await _db.HashKeysAsync(_hash).ConfigureAwait(false)).To(IdFromRedis);
    }

    public async Task<TValue[]> GetValuesAsync()
    {
        return (await _db.HashValuesAsync(_hash).ConfigureAwait(false)).To(ValueFromRedis);
    }

    #endregion IAsyncReader

    #region IReader

    public Boolean ExistsById(TId id)
    {
        return _db.HashExists(_hash, IdToRedis(id));
    }

    public Int64 GetSizeById(TId id) => _db.HashStringLength(_hash, IdToRedis(id));

    public TValue? GetById(TId id)
    {
        return ValueFromRedis(_db.HashGet(_hash, IdToRedis(id)));
    }

    public TValue?[] GetByIds(params TId[] ids)
    {
        if (ids is null) throw new ArgumentNullException(nameof(ids));
        if (ids.Length == 0) throw new ArgumentException("is empty", nameof(ids));

        return _db.HashGet(_hash, ids.To(IdToRedis)).To(ValueFromRedis);
    }

    public Int64 Count()
    {
        return _db.HashLength(_hash);
    }

    public IdValue<TId, TValue>[] GetAll()
    {
        return _db.HashGetAll(_hash).To(FromRedis);
    }

    public TId[] GetIds()
    {
        return _db.HashKeys(_hash).To(IdFromRedis);
    }

    public TValue[] GetValues()
    {
        return _db.HashValues(_hash).To(ValueFromRedis);
    }

    #endregion IReader

    #region IAsyncUpdater

    public Task<Boolean> UpdateByIdAsync(TValue value, TId id)
    {
        return _db.HashSetAsync(_hash, IdToRedis(id), ValueToRedis(value), When.Always);
    }

    public Task<Boolean> UpdateByIdIfExistsAsync(TValue value, TId id)
        => _db.HashSetIfExistsAsync(_hash, IdToRedis(id), ValueToRedis(value));

    public Task UpdateByIdsAsync(params IdValue<TId, TValue>[] data)
    {
        return _db.HashSetAsync(_hash, data.To(ToRedis));
    }

    public Task UpdateByIdsIfExistsAsync(params IdValue<TId, TValue>[] data)
    {
        throw new NotSupportedException();
    }

    #endregion IAsyncUpdater

    #region IUpdater

    public Boolean UpdateById(TValue value, TId id)
        => _db.HashSet(_hash, IdToRedis(id), ValueToRedis(value), When.Always);

    public Boolean UpdateByIdIfExists(TValue value, TId id)
        => _db.HashSetIfExists(_hash, IdToRedis(id), ValueToRedis(value));

    public void UpdateByIds(params IdValue<TId, TValue>[] data)
        => _db.HashSet(_hash, data.To(ToRedis));

    public void UpdateByIdsIfExists(params IdValue<TId, TValue>[] data)
        => throw new NotSupportedException();

    #endregion IUpdater

    #region IAsyncUpdaterConditional

    public Task<Boolean> UpdateByIdIfEqualAsync(TValue value, TId id, TValue equalValue)
        => _db.HashSetIfEqualAsync(_hash, IdToRedis(id), ValueToRedis(value), ValueToRedis(equalValue));

    public Task<Boolean> UpdateByIdIfNotEqualAsync(TValue value, TId id, TValue notEqualValue)
        => _db.HashSetIfNotEqualAsync(_hash, IdToRedis(id), ValueToRedis(value), ValueToRedis(notEqualValue));

    #endregion IAsyncUpdaterConditional

    #region IUpdaterConditional

    public Boolean UpdateByIdIfEqual(TValue value, TId id, TValue equalValue)
        => _db.HashSetIfEqual(_hash, IdToRedis(id), ValueToRedis(value), ValueToRedis(equalValue));

    public Boolean UpdateByIdIfNotEqual(TValue value, TId id, TValue notEqualValue)
        => _db.HashSetIfNotEqual(_hash, IdToRedis(id), ValueToRedis(value), ValueToRedis(notEqualValue));

    #endregion IUpdaterConditional

    #region IAsyncReaderUpdater

    public async Task<TValue?> GetUpdateByIdAsync(TValue value, TId id)
        => ValueFromRedis(await _db.HashGetSetAsync(_hash, IdToRedis(id), ValueToRedis(value)).ConfigureAwait(false));

    public Task<TValue?> GetUpdateByIdIfExistsAsync(TValue value, TId id)
        => throw new NotImplementedException();

    #endregion IAsyncReaderUpdater

    #region IReaderUpdater

    public TValue? GetUpdateById(TValue value, TId id)
        => ValueFromRedis(_db.HashGetSet(_hash, IdToRedis(id), ValueToRedis(value)));

    public TValue? GetUpdateByIdIfExists(TValue value, TId id)
        => throw new NotImplementedException();

    #endregion IReaderUpdater

    #region IAsyncReaderUpdaterConditional

    public Task<TValue?> GetUpdateByIdIfNotEqualAsync(TValue value, TId id, TValue notEqualValue)
        => throw new NotImplementedException();

    #endregion IAsyncReaderUpdaterConditional

    #region IReaderUpdaterConditional

    public TValue? GetUpdateByIdIfNotEqual(TValue value, TId id, TValue notEqualValue)
        => throw new NotImplementedException();

    #endregion IReaderUpdaterConditional

    #region IAsyncDeleter

    public Task<Boolean> DeleteByIdAsync(TId id)
    {
        return _db.HashDeleteAsync(_hash, IdToRedis(id));
    }

    public Task<Int64> DeleteByIdsAsync(params TId[] ids)
    {
        if (ids is null) throw new ArgumentNullException(nameof(ids));
        if (ids.Length == 0) throw new ArgumentException("is empty", nameof(ids));

        return _db.HashDeleteAsync(_hash, ids.To(IdToRedis));
    }

    public Task<Boolean> DeleteAllAsync()
    {
        return _db.KeyDeleteAsync(_hash);
    }

    #endregion IAsyncDeleter

    #region IDeleter

    public Boolean DeleteById(TId id)
    {
        return _db.HashDelete(_hash, IdToRedis(id));
    }

    public Int64 DeleteByIds(params TId[] ids)
    {
        if (ids is null) throw new ArgumentNullException(nameof(ids));
        if (ids.Length == 0) throw new ArgumentException("is empty", nameof(ids));

        return _db.HashDelete(_hash, ids.To(IdToRedis));
    }

    public Boolean DeleteAll()
    {
        return _db.KeyDelete(_hash);
    }

    #endregion IDeleter

    #region IAsyncDeleterConditional

    public Task<Boolean> DeleteByIdIfEqualAsync(TId id, TValue equalValue)
        => _db.HashDeleteIfEqualAsync(_hash, IdToRedis(id), ValueToRedis(equalValue));

    public Task<Boolean> DeleteByIdIfNotEqualAsync(TId id, TValue notEqualValue)
        => _db.HashDeleteIfNotEqualAsync(_hash, IdToRedis(id), ValueToRedis(notEqualValue));

    #endregion IAsyncDeleterConditional

    #region IDeleterConditional

    public Boolean DeleteByIdIfEqual(TId id, TValue equalValue)
        => _db.HashDeleteIfEqual(_hash, IdToRedis(id), ValueToRedis(equalValue));

    public Boolean DeleteByIdIfNotEqual(TId id, TValue notEqualValue)
        => _db.HashDeleteIfNotEqual(_hash, IdToRedis(id), ValueToRedis(notEqualValue));

    #endregion IDeleterConditional

    #region IAsyncReaderDeleter

    public async Task<TValue?> GetDeleteByIdAsync(TId id)
        => ValueFromRedis(await _db.HashGetDeleteAsync(_hash, IdToRedis(id)).ConfigureAwait(false));

    #endregion IAsyncReaderDeleter

    #region IReaderDeleter

    public TValue? GetDeleteById(TId id)
        => ValueFromRedis(_db.HashGetDelete(_hash, IdToRedis(id)));

    #endregion IReaderDeleter

    #region IAsyncReaderDeleterConditional

    public Task<TValue?> GetDeleteByIdIfNotEqualAsync(TId id, TValue notEqualValue)
        => throw new NotImplementedException();

    #endregion IAsyncReaderDeleterConditional

    #region IReaderDeleterConditional

    public TValue? GetDeleteByIdIfNotEqual(TId id, TValue notEqualValue)
        => throw new NotImplementedException();

    #endregion IReaderDeleterConditional

    #region Private

    protected HashEntry ToRedis(IdValue<TId, TValue> pair) => new(IdToRedis(pair.Id), ValueToRedis(pair.Value));

    protected IdValue<TId, TValue> FromRedis(HashEntry entry) => new(IdFromRedis(entry.Name), ValueFromRedis(entry.Value));

    protected RedisValue IdToRedis(TId id) => id.ToRedisValue(_idSerialize);

    protected RedisValue ValueToRedis(TValue value) => value.ToRedisValue(_valueSerialize);

    protected TId? IdFromRedis(RedisValue value) => value.TryTo(_idDeserialize);

    protected TValue? ValueFromRedis(RedisValue value) => value.TryTo(_valueDeserialize);

    #endregion Private
}