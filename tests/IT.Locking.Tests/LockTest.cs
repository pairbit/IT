using System.Collections.Concurrent;

namespace IT.Locking.Tests;

public class LockTest
{
    private readonly Random _random = new();

    [Test]
    public Task Test() => Parallel(InsertData);

    protected async Task Parallel(Action<IDictionary<Guid, Byte>> action)
    {
        var data = new ConcurrentDictionary<Guid, Byte>();
        var tasks = new Task[10];

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                for (int i = 0; i < 500; i++)
                {
                    action(data);
                }
            });
        }

        await Task.WhenAll(tasks);

        Console.WriteLine($"{data.Count} from 5000 (255 unique)");

        foreach (var item in data.OrderBy(x => x.Value))
        {
            Console.WriteLine($"{item.Key} - {item.Value}");
        }
    }

    private void InsertData(IDictionary<Guid, Byte> data) 
        => InsertData(data, (byte)_random.Next(0, 256));

    protected virtual void InsertData(IDictionary<Guid, Byte> data, Byte value)
    {
        data.Add(Guid.NewGuid(), value);
    }
}