using IT.Generation;
using KGySoft.CoreLibraries;

namespace IT.Tests.Generation;

public class KGySoftTest
{
    private Random _random = new Random();
    private IGenerator _generator;

    [SetUp]
    public void Setup()
    {
        _generator = new IT.Generation.KGySoft.Generator();
    }

    [Test]
    public void Test1()
    {
        var user1 = _random.NextObject<User>();
        var user2 = _generator.Generate<User>();

        Assert.IsFalse(user1.Equals(user2));

        var user3 = _random.NextObject(typeof(User));
        var user4 = _generator.Generate(typeof(User));

        Assert.IsFalse(user3.Equals(user4));

    }
}