using IT.Generation;
using IT.Serialization;

namespace IT.Tests.Serialization;

public abstract class SerializerTest
{
    private static readonly IGenerator _generator = new Generation.KGySoft.Generator();
    private static readonly Person _person = _generator.Generate<Person>();
    private static readonly Object _personObject = _generator.Generate(typeof(Person));

    private readonly ISerializer _serializer;
    private readonly ITextSerializer _textSerializer;

    public SerializerTest(ISerializer serializer, ITextSerializer textSerializer)
    {
        _serializer = serializer;
        _textSerializer = textSerializer;
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
        var serialized = _serializer.Serialize(_personObject);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var person = _serializer.Deserialize(serialized, typeof(Person));

        Assert.NotNull(person);

        Assert.True(_personObject.Equals(person));
    }

    [Test]
    public void TextSerializerGeneric()
    {
        var serialized = _textSerializer.Serialize(_person);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var person = _textSerializer.Deserialize<Person>(serialized);

        Assert.NotNull(person);

        Assert.True(_person.Equals(person));
    }

    [Test]
    public void TextSerializerNonGeneric()
    {
        var serialized = _textSerializer.Serialize(_personObject);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var person = _textSerializer.Deserialize(serialized, typeof(Person));

        Assert.NotNull(person);

        Assert.True(_personObject.Equals(person));
    }
}