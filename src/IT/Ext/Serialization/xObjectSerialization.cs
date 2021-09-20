using IT.Serialization;
using Newtonsoft.Json;
using System;

namespace IT.Ext
{
    public static class xObjectSerialization
    {
        #region Json

        public static String ToJson(this Object obj, Formatting? formatting = null, JsonSerializerSettings settings = null)
        {
            return Json.Serialize(obj, formatting, settings);
        }

        public static String ToJson(this Object obj, JsonFields fields, Formatting? formatting = null)
        {
            return Json.Serialize(obj, fields, formatting);
        }

        public static String ToJsonMeta(this Object obj, Formatting? formatting = null)
        {
            return obj.ToJson(JsonFields.Meta, formatting);
        }

        public static String ToJsonData(this Object obj, Formatting? formatting = null)
        {
            return obj.ToJson(JsonFields.Data, formatting);
        }

        public static String ToJsonWritable(this Object obj, Formatting? formatting = null)
        {
            return obj.ToJson(JsonFields.Writable, formatting);
        }

        #endregion Json

        #region Xml

        public static String ToXml(this Object obj, Formatting? formatting = null, JsonSerializerSettings settings = null, String root = null)
        {
            return Serialization.Xml.Serialize(obj, formatting, settings, root);
        }

        public static String ToXml(this Object obj, JsonFields fields, Formatting? formatting = null, String root = null)
        {
            return Serialization.Xml.Serialize(obj, fields, formatting, root);
        }

        public static String ToXmlMeta(this Object obj, Formatting? formatting = null, String root = null)
        {
            return obj.ToXml(JsonFields.Meta, formatting, root);
        }

        public static String ToXmlData(this Object obj, Formatting? formatting = null, String root = null)
        {
            return obj.ToXml(JsonFields.Data, formatting, root);
        }

        public static String ToXmlWritable(this Object obj, Formatting? formatting = null, String root = null)
        {
            return obj.ToXml(JsonFields.Writable, formatting, root);
        }

        #endregion Xml
    }
}