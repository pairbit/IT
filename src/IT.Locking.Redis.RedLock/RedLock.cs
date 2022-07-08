using RedLockNet;
using System.Threading.Tasks;

namespace IT.Locking.Redis.RedLock;

internal class RedLock : ILock
{
    private readonly IRedLock _redlock;

    public RedLock(IRedLock redlock)
    {
        _redlock = redlock;
    }

    public void Dispose() => _redlock.Dispose();

    public ValueTask DisposeAsync() => _redlock.DisposeAsync();

    public void Unlock() => _redlock.Dispose();

    public ValueTask UnlockAsync() => _redlock.DisposeAsync();
}