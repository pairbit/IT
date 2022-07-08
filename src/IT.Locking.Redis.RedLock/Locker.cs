using RedLockNet;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking.Redis.RedLock;

public class Locker : Locking.Locker
{
    private readonly IDistributedLockFactory _factory;

    public Locker(IDistributedLockFactory factory)
    {
        _factory = factory;
    }

    #region IAsyncLocker

    public override async Task<ILock?> LockAsync(String resource, TimeSpan expiry)
    {
        var @lock = await _factory.CreateLockAsync(resource, expiry).ConfigureAwait(false);
        return @lock.IsAcquired ? new RedLock(@lock) : null;
    }

    public override async Task<ILock?> LockAsync(String resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        var @lock = await _factory.CreateLockAsync(resource, expiry, wait, retry, cancellationToken).ConfigureAwait(false);
        return @lock.IsAcquired ? new RedLock(@lock) : null;
    }

    #endregion IAsyncLocker

    #region ILocker

    public override ILock? Lock(String resource, TimeSpan expiry)
    {
        var @lock = _factory.CreateLock(resource, expiry);
        return @lock.IsAcquired ? new RedLock(@lock) : null;
    }

    public override ILock? Lock(String resource, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken)
    {
        var @lock = _factory.CreateLock(resource, expiry, wait, retry, cancellationToken);
        return @lock.IsAcquired ? new RedLock(@lock) : null;
    }

    #endregion ILocker
}