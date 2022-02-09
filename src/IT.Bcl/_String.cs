#if NETSTANDARD2_0

namespace System
{
    public static class _String
    {
        public static String[] Split(this String value, String separator, StringSplitOptions options = StringSplitOptions.None)
            => value.Split(new[] { separator }, options);

        public static Boolean StartsWith(this String value, Char ch) => value.StartsWith(ch.ToString());

        public static Boolean EndsWith(this String value, Char ch) => value.EndsWith(ch.ToString());

        public static Boolean Contains(this String value, Char ch) => value.Contains(ch.ToString());
    }
}

#endif