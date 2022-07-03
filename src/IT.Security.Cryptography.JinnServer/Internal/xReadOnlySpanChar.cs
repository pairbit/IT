using System;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal static class xReadOnlySpanChar
{
    public static String? Tos(this ReadOnlySpan<Char> chars) => chars.Length == 0 ? null : chars.ToString();
}