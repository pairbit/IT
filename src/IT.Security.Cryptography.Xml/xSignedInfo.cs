using IT.Security.Cryptography;
using IT.Security.Cryptography.Internal;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public static class xSignedInfo
{
    public static void LoadXmlByTag(this SignedInfo signedInfo, XmlDocument xml)
    {
        signedInfo.LoadXml((XmlElement)xml.FindByName("SignedInfo"));
    }

    public static void ComputeDigestReferences(this SignedInfo signedInfo, IHasher hasher, XmlDocument xml)
    {
        var references = signedInfo.References;

        for (int i = references.Count - 1; i >= 0; i--)
        {
            var reference = (Reference?)references[i];

            if (reference is null) continue;

            var uri = reference.Uri;

            var data = uri is null || uri.Length == 0 ? RemoveSignature(xml.DocumentElement.OuterXml) : xml.FindById(uri).OuterXml;

            var dataTransformed = reference.TransformChain.Transform(data);

            var hash = hasher.Hash(reference.DigestMethod, Encoding.UTF8.GetBytes(dataTransformed));

            reference.DigestValue = hash;
        }
    }

    private static String RemoveSignature(String xml) => Tag.Remove(xml, "Signature", StringComparison.OrdinalIgnoreCase);

    public static Byte[] ComputeHash(this SignedInfo signedInfo, IHasher hasher)
    {
        var newSignedInfoXml = signedInfo.GetXml();

        signedInfo.CanonicalizationMethodObject.LoadInputDocument(newSignedInfoXml.OuterXml);

        //var signedInfoHash = signedInfo.CanonicalizationMethodObject.GetDigestedOutput(alg).ToHex();

        var signedInfoCan = signedInfo.CanonicalizationMethodObject.Transform();

        return hasher.Hash(signedInfo.SignatureMethod, Encoding.UTF8.GetBytes(signedInfoCan));
    }
}