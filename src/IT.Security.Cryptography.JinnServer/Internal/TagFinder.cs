using System;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal class TagFinder
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

    public static Range Inner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var closeIndex = LastClose(chars, name, ns, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                var openIndex = FirstOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    var offset = openIndex + namelen + (nslen == 0 ? 1 : nslen + 2);

                    chars = chars[offset..];

                    openIndex = chars.IndexOf(Gt);

                    if (openIndex > -1) return new Range(openIndex + offset + 1, closeIndex);
                }
            }
        }
        return default;
    }

    public static Range Inner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var ns = LastClose(chars, name, out var closeIndex, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                var openIndex = FirstOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    var offset = openIndex + namelen + (nslen == 0 ? 1 : nslen + 2);

                    chars = chars[offset..];

                    openIndex = chars.IndexOf(Gt);

                    if (openIndex > -1) return new Range(openIndex + offset + 1, closeIndex);
                }
            }
        }
        return default;
    }

    public static ReadOnlySpan<Char> Inner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Range range, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var ns = LastClose(chars, name, out var closeIndex, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                var openIndex = FirstOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    var offset = openIndex + namelen + (nslen == 0 ? 1 : nslen + 2);

                    chars = chars[offset..];

                    openIndex = chars.IndexOf(Gt);

                    if (openIndex > -1)
                    {
                        range = new Range(openIndex + offset + 1, closeIndex);
                        return ns;
                    }
                }
            }
        }
        range = default;
        return default;
    }

    public static Range Outer(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var closeIndex = LastClose(chars, name, ns, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                var openIndex = FirstOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    return new Range(openIndex, closeIndex + namelen + (nslen == 0 ? 3 : nslen + 4));
                }
            }
        }
        return default;
    }

    public static Range Outer(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var ns = LastClose(chars, name, out var closeIndex, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                var openIndex = FirstOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    return new Range(openIndex, closeIndex + namelen + (nslen == 0 ? 3 : nslen + 4));
                }
            }
        }
        return default;
    }

    public static ReadOnlySpan<Char> Outer(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Range range, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var ns = LastClose(chars, name, out var closeIndex, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                var openIndex = FirstOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    range = new Range(openIndex, closeIndex + namelen + (nslen == 0 ? 3 : nslen + 4));
                    return ns;
                }
            }
        }
        range = default;
        return default;
    }

    public static Int32 FirstOpen(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison)
    {
        var len = chars.Length;
        var namelen = name.Length;
        var nslen = ns.Length;

        //Example1: "<Tag>"
        //Example2: "<Tag "
        if (nslen == 0)
        {
            if (len >= namelen + 2)
            {
                do
                {
                    var li = chars.IndexOf(name, comparison);

                    if (li < 1) break;

                    //Debug.Print(chars.ToString());

                    var i = li + namelen;
                    //">"
                    if (i < chars.Length)
                    {
                        var ch = chars[i];
                        if (ch == Gt || ch == Space)
                        {
                            i = li - 1;
                            if (chars[i] == Lt) return i + (len - chars.Length);
                        }
                    }

                    chars = chars[(li + namelen)..];
                } while (true);
            }
        }
        //Example1: "<ns:Tag "
        //Example3: "<ns:Tag>"
        else
        {
            if (len >= namelen + nslen + 3)
            {
                //"<ns:"
                var mi = nslen + 2;
                do
                {
                    var li = chars.IndexOf(name, comparison);

                    if (li < mi) break;

                    //Debug.Print(chars.ToString());

                    var i = li + namelen;
                    if (i < chars.Length)
                    {
                        var ch = chars[i];
                        if (ch == Gt || ch == Space)
                        {
                            i = li - 1;
                            if (chars[i] == Colon)//:
                            {
                                var si = i - nslen;
                                if (chars[si..i].SequenceEqual(ns) && chars[--si] == Lt)
                                    return si + (len - chars.Length);
                            }
                        }
                    }

                    chars = chars[(li + namelen)..];
                } while (true);
            }
        }

        return -1;
    }

    public static Range LastInner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var closeIndex = LastClose(chars, name, ns, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                chars = chars[..closeIndex];

                var openIndex = LastOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    var offset = openIndex + namelen + (nslen == 0 ? 1 : nslen + 2);

                    chars = chars[offset..];

                    openIndex = chars.IndexOf(Gt);

                    if (openIndex > -1) return new Range(openIndex + offset + 1, closeIndex);
                }
            }
        }
        return default;
    }

    public static Range LastInner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var ns = LastClose(chars, name, out var closeIndex, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                chars = chars[..closeIndex];

                var openIndex = LastOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    var offset = openIndex + namelen + (nslen == 0 ? 1 : nslen + 2);

                    chars = chars[offset..];

                    openIndex = chars.IndexOf(Gt);

                    if (openIndex > -1) return new Range(openIndex + offset + 1, closeIndex);
                }
            }
        }
        return default;
    }

    public static ReadOnlySpan<Char> LastInner(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Range range, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var ns = LastClose(chars, name, out var closeIndex, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                chars = chars[..closeIndex];

                var openIndex = LastOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    var offset = openIndex + namelen + (nslen == 0 ? 1 : nslen + 2);

                    chars = chars[offset..];

                    openIndex = chars.IndexOf(Gt);

                    if (openIndex > -1)
                    {
                        range = new Range(openIndex + offset + 1, closeIndex);
                        return ns;
                    }
                }
            }
        }
        range = default;
        return default;
    }

    public static Range LastOuter(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var closeIndex = LastClose(chars, name, ns, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                chars = chars[..closeIndex];

                var openIndex = LastOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    return new Range(openIndex, closeIndex + namelen + (nslen == 0 ? 3 : nslen + 4));
                }
            }
        }
        return default;
    }

    public static Range LastOuter(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var ns = LastClose(chars, name, out var closeIndex, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                chars = chars[..closeIndex];

                var openIndex = LastOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    return new Range(openIndex, closeIndex + namelen + (nslen == 0 ? 3 : nslen + 4));
                }
            }
        }
        return default;
    }

    public static ReadOnlySpan<Char> LastOuter(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Range range, StringComparison comparison)
    {
        var namelen = name.Length;

        //<Tag></Tag>
        if (chars.Length >= (namelen * 2) + 5)
        {
            var ns = LastClose(chars, name, out var closeIndex, comparison);

            //<Tag>
            if (closeIndex >= namelen + 2)
            {
                chars = chars[..closeIndex];

                var openIndex = LastOpen(chars, name, ns, comparison);
                if (openIndex > -1)
                {
                    var nslen = ns.Length;
                    range = new Range(openIndex, closeIndex + namelen + (nslen == 0 ? 3 : nslen + 4));
                    return ns;
                }
            }
        }
        range = default;
        return default;
    }

    public static Int32 LastOpen(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison)
    {
        var namelen = name.Length;
        var nslen = ns.Length;

        //Example1: "<Tag>"
        //Example2: "<Tag "
        if (nslen == 0)
        {
            if (chars.Length >= namelen + 2)
            {
                do
                {
                    var li = chars.LastIndexOf(name, comparison);

                    if (li < 1) break;

                    //Debug.Print(chars.ToString());

                    var i = li + namelen;
                    //">"
                    if (i < chars.Length)
                    {
                        var ch = chars[i];
                        if (ch == Gt || ch == Space)
                        {
                            i = li - 1;
                            if (chars[i] == Lt) return i;
                        }
                    }

                    chars = chars[..li];
                } while (true);
            }
        }
        //Example1: "<ns:Tag "
        //Example3: "<ns:Tag>"
        else
        {
            if (chars.Length >= namelen + nslen + 3)
            {
                //"<ns:"
                var mi = nslen + 2;
                do
                {
                    var li = chars.LastIndexOf(name, comparison);

                    if (li < mi) break;

                    //Debug.Print(chars.ToString());

                    var i = li + namelen;
                    if (i < chars.Length)
                    {
                        var ch = chars[i];
                        if (ch == Gt || ch == Space)
                        {
                            i = li - 1;
                            if (chars[i] == Colon)//:
                            {
                                var si = i - nslen;
                                if (chars[si..i].SequenceEqual(ns) && chars[--si] == Lt)
                                    return si;
                            }
                        }
                    }

                    chars = chars[..li];
                } while (true);
            }
        }

        return -1;
    }

    public static Int32 LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison)
    {
        //Close tags

        //Example3: "</Tag>"
        //Template: "Tag>"

        var namelen = name.Length;
        var nslen = ns.Length;

        if (nslen == 0)
        {
            if (chars.Length >= namelen + 3)
            {
                do
                {
                    var li = chars.LastIndexOf(name, comparison);

                    if (li < 2) break;

                    //Debug.Print(chars.ToString());

                    var i = li + namelen;
                    //">"
                    if (i < chars.Length && chars[i] == Gt)
                    {
                        i = li - 1;
                        if (chars[i] == Slash && chars[--i] == Lt)
                            return i;
                    }

                    chars = chars[..li];
                } while (true);
            }
        }
        //Example1: "</ns:Tag>"
        //Example2: "</*:Tag>"
        else
        {
            if (chars.Length >= namelen + nslen + 4)
            {
                //"</ns:"
                var mi = nslen + 3;
                do
                {
                    var li = chars.LastIndexOf(name, comparison);

                    if (li < mi) break;

                    //Debug.Print(chars.ToString());

                    var i = li + namelen;
                    if (i < chars.Length && chars[i] == Gt)//>
                    {
                        i = li - 1;
                        if (chars[i] == Colon)//:
                        {
                            var si = i - nslen;
                            if (chars[si..i].SequenceEqual(ns) && chars[--si] == Slash && chars[--si] == Lt)
                                return si;
                        }

                    }

                    chars = chars[..li];
                } while (true);
            }
        }

        return -1;
    }
    
    public static Int32 LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison)
    {
        //Close tags
        //Example1: "</ns:Tag>"
        //Example2: "</*:Tag>"
        //Example3: "</Tag>"
        //Template: "Tag>"

        var namelen = name.Length;

        if (chars.Length >= namelen + 3)
        {
            do
            {
                //"Tag"
                var index = chars.LastIndexOf(name, comparison);

                if (index < 2) break;

                //Debug.Print(chars.ToString());

                var i = index + namelen;
                //">"
                if (i < chars.Length && chars[i] == Gt)
                {
                    i = index - 1;
                    var ch = chars[i];

                    //</Tag>
                    if (ch == Slash)
                    {
                        if (chars[--i] == Lt) return i;
                    }
                    //:Tag>
                    else if (ch == Colon)
                    {
                        i = chars[..i].LastIndexOf(OpenCloser);
                        if (i > -1) return i;
                    }
                }

                chars = chars[..index];
            } while (true);
        }
        return -1;
    }

    public static ReadOnlySpan<Char> LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Int32 index, StringComparison comparison)
    {
        //Close tags
        //Example1: "</ns:Tag>"
        //Example2: "</*:Tag>"
        //Example3: "</Tag>"
        //Template: "Tag>"

        var namelen = name.Length;

        if (chars.Length >= namelen + 3)
        {
            do
            {
                //"Tag"
                var li = chars.LastIndexOf(name, comparison);

                if (li < 2) break;

                //Debug.Print(chars.ToString());

                var i = li + namelen;
                //">"
                if (i < chars.Length && chars[i] == Gt)
                {
                    i = li - 1;
                    var ch = chars[i];

                    //</Tag>
                    if (ch == Slash)
                    {
                        if (chars[--i] == Lt)
                        {
                            index = i;
                            return default;
                        }
                    }
                    //:Tag>
                    else if (ch == Colon)
                    {
                        var si = chars[..i].LastIndexOf(OpenCloser);
                        if (si > -1)
                        {
                            index = si;
                            return chars[(si + 2)..i];
                        }
                    }
                }

                chars = chars[..li];
            } while (true);
        }
        index = -1;
        return default;
    }
}