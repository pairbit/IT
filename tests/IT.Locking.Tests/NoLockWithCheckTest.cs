namespace IT.Locking.Tests;

public class NoLockWithCheckTest : LockTest
{
    protected override void InsertData(IDictionary<Guid, byte> data, byte value)
    {
        if (!data.Values.Contains(value))
        {
            Task.Delay(10).Wait();
            data.Add(Guid.NewGuid(), value);
        }
    }
}