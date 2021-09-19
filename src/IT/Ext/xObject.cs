using System;
using System.Collections;

namespace IT.Ext
{
    public static class xObject
    {
        public static Boolean IsSpace(Object value)
        {
            if (value is Char ch)
            {
                if (!Char.IsWhiteSpace(ch)) return false;
            }
            else if (value is String str)
            {
                if (!str.IsSpace()) return false;
            }
            else if (value is IEnumerable values)
            {
                if (!values.IsEmpty()) return false;
            }
            else if (value != null) return false;

            return true;
        }
    }
}