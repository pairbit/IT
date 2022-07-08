using Medallion.Threading;
using System.Threading.Tasks;

namespace IT.Locking.Medallion;

internal class Lock : ILock
{
    private readonly IDistributedSynchronizationHandle _handle;

    public Lock(IDistributedSynchronizationHandle handle)
    {
        _handle = handle;
    }

    public void Dispose() => _handle.Dispose();

    public ValueTask DisposeAsync() => _handle.DisposeAsync();

    public void Unlock() => _handle.Dispose();

    public ValueTask UnlockAsync() => _handle.DisposeAsync();
}