using System;

namespace IT.Locking.Redis;

public record Options
{
    public String? Prefix { get; set; }

    public Int32? ExpiryMilliseconds { get; set; }

    public Int32? RetryMinMs { get; set; }

    //public Int32? RetryMaxMs { get; set; }//0.8sec
}