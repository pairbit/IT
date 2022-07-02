#if NETSTANDARD2_0

using System;
using System.Runtime.CompilerServices;

namespace IT.Text;

internal static class xITagFinder
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range Inner(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, String ns, StringComparison comparison)
        => finder.Inner(chars, name.AsSpan(), ns.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range Inner(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, StringComparison comparison)
        => finder.Inner(chars, name.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Char> Inner(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, out Range range, StringComparison comparison)
        => finder.Inner(chars, name.AsSpan(), out range, comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range Outer(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, String ns, StringComparison comparison)
        => finder.Outer(chars, name.AsSpan(), ns.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range Outer(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, StringComparison comparison)
        => finder.Outer(chars, name.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Char> Outer(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, out Range range, StringComparison comparison)
        => finder.Outer(chars, name.AsSpan(), out range, comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int32 FirstOpen(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, String ns, StringComparison comparison)
        => finder.FirstOpen(chars, name.AsSpan(), ns.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range LastInner(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, String ns, StringComparison comparison)
        => finder.LastInner(chars, name.AsSpan(), ns.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range LastInner(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, StringComparison comparison)
        => finder.LastInner(chars, name.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Char> LastInner(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, out Range range, StringComparison comparison)
        => finder.LastInner(chars, name.AsSpan(), out range, comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range LastOuter(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, String ns, StringComparison comparison)
        => finder.LastOuter(chars, name.AsSpan(), ns.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range LastOuter(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, StringComparison comparison)
        => finder.LastOuter(chars, name.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Char> LastOuter(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, out Range range, StringComparison comparison)
        => finder.LastOuter(chars, name.AsSpan(), out range, comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int32 LastOpen(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, String ns, StringComparison comparison)
        => finder.LastOpen(chars, name.AsSpan(), ns.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int32 LastClose(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, String ns, StringComparison comparison)
        => finder.LastClose(chars, name.AsSpan(), ns.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int32 LastClose(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, StringComparison comparison)
        => finder.LastClose(chars, name.AsSpan(), comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Char> LastClose(this ITagFinder finder, ReadOnlySpan<Char> chars, String name, out Int32 index, StringComparison comparison)
        => finder.LastClose(chars, name.AsSpan(), out index, comparison);
}

#endif
