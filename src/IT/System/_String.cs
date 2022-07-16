#if NETSTANDARD2_0

using System.Buffers;

namespace System;

public static class _String
{
    public static String Create<TState>(int length, TState state, SpanAction<char, TState> action)
    {
        if (length == 0) return String.Empty;

        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
        if (action is null) throw new ArgumentNullException(nameof(action));

        var pool = ArrayPool<Char>.Shared;
        var rented = pool.Rent(length);

        try
        {
            action(rented, state);
            return new String(rented, 0, length);
        }
        finally
        {
            pool.Return(rented);
        }
    }

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