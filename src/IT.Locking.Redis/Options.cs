using System;

namespace IT.Locking.Redis;

public record Options
{
    public String? Prefix { get; set; }
}