using IT.Working;
using System;

namespace StackExchange.Redis;

internal static class xIDatabaseQueue
{
    internal static Boolean SetStatus(this IDatabase db, String queue, RedisValue value, JobStatus status, When when = When.Always)
        => db.HashSet($"{queue}:Status", value, (Int64)status, when);
}