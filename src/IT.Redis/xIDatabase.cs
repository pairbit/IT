using System;

namespace StackExchange.Redis;

public static class xIDatabase
{
    #region String

    public static Boolean StringSetIfEqual(this IDatabase db, RedisKey key, RedisValue value, RedisValue equalValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)db.ScriptEvaluate(Lua.StringSetIfEqual, new RedisKey[] { key }, new RedisValue[] { value, equalValue }, flags);

    public static Boolean StringSetIfNotEqual(this IDatabase db, RedisKey key, RedisValue value, RedisValue notEqualValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)db.ScriptEvaluate(Lua.StringSetIfNotEqual, new RedisKey[] { key }, new RedisValue[] { value, notEqualValue }, flags);

    public static Boolean StringDeleteIfEqual(this IDatabase db, RedisKey key, RedisValue equalValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)db.ScriptEvaluate(Lua.StringDeleteIfEqual, new RedisKey[] { key }, new RedisValue[] { equalValue }, flags);

    public static Boolean StringDeleteIfNotEqual(this IDatabase db, RedisKey key, RedisValue notEqualValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)db.ScriptEvaluate(Lua.StringDeleteIfNotEqual, new RedisKey[] { key }, new RedisValue[] { notEqualValue }, flags);

    public static Boolean StringDeleteIfContains(this IDatabase db, RedisKey key, RedisValue[] equalValues, CommandFlags flags = CommandFlags.None)
    {
        if (equalValues == null) return false;

        var count = equalValues.Length;

        if (count == 0) return false;

        var script = count == 1 ? Lua.StringDeleteIfEqual : Lua.StringDeleteIfContains;

        return (Boolean)db.ScriptEvaluate(script, new RedisKey[] { key }, equalValues, flags);
    }

    public static Boolean StringDeleteIfNotContains(this IDatabase db, RedisKey key, RedisValue[] notEqualValues, CommandFlags flags = CommandFlags.None)
    {
        if (notEqualValues == null) return db.KeyDelete(key, flags);

        var count = notEqualValues.Length;

        if (count == 0) return db.KeyDelete(key, flags);

        var script = count == 1 ? Lua.StringDeleteIfNotEqual : Lua.StringDeleteIfNotContains;

        return (Boolean)db.ScriptEvaluate(script, new RedisKey[] { key }, notEqualValues, flags);
    }

    #endregion String

    #region Hash

    public static RedisValue HashGetSet(this IDatabase db, RedisKey key, RedisValue hashField, RedisValue value, CommandFlags flags = CommandFlags.None)
        => (RedisValue)db.ScriptEvaluate(Lua.HashGetSet, new RedisKey[] { key }, new RedisValue[] { hashField, value }, flags);

    public static RedisValue HashGetDelete(this IDatabase db, RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        => (RedisValue)db.ScriptEvaluate(Lua.HashGetDelete, new RedisKey[] { key }, new RedisValue[] { hashField }, flags);

    public static Boolean HashSetIfExists(this IDatabase db, RedisKey key, RedisValue hashField, RedisValue value, CommandFlags flags = CommandFlags.None)
        => (Boolean)db.ScriptEvaluate(Lua.HashSetIfExists, new RedisKey[] { key }, new RedisValue[] { hashField, value }, flags);

    public static Boolean HashSetIfEqual(this IDatabase db, RedisKey key, RedisValue hashField, RedisValue value, RedisValue equalValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)db.ScriptEvaluate(Lua.HashSetIfEqual, new RedisKey[] { key }, new RedisValue[] { hashField, value, equalValue }, flags);

    public static Boolean HashSetIfNotEqual(this IDatabase db, RedisKey key, RedisValue hashField, RedisValue value, RedisValue notEqualValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)db.ScriptEvaluate(Lua.HashSetIfNotEqual, new RedisKey[] { key }, new RedisValue[] { hashField, value, notEqualValue }, flags);

    public static Boolean HashDeleteIfEqual(this IDatabase db, RedisKey key, RedisValue hashField, RedisValue equalValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)db.ScriptEvaluate(Lua.HashDeleteIfEqual, new RedisKey[] { key }, new RedisValue[] { hashField, equalValue }, flags);

    public static Boolean HashDeleteIfNotEqual(this IDatabase db, RedisKey key, RedisValue hashField, RedisValue notEqualValue, CommandFlags flags = CommandFlags.None)
        => (Boolean)db.ScriptEvaluate(Lua.HashDeleteIfNotEqual, new RedisKey[] { key }, new RedisValue[] { hashField, notEqualValue }, flags);

    #endregion Hash

    #region List

    //public static Boolean ListExist(this IDatabase db, RedisKey key, RedisValue value, Int64 start = 0, Int64 stop = -1, CommandFlags flags = CommandFlags.None)
    //{
    //    var items = db.ListRange(key, start, stop, flags);

    //    foreach (var item in items)
    //    {
    //        if (item.Equals(value)) return true;
    //    }

    //    return false;
    //}

    #endregion
}