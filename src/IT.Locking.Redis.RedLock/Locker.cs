using Microsoft.Extensions.Logging;
using RedLockNet;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking.Redis.RedLock;

public class Locker : Locking.Locker
{
    private readonly IDistributedLockFactory _factory;
    protected readonly Func<Options?>? _getOptions;
    protected readonly ILogger? _logger;

    protected override Int32? RetryMin => _getOptions?.Invoke()?.RetryMin;

    public Locker(IDistributedLockFactory factory, Func<Options?>? getOptions = null, ILogger<Locker>? logger = null)
    {
        _factory = factory;
        _getOptions = getOptions;
        _logger = logger;
    }

    #region IAsyncLocker

    public override async Task<IAsyncLocked?> TryAcquireAsync(String name, TimeSpan wait, CancellationToken cancellationToken)
    {
        var options = _getOptions?.Invoke();
        var expiryMilliseconds = options?.Expiry;
        var expiry = expiryMilliseconds.HasValue ? TimeSpan.FromMilliseconds(expiryMilliseconds.Value) : ExpiryDefault;

#if DEBUG
        if (Debugger.IsAttached) expiry = ExpiryDebug;
#endif

        var redlock = await _factory.CreateLockAsync(name, expiry).ConfigureAwait(false);
        if (redlock.IsAcquired) return new Locked(redlock);

        if (wait > TimeSpan.Zero)
        {
            var min = options?.RetryMin ?? RetryMinDefault;
            var max = (Int32)wait.TotalMilliseconds;
            var stopwatch = Stopwatch.StartNew();
            do
            {
                var retry = max <= min ? max : GetRandom().Next(min, max);

                LogDelay(name, retry);

                await Task.Delay(retry, cancellationToken).ConfigureAwait(false);

                redlock = await _factory.CreateLockAsync(name, expiry).ConfigureAwait(false);
                if (redlock.IsAcquired) return new Locked(redlock);

            } while (stopwatch.Elapsed <= wait);
        }

        return null;
    }

    #endregion IAsyncLocker

    #region ILocker

    public override ILocked? TryAcquire(String name, TimeSpan wait, CancellationToken cancellationToken)
    {
        var options = _getOptions?.Invoke();
        var expiryMilliseconds = options?.Expiry;
        var expiry = expiryMilliseconds.HasValue ? TimeSpan.FromMilliseconds(expiryMilliseconds.Value) : ExpiryDefault;

#if DEBUG
        if (Debugger.IsAttached) expiry = ExpiryDebug;
#endif

        var redlock = _factory.CreateLock(name, expiry);
        if (redlock.IsAcquired) return new Locked(redlock);

        if (wait > TimeSpan.Zero)
        {
            var min = options?.RetryMin ?? RetryMinDefault;
            var max = (Int32)wait.TotalMilliseconds;
            var stopwatch = Stopwatch.StartNew();
            do
            {
                var retry = max <= min ? max : GetRandom().Next(min, max);

                LogDelay(name, retry);

                Task.Delay(retry, cancellationToken).Wait(cancellationToken);

                redlock = _factory.CreateLock(name, expiry);
                if (redlock.IsAcquired) return new Locked(redlock);

            } while (stopwatch.Elapsed <= wait);
        }

        return null;
    }

    #endregion ILocker

    protected override void LogDelay(String name, Int32 retry)
    {
#if DEBUG
        if (_logger == null)
            Debug.WriteLine($"Lock '{name}' delay {retry}ms");
#endif
        if (_logger != null && _logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug($"Lock '{name}' delay {retry}ms");
    }
}