using IT.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IT.Ext
{
    public static class xIEnumerableT
    {
        public static Boolean Any<T>(this IEnumerable<T> enumerable, Int32 count)
        {
            if (enumerable != null && count >= 0)
            {
                if (count == 0) return true;

                foreach (var item in enumerable)
                {
                    if (--count == 0) return true;
                }
            }
            return false;
        }

        public static Boolean Any<T>(this IEnumerable<T> enumerable, Func<T, Boolean> predicate, Int32 count) => enumerable.Where(predicate).Any(count);

        internal static IEnumerable<T> WhereOrNull<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector, TKey key, IEqualityComparer<TKey> comparer = null)
        {
            Arg.NotNull(keySelector, nameof(keySelector));
            if (comparer == null) comparer = EqualityComparer<TKey>.Default;

            var filtered = enumerable.Where(x => comparer.Equals(keySelector(x), key));
            if (key != null && filtered.IsEmpty()) filtered = enumerable.Where(x => keySelector(x) == null);
            return filtered;
        }

        internal static IEnumerable<T> WhereOrNull<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey[]> keySelector, TKey key, IEqualityComparer<TKey> comparer = null)
        {
            Arg.NotNull(keySelector, nameof(keySelector));
            if (comparer == null) comparer = EqualityComparer<TKey>.Default;

            var filtered = enumerable.Where(x => keySelector(x)?.Contains(key, comparer) == true);
            if (filtered.IsEmpty()) filtered = enumerable.Where(x => keySelector(x) == null);
            return filtered;
        }

        public static Boolean IsUnique<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer = null)
        {
            var hashSet = new HashSet<T>(comparer);
            return enumerable.All(hashSet.Add);
        }

        public static IEnumerable<TKey> GetDuplicates<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer = null)
        {
            foreach (var group in enumerable.GroupBy(keySelector, comparer))
            {
                if (group.Any(2)) yield return group.Key;
            }
        }

        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer = null)
        {
            var hashSet = new HashSet<T>(comparer);
            return enumerable.Where(hashSet.Add);
        }

        /// <summary>
        /// enumerable ?? []
        /// </summary>
        public static IEnumerable<T> Empty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable != null)
            {
                foreach (var value in enumerable)
                {
                    yield return value;
                }
            }
        }

        public static IEnumerable<T> UnSpace<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable != null)
            {
                foreach (var value in enumerable)
                {
                    if (!xObject.IsSpace(value)) yield return value;
                }
            }
        }

        public static Task SelectAsync<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, Task> selector)
            => Task.WhenAll(enumerable.Select(selector));

        public static Task<TResult[]> SelectAsync<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, Task<TResult>> selector)
            => Task.WhenAll(enumerable.Select(selector));
    }
}