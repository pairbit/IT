using Newtonsoft.Json;
using System;

namespace IT.Security.Cryptography.JinnServer.Internal.Converters;

internal abstract class BaseJsonConverter : JsonConverter
{
    public override Boolean CanConvert(Type objectType) => true;

    public override Object? ReadJson(JsonReader reader, Type objectType, Object? existingValue, JsonSerializer serializer) => existingValue;

    public override void WriteJson(JsonWriter writer, Object? value, JsonSerializer serializer) => serializer.Serialize(writer, value);
}