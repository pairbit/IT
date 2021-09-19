using System;

namespace IT.Comparing
{
    public static class xIComparable
    {
        public static Boolean Range(this IComparable value, Object min, Object max)
        {
            return value.Ge(min) && value.Le(max);
        }

        /// <summary>
        /// Less than is &lt;
        /// </summary>
        public static Boolean Lt(this IComparable value, Object other)
        {
            return value.CompareTo(other) == -1;
        }

        /// <summary>
        /// Greater than is &gt;
        /// </summary>
        public static Boolean Gt(this IComparable value, Object other)
        {
            return value.CompareTo(other) == 1;
        }

        /// <summary>
        /// Less Equal &lt;=
        /// </summary>
        public static Boolean Le(this IComparable value, Object other)
        {
            return value.CompareTo(other) <= 0;
        }

        /// <summary>
        /// Greater Equal &gt;=
        /// </summary>
        public static Boolean Ge(this IComparable value, Object other)
        {
            return value.CompareTo(other) >= 0;
        }
    }
}