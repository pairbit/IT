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

    public override ILock? TryLock(String name, TimeSpan expiry, CancellationToken cancellationToken)
    {
        var handle = _provider.TryAcquireLock(name, expiry, cancellationToken);
        return handle is not null ? new Lock(handle) : null;
    }

    public override async Task<ILock?> TryLockAsync(String name, TimeSpan expiry, CancellationToken cancellationToken)
    {
        var handle = await _provider.TryAcquireLockAsync(name, expiry, cancellationToken).ConfigureAwait(false);
        return handle is not null ? new Lock(handle) : null;
    }
}