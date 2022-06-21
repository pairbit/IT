using System;
using System.Threading;

namespace IT.Locking;

public interface ILocker : IAsyncLocker
{
    ILock? Lock(String resource, TimeSpan expiry);

    //ILock? Lock(String resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);
}