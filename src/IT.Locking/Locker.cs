using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking;

public abstract class Locker : ILocker
{
    protected const Int32 RetryMinDefault = 10;
    protected static readonly TimeSpan ExpiryDefault = TimeSpan.FromSeconds(30);
    protected static readonly TimeSpan ExpiryDebug = TimeSpan.FromMinutes(3);

    protected virtual Int32? RetryMin => null;

    #region IAsyncLocker

    public virtual IAsyncLock NewAsyncLock(String name)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));

        return new Lock(name, this);
    }

    Task<IAsyncLocked?> IAsyncLocker.TryAcquireAsync(String name, TimeSpan wait, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (wait < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(wait));
        return TryAcquireAsync(name, wait, cancellationToken);
    }

    public abstract Task<IAsyncLocked?> TryAcquireAsync(String name, TimeSpan wait, CancellationToken cancellationToken);

    public virtual async Task<T?> TryAcquireWithCheckAsync<T>(String name,
        Func<CancellationToken, Task<T?>> checkAsync, Func<CancellationToken, Task<T>> getResultAsync,
        TimeSpan wait, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (wait < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(wait));
        if (checkAsync is null) throw new ArgumentNullException(nameof(checkAsync));
        if (getResultAsync is null) throw new ArgumentNullException(nameof(getResultAsync));

        var comparer = EqualityComparer<T?>.Default;

        var result = await checkAsync(cancellationToken).ConfigureAwait(false);

        if (!comparer.Equals(result, default)) return result!;

        await using (var locked = await TryAcquireAsync(name, default, cancellationToken).ConfigureAwait(false))
        {
            if (locked != null)
            {
                result = await checkAsync(cancellationToken).ConfigureAwait(false);

                if (comparer.Equals(result, default))
                    result = await getResultAsync(cancellationToken).ConfigureAwait(false);

                return result!;
            }
        }

        if (wait > TimeSpan.Zero)
        {
            var min = RetryMin ?? RetryMinDefault;
            var max = (Int32)wait.TotalMilliseconds;
            var stopwatch = Stopwatch.StartNew();

            do
            {
                var retry = max <= min ? max : GetRandom().Next(min, max);

                LogDelay(name, retry);

                await Task.Delay(retry, cancellationToken).ConfigureAwait(false);

                result = await checkAsync(cancellationToken).ConfigureAwait(false);

                if (!comparer.Equals(result, default)) return result!;

                await using var locked = await TryAcquireAsync(name, default, cancellationToken).ConfigureAwait(false);
                if (locked != null)
                {
                    result = await checkAsync(cancellationToken).ConfigureAwait(false);

                    if (comparer.Equals(result, default))
                        result = await getResultAsync(cancellationToken).ConfigureAwait(false);

                    return result!;
                }
            } while (stopwatch.Elapsed <= wait);
        }

        return default;
    }

    public virtual async Task<Boolean> TryAcquireWithCheckAsync(String name,
        Func<CancellationToken, Task<Boolean>> checkAsync, Func<CancellationToken, Task> doAsync,
        TimeSpan wait, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (wait < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(wait));
        if (checkAsync is null) throw new ArgumentNullException(nameof(checkAsync));
        if (doAsync is null) throw new ArgumentNullException(nameof(doAsync));

        if (await checkAsync(cancellationToken).ConfigureAwait(false)) return true;

        await using (var locked = await TryAcquireAsync(name, default, cancellationToken).ConfigureAwait(false))
        {
            if (locked != null)
            {
                if (!await checkAsync(cancellationToken).ConfigureAwait(false))
                    await doAsync(cancellationToken).ConfigureAwait(false);

                return true;
            }
        }

        if (wait > TimeSpan.Zero)
        {
            var min = RetryMin ?? RetryMinDefault;
            var max = (Int32)wait.TotalMilliseconds;
            var stopwatch = Stopwatch.StartNew();

            do
            {
                var retry = max <= min ? max : GetRandom().Next(min, max);

                LogDelay(name, retry);

                await Task.Delay(retry, cancellationToken).ConfigureAwait(false);

                if (await checkAsync(cancellationToken).ConfigureAwait(false)) return true;

                await using var locked = await TryAcquireAsync(name, default, cancellationToken).ConfigureAwait(false);

                if (locked != null)
                {
                    if (!await checkAsync(cancellationToken).ConfigureAwait(false))
                        await doAsync(cancellationToken).ConfigureAwait(false);

                    return true;
                }
            } while (stopwatch.Elapsed <= wait);
        }

        return false;
    }

    #endregion IAsyncLocker

    #region ILocker

    public virtual ILock NewLock(String name)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));

        return new Lock(name, this);
    }

    ILocked? ILocker.TryAcquire(String name, TimeSpan wait, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (wait < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(wait));
        return TryAcquire(name, wait, cancellationToken);
    }

    public abstract ILocked? TryAcquire(String name, TimeSpan wait, CancellationToken cancellationToken);

    public virtual T? TryAcquireWithCheck<T>(String name,
        Func<CancellationToken, T?> check, Func<CancellationToken, T> getResult,
        TimeSpan wait, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (wait < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(wait));
        if (check is null) throw new ArgumentNullException(nameof(check));
        if (getResult is null) throw new ArgumentNullException(nameof(getResult));

        var comparer = EqualityComparer<T?>.Default;

        var result = check(cancellationToken);

        if (!comparer.Equals(result, default)) return result!;

        using (var locked = TryAcquire(name, default, cancellationToken))
        {
            if (locked != null)
            {
                result = check(cancellationToken);

                if (comparer.Equals(result, default))
                    result = getResult(cancellationToken);

                return result!;
            }
        }

        if (wait > TimeSpan.Zero)
        {
            var min = RetryMin ?? RetryMinDefault;
            var max = (Int32)wait.TotalMilliseconds;
            var stopwatch = Stopwatch.StartNew();

            do
            {
                var retry = max <= min ? max : GetRandom().Next(min, max);

                LogDelay(name, retry);

                Task.Delay(retry, cancellationToken).Wait(cancellationToken);

                result = check(cancellationToken);

                if (!comparer.Equals(result, default)) return result!;

                using var locked = TryAcquire(name, default, cancellationToken);
                if (locked != null)
                {
                    result = check(cancellationToken);

                    if (comparer.Equals(result, default))
                        result = getResult(cancellationToken);

                    return result!;
                }
            } while (stopwatch.Elapsed <= wait);
        }

        return default;
    }

    public virtual Boolean TryAcquireWithCheck(String name,
        Func<CancellationToken, Boolean> check, Action<CancellationToken> action,
        TimeSpan wait, CancellationToken cancellationToken)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (wait < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(wait));
        if (check is null) throw new ArgumentNullException(nameof(check));
        if (action is null) throw new ArgumentNullException(nameof(action));

        if (check(cancellationToken)) return true;

        using (var locked = TryAcquire(name, default, cancellationToken))
        {
            if (locked != null)
            {
                if (!check(cancellationToken))
                    action(cancellationToken);

                return true;
            }
        }

        if (wait > TimeSpan.Zero)
        {
            var min = RetryMin ?? RetryMinDefault;
            var max = (Int32)wait.TotalMilliseconds;
            var stopwatch = Stopwatch.StartNew();

            do
            {
                var retry = max <= min ? max : GetRandom().Next(min, max);

                LogDelay(name, retry);

                Task.Delay(retry, cancellationToken).Wait(cancellationToken);

                if (check(cancellationToken)) return true;

                using var locked = TryAcquire(name, default, cancellationToken);

                if (locked != null)
                {
                    if (!check(cancellationToken))
                        action(cancellationToken);

                    return true;
                }
            } while (stopwatch.Elapsed <= wait);
        }

        return false;
    }

    #endregion ILocker

    protected virtual void LogDelay(String name, Int32 retry)
    {

    }

    protected static Random GetRandom()
    {
#if NET6_0
        return Random.Shared;
#else
        return _Random.Shared;
#endif
    }
}