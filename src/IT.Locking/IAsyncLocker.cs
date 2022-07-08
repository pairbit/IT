using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking;

public interface IAsyncLocker
{
    Task<ILock?> LockAsync(String resource, TimeSpan expiry);

    Task<ILock?> LockAsync(String resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);

    Task<T?> LockWithDoubleCheckAsync<T>(String resource,
        Func<CancellationToken, Task<T?>> checkAsync, Func<CancellationToken, Task<T>> getResultAsync,
        TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);
}