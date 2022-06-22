using Bogus;

namespace IT.Tests.Generation;

public class Bogus
{
    private Random _random = new Random();
    private Faker<User> _faker;

    //[SetUp]
    public void Setup()
    {
        _faker = new Faker<User>();
    }

    //[Test]
    public void Test1()
    {
        var user1 = _faker.Generate();
        var user2 = _faker.Generate();

        Assert.IsFalse(user1.Equals(user2));

        Assert.Pass();
    }
}