using System;

namespace IT.Locking.Redis;

public record Options
{
    public String? Prefix { get; set; }

    public Int32? Expiry { get; set; }

    public Int32? RetryMin { get; set; }

    //public Int32? RetryMaxMs { get; set; }//0.8sec
}