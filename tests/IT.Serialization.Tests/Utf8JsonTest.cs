using IT.Serialization.Utf8Json;

namespace IT.Serialization.Tests;

public class Utf8JsonTest : TextSerializerTest
{
    private static TextSerializer _serializer = new();

    public Utf8JsonTest() : base(_serializer) { }
}