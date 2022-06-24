using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace IT.Security.Cryptography.JinnServer.Internal.Converters;

internal class ReadArrayJsonConverter : BaseJsonConverter
{
    public override Boolean CanWrite => false;

    public override Object? ReadJson(JsonReader reader, Type objectType, Object? existingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        if (token.Type == JTokenType.Array) return token.ToObject(objectType, serializer);
        var type = objectType.GetElementType();
        if (type is null) throw new InvalidOperationException("objectType.GetElementType() is null");
        var array = Array.CreateInstance(type, 1);
        array.SetValue(token.ToObject(type, serializer), 0);
        return array;
    }
}