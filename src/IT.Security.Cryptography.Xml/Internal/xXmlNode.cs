namespace System.Xml;

internal static class xXmlNode
{
    public static XmlDeclaration? FindDeclaration(this XmlNode xml) => xml.FirstChild as XmlDeclaration;

    public static XmlNode? TryFindSignature(this XmlNode xml) => xml?.TryFindByName("Signature");

    public static XmlNodeList? TryFindAllByName(this XmlNode xml, String name) => xml.SelectNodes($"//*[local-name()='{name}']");

    public static XmlNode? TryFindByName(this XmlNode xml, String name)
        => StringComparer.OrdinalIgnoreCase.Equals(xml.LocalName, name) ? xml : xml.SelectSingleNode($"//*[local-name()='{name}']");

    public static XmlNode FindByName(this XmlNode xml, String name, String? paramName = null)
    {
        if (xml is null) throw new ArgumentNullException(nameof(xml));
        return xml.TryFindByName(name) ?? throw new ArgumentException($"Не найден тег с названием '{name}'", paramName);
    }

    public static XmlNode? TryFindById(this XmlNode xml, String id)
    {
        if (id.Length != 0 && id[0] == '#') id = id.Substring(1);
        return xml.SelectSingleNode($"//*[@Id='{id}']");
    }

    public static XmlNode FindById(this XmlNode xml, String id, Func<String>? getParamName = null)
        => xml.TryFindById(id) ?? throw new ArgumentException($"Не найден тег с ид '{id}'", getParamName?.Invoke());
}