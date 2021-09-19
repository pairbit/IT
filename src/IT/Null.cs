using System;

namespace IT
{
    public sealed class Null
    {
        public static readonly Null Value = new Null();

        private Null() { }

        public override String ToString() => "null";
    }
}