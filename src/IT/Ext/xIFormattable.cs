using System;

namespace IT.Ext
{
    public static class xIFormattable
    {
        /// <summary>
        /// ToString
        /// </summary>
        public static String Tos(this IFormattable value, String format, IFormatProvider provider = null) => value?.ToString(format, provider);
    }
}