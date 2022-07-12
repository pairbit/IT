using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking.Redis;

public class Locker : Locking.Locker
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

    #region IAsyncLocker

    public override async Task<ILock?> TryLockAsync(String name, TimeSpan expiry, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));

        RedisKey key = _prefix is null ? name : $"{_prefix}:{name}";
        RedisValue value = _newId();
        if (Debugger.IsAttached) expiry = ExpiryDebug;

        return await _db.StringSetAsync(key, value, expiry, when: When.NotExists).ConfigureAwait(false) ? new Lock(_db, key, value) : null;
    }

    #endregion IAsyncLocker

    #region ILocker

    public override ILock? TryLock(String name, TimeSpan expiry, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));

        RedisKey key = _prefix is null ? name : $"{_prefix}:{name}";
        RedisValue value = _newId();
        if (Debugger.IsAttached) expiry = ExpiryDebug;

        return _db.StringSet(key, value, expiry, when: When.NotExists) ? new Lock(_db, key, value) : null;
    }

    #endregion ILocker
}