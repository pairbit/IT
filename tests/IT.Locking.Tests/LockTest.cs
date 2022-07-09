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
        using var @lock = _locker.Lock($"InsertData-{value}", TimeSpan.FromSeconds(1));
        if (@lock != null)
        {
            if (!data.Values.Contains(value))
            {
                base.InsertData(data, value);
            }
        }
    }
}