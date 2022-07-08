using Medallion.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking.Medallion;

public class Locker : Locking.Locker
{
    private readonly IDistributedLockProvider _provider;

    public Locker(IDistributedLockProvider provider)
    {
        _provider = provider;
    }

    public override ILock? Lock(String resource, TimeSpan expiry, CancellationToken cancellationToken)
    {
        var handle = _provider.TryAcquireLock(resource, expiry, cancellationToken);
        return handle is not null ? new Lock(handle) : null;
    }

    public override async Task<ILock?> LockAsync(String resource, TimeSpan expiry, CancellationToken cancellationToken)
    {
        var handle = await _provider.TryAcquireLockAsync(resource, expiry, cancellationToken).ConfigureAwait(false);
        return handle is not null ? new Lock(handle) : null;
    }
}