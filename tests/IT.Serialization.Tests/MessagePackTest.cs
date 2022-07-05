using MessagePack;
using System.Text.Json;

namespace IT.Serialization.Tests;

public class MessagePackTest : SerializerTest
{
    //private static JsonSerializerOptions _options = new ();
    private static MessagePack.Serializer _serializer = new();

    public MessagePackTest() : base(_serializer) { }

    protected override void Dump<T>(T obj, byte[] bytes)
    {
        var jsonArray = MessagePackSerializer.ConvertToJson(bytes);

        Console.WriteLine(jsonArray);

        var json = JsonSerializer.Serialize(obj);

        Console.WriteLine(json);

        //var bdump = MessagePackSerializer.ConvertFromJson(json);

        //if (!bdump.SequenceEqual(bytes))
        //     throw new InvalidOperationException();


    }
}