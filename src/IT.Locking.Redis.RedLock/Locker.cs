using Microsoft.Extensions.Logging;
using RedLockNet;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking.Redis.RedLock;

public class Locker : Locking.Locker
{
    private const Int32 RetryMillisecondsDefault = 100;
    private static readonly TimeSpan ExpiryDefault = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan ExpiryDebug = TimeSpan.FromMinutes(3);

    private readonly IDistributedLockFactory _factory;
    protected readonly Func<Options?>? _getOptions;
    protected readonly ILogger? _logger;

    public Locker(IDistributedLockFactory factory, Func<Options?>? getOptions = null, ILogger<Locker>? logger = null)
    {
        _factory = factory;
        _getOptions = getOptions;
        _logger = logger;
    }

    #region IAsyncLocker

    public override async Task<IAsyncLocked?> TryAcquireAsync(String name, TimeSpan wait, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (wait < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(wait));

        var options = _getOptions?.Invoke();
        var expiryMilliseconds = options?.ExpiryMilliseconds;
        var expiry = expiryMilliseconds.HasValue ? TimeSpan.FromMilliseconds(expiryMilliseconds.Value) : ExpiryDefault;

#if DEBUG
        if (Debugger.IsAttached) expiry = ExpiryDebug;
#endif

        var redlock = await _factory.CreateLockAsync(name, expiry).ConfigureAwait(false);
        if (redlock.IsAcquired) return new Locked(redlock);

        if (wait > TimeSpan.Zero)
        {
            var retry = options?.RetryMilliseconds ?? RetryMillisecondsDefault;
            var stopwatch = Stopwatch.StartNew();
            do
            {
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
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (wait < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(wait));

        var options = _getOptions?.Invoke();
        var expiryMilliseconds = options?.ExpiryMilliseconds;
        var expiry = expiryMilliseconds.HasValue ? TimeSpan.FromMilliseconds(expiryMilliseconds.Value) : ExpiryDefault;

#if DEBUG
        if (Debugger.IsAttached) expiry = ExpiryDebug;
#endif

        var redlock = _factory.CreateLock(name, expiry);
        if (redlock.IsAcquired) return new Locked(redlock);

        if (wait > TimeSpan.Zero)
        {
            var retry = options?.RetryMilliseconds ?? RetryMillisecondsDefault;
            var stopwatch = Stopwatch.StartNew();
            do
            {
                Task.Delay(retry, cancellationToken).Wait(cancellationToken);

                redlock = _factory.CreateLock(name, expiry);
                if (redlock.IsAcquired) return new Locked(redlock);

            } while (stopwatch.Elapsed <= wait);
        }

        return null;
    }

    #endregion ILocker
}