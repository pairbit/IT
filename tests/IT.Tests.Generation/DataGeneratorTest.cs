using DataGenerator;

namespace IT.Tests.Generation;

public class DataGeneratorTest
{

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Test1()
    {
        var user1 = Generator.Default.Single<Person>(); 
        var user2 = Generator.Default.Single<Person>();

        Assert.IsFalse(user1.Equals(user2));
    }
}