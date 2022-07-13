using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Locking;

internal class Lock : ILock
{
    private readonly ILocker _locker;
    private readonly String _name;

    public String Name => _name;

    public Lock(String name, ILocker locker)
    {
        _locker = locker;
        _name = name;
    }

    public ILocked? TryAcquire(TimeSpan wait, CancellationToken cancellationToken)
        => _locker.TryAcquire(_name, wait, cancellationToken);

    public Task<IAsyncLocked?> TryAcquireAsync(TimeSpan wait, CancellationToken cancellationToken)
        => _locker.TryAcquireAsync(_name, wait, cancellationToken);
}