using System;
using System.Text;
using System.Xml;

namespace IT.Security.Cryptography;

public class SignatureBuilder : ISignatureBuilder
{
    private const String XmlDsigExcC14NTransformUrl = "http://www.w3.org/2001/10/xml-exc-c14n#";

    private const String XmlDsigXsltTransformUrl = "http://www.w3.org/TR/1999/REC-xslt-19991116";

    private const String XmlDsigEnvelopedSignatureTransformUrl = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";

    private const String DeclarationUTF8 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

    public void ReplaceSignatureMethod(StringBuilder mask, String signatureMethod)
    {
        mask.Replace("#SignatureMethod#", signatureMethod);
    }

    public void ReplaceDigestMethod(StringBuilder mask, String digestMethod)
    {
        mask.Replace("#DigestMethod#", digestMethod);
    }

    public void RemoveTransformEnveloped(StringBuilder mask)
    {
        mask.Replace("<ds:Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" />", String.Empty);
    }

    public void PrepareViaData(StringBuilder mask)
    {
        RemoveDigestValues(mask);

        mask.Replace("#SignatureValue#", String.Empty);
        mask.Replace("#Certificate#", String.Empty);
    }

    public void PrepareViaHash(StringBuilder mask, String certificate)
    {
        mask.Replace("#Certificate#", TryToBase64(certificate));
    }

    public void ReplaceId(StringBuilder mask, String id)
    {
        mask.Replace("#ID#", id);
    }

    public void ReplaceTransform(StringBuilder mask, String transform)
    {
        if (transform is null)
        {
            mask.Replace("#Transform.Algorithm#", XmlDsigExcC14NTransformUrl);

            mask.Replace("#Transform#", String.Empty);
        }
        else
        {
            mask.Replace("#Transform.Algorithm#", XmlDsigXsltTransformUrl);

            mask.Replace("#Transform#", RemoveDeclaration(transform!));
        }
    }

    public String Build(ReadOnlySpan<Char> mask, String data)
    {
        var index = mask.IndexOf("#Data#".AsSpan());

        if (index > -1)
        {
            //mask.Replace("#Data#", RemoveDeclaration(data));
            return mask.ToString();
        }

        var xml = new XmlDocument
        {
            PreserveWhitespace = true
        };

        xml.LoadXml(TryAddDeclaration(data));


        return xml.OuterXml;
    }

    public void Detach(string sign, out string data, out string signDetached)
    {
        throw new NotImplementedException();
    }

    public void ReplaceSignatureValue(string mask, ReadOnlySpan<byte> signatureValue)
    {
        throw new NotImplementedException();
    }

    public byte[] Hash(out string mask, IHasher hasher)
    {
        throw new NotImplementedException();
    }

    #region Private

    //private static XmlNode AppendChild(this XmlNode xml, String newChild)
    //{
    //    Arg.NotNull(xml);
    //    Arg.NotNull(newChild);

    //    var childElement = IT.Xml.LoadDocument(newChild).DocumentElement;

    //    Arg.NotNull(childElement);

    //    var ownerDocument = xml.OwnerDocument;

    //    Arg.NotNull(ownerDocument);

    //    var childNode = ownerDocument!.ImportNode(childElement!, true);

    //    var appendChild = xml.AppendChild(childNode);

    //    Arg.NotNull(appendChild);

    //    return appendChild!;
    //}

    private static String TryAddDeclaration(String xml)
        => xml.Contains("<?xml") ? xml : DeclarationUTF8 + xml;

    private static String RemoveDeclaration(String xml)
    {
        var index = xml.IndexOf("<?xml");
        if (index > -1)
        {
            var lastIndex = xml.IndexOf("?>", index);
            var count = lastIndex - index;
            xml = xml.Remove(index, count + 2);
        }
        return xml;
    }

    private const String DigestValuePrefix = ">I0RpZ2VzdFZhbHVlWz";

    private const String DigestValuePostfix = "dIw==<";

    private static String GetDigestValueBase64(Int32 i)
    {
        if (i == 0) return DigestValuePrefix + "B" + DigestValuePostfix;
        if (i == 1) return DigestValuePrefix + "F" + DigestValuePostfix;
        if (i == 2) return DigestValuePrefix + "J" + DigestValuePostfix;
        if (i == 3) return DigestValuePrefix + "N" + DigestValuePostfix;

        var bytes = Encoding.UTF8.GetBytes($"#DigestValue[{i}]#");
        var base64 = Convert.ToBase64String(bytes);

        return $">{base64}<";
    }

    private static void RemoveDigestValues(StringBuilder mask)
    {
        var i = 0;

        //TODO: переписать
        var str = mask.ToString();

        //смещение, так как мы удаляем не с конца
        //с конца удалять не можем, так как не знаем сколько референсов указано
        var offset = 0;
        var index = 0;
        do
        {
            var digestValue = GetDigestValueBase64(i++);

            index = str.IndexOf(digestValue, index);

            if (index == -1) break;

            index++;

            var length = digestValue.Length - 2;

            mask.Remove(index - offset, length);

            offset += length;

        } while (true);
    }

    private static String TryToBase64(String value)
    {
        try
        {
            var bytes = Convert.FromBase64String(value);

            return value.IndexOf('\n') == -1 ? value : Convert.ToBase64String(bytes);
        }
        catch (FormatException)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }
    }

    #endregion
}