using System;
using System.Diagnostics;

namespace IT.Text;

public class TagFinder : ITagFinder
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

    //    public Range Find(ReadOnlySpan<Char> chars, string name, string? ns, StringComparison comparison)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Range Find(ReadOnlySpan<Char> chars, string name, StringComparison comparison)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //public static String Remove(String ml, String name, String? ns, StringComparison comparison = StringComparison.Ordinal)
    //    //{
    //    //    var namelen = name.Length;

    //    //    //<Tag></Tag>
    //    //    if (ml.Length >= (namelen * 2) + 5)
    //    //    {
    //    //        var closeIndex = FindClose(ml, name, ns, comparison);

    //    //        //<Tag>
    //    //        if (closeIndex >= namelen + 2)
    //    //        {
    //    //            var openIndex = FindOpen(ml, name, ns, comparison);
    //    //            if (openIndex > -1)
    //    //            {
    //    //                closeIndex += namelen + 3 + (ns == null ? 0 : ns.Length + 1);
    //    //                ml = ml.Remove(openIndex, closeIndex - openIndex);
    //    //            }
    //    //        }
    //    //    }
    //    //    return ml;
    //    //}

    //    //public static String Remove(String ml, String name, StringComparison comparison = StringComparison.Ordinal)
    //    //{
    //    //    var namelen = name.Length;

    //    //    //<Tag></Tag>
    //    //    if (ml.Length >= (namelen * 2) + 5)
    //    //    {
    //    //        var closeIndex = FindClose(ml, name, out var ns, comparison);

    //    //        //<Tag>
    //    //        if (closeIndex >= namelen + 2)
    //    //        {
    //    //            var openIndex = FindOpen(ml, name, ns, comparison);
    //    //            if (openIndex > -1)
    //    //            {
    //    //                closeIndex += namelen + 3 + (ns == null ? 0 : ns.Length + 1);
    //    //                ml = ml.Remove(openIndex, closeIndex - openIndex);
    //    //            }
    //    //        }
    //    //    }
    //    //    return ml;
    //    //}

    //    public Boolean Contains(ReadOnlySpan<Char> chars, String name, String? ns, StringComparison comparison = StringComparison.Ordinal)
    //    {
    //        var namelen = name.Length;

    //        //<Tag></Tag>
    //        if (chars.Length >= (namelen * 2) + 5)
    //        {
    //            var closeIndex = FindClose(chars, name, ns, comparison);

    //            //<Tag>
    //            if (closeIndex >= namelen + 2)
    //            {
    //                var openIndex = FindOpen(chars, name, ns, comparison);
    //                if (openIndex > -1) return true;
    //            }
    //        }
    //        return false;
    //    }

    //    public Boolean Contains(ReadOnlySpan<Char> chars, String name, StringComparison comparison = StringComparison.Ordinal)
    //    {
    //        var namelen = name.Length;

    //        //<Tag></Tag>
    //        if (chars.Length >= (namelen * 2) + 5)
    //        {
    //            var closeIndex = FindClose(chars, name, out var ns, comparison);

    //            //<Tag>
    //            if (closeIndex >= namelen + 2)
    //            {
    //                var openIndex = FindOpen(chars, name, ns, comparison);
    //                if (openIndex > -1) return true;
    //            }
    //        }
    //        return false;
    //    }

    //public Int32 FirstClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison = StringComparison.Ordinal)
    //{
    //    //Close tags
    //    //Example1: "</ns:Tag>"
    //    //Example2: "</*:Tag>"
    //    //Example3: "</Tag>"
    //    //Template: "Tag>"

    //    var namelen = name.Length;
    //    var len = chars.Length;

    //    if (len >= namelen + 3)
    //    {
    //        do
    //        {
    //            //"Tag>"
    //            var index = chars.IndexOf(name, comparison);

    //            if (index == -1) break;

    //            if (index > 1)
    //            {
    //                var i = index;

    //                i--;
    //                var ch = chars[i];

    //                //</Tag>
    //                if (ch == Slash)
    //                {
    //                    i--;
    //                    if (chars[i] == Lt) return i + (len - chars.Length);
    //                }
    //                //:Tag>
    //                else if (ch == Colon)
    //                {
    //                    i--;
    //                    chars = chars[..i];
    //                    i = chars.LastIndexOf(OpenCloser.AsSpan());
    //                    if (i > -1) return i + (len - chars.Length);
    //                }
    //            }

    //            chars = chars[(index + namelen + 1)..];

    //            Debug.Print(chars.ToString());
    //        } while (true);
    //    }
    //    return -1;
    //}

    public Int32 LastOpen(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison)
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

    public Int32 LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, ReadOnlySpan<Char> ns, StringComparison comparison)
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

    public Int32 LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, StringComparison comparison)
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

    public ReadOnlySpan<Char> LastClose(ReadOnlySpan<Char> chars, ReadOnlySpan<Char> name, out Int32 index, StringComparison comparison)
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