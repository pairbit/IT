using System;
using System.Collections.Generic;

namespace StackExchange.Redis;

public static class xHashEntry
{
    public static HashEntry[] ToHashEntries(this IDictionary<String, String> dictionary)
        => dictionary.ToArray(x => new HashEntry(x.Key, x.Value ?? RedisValue.EmptyString));

    public static HashEntry[] ToHashEntries(this IDictionary<String, Byte[]> dictionary)
        => dictionary.ToArray(x => new HashEntry(x.Key, x.Value ?? RedisValue.EmptyString));

    
}