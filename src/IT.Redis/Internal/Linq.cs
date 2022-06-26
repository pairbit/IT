using System;
using System.Collections.Generic;

namespace StackExchange.Redis;

internal static class Linq
{
    public static T[] ToArray<T, TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        Func<KeyValuePair<TKey, TValue>, T> selector)
    {
        var array = new T[dictionary.Count];

        var i = 0;
        foreach (var item in dictionary)
        {
            array[i++] = selector(item);
        }

        return array;
    }

    public static TResult[] To<TSource, TResult>(this IReadOnlyList<TSource> list, Func<TSource, TResult> selector)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (list.Count == 0) return Array.Empty<TResult>();

        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var result = new TResult[list.Count];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = selector(list[i]);
        }

        return result;
    }
}