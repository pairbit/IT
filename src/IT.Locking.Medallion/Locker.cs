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

    public override ILocked? TryAcquire(String name, TimeSpan wait, CancellationToken cancellationToken)
    {
        var handle = _provider.CreateLock(name).TryAcquire(wait, cancellationToken);
        return handle is not null ? new Locked(handle) : null;
    }

    public override async Task<IAsyncLocked?> TryAcquireAsync(String name, TimeSpan wait, CancellationToken cancellationToken)
    {
        var handle = await _provider.CreateLock(name).TryAcquireAsync(wait, cancellationToken).ConfigureAwait(false);
        return handle is not null ? new Locked(handle) : null;
    }
}