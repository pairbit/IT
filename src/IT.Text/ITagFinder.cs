using System;

namespace IT.Text;

public interface ITagFinder
{
    //Boolean Contains(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal);

    //Boolean Contains(ReadOnlySpan<Char> chars, String name, StringComparison comparison = StringComparison.Ordinal);

    //Range First(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal);

    //Range First(ReadOnlySpan<Char> chars, String name, StringComparison comparison = StringComparison.Ordinal);

    //Range Last(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal);

    //Range Last(ReadOnlySpan<Char> chars, String name, StringComparison comparison = StringComparison.Ordinal);

    //Int32 FirstOpen(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal);

    //Int32 FirstClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlyMemory<Char>? ns, StringComparison comparison = StringComparison.Ordinal);

    //Int32 FirstClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out ReadOnlyMemory<Char>? ns, StringComparison comparison = StringComparison.Ordinal);

    //Int32 FirstClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison = StringComparison.Ordinal);

    //Int32 LastOpen(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison = StringComparison.Ordinal);

    Int32 LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison);

    ReadOnlySpan<Char> LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Int32 index, StringComparison comparison);

    Int32 LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison);
}