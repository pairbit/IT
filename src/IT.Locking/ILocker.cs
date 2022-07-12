using System;
using System.Threading;

namespace IT.Locking;

public interface ILocker : IAsyncLocker
{
    ILock? TryLock(String name, TimeSpan expiry, CancellationToken cancellationToken = default);

    ILock? TryLock(String name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);

    T? TryLockWithDoubleCheck<T>(String name, Func<CancellationToken, T?> check, Func<CancellationToken, T> getResult,
        TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);

    Boolean TryLockWithDoubleCheck(String name, Func<CancellationToken, Boolean> check, Action<CancellationToken> action,
        TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);
}