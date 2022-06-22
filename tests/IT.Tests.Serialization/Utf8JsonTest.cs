using IT.Serialization.Utf8Json;

namespace IT.Tests.Serialization;

public class Utf8JsonTest : SerializerTest
{
    private static Serializer _serializer = new();

    public Utf8JsonTest() : base(_serializer, _serializer) { }
}