using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking.Redis;

public class Locker : Locking.Locker
{
    private const Int32 RetryMinDefault = 10;
    private static readonly TimeSpan ExpiryDefault = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan ExpiryDebug = TimeSpan.FromMinutes(3);

    protected readonly IDatabase _db;
    protected readonly Func<Options?>? _getOptions;
    protected readonly Func<ReadOnlyMemory<Byte>> _newId;
    protected readonly ILogger? _logger;

    public Locker(IDatabase db,
        Func<Options?>? getOptions = null,
        Func<ReadOnlyMemory<Byte>>? newId = null,
        ILogger<Locker>? logger = null)
    {
        _db = db;
        _getOptions = getOptions;
        _newId = newId ?? (() => Guid.NewGuid().ToByteArray());
        _logger = logger;
    }

    #region IAsyncLocker

    public override async Task<IAsyncLocked?> TryAcquireAsync(String name, TimeSpan wait, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (wait < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(wait));

        var options = _getOptions?.Invoke();
        var prefix = options?.Prefix;
        var expiryMilliseconds = options?.Expiry;

        RedisKey key = prefix is null ? name : $"{prefix}:{name}";
        RedisValue value = _newId();
        var expiry = expiryMilliseconds.HasValue ? TimeSpan.FromMilliseconds(expiryMilliseconds.Value) : ExpiryDefault;
#if DEBUG
        if (Debugger.IsAttached) expiry = ExpiryDebug;
#endif
        if (await _db.StringSetAsync(key, value, expiry, when: When.NotExists).ConfigureAwait(false))
            return new Locked(_db, key, value);

        if (wait > TimeSpan.Zero)
        {
            var min = options?.RetryMin ?? RetryMinDefault;
            var max = (Int32)wait.TotalMilliseconds;
            var stopwatch = Stopwatch.StartNew();
            do
            {
                var retry = max <= min ? max : GetRandom().Next(min, max);
#if DEBUG
                if (_logger == null)
                    Debug.WriteLine($"Lock '{name}' delay {retry}ms");
#endif
                if (_logger != null && _logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug($"Lock '{name}' delay {retry}ms");

                await Task.Delay(retry, cancellationToken).ConfigureAwait(false);

                if (await _db.StringSetAsync(key, value, expiry, when: When.NotExists).ConfigureAwait(false))
                    return new Locked(_db, key, value);
            } while (stopwatch.Elapsed <= wait);
        }

        return null;
    }

    #endregion IAsyncLocker

    #region ILocker

    public override ILocked? TryAcquire(String name, TimeSpan wait, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (wait < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(wait));

        var options = _getOptions?.Invoke();
        var prefix = options?.Prefix;
        var expiryMilliseconds = options?.Expiry;

        RedisKey key = prefix is null ? name : $"{prefix}:{name}";
        RedisValue value = _newId();
        var expiry = expiryMilliseconds.HasValue ? TimeSpan.FromMilliseconds(expiryMilliseconds.Value) : ExpiryDefault;
#if DEBUG
        if (Debugger.IsAttached) expiry = ExpiryDebug;
#endif
        if (_db.StringSet(key, value, expiry, when: When.NotExists))
            return new Locked(_db, key, value);

        if (wait > TimeSpan.Zero)
        {
            var min = options?.RetryMin ?? RetryMinDefault;
            var max = (Int32)wait.TotalMilliseconds;
            var stopwatch = Stopwatch.StartNew();
            do
            {
                var retry = max <= min ? max : GetRandom().Next(min, max);
#if DEBUG
                if (_logger == null) 
                    Debug.WriteLine($"Lock '{name}' delay {retry}ms");
#endif
                if (_logger != null && _logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug($"Lock '{name}' delay {retry}ms");

                Task.Delay(retry, cancellationToken).Wait(cancellationToken);

                if (_db.StringSet(key, value, expiry, when: When.NotExists))
                    return new Locked(_db, key, value);
            } while (stopwatch.Elapsed <= wait);
        }

        return null;
    }

    #endregion ILocker

    private Random GetRandom()
    {
#if NET6_0
        return Random.Shared;
#else
        return _Random.Shared;
#endif
    }
}