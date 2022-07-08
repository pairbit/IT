using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking;

public abstract class Locker : ILocker
{
    #region IAsyncLocker

    public abstract Task<ILock?> LockAsync(String resource, TimeSpan expiry);

    public virtual async Task<ILock?> LockAsync(String resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        while (stopwatch.Elapsed <= wait)
        {
            var @lock = await LockAsync(resource, expiry).ConfigureAwait(false);

            if (@lock is not null) return @lock;

            await Task.Delay(retry, cancellationToken).ConfigureAwait(false);
        }
        return null;
    }

    public virtual async Task<T?> LockWithDoubleCheckAsync<T>(String resource,
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

            await using var @lock = await LockAsync(resource, expiry).ConfigureAwait(false);

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

    #endregion IAsyncLocker

    #region ILocker

    public abstract ILock? Lock(String resource, TimeSpan expiry);

    public virtual ILock? Lock(String resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        while (stopwatch.Elapsed <= wait)
        {
            var @lock = Lock(resource, expiry);

            if (@lock is not null) return @lock;

            Task.Delay(retry, cancellationToken).Wait(cancellationToken);
        }
        return null;
    }

    public virtual T? LockWithDoubleCheck<T>(String resource,
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

            using var @lock = Lock(resource, expiry);

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

    #endregion ILocker
}