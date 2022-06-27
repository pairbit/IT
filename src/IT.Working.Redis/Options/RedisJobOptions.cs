using System;

namespace IT.Working.Redis.Options;

public record RedisJobOptions
{
    public QueuePolicy QueuePolicy { get; init; }

    public String? SharedQueueName { get; init; }
}