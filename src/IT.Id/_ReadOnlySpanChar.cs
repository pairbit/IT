#if NETSTANDARD2_0

namespace System;

internal static class _ReadOnlySpanChar
{
    public static bool SequenceEqual(this ReadOnlySpan<char> span, String other)
        => span.SequenceEqual(other.AsSpan());
}

#endif