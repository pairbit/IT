using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking;

public static class xIAsyncLocker
{
    public static async Task<TResult> LockWithDoubleCheckAsync<TResult>(this IAsyncLocker locker, String resource,
        Func<CancellationToken, Task<TResult?>> checkAsync, Func<CancellationToken, Task<TResult>> getResultAsync,
        TimeSpan expiry, TimeSpan retry, CancellationToken cancellationToken = default)
    {
        if (checkAsync is null) throw new ArgumentNullException(nameof(checkAsync));
        if (getResultAsync is null) throw new ArgumentNullException(nameof(getResultAsync));

        var comparer = EqualityComparer<TResult?>.Default;

        do
        {
            var result = await checkAsync(cancellationToken).ConfigureAwait(false);

            if (!comparer.Equals(result, default)) return result!;

            await using var @lock = await locker.LockAsync(resource, expiry).ConfigureAwait(false);

            if (@lock != null)
            {
                result = await checkAsync(cancellationToken).ConfigureAwait(false);

                if (comparer.Equals(result, default))
                    result = await getResultAsync(cancellationToken).ConfigureAwait(false);

                await @lock.UnlockAsync().ConfigureAwait(false);

                return result!;
            }

            await Task.Delay(retry, cancellationToken).ConfigureAwait(false);

        } while (true);
    }
}