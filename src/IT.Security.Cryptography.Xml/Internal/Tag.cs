using System;

namespace IT.Security.Cryptography.Internal;

internal static class Tag
{
    public const Char Lt = '<';
    public const Char Gt = '>';
    public const Char Eq = '=';
    public const Char BSlash = '\\';
    public const Char Slash = '/';
    public const Char Space = ' ';
    public const Char Quote = '\'';
    public const Char DQuote = '"';
    public const Char Colon = ':';
    public const String OpenCloser = "</";
    public const String EndCloser = "/>";

    public static String Remove(String ml, String name, StringComparison comparison = StringComparison.Ordinal)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (ml.Length >= (namelen * 2) + 5)
        {
            var closeIndex = FindClose(ml, name, out var ns, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                var openIndex = FindOpen(ml, name, ns, comparison);
                if (openIndex > -1)
                {
                    closeIndex += namelen + 3 + (ns == null ? 0 : ns.Length + 1);
                    ml = ml.Remove(openIndex, closeIndex - openIndex);
                }
            }
        }
        return ml;
    }

    public static Int32 FindOpen(String ml, String name, String? ns, StringComparison comparison = StringComparison.Ordinal)
    {
        //Open tags with attr
        //Example1: "<ns:Tag "
        //Example2: "<*:Tag "
        //Example3: "<Tag "

        //Open tags without attr
        //Example1: "<ns:Tag>"
        //Example2: "<*:Tag>"
        //Example3: "<Tag>"

        var tag = ns == null ? Lt + name : Lt + ns + Colon + name;
        var len = tag.Length;

        if (ml.Length > len)
        {
            var index = ml.IndexOf(tag, comparison);
            if (index > -1)
            {
                var ch = ml[index + len];
                if (ch == Space || ch == Gt) return index;
            }
        }
        return -1;
    }

    public static Int32 FindClose(String ml, String name, out String? ns, StringComparison comparison = StringComparison.Ordinal)
    {
        //Close tags
        //Example1: "</ns:Tag>"
        //Example2: "</*:Tag>"
        //Example3: "</Tag>"
        //Template: "Tag>"
        if (ml.Length >= name.Length + 3)
        {
            var lastIndex = ml.LastIndexOf(name + Gt, comparison);

            if (lastIndex > -1)
            {
                lastIndex--;
                var ch = ml[lastIndex];

                //</Tag>
                //:Tag>
                if (ch == Slash)
                {
                    lastIndex--;
                    if (ml[lastIndex] == Lt)
                    {
                        ns = null;
                        return lastIndex;
                    }
                }
                else if (ch == Colon)
                {
                    var startIndex = ml.LastIndexOf(OpenCloser, lastIndex - 1);
                    if (startIndex > -1)
                    {
                        //ns = ml[(startIndex + 2)..lastIndex];
                        ns = ml.Substring(startIndex + 2, lastIndex - (startIndex + 2));
                        return startIndex;
                    }
                }
            }
        }
        ns = null;
        return -1;
    }
}