using System.Linq;
using System.Xml.Serialization;

namespace System;

/// <summary>
/// Markup language
/// </summary>
public static class Ml
{
    public const char Lt = '<';
    public const char Gt = '>';
    public const char Eq = '=';
    public const char BSlash = '\\';
    public const char Slash = '/';
    public const char Space = ' ';
    public const char Quote = '\'';
    public const char DQuote = '"';
    public const char Colon = ':';

    public static readonly string TagCloser = string.Concat(Slash, Gt);

    public static string TagName(string name, string? prefix = null) => (prefix == null ? "" : prefix + Colon) + name;

    public static string OpenTag(string name, string? prefix = null, bool withAttrs = false) => Lt + TagName(name, prefix) + (withAttrs ? Space : Gt);

    public static string CloseTag(string name, string? prefix = null) => string.Concat(Lt, Slash, TagName(name, prefix), Gt);

    public static string AttrValue(string value, bool dquote = true) => string.Concat(Eq, dquote ? DQuote : Quote, value, dquote ? DQuote : Quote);

    public static string FindTagPrefix(string ml, string tagName, string ns)
    {
        var end = ml.IndexOfAttrValue(ns);
        if (end == -1) return null;
        var start = ml.LastIndexOf(Colon, end);
        if (start == -1) return null;
        return ml[(start + 1)..end];
    }

    public static string FindRoot(string ml, Type type, bool? withAttrs = null)
    {
        if (ml is null) throw new ArgumentNullException(ml);

        var name = type.Name;
        string prefix = null;
        var xmlRoot = type.GetCustomAttributes(typeof(XmlRootAttribute), false).Cast<XmlRootAttribute>().SingleOrDefault();
        if (xmlRoot != null)
        {
            var elementName = xmlRoot.ElementName;
            if (elementName is not null && elementName.Length > 0) name = elementName;
            if (xmlRoot.Namespace != null) prefix = FindTagPrefix(ml, name, xmlRoot.Namespace);
        }

        var openTagIndex = ml.IndexOf(OpenTag(name, prefix, withAttrs ?? false));
        if (openTagIndex == -1 && withAttrs == null) openTagIndex = ml.IndexOf(OpenTag(name, prefix, true));

        if (openTagIndex < 0) throw new ArgumentOutOfRangeException(nameof(openTagIndex), $"Tag '{name}' not found");

        var closeTag = CloseTag(name, prefix);
        var closeTagIndex = ml.IndexOf(closeTag);
        if (closeTagIndex < 0 ) throw new ArgumentOutOfRangeException(nameof(openTagIndex), $"Tag '{name}' not found");

        return ml[openTagIndex..(closeTagIndex + closeTag.Length)];
    }

    private static int IndexOfAttrValue(this string ml, string attrValue, int startIndex = 0)
    {
        var index = ml.IndexOf(AttrValue(attrValue), startIndex);
        if (index == -1) index = ml.IndexOf(AttrValue(attrValue, false), startIndex);
        return index;
    }
}