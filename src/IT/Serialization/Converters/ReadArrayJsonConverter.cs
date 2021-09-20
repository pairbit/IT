using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace IT.Serialization.Converters
{
    public class ReadArrayJsonConverter : BaseJsonConverter
    {
        public override Boolean CanWrite => false;

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Array) return token.ToObject(objectType, serializer);
            var type = objectType.GetElementType();
            var array = Array.CreateInstance(type, 1);
            array.SetValue(token.ToObject(type, serializer), 0);
            return array;
        }
    }
}