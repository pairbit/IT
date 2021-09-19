using System.Collections.Generic;

namespace IT.Ext
{
    public static class xIDictionaryTKeyTValue
    {
        public static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key, Null<TValue> trash = null)
            where TValue : class
        {
            return value.TryGetValue(key, out TValue item) ? item : null;
        }

        public static TValue? TryGet<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key, TValue? trash = null)
            where TValue : struct
        {
            return value.TryGetValue(key, out TValue item) ? (TValue?)item : null;
        }

        public static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key)
        {
            return value.TryGetValue(key, out TValue item) ? item : default;
        }
    }
}