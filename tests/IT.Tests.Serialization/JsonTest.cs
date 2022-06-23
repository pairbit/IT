using IT.Serialization.Json;

namespace IT.Tests.Serialization;

public class JsonTest : SerializerTest
{
    //private static JsonSerializerOptions _options = new ();
    private static Serializer _serializer = new();

    public JsonTest() : base(_serializer, _serializer) { }
}