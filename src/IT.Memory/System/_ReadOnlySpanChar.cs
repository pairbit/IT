#if NETSTANDARD2_0 || NETSTANDARD2_1

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System;

public static class _ReadOnlySpanChar
{
    public static int LastIndexOf(this ReadOnlySpan<Char> span, ReadOnlySpan<Char> value, StringComparison comparisonType)
    {
        Debug.WriteLine("!!SLOWLY!!");
        return span.ToString().LastIndexOf(value.ToString(), comparisonType);
    }

#if NETSTANDARD2_0

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf(this ReadOnlySpan<Char> span, String value) => span.LastIndexOf(value.AsSpan());

#endif
}

#endif