using Newtonsoft.Json;
using System;

namespace IT.Serialization.Converters
{
    public abstract class BaseJsonConverter : JsonConverter
    {
        public override Boolean CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => existingValue;

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer) => serializer.Serialize(writer, value);
    }
}