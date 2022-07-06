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

    protected virtual void Dump<T>(T obj, Byte[] bytes) { }

    [Test]
    public void SerializerGeneric()
    {
        var serialized = _serializer.Serialize(_person);

        Dump(_person, serialized);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var person = _serializer.Deserialize<Person>(serialized);

        Assert.NotNull(person);
        
        Assert.True(_person.Equals(person));

        var path = @"C:\var\SerializerTest_Generic.log";

        File.Delete(path);

        using var file = File.OpenWrite(path);
        _serializer.Serialize(file, _person);
        file.Close();

        using var reader = File.OpenRead(path);
        person = _serializer.Deserialize<Person>(reader);
        reader.Close();

        Assert.True(_person.Equals(person));
    }

    [Test]
    public void SerializerNonGeneric()
    {
        var serialized = _serializer.Serialize(typeof(Person), _personObject);

        Dump(_personObject, serialized);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var person = _serializer.Deserialize(typeof(Person), serialized);

        Assert.NotNull(person);

        Assert.True(_personObject.Equals(person));

        var path = @"C:\var\SerializerTest_NonGeneric.log";

        File.Delete(path);

        using var file = File.OpenWrite(path);
        _serializer.Serialize(file, _person);
        file.Close();

        using var reader = File.OpenRead(path);
        person = _serializer.Deserialize<Person>(reader);
        reader.Close();

        Assert.True(_person.Equals(person));
    }
}