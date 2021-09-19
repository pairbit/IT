using IT.Ext;
using IT.Validation;
using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace IT
{
    public static class Xml
    {
        public static XmlDocument LoadDocument(String xml, Boolean removeNamespaces = false)
        {
            if (removeNamespaces) xml = RemoveNamespaces(xml);
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        public static XmlDocument TryLoadDocument(String xml, Boolean removeNamespaces = false)
        {
            try
            {
                return LoadDocument(xml, removeNamespaces);
            }
            catch (XmlException)
            {
                return null;
            }
        }

        private const String XmlDeclaration = @"<?xml version=""1.0"" encoding=""{0}""?>";

        public static String ToFormat(this XmlDocument xml, Boolean indent = true, Boolean omitXmlDeclaration = false)
        {
            Arg.NotNull(xml);

            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = indent,
                IndentChars = "\t",
                OmitXmlDeclaration = true
            };
            using (var writer = XmlWriter.Create(sb, settings))
            {
                xml.Save(writer);
            }
            if (!omitXmlDeclaration && settings.OmitXmlDeclaration)
            {
                sb.Insert(0, settings.NewLineChars);
                sb.Insert(0, XmlDeclaration.Format(settings.Encoding.HeaderName));
            }
            return sb.ToString();
        }

        public static String TryToXmlFormat(this String xml, Boolean indent = true, Boolean omitXmlDeclaration = false)
            => TryLoadDocument(xml)?.ToFormat(indent, omitXmlDeclaration) ?? xml;

        public static String RemoveNamespaces(String xml)
        {
            var xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xml));

            return xmlDocumentWithoutNs.ToString();
        }

        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                var xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes().Where(x => !x.IsNamespaceDeclaration))
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
    }
}