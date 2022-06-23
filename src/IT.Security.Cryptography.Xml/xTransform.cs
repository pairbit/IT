using System.IO;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public static class xTransform
{
    public static void LoadInputDocument(this Transform transform, String xml)
    {
        if (xml.Contains("\r\n"))
        {
            xml = xml.Replace("\r\n", "\n");
        }
        //if (xml.Contains("\r"))
        //{
        //    //xml = xml.Replace("\r", "");
        //}

        transform.LoadInputDocument(LoadDocument(xml));
    }

    public static void LoadInputDocument(this Transform transform, XmlDocument xml) => transform.LoadInput(xml);

    public static String Transform(this Transform transform, Encoding? encoding = null)
    {
        if (transform is null) throw new ArgumentNullException(nameof(transform));
        if (encoding is null) encoding = Encoding.UTF8;

        var output = transform.GetOutput();
        if (output is XmlDocument outputXml) return outputXml.InnerXml;
        if (output is Stream outputStream)
        {
            //TODO: Вдруг длинна очень большая??
            var bytes = new Byte[outputStream.Length];
            outputStream.Read(bytes, 0, bytes.Length);
            return encoding.GetString(bytes);
        } 

        throw new NotImplementedException($"Output type {output.GetType()} not implemented");
    }

    private static XmlDocument LoadDocument(String xml)
    {
        try
        {
            var doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xml);
            return doc;
        }
        catch (XmlException ex)
        {
            throw new ArgumentException("Данные не являются XML документом", nameof(xml), ex);
        }
    }
}