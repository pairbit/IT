using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking;

public interface IAsyncLock
{
    String Name { get; }

    Task<IAsyncLocked?> TryAcquireAsync(TimeSpan wait = default, CancellationToken cancellationToken = default);

    //Task<T?> TryLockWithDoubleCheckAsync<T>(
    //    Func<CancellationToken, Task<T?>> checkAsync, Func<CancellationToken, Task<T>> getResultAsync,
    //    TimeSpan wait, TimeSpan expiry, TimeSpan retry, CancellationToken cancellationToken = default);

    //Task<Boolean> TryLockWithDoubleCheckAsync(
    //    Func<CancellationToken, Task<Boolean>> checkAsync, Func<CancellationToken, Task> doAsync,
    //    TimeSpan wait, TimeSpan expiry, TimeSpan retry, CancellationToken cancellationToken = default);
}