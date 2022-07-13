using System;

namespace IT.Locking.Redis.RedLock;

public record Options
{
    public Int32? ExpiryMilliseconds { get; set; }

    public Int32? RetryMilliseconds { get; set; }
}