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
        var expiry = Debugger.IsAttached ? TimeSpan.FromMinutes(1) : TimeSpan.FromSeconds(1);

        //simple
        //using var @lock = _locker.TryLock($"InsertData-{value}", expiry);

        //if (@lock != null)
        //{
        //    if (!data.Values.Contains(value))
        //    {
        //        base.InsertData(data, value);
        //    }
        //}

        var wait = Debugger.IsAttached ? TimeSpan.FromMinutes(5) : TimeSpan.FromSeconds(2);
        var retry = Debugger.IsAttached ? TimeSpan.FromSeconds(1) : TimeSpan.FromMilliseconds(100);

        //good
        var status = _locker.TryLockWithDoubleCheck($"InsertData-{value}",
            _ => data.Values.Contains(value),
            _ => base.InsertData(data, value),
            expiry, wait, retry);

        if (!status) throw new InvalidOperationException();
    }
}