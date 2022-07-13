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
        //var wait = Debugger.IsAttached ? TimeSpan.FromMinutes(1) : TimeSpan.FromMilliseconds(200);

        var name = $"InsertData-{value}";

        //simple
        using var locked = _locker.TryAcquire(name);

        if (locked != null)
        {
            //using var lock2 = _locker.TryLock(name, expiry);

            //if (lock2 != null) throw new InvalidOperationException();

            if (!data.Values.Contains(value))
            {
                Task.Delay(150).Wait();
                base.InsertData(data, value);
            }
        }
        else
        {
            Interlocked.Increment(ref _noLock);
        }

        //var wait = Debugger.IsAttached ? TimeSpan.FromMinutes(5) : TimeSpan.FromMilliseconds(10);
        //var retry = Debugger.IsAttached ? TimeSpan.FromSeconds(1) : TimeSpan.FromMilliseconds(100);

        ////good
        //var status = _locker.TryLockWithDoubleCheck($"InsertData-{value}",
        //    _ => data.Values.Contains(value),
        //    _ => base.InsertData(data, value),
        //    expiry, wait, retry);

        //if (!status) throw new InvalidOperationException();
    }
}