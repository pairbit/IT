using IT.Serialization;
using Newtonsoft.Json;
using System;

namespace IT.Ext
{
    public static class xStringSerialization
    {
        #region Json

        public static String ToJsonFormat(this String value, Formatting? formatting = Formatting.Indented, JsonSerializerSettings settings = null)
            => value.FromJson(null, settings).ToJson(formatting, settings);

        public static Object FromJson(this String value, Type type = null, JsonSerializerSettings settings = null)
            => Json.Deserialize(value, type, settings);

        public static T FromJson<T>(this String value, JsonSerializerSettings settings = null)
            => Json.Deserialize<T>(value, settings);

        #endregion Json

        #region Xml

        public static Object FromXml(this String value, Type type, JsonSerializerSettings settings = null, Boolean ignoreRoot = true, Boolean findRoot = false)
            => Serialization.Xml.Deserialize(value, type, settings, ignoreRoot, findRoot);

        public static T FromXml<T>(this String value, JsonSerializerSettings settings = null, Boolean ignoreRoot = true, Boolean findRoot = false)
            => Serialization.Xml.Deserialize<T>(value, settings, ignoreRoot, findRoot);

        #endregion Xml
    }
}