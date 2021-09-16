using System;

namespace IT
{
    public class Class1
    {
        public const String Test = "Test";

        public static Int32 Test2 { get; private set; }

        public static void Init()
        {
            Test2 = 2345234;
        }
    }
}