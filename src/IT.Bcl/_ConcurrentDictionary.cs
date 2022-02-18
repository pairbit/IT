﻿#if NETSTANDARD2_0

using System;
using System.Collections.Concurrent;

namespace IT.Bcl;

public static class _ConcurrentDictionary
{
    public static TValue GetOrAdd<TKey, TValue, TArg>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument)
        => dictionary.GetOrAdd(key, x => valueFactory(x, factoryArgument));
}

#endif