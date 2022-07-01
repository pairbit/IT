using System;

namespace IT.Text;

public interface ITagFinder
{
    //Boolean Contains(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal);

    //Boolean Contains(ReadOnlySpan<Char> chars, String name, StringComparison comparison = StringComparison.Ordinal);

    //Range First(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal);

    //Range First(ReadOnlySpan<Char> chars, String name, StringComparison comparison = StringComparison.Ordinal);

    //Int32 FirstClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlyMemory<Char>? ns, StringComparison comparison = StringComparison.Ordinal);

    //Int32 FirstClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out ReadOnlyMemory<Char>? ns, StringComparison comparison = StringComparison.Ordinal);

    //Int32 FirstClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison = StringComparison.Ordinal);

    Range Inner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison);

    Range Inner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison);

    ReadOnlySpan<Char> Inner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Range range, StringComparison comparison);

    Range Outer(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison);

    Range Outer(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison);

    ReadOnlySpan<Char> Outer(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Range range, StringComparison comparison);

    /// <summary>
    /// Данный метод ищет только начало открывающегося тега, последний '&gt;' не ищет!!!
    /// </summary>
    Int32 FirstOpen(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison);

    Range LastInner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison);

    Range LastInner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison);

    ReadOnlySpan<Char> LastInner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Range range, StringComparison comparison);

    Range LastOuter(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison);

    Range LastOuter(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison);

    ReadOnlySpan<Char> LastOuter(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Range range, StringComparison comparison);

    /// <summary>
    /// Данный метод ищет только начало открывающегося тега, последний '&gt;' не ищет!!!
    /// </summary>
    Int32 LastOpen(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison);

    Int32 LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison);

    Int32 LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison);

    ReadOnlySpan<Char> LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Int32 index, StringComparison comparison);
}