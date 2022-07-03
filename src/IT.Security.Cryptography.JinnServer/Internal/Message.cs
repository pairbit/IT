using System;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal static class Message
{
    public static String? Build(String? type, String? comment, String? prefix = null)
    {
        if (type is null) return comment is null ? prefix : $"{prefix}{comment}";

        return comment is null ? $"{prefix}[{type}]" : $"{prefix}[{type}] {comment}";
    }
}