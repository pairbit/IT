using System;
using System.Threading;

namespace IT.Locking;

public interface ILocker : IAsyncLocker
{
    ILock? Lock(String resource, TimeSpan expiry);

    ILock? Lock(String resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);

    T? LockWithDoubleCheck<T>(String resource, Func<CancellationToken, T?> check, Func<CancellationToken, T> getResult, 
        TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);
}