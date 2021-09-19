using IT.Comparing;
using IT.Exceptions;
using IT.Ext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace IT.Validation
{
    //, [CallerMemberName] String memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0
    //https://andrey.moveax.ru/post/csharp-debugger-attributes
    [DebuggerNonUserCode]
    public class Arg
    {
        #region Range

        public static Boolean Range(int value, int min, int max, String arg = null, String msg = null, Boolean off = false)
        {
            var valid = value >= min && value <= max;
            return valid || off ? valid : throw Ex.Range(arg, value, msg.Format(value, min, max));
        }

        /// <summary>
        /// value &gt; 0
        /// </summary>
        public static Boolean Positive<T>(T value, String arg = null, String msg = null, Boolean off = false) where T : IComparable<T>
        {
            var valid = value.IsPositive();
            return valid || off ? valid : throw Ex.Range(arg, value, msg);
        }

        /// <summary>
        /// value &lt; 0
        /// </summary>
        public static Boolean Negative<T>(T value, String arg = null, String msg = null, Boolean off = false) where T : IComparable<T>
        {
            var valid = value.IsNegative();
            return valid || off ? valid : throw Ex.Range(arg, value, msg);
        }

        public static Boolean Signed<T>(T value, String arg = null, String msg = null, Boolean off = false) where T : IComparable<T>
        {
            var valid = value.IsSigned();
            return valid || off ? valid : throw Ex.Range(arg, value, msg);
        }

        public static Boolean Unsigned<T>(T value, String arg = null, String msg = null, Boolean off = false) where T : IComparable<T>
        {
            var valid = value.IsUnsigned();
            return valid || off ? valid : throw Ex.Range(arg, value, msg);
        }

        public static Boolean Enum(Enum queue)
        {
            return true;
        }

        #endregion Range

        #region Invalid

        public static Boolean True(Boolean valid, String msg = null, Boolean off = false)
        {
            return valid || off ? valid : throw Ex.Invalid(msg);
        }

        public static Boolean False(Boolean value, String msg = null, Boolean off = false)
        {
            var valid = !value;
            return valid || off ? valid : throw Ex.Invalid(msg);
        }

        public static Boolean Eq(Object obj, Object value, String msg = null, Boolean off = false)
        {
            var valid = obj.Equals(value);
            return valid || off ? valid : throw Ex.Invalid(msg.Format(obj, value));
        }

        public static Boolean NotEq(Object obj, Object value, String msg = null, Boolean off = false)
        {
            var valid = !obj.Equals(value);
            return valid || off ? valid : throw Ex.Invalid(msg.Format(obj, value));
        }

        /// <summary>
        /// Regex.IsMatch
        /// </summary>
        public static Boolean Match(String value, String pattern, String msg = null, Boolean off = false, RegexOptions? options = null)
        {
            var valid = options == null ? Regex.IsMatch(value, pattern) : Regex.IsMatch(value, pattern, options.Value);
            return valid || off ? valid : throw Ex.Invalid(msg.Format(value, pattern));
        }

        #endregion Invalid

        public static Boolean Null(Object value, String arg = null, String msg = null, Boolean off = false)
        {
            var valid = value == null;
            return valid || off ? valid : throw Ex.NotNull(arg, msg);
        }

        public static Boolean NotNull(Object value, String arg = null, String msg = null, Boolean off = false)
        {
            var valid = value != null;
            return valid || off ? valid : throw Ex.Null(arg, msg);
        }

        public static Boolean NotSpace(IEnumerable value, String arg = null, String msg = null, Boolean off = false)
        {
            var valid = NotNull(value, arg, msg, off) && !value.Cast().IsSpace();
            return valid || off ? valid : throw Ex.Space(arg, msg);
        }

        public static Boolean Unique(IEnumerable value, String arg = null, String msg = null, Boolean off = false, IEqualityComparer<Object> comparer = null)
        {
            var valid = value.Cast().IsUnique(comparer);
            return valid || off ? valid : throw Ex.Unique(arg, msg);
        }

        public static Boolean Unique<T, TKey>(IEnumerable<T> value, Func<T, TKey> keySelector, String arg = null, String msg = null, Boolean off = false, IEqualityComparer<TKey> comparer = null)
        {
            var duplicate = value.GetDuplicates(keySelector, comparer).FirstOrDefault();
            var valid = duplicate == null;
            return valid || off ? valid : throw Ex.Unique(arg, msg.Format(duplicate));
        }

        public static Boolean Required(Object value, String arg = null, String msg = null, Boolean off = false)
        {
            var valid = IsRequired(value);
            return valid || off ? valid : throw Ex.Required(arg, msg);
        }

        private static Boolean IsRequired(Object value)
        {
            if (value == null) return false;
            if (value is String str) return !str.IsSpace();
            if (value is IEnumerable ie) return NotSpace(ie, off: true);
            return true;
        }

        internal static void Single(IEnumerable values)
        {
            throw new NotImplementedException();
        }
    }
}