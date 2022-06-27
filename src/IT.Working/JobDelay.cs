using System;

namespace IT.Working;

public readonly record struct JobDelay
{
    public Job Job { get; init; }

    public Int64 Delay { get; init; }

    public Boolean Repeat { get; init; }
}