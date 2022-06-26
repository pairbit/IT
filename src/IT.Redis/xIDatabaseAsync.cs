using System;
using System.Threading.Tasks;

namespace StackExchange.Redis;

public static class xIDatabaseAsync
{
    #region String

    public static async Task<Boolean> StringSetIfEqualAsync(this IDatabaseAsync db, RedisKey key, RedisValue value, RedisValue equalValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)await db.ScriptEvaluateAsync(Lua.StringSetIfEqual, new RedisKey[] { key }, new RedisValue[] { value, equalValue }, flags).ConfigureAwait(false);

    public static async Task<Boolean> StringSetIfNotEqualAsync(this IDatabaseAsync db, RedisKey key, RedisValue value, RedisValue notEqualValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)await db.ScriptEvaluateAsync(Lua.StringSetIfNotEqual, new RedisKey[] { key }, new RedisValue[] { value, notEqualValue }, flags).ConfigureAwait(false);

    public static async Task<Boolean> StringDeleteIfEqualAsync(this IDatabaseAsync db, RedisKey key, RedisValue equalValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)await db.ScriptEvaluateAsync(Lua.StringDeleteIfEqual, new RedisKey[] { key }, new RedisValue[] { equalValue }, flags).ConfigureAwait(false);

    public static async Task<Boolean> StringDeleteIfNotEqualAsync(this IDatabaseAsync db, RedisKey key, RedisValue notEqualValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)await db.ScriptEvaluateAsync(Lua.StringDeleteIfNotEqual, new RedisKey[] { key }, new RedisValue[] { notEqualValue }, flags).ConfigureAwait(false);

    #endregion String

    #region Hash

    public static async Task<RedisValue> HashGetSetAsync(this IDatabaseAsync db, RedisKey key, RedisValue hashField, RedisValue value, CommandFlags flags = CommandFlags.None)
        => (RedisValue)await db.ScriptEvaluateAsync(Lua.HashGetSet, new RedisKey[] { key }, new RedisValue[] { hashField, value }, flags).ConfigureAwait(false);

    public static async Task<RedisValue> HashGetDeleteAsync(this IDatabaseAsync db, RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        => (RedisValue)await db.ScriptEvaluateAsync(Lua.HashGetDelete, new RedisKey[] { key }, new RedisValue[] { hashField }, flags).ConfigureAwait(false);

    public static async Task<Boolean> HashSetIfExistsAsync(this IDatabaseAsync db, RedisKey key, RedisValue hashField, RedisValue value, CommandFlags flags = CommandFlags.None)
        => (Boolean)await db.ScriptEvaluateAsync(Lua.HashSetIfExists, new RedisKey[] { key }, new RedisValue[] { hashField, value }, flags).ConfigureAwait(false);

    public static async Task<Boolean> HashSetIfEqualAsync(this IDatabaseAsync db, RedisKey key, RedisValue hashField, RedisValue value, RedisValue equalValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)await db.ScriptEvaluateAsync(Lua.HashSetIfEqual, new RedisKey[] { key }, new RedisValue[] { hashField, value, equalValue }, flags).ConfigureAwait(false);

    public static async Task<Boolean> HashSetIfNotEqualAsync(this IDatabaseAsync db, RedisKey key, RedisValue hashField, RedisValue value, RedisValue notEqualValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)await db.ScriptEvaluateAsync(Lua.HashSetIfNotEqual, new RedisKey[] { key }, new RedisValue[] { hashField, value, notEqualValue }, flags).ConfigureAwait(false);

    public static async Task<Boolean> HashDeleteIfEqualAsync(this IDatabaseAsync db, RedisKey key, RedisValue hashField, RedisValue equalValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)await db.ScriptEvaluateAsync(Lua.HashDeleteIfEqual, new RedisKey[] { key }, new RedisValue[] { hashField, equalValue }, flags).ConfigureAwait(false);

    public static async Task<Boolean> HashDeleteIfNotEqualAsync(this IDatabaseAsync db, RedisKey key, RedisValue hashField, RedisValue notEqualValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)await db.ScriptEvaluateAsync(Lua.HashDeleteIfNotEqual, new RedisKey[] { key }, new RedisValue[] { hashField, notEqualValue }, flags).ConfigureAwait(false);

    #endregion Hash

    //public static async Task<Boolean> ListExistAsync(this IDatabaseAsync db, RedisKey key, RedisValue value, Int64 start = 0, Int64 stop = -1, CommandFlags flags = CommandFlags.None)
    //{
    //    //TODO: Переписать на LUA
    //    var items = await db.ListRangeAsync(key, start, stop, flags).CA();

    //    foreach (var item in items)
    //    {
    //        if (item.Equals(value)) return true;
    //    }

    //    return false;
    //}

    //private static async Task<RedisScanResult> ScanAsync(this IDatabaseAsync db, Int64 cursor, String pattern, Int32 count)
    //{
    //    var result = await db.ExecuteAsync("SCAN", cursor.ToString(), "MATCH", pattern, "COUNT", count.ToString()).CA();
    //    var results = (RedisResult[])result;
    //    var nextCursor = Int64.Parse((String)results[0]);
    //    var keys = (String[])results[1];
    //    return new RedisScanResult { Cursor = nextCursor, Keys = keys };
    //}

    //public static async Task KeyScanAsync(this IDatabaseAsync db, String pattern, Func<String[], Task> iteration, Int32 count = 1000)
    //{
    //    Arg.NotNull(db);
    //    Arg.NotNull(pattern);
    //    Arg.NotNull(iteration);

    //    long cursor = 0;
    //    do
    //    {
    //        var result = await db.ScanAsync(cursor, pattern, count).CA();

    //        await iteration(result.Keys).CA();

    //        cursor = result.Cursor;
    //    }
    //    while (cursor != 0);
    //}

    //public static async Task KeyScanAsync(this IDatabaseAsync db, String pattern, Func<String[], Task<Boolean>> iteration, Int32 count = 1000)
    //{
    //    Arg.NotNull(db);
    //    Arg.NotNull(pattern);
    //    Arg.NotNull(iteration);

    //    long cursor = 0;
    //    do
    //    {
    //        var result = await db.ScanAsync(cursor, pattern, count).CA();

    //        if (!await iteration(result.Keys).CA()) return;

    //        cursor = result.Cursor;
    //    }
    //    while (cursor != 0);
    //}

    //public static async Task<Boolean> KeyScanExistsAsync(this IDatabaseAsync db, String pattern, Int32 count = 1000)
    //{
    //    Arg.NotNull(pattern, nameof(pattern));
    //    long cursor = 0;
    //    do
    //    {
    //        var result = await db.ScanAsync(cursor, pattern, count).CA();

    //        if (result.Keys.Any()) return true;

    //        cursor = result.Cursor;
    //    }
    //    while (cursor != 0);
    //    return false;
    //}

    //public static Task KeysDeleteAsync(this IDatabaseAsync db, String pattern, CommandFlags flags = CommandFlags.None)
    //    => db.KeyScanAsync(pattern, keys => db.KeyDeleteAsync(keys.Select(x => (RedisKey)x).ToArray(), flags));
}