
namespace IT.Serialization.Tests;

public class MessagePackTest : SerializerTest
{
    //private static JsonSerializerOptions _options = new ();
    private static MessagePack.Serializer _serializer = new();

    public MessagePackTest() : base(_serializer) { }
}