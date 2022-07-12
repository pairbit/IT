using System.Diagnostics;

namespace IT.Locking.Tests;

public abstract class LockTest : NoLockTest
{
    private readonly ILocker _locker;

    public LockTest(ILocker locker)
    {
        _locker = locker;
    }

    protected override void InsertData(IDictionary<Guid, byte> data, byte value)
    {
        var expiry = Debugger.IsAttached ? TimeSpan.FromMinutes(10) : TimeSpan.FromSeconds(1);

        using var @lock = _locker.TryLock($"InsertData-{value}", expiry);

        if (@lock != null)
        {
            if (!data.Values.Contains(value))
            {
                base.InsertData(data, value);
            }
        }
    }
}