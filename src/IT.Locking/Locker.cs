using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking;

public abstract class Locker : ILocker
{
    #region IAsyncLocker

    public virtual IAsyncLock NewAsyncLock(String name)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));

        return new Lock(name, this);
    }

    public abstract Task<IAsyncLocked?> TryAcquireAsync(String name, TimeSpan wait, CancellationToken cancellationToken = default);

    public virtual async Task<T?> TryLockWithDoubleCheckAsync<T>(String name,
        Func<CancellationToken, Task<T?>> checkAsync, Func<CancellationToken, Task<T>> getResultAsync,
        TimeSpan wait, TimeSpan expiry, TimeSpan retry, CancellationToken cancellationToken)
    {
        if (checkAsync is null) throw new ArgumentNullException(nameof(checkAsync));
        if (getResultAsync is null) throw new ArgumentNullException(nameof(getResultAsync));

        var comparer = EqualityComparer<T?>.Default;

        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed <= wait)
        {
            var result = await checkAsync(cancellationToken).ConfigureAwait(false);

            if (!comparer.Equals(result, default)) return result!;

            await using var locked = await TryAcquireAsync(name, expiry, cancellationToken).ConfigureAwait(false);

            if (locked != null)
            {
                result = await checkAsync(cancellationToken).ConfigureAwait(false);

                if (comparer.Equals(result, default))
                    result = await getResultAsync(cancellationToken).ConfigureAwait(false);

                return result!;
            }

            await Task.Delay(retry, cancellationToken).ConfigureAwait(false);
        }

        return default;
    }

    public virtual async Task<Boolean> TryLockWithDoubleCheckAsync(String name,
        Func<CancellationToken, Task<Boolean>> checkAsync, Func<CancellationToken, Task> doAsync,
        TimeSpan wait, TimeSpan expiry, TimeSpan retry, CancellationToken cancellationToken)
    {
        if (checkAsync is null) throw new ArgumentNullException(nameof(checkAsync));
        if (doAsync is null) throw new ArgumentNullException(nameof(doAsync));

        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed <= wait)
        {
            if (await checkAsync(cancellationToken).ConfigureAwait(false)) return true;

            await using var locked = await TryAcquireAsync(name, expiry, cancellationToken).ConfigureAwait(false);

            if (locked != null)
            {
                if (!await checkAsync(cancellationToken).ConfigureAwait(false))
                    await doAsync(cancellationToken).ConfigureAwait(false);

                return true;
            }

            await Task.Delay(retry, cancellationToken).ConfigureAwait(false);
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

    public abstract ILocked? TryAcquire(String name, TimeSpan wait, CancellationToken cancellationToken = default);

    public virtual T? TryLockWithDoubleCheck<T>(String name,
        Func<CancellationToken, T?> check, Func<CancellationToken, T> getResult,
        TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        if (check is null) throw new ArgumentNullException(nameof(check));
        if (getResult is null) throw new ArgumentNullException(nameof(getResult));

        var comparer = EqualityComparer<T?>.Default;

        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed <= wait)
        {
            var result = check(cancellationToken);

            if (!comparer.Equals(result, default)) return result!;

            using var locked = TryAcquire(name, default, cancellationToken);

            if (locked != null)
            {
                result = check(cancellationToken);

                if (comparer.Equals(result, default))
                    result = getResult(cancellationToken);

                return result!;
            }

            Task.Delay(retry, cancellationToken).Wait(cancellationToken);
        }

        return default;
    }

    public virtual Boolean TryLockWithDoubleCheck(String name,
        Func<CancellationToken, Boolean> check, Action<CancellationToken> action,
        TimeSpan wait, TimeSpan expiry, TimeSpan retry, CancellationToken cancellationToken)
    {
        if (check is null) throw new ArgumentNullException(nameof(check));
        if (action is null) throw new ArgumentNullException(nameof(action));

        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed <= wait)
        {
            if (check(cancellationToken)) return true;

            using var locked = TryAcquire(name, expiry, cancellationToken);

            if (locked != null)
            {
                if (!check(cancellationToken))
                    action(cancellationToken);

                return true;
            }

            Task.Delay(retry, cancellationToken).Wait(cancellationToken);
        }

        return false;
    }

    #endregion ILocker
}