
namespace IT.Serialization.Tests;

public class JsonTest : SerializerTest
{
    //private static JsonSerializerOptions _options = new ();
    private static Json.TextSerializer _serializer = new();

    public JsonTest() : base(_serializer, _serializer) { }
}