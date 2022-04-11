using NUnit.Framework;
using System;

namespace IT.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            //Class1.Init();
        }

        [Test]
        public void Test1()
        {
            //Assert.Pass();

            var str = "abcdez";

            Assert.AreEqual(str.StartsWith('a'), _StartsWith(str, 'a'));

            Assert.AreEqual(str.EndsWith('z'), _EndsWith(str, 'z'));

            str = "";

            Assert.AreEqual(str.EndsWith('z'), _EndsWith(str, 'z'));

            str = "z";

            Assert.AreEqual(str.EndsWith('z'), _EndsWith(str, 'z'));

            //Assert.AreEqual(Class1.Test2, 2345234);
        }

        public static Boolean _StartsWith(String value, Char ch) => value.Length != 0 && value[0] == ch;

        public static Boolean _EndsWith(String value, Char ch)
        {
            var lastPos = value.Length - 1;
            return lastPos >= 0 && value[lastPos] == ch;
        }
    }
}