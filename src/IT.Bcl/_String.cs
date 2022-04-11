#if NETSTANDARD2_0

namespace System;

public static class _String
{
    public static String[] Split(this String value, String separator, StringSplitOptions options = StringSplitOptions.None)
        => value.Split(new[] { separator }, options);

    public static Boolean StartsWith(this String value, Char ch) => value.Length != 0 && value[0] == ch;

    public static Boolean EndsWith(this String value, Char ch)
    {
        var lastPos = value.Length - 1;
        return lastPos >= 0 && value[lastPos] == ch;
    }

    public static Boolean Contains(this String value, String str, StringComparison comparisonType)
        => value.IndexOf(str, comparisonType) >= 0;

    public static Boolean Contains(this String value, Char ch) => value.IndexOf(ch) >= 0;

    public static Boolean Contains(this String value, Char ch, StringComparison comparisonType)
        => value.IndexOf(ch.ToString(), comparisonType) >= 0;

    public static Int32 IndexOf(this String value, Char ch, StringComparison comparisonType)
        => value.IndexOf(ch.ToString(), comparisonType);
}

#endif