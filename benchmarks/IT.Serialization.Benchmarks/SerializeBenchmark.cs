using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Generation;
using IT.Serialization.Benchmarks.Data;

namespace IT.Serialization.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class SerializeBenchmark
{
    protected static readonly IGenerator _generator = new Generation.KGySoft.Generator();
    protected static readonly Person _person = _generator.Generate<Person>();
    protected static readonly Object _personObject = _generator.Generate(typeof(Person));

    private readonly ISerializer _jsonSerializer;
    private readonly byte[] _personJsonSerialize;
    private readonly ISerializer _messagePackSerializer;
    private readonly byte[] _personMessagePackSerialize;

    public SerializeBenchmark()
    {
        _jsonSerializer = new Json.TextSerializer();
        _personJsonSerialize = Json_Serialize();
        _messagePackSerializer = new MessagePack.Serializer();
        _personMessagePackSerialize = MessagePack_Serialize();
    }

    [Benchmark]
    public byte[] Json_Serialize() => _jsonSerializer.Serialize(_person);

    [Benchmark]
    public byte[] MessagePack_Serialize() => _messagePackSerializer.Serialize(_person);

    [Benchmark]
    public Person? Json_Deserialize() => _jsonSerializer.Deserialize<Person>(_personJsonSerialize);

    [Benchmark]
    public Person? MessagePack_Deserialize() => _messagePackSerializer.Deserialize<Person>(_personMessagePackSerialize);

    [Benchmark]
    public void Json()
    {
        var bytes = _jsonSerializer.Serialize(_person);
        var person = _jsonSerializer.Deserialize<Person>(bytes);
        if (!_person.Equals(person)) throw new InvalidOperationException();
    }

    [Benchmark]
    public void MessagePack()
    {
        var bytes = _messagePackSerializer.Serialize(_person);
        var person = _messagePackSerializer.Deserialize<Person>(bytes);
        if (!_person.Equals(person)) throw new InvalidOperationException();
    }
}
