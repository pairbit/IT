using System;
using System.Threading;

namespace IT.Locking;

public interface ILocker : IAsyncLocker
{
    ILock NewLock(String name);

    ILocked? TryAcquire(String name, TimeSpan wait = default, CancellationToken cancellationToken = default);

    //T? TryLockWithDoubleCheck<T>(String name, Func<CancellationToken, T?> check, Func<CancellationToken, T> getResult,
    //    TimeSpan wait, TimeSpan expiry, TimeSpan retry, CancellationToken cancellationToken = default);

    //Boolean TryLockWithDoubleCheck(String name, Func<CancellationToken, Boolean> check, Action<CancellationToken> action,
    //    TimeSpan wait, TimeSpan expiry, TimeSpan retry, CancellationToken cancellationToken = default);
}