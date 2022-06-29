using System;

namespace IT.Text;

public interface ITagFinder
{
    Boolean Contains(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal);

    Boolean Contains(ReadOnlySpan<Char> chars, String name, StringComparison comparison = StringComparison.Ordinal);

    Range Find(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal);

    Range Find(ReadOnlySpan<Char> chars, String name, StringComparison comparison = StringComparison.Ordinal);

    Int32 FindOpen(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal);

    Int32 FindClose(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal);

    Int32 FindClose(ReadOnlySpan<Char> chars, String name, out String? ns, StringComparison comparison = StringComparison.Ordinal);

    Int32 FindClose(ReadOnlySpan<Char> chars, String name, StringComparison comparison = StringComparison.Ordinal);
}