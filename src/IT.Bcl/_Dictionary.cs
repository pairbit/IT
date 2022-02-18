#if NETSTANDARD2_0

namespace System.Collections.Generic;

public static class _Dictionary
{
    public static Boolean TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key)) return false;

        dictionary.Add(key, value);

        return true;
    }
}

#endif