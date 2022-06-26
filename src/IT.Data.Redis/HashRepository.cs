using StackExchange.Redis;
using System;

namespace IT.Data.Redis;

public class HashRepository
{
    protected readonly IDatabase _db;
    protected readonly RedisKey _hash;

    public HashRepository(IDatabase db, String hash)
    {
        if (hash is null) throw new ArgumentNullException(nameof(hash));
        if (hash.Length == 0) throw new ArgumentException("is empty", nameof(hash));
        _db = db;
        _hash = hash;
    }
}