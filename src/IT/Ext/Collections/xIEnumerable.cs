using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IT.Ext
{
    public static class xIEnumerable
    {
        public static IEnumerable<Object> Cast(this IEnumerable enumerable) => enumerable.Cast<Object>();

        public static Boolean IsEmpty(this IEnumerable enumerable) => enumerable == null || !enumerable.GetEnumerator().MoveNext();

        public static Boolean IsSpace(this IEnumerable enumerable)
        {
            if (enumerable != null)
            {
                foreach (var value in enumerable)
                {
                    if (!xObject.IsSpace(value)) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// enumerable ?? []
        /// </summary>
        public static IEnumerable Empty(this IEnumerable enumerable)
        {
            if (enumerable != null)
            {
                foreach (var value in enumerable)
                {
                    yield return value;
                }
            }
        }

        public static IEnumerable UnSpace(this IEnumerable enumerable)
        {
            if (enumerable != null)
            {
                foreach (var value in enumerable)
                {
                    if (!xObject.IsSpace(value)) yield return value;
                }
            }
        }
    }
}