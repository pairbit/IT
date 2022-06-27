using System;

namespace IT.Working.Redis.Options;

public record RedisWorkingOptions
{
    public String? Prefix { get; init; }
}