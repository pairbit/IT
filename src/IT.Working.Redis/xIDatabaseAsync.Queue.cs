using IT.Working;
using System;
using System.Threading.Tasks;

namespace StackExchange.Redis;

internal static class xIDatabaseAsyncQueue
{
    internal static Task<Boolean> SetStatusAsync(this IDatabaseAsync db, String queue, RedisValue value, JobStatus status, When when = When.Always)
        => db.HashSetAsync($"{queue}:Status", value, (Int64)status, when);

    public static async Task<RedisValue> DequeueAsync(this IDatabaseAsync db, String queue, String worker)
    {
        var value = await db.ListRightPopLeftPushAsync($"{queue}:Enqueued", $"{queue}:Processing:{worker}").ConfigureAwait(false);

        if (!value.IsNull)
        {
            await db.SetStatusAsync(queue, value, JobStatus.Processing).ConfigureAwait(false);
        }

        return value;
    }

    public static async Task QueueAddToSucceededAsync(this IDatabaseAsync db, String queue, String worker, RedisValue value)
    {
        await db.KeyDeleteAsync($"{queue}:Processing:{worker}").ConfigureAwait(false);

        await db.SetStatusAsync(queue, value, JobStatus.Succeeded).ConfigureAwait(false);
    }

    public static async Task QueueAddToFailedAsync(this IDatabaseAsync db, String queue, String worker, RedisValue value)
    {
        await db.KeyDeleteAsync($"{queue}:Processing:{worker}").ConfigureAwait(false);

        await db.SetStatusAsync(queue, value, JobStatus.Failed).ConfigureAwait(false);
    }

    public static async Task QueueRollbackAsync(this IDatabaseAsync db, String queue, String worker, CommandFlags flags = CommandFlags.None)
    {
        var value = await db.ListRightPopLeftPushAsync($"{queue}:Processing:{worker}", $"{queue}:Enqueued", flags).ConfigureAwait(false);

        if (value.IsNull) throw new InvalidOperationException("value.IsNull");

        await db.SetStatusAsync(queue, value, JobStatus.Enqueued).ConfigureAwait(false);
    }

    public static async Task QueueAddToDeletedAsync(this IDatabaseAsync db, String queue, String worker, RedisValue value)
    {
        await db.KeyDeleteAsync($"{queue}:Processing:{worker}").ConfigureAwait(false);

        await db.HashDeleteAsync($"{queue}:Status", value).ConfigureAwait(false);

        //var value = await db.ListRightPopLeftPushAsync($"{queue}:Processing:{worker}", $"{queue}:Deleted", flags).CA();

        //Operation.Invalid(value.IsNull);
    }
}