using IT.Generation;
using IT.Serialization.Tests.Data;
using MessagePack;

namespace IT.Serialization.Tests;

public abstract class SerializerTest
{
    protected static readonly IGenerator _generator = new Generation.KGySoft.Generator();
    protected static readonly Person _person = _generator.Generate<Person>();
    protected static readonly Object _personObject = _generator.Generate(typeof(Person));

    private readonly ISerializer _serializer;

    public SerializerTest(ISerializer serializer)
    {
        _serializer = serializer;
    }

    [Test]
    public void SerializerGeneric()
    {
        var serialized = _serializer.Serialize(_person);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var person = _serializer.Deserialize<Person>(serialized);

        Assert.NotNull(person);
        
        Assert.True(_person.Equals(person));
    }

    [Test]
    public void SerializerNonGeneric()
    {
        var serialized = _serializer.Serialize(typeof(Person), _personObject);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var person = _serializer.Deserialize(typeof(Person), serialized);

        Assert.NotNull(person);

        Assert.True(_personObject.Equals(person));
    }
}