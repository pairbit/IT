using System;
using System.Collections;

namespace IT.Ext
{
    public static class xIEnumerator
    {
        public static Object Next(this IEnumerator enumerator)
        {
            return enumerator != null && enumerator.MoveNext() ? enumerator.Current : null;
        }
    }
}