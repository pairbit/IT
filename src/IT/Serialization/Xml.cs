using IT.Ext;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace IT.Serialization
{
    public static class Xml
    {
        public static String Serialize(Object obj, JsonFields fields, Formatting? formatting = null, String root = null)
        {
            var json = Json.Serialize(obj, fields, formatting);
            var xnode = JsonConvert.DeserializeXNode(json, root ?? GetRoot(obj.GetType()));
            return xnode.ToString();
        }

        public static String Serialize(Object obj, Formatting? formatting = null, JsonSerializerSettings settings = null, String root = null)
        {
            var json = Json.Serialize(obj, formatting, settings);
            var xnode = JsonConvert.DeserializeXNode(json, root ?? GetRoot(obj.GetType()));
            return xnode.ToString();
        }

        public static Object Deserialize(String value, Type type, JsonSerializerSettings settings = null, Boolean ignoreRoot = true, Boolean findRoot = false)
        {
            return Json.Deserialize(XmlToJson(value, type, settings, ignoreRoot, findRoot), type, settings);
        }

        public static T Deserialize<T>(String value, JsonSerializerSettings settings = null, Boolean ignoreRoot = true, Boolean findRoot = false)
        {
            var json = XmlToJson(value, typeof(T), settings, ignoreRoot, findRoot);

            return Json.Deserialize<T>(json, settings);
        }

        private static String GetRoot(Type type)
        {
            var xmlRoot = type.GetAttribute<XmlRootAttribute>();
            return xmlRoot == null || xmlRoot.ElementName.IsEmpty() ? type.Name : xmlRoot.ElementName;
        }

        private static String XmlToJson(String value, Type type, JsonSerializerSettings settings, Boolean ignoreRoot, Boolean findRoot)
        {
            if (findRoot) value = Ml.FindRoot(value, type);
            var doc = IT.Xml.LoadDocument(value, true);
            var formatting = settings?.Formatting ?? Json.Formatting;
            return JsonConvert.SerializeXmlNode(doc, formatting, ignoreRoot);
        }
    }
}