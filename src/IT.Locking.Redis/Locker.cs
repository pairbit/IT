using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IT.Locking.Redis;

public class Locker : ILocker
{
    private static readonly TimeSpan ExpiryDebug = TimeSpan.FromMinutes(3);
    protected readonly IDatabase _db;
    protected readonly String? _prefix;
    protected readonly Func<ReadOnlyMemory<Byte>> _newId;

    public Locker(IDatabase db, Func<Options>? getOptions = null, Func<ReadOnlyMemory<Byte>>? newId = null)
    {
        _db = db;
        _prefix = getOptions?.Invoke()?.Prefix;
        _newId = newId ?? (() => Guid.NewGuid().ToByteArray());
    }

    #region ILocker

    public ILock? Lock(String resource, TimeSpan expiry)
    {
        if (resource is null) throw new ArgumentNullException(nameof(resource));
        if (resource.Length == 0) throw new ArgumentException("is empty", nameof(resource));

        RedisKey key = _prefix is null ? resource : $"{_prefix}:{resource}";
        RedisValue value = _newId();

        return _db.StringSet(key, value, Debugger.IsAttached ? ExpiryDebug : expiry, when: When.NotExists) ? new _Lock(_db, key, value) : null;
    }

    //public ILock? Lock(String resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default)
    //{
    //    throw new NotImplementedException();
    //}

    #endregion ILocker

    #region IAsyncLocker

    public async Task<ILock?> LockAsync(String resource, TimeSpan expiry)
    {
        if (resource is null) throw new ArgumentNullException(nameof(resource));
        if (resource.Length == 0) throw new ArgumentException("is empty", nameof(resource));

        RedisKey key = _prefix is null ? resource : $"{_prefix}:{resource}";
        RedisValue value = _newId();

        return await _db.StringSetAsync(key, value, Debugger.IsAttached ? ExpiryDebug : expiry, when: When.NotExists).ConfigureAwait(false)
            ? new _Lock(_db, key, value) : null;
    }

    //public Task<ILock?> LockAsync(String resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default)
    //{
    //    throw new NotImplementedException();
    //}

    #endregion IAsyncLocker
}