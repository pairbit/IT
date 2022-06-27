using System;

namespace IT.Working;

public record Job
{
    public String Name { get; }

    public String? Arg { get; init; }

    public String? Queue { get; init; }

    public Job(String name)
    {
        Name = name;
    }
}