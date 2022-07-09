using System;
using System.Threading;

namespace IT.Locking;

public interface ILocker : IAsyncLocker
{
    ILock? Lock(String name, TimeSpan expiry, CancellationToken cancellationToken = default);

    ILock? Lock(String name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);

    T? LockWithDoubleCheck<T>(String name, Func<CancellationToken, T?> check, Func<CancellationToken, T> getResult, 
        TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);
}