using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking;

public interface IAsyncLocker
{
    Task<ILock?> LockAsync(String resource, TimeSpan expiry);

    //Task<ILock?> LockAsync(String resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken = default);
}