using System;

namespace IT.Working.Redis.Options;

public record Queue
{
    public Int32 Workers { get; init; }
}