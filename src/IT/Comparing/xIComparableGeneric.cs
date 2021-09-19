using System;

namespace IT.Comparing
{
    public static class xIComparableGeneric
    {
        /*
        /// <summary>
        /// value == 0
        /// </summary>
        public static Boolean IsZero<T>(this T value) where T : IComparable<T>
        {
            return value.Eq(default(T));
        }
        */

        /// <summary>
        /// value &lt; 0
        /// </summary>
        public static Boolean IsNegative<T>(this T value) where T : IComparable<T>
        {
            return value.Lt(default);
        }

        /// <summary>
        /// value &gt; 0
        /// </summary>
        public static Boolean IsPositive<T>(this T value) where T : IComparable<T>
        {
            return value.Gt(default);
        }

        /// <summary>
        /// value &lt;= 0
        /// </summary>
        public static Boolean IsSigned<T>(this T value) where T : IComparable<T>
        {
            return value.Le(default);
        }

        /// <summary>
        /// value &gt;= 0
        /// </summary>
        public static Boolean IsUnsigned<T>(this T value) where T : IComparable<T>
        {
            return value.Ge(default);
        }

        public static Boolean Range<T>(this T value, T min, T max) where T : IComparable<T>
        {
            return value.Ge(min) && value.Le(max);
        }

        /// <summary>
        /// Less than is &lt;
        /// </summary>
        public static Boolean Lt<T>(this T value, T other) where T : IComparable<T>
        {
            return value.CompareTo(other) == -1;
        }

        /// <summary>
        /// Greater than is &gt;
        /// </summary>
        public static Boolean Gt<T>(this T value, T other) where T : IComparable<T>
        {
            return value.CompareTo(other) == 1;
        }

        /// <summary>
        /// Less Equal &lt;=
        /// </summary>
        public static Boolean Le<T>(this T value, T other) where T : IComparable<T>
        {
            return value.CompareTo(other) <= 0;
        }

        /// <summary>
        /// Greater Equal &gt;=
        /// </summary>
        public static Boolean Ge<T>(this T value, T other) where T : IComparable<T>
        {
            return value.CompareTo(other) >= 0;
        }
    }
}