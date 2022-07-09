using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking;

public interface IAsyncLocker
{
    Task<ILock?> LockAsync(String name, TimeSpan expiry, CancellationToken cancellationToken = default);

    Task<ILock?> LockAsync(String name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);

    Task<T?> LockWithDoubleCheckAsync<T>(String name,
        Func<CancellationToken, Task<T?>> checkAsync, Func<CancellationToken, Task<T>> getResultAsync,
        TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);
}