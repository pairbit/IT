using NUnit.Framework;

namespace IT.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Class1.Init();
        }

        [Test]
        public void Test1()
        {
            //Assert.Pass();
            Assert.AreEqual(Class1.Test, "Test");
            Assert.AreEqual(Class1.Test2, 2345234);
        }
    }
}