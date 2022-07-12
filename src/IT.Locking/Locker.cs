using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking;

public abstract class Locker : ILocker
{
    #region IAsyncLocker

    public abstract Task<ILock?> TryLockAsync(String name, TimeSpan expiry, CancellationToken cancellationToken = default);

    public virtual async Task<ILock?> TryLockAsync(String name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        while (stopwatch.Elapsed <= wait)
        {
            var @lock = await TryLockAsync(name, expiry).ConfigureAwait(false);

            if (@lock is not null) return @lock;

            await Task.Delay(retry, cancellationToken).ConfigureAwait(false);
        }
        return null;
    }

    public virtual async Task<T?> TryLockWithDoubleCheckAsync<T>(String name,
        Func<CancellationToken, Task<T?>> checkAsync, Func<CancellationToken, Task<T>> getResultAsync,
        TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        if (checkAsync is null) throw new ArgumentNullException(nameof(checkAsync));
        if (getResultAsync is null) throw new ArgumentNullException(nameof(getResultAsync));

        var comparer = EqualityComparer<T?>.Default;

        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed <= wait)
        {
            var result = await checkAsync(cancellationToken).ConfigureAwait(false);

            if (!comparer.Equals(result, default)) return result!;

            await using var @lock = await TryLockAsync(name, expiry).ConfigureAwait(false);

            if (@lock != null)
            {
                result = await checkAsync(cancellationToken).ConfigureAwait(false);

                if (comparer.Equals(result, default))
                    result = await getResultAsync(cancellationToken).ConfigureAwait(false);

                await @lock.UnlockAsync().ConfigureAwait(false);

                return result!;
            }

            await Task.Delay(retry, cancellationToken).ConfigureAwait(false);
        }

        return default;
    }

    public virtual async Task<Boolean> TryLockWithDoubleCheckAsync(String name,
        Func<CancellationToken, Task<Boolean>> checkAsync, Func<CancellationToken, Task> doAsync,
        TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        if (checkAsync is null) throw new ArgumentNullException(nameof(checkAsync));
        if (doAsync is null) throw new ArgumentNullException(nameof(doAsync));

        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed <= wait)
        {
            if (await checkAsync(cancellationToken).ConfigureAwait(false)) return true;

            await using var @lock = await TryLockAsync(name, expiry).ConfigureAwait(false);

            if (@lock != null)
            {
                if (!await checkAsync(cancellationToken).ConfigureAwait(false))
                    await doAsync(cancellationToken).ConfigureAwait(false);

                await @lock.UnlockAsync().ConfigureAwait(false);

                return true;
            }

            await Task.Delay(retry, cancellationToken).ConfigureAwait(false);
        }

        return false;
    }

    #endregion IAsyncLocker

    #region ILocker

    public abstract ILock? TryLock(String name, TimeSpan expiry, CancellationToken cancellationToken = default);

    public virtual ILock? TryLock(String name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        while (stopwatch.Elapsed <= wait)
        {
            var @lock = TryLock(name, expiry);

            if (@lock is not null) return @lock;

            Task.Delay(retry, cancellationToken).Wait(cancellationToken);
        }
        return null;
    }

    public virtual T? TryLockWithDoubleCheck<T>(String name,
        Func<CancellationToken, T?> check, Func<CancellationToken, T> getResult,
        TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        if (check is null) throw new ArgumentNullException(nameof(check));
        if (getResult is null) throw new ArgumentNullException(nameof(getResult));

        var comparer = EqualityComparer<T?>.Default;

        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed <= wait)
        {
            var result = check(cancellationToken);

            if (!comparer.Equals(result, default)) return result!;

            using var @lock = TryLock(name, expiry);

            if (@lock != null)
            {
                result = check(cancellationToken);

                if (comparer.Equals(result, default))
                    result = getResult(cancellationToken);

                @lock.UnlockAsync();

                return result!;
            }

            Task.Delay(retry, cancellationToken).Wait(cancellationToken);
        }

        return default;
    }

    public virtual Boolean TryLockWithDoubleCheck(String name,
        Func<CancellationToken, Boolean> check, Action<CancellationToken> action,
        TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        if (check is null) throw new ArgumentNullException(nameof(check));
        if (action is null) throw new ArgumentNullException(nameof(action));

        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed <= wait)
        {
            if (check(cancellationToken)) return true;

            using var @lock = TryLock(name, expiry);

            if (@lock != null)
            {
                if (!check(cancellationToken))
                    action(cancellationToken);

                @lock.Unlock();

                return true;
            }

            Task.Delay(retry, cancellationToken).Wait(cancellationToken);
        }

        return false;
    }

    #endregion ILocker
}