using IT.Generation;
using IT.Serialization.Tests.Data;

namespace IT.Serialization.Tests;

public abstract class TextSerializerTest : SerializerTest
{
    private readonly ITextSerializer _textSerializer;

    public TextSerializerTest(ITextSerializer textSerializer) : base(textSerializer)
    {
        _textSerializer = textSerializer;
    }

    [Test]
    public void TextSerializerGeneric()
    {
        var serialized = _textSerializer.SerializeToText(_person);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var person = _textSerializer.Deserialize<Person>(serialized.AsMemory());

        Assert.NotNull(person);

        Assert.True(_person.Equals(person));
    }

    [Test]
    public void TextSerializerNonGeneric()
    {
        var serialized = _textSerializer.SerializeToText(typeof(Person), _personObject);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var person = _textSerializer.Deserialize(typeof(Person), serialized.AsMemory());

        Assert.NotNull(person);

        Assert.True(_personObject.Equals(person));
    }
}