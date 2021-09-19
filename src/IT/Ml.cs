using IT.Comparing;
using IT.Ext;
using IT.Validation;
using System;
using System.Xml.Serialization;

namespace IT
{
    /// <summary>
    /// Markup language
    /// </summary>
    public static class Ml
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

        public static readonly String TagCloser = String.Concat(Slash, Gt);

        public static String TagName(String name, String prefix = null) => (prefix == null ? "" : prefix + Colon) + name;

        public static String OpenTag(String name, String prefix = null, Boolean withAttrs = false) => Lt + TagName(name, prefix) + (withAttrs ? Space : Gt);

        public static String CloseTag(String name, String prefix = null) => String.Concat(Lt, Slash, TagName(name, prefix), Gt);

        public static String AttrValue(String value, Boolean dquote = true) => String.Concat(Eq, dquote ? DQuote : Quote, value, dquote ? DQuote : Quote);

        public static String FindTagPrefix(String ml, String tagName, String ns)
        {
            var end = ml.IndexOfAttrValue(ns);
            if (end == -1) return null;
            var start = ml.LastIndexOf(Colon, end);
            if (start == -1) return null;
            return ml.Substring(start + 1, end);
        }

        public static String FindRoot(String ml, Type type, Boolean? withAttrs = null)
        {
            Arg.NotNull(ml, nameof(ml));
            var name = type.Name;
            String prefix = null;
            var xmlRoot = type.GetAttribute<XmlRootAttribute>();
            if (xmlRoot != null)
            {
                if (!xmlRoot.ElementName.IsEmpty()) name = xmlRoot.ElementName;
                if (xmlRoot.Namespace != null) prefix = FindTagPrefix(ml, name, xmlRoot.Namespace);
            }

            var openTagIndex = ml.IndexOf(OpenTag(name, prefix, withAttrs ?? false));
            if (openTagIndex == -1 && withAttrs == null) openTagIndex = ml.IndexOf(OpenTag(name, prefix, true));

            var msg = "Tag '{0}' not found".Format(name);
            Arg.True(openTagIndex.IsUnsigned(), msg);

            var closeTag = CloseTag(name, prefix);
            var closeTagIndex = ml.IndexOf(closeTag);
            Arg.True(closeTag.IsUnsigned(), msg);

            return ml.Substring(openTagIndex, closeTagIndex + closeTag.Length);
        }

        private static Int32 IndexOfAttrValue(this String ml, String attrValue, Int32 startIndex = 0)
        {
            var index = ml.IndexOf(AttrValue(attrValue), startIndex);
            if (index == -1) index = ml.IndexOf(AttrValue(attrValue, false), startIndex);
            return index;
        }
    }
}