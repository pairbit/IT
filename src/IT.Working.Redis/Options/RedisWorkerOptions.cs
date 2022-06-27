namespace IT.Working.Redis.Options;

public record RedisWorkerOptions
{
    public ServerQueues Queues { get; init; }
}