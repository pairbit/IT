using System;
using System.Threading;

namespace IT.Locking;

public interface ILock : IAsyncLock
{
    ILocked? TryAcquire(TimeSpan wait = default, CancellationToken cancellationToken = default);

    //T? TryLockWithDoubleCheck<T>(Func<CancellationToken, T?> check, Func<CancellationToken, T> getResult,
    //    TimeSpan wait, TimeSpan expiry, TimeSpan retry, CancellationToken cancellationToken = default);

    //Boolean TryLockWithDoubleCheck(Func<CancellationToken, Boolean> check, Action<CancellationToken> action,
    //    TimeSpan wait, TimeSpan expiry, TimeSpan retry, CancellationToken cancellationToken = default);
}