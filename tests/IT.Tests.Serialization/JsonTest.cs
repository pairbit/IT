using IT.Serialization.Json;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace IT.Tests.Serialization;

public class JsonTest : SerializerTest
{
    private static JsonSerializerOptions _options = new ();
    private static Serializer _serializer = new(Options.Create(_options));

    public JsonTest() : base(_serializer, _serializer) { }
}