using System;
using System.Globalization;
using System.Linq;

namespace IT.Ext
{
    public static class xString
    {
        public static String ToCamelCase(this String value)
        {
            return value.IsEmpty() || value.Length < 2 ? value : Char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static String ToTitleCase(this String value, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null) cultureInfo = CultureInfo.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(value);
        }

        /// <summary>
        /// value ?? String.Empty
        /// </summary>
        public static String Empty(this String value) => value ?? String.Empty;

        /// <summary>
        /// value.IsEmpty() ? null : value
        /// </summary>
        public static String UnEmpty(this String value) => value.IsEmpty() ? null : value;

        /// <summary>
        /// value ?? " "
        /// </summary>
        public static String Space(this String value) => value ?? " ";

        /// <summary>
        /// value.IsSpace() ? null : value
        /// </summary>
        public static String UnSpace(this String value) => value.IsSpace() ? null : value;

        public static String TryTrim(this String value, params Char[] trimChars)
        {
            if (value != null)
            {
                value = value.Trim(trimChars);
                if (value.Length == 0) value = null;
            }
            return value;
        }

        public static String Format(this String value, params Object[] args)
        {
            return value != null ? String.Format(value, args.Select(x => x ?? "null").ToArray()) : value;
        }

        public static String Format(this String value, IFormatProvider provider, params Object[] args)
        {
            return String.Format(provider, value, args);
        }

        /// <summary>
        /// Сократить строку
        /// </summary>
        /// <param name="value">Входная строка</param>
        /// <param name="maxLength">Максимальная длинна строки</param>
        /// <param name="exact">Сократить точно на указанное количество символов</param>
        /// <param name="end">Окончание в случае если строка была обрезана</param>
        /// <param name="separators">Разделители</param>
        /// <returns>Обрезанная строка</returns>
        public static String Shorten(this String value, Int32 maxLength, Boolean exact = false, String end = "...", params Char[] separators)
        {
            if (maxLength > 0 && !value.IsEmpty())
            {
                if (maxLength >= value.Length) return value;
                if (end.IsSpace()) end = String.Empty;
                var max = maxLength - end.Length;
                if (max > 0)
                {
                    if (!exact)
                    {
                        var isEnd = false;
                        do
                        {
                            if (IsSeparator(value[max], separators))
                            {
                                isEnd = true;
                            }
                            else if (isEnd)
                            {
                                max++;
                                break;
                            }
                        } while (--max > 0);
                    }
                    if (max > 0) return value.Substring(0, max) + end;
                }
            }
            return String.Empty;
        }

        private static Boolean IsSeparator(Char ch, params Char[] separators) => ch.IsSpace() || ch.In(separators) || ch == '.';

        public static String[] Split(this String value, String separator, StringSplitOptions options = default) => value.Split(separator.InArray(), options);

        public static String[] SplitNewLine(this String value) => value.Split(Environment.NewLine);

        public static Char? TryGet(this String value, Int32 index) => value != null && value.Length > index && index >= 0 ? (Char?)value[index] : null;

        public static String WithoutBOM(this String value) => value.TryGet(0)?.IsBOM() ?? false ? value.Substring(1) : value;
    }
}