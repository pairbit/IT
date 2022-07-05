using IT.Generation.Tests.Data;
using KGySoft.CoreLibraries;

namespace IT.Generation.Tests;

public class KGySoftTest
{
    private Random _random = new Random();
    private IGenerator _generator;

    [SetUp]
    public void Setup()
    {
        _generator = new KGySoft.Generator();
    }

    [Test]
    public void Test1()
    {
        var user1 = _random.NextObject<Person>();
        var user2 = _generator.Generate<Person>();

        Assert.IsFalse(user1.Equals(user2));

        var user3 = _random.NextObject(typeof(Person));
        var user4 = _generator.Generate(typeof(Person));

        Assert.IsFalse(user3.Equals(user4));

    }
}