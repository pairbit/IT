using System;

namespace IT.Working;

public readonly record struct JobRepeat
{
    public Job Job { get; init; }

    public Boolean Repeat { get; init; }
}