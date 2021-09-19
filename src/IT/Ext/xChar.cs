using System;

namespace IT.Ext
{
    public static class xChar
    {
        public const Char Space = ' ';

        /// <summary>
        /// ' '.Repeat(count)
        /// </summary>
        public static String Spaces(Int32 count) => Space.Repeat(count);

        /// <summary>
        /// Char.IsWhiteSpace(value)
        /// </summary>
        public static Boolean IsSpace(this Char value) => Char.IsWhiteSpace(value);

        /// <summary>
        /// new(value, count)
        /// </summary>
        public static String Repeat(this Char value, Int32 count) => new String(value, count);

        /// <summary>
        /// value == 65279
        /// </summary>
        /// <remarks>
        /// http://www.fileformat.info/info/unicode/char/feff/index.htm
        /// </remarks>
        public static Boolean IsBOM(this Char value) => value == 65279;
    }
}