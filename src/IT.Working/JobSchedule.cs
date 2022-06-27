using System;

namespace IT.Working;

public readonly record struct JobSchedule
{
    public Job Job { get; init; }

    public String Schedule { get; init; }
}