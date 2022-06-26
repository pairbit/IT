using System;
using System.Collections.Generic;

namespace StackExchange.Redis;

public static class xRedisKey
{
    public static RedisKey[] ToRedisKeys(this String[] keys) => keys.To(x => (RedisKey)x);

    public static RedisKey[] ToRedisKeys(this String[] keys, String fullPath) => keys.To(x => (RedisKey)(fullPath + ":" + x));

    public static KeyValuePair<RedisKey, RedisValue>[] ToRedisKeyValue(this IDictionary<String, String> value, String fullPath)
        => value.ToArray(x => new KeyValuePair<RedisKey, RedisValue>(fullPath + ":" + x.Key, x.Value));

    public static KeyValuePair<RedisKey, RedisValue>[] ToRedisKeyValue(this IDictionary<String, Byte[]> value, String fullPath)
        => value.ToArray(x => new KeyValuePair<RedisKey, RedisValue>(fullPath + ":" + x.Key, x.Value));
}