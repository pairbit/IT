using IT.Resources;
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace IT
{
    public static class Xslt
    {
        public static XslCompiledTransform Load(IXPathNavigable stylesheet)
        {
            var transform = new XslCompiledTransform();
            transform.Load(stylesheet);
            return transform;
        }

        public static XslCompiledTransform Load(String xslt)
        {
            return Load(Xml.LoadDocument(xslt));
        }

        public static String Transform(String xml, String xslt)
        {
            try
            {
                var transform = Load(xslt);

                var xmlDocument = Xml.LoadDocument(xml);

                using (var writer = new StringWriter())
                using (var reader = new StringReader(xmlDocument.DocumentElement.OuterXml))
                using (var xmlReader = new XmlTextReader(reader))
                {
                    transform.Transform(xmlReader, null, writer);
                    return writer.ToString();
                }
            }
            catch (XmlException ex)
            {
                throw new XmlException(Res.Get("System_Xslt_Transform_Fail"), ex);
            }
        }
    }
}