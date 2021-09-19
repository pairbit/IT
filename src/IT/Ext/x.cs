using System;
using System.Collections.Generic;
using System.Linq;

namespace IT.Ext
{
    public static class x
    {
        /// <summary>
        /// ToString
        /// </summary>
        public static String Tos<T>(this T value) => value?.ToString();

        /// <summary>
        /// GetHashCode
        /// </summary>
        public static Int32 HashCode<T>(this T value) => value == null ? 0 : value.GetHashCode();

        /// <summary>
        /// new T[] { value }
        /// </summary>
        public static T[] InArray<T>(this T value) => new T[] { value };

        /// <summary>
        /// values.Contains(value)
        /// </summary>
        public static Boolean In<T>(this T value, params T[] values) => values.Contains(value);

        /// <summary>
        /// values.Contains(value)
        /// </summary>
        public static Boolean In<T>(this T value, IEnumerable<T> values) => values.Contains(value);

        /// <summary>
        /// value ?? defaultValue
        /// </summary>
        public static T Default<T>(this T? value, T defaultValue = default) where T : struct => value ?? defaultValue;
    }
}