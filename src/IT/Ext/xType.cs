using System;
using System.Linq;

namespace IT.Ext
{
    public static class xType
    {
        public static Boolean IsSimple(this Type type)
        {
            return type.IsValueType || type == typeof(String);
        }

        private static readonly String[] Postfixes = { "Controller", "Attribute" };

        public static String GetNameWithoutPostfix(this Type type, String postfix = null) => WithoutPostfix(type.Name, postfix);

        public static String GetFullNameWithoutPostfix(this Type type, String postfix = null) => WithoutPostfix(type.FullName, postfix);

        private static String WithoutPostfix(String name, String postfix)
        {
            if (postfix == null)
            {
                postfix = Postfixes.SingleOrDefault(x => name.EndsWith(x));
                if (postfix == null) return name;
            }
            else if (!name.EndsWith(postfix)) return name;

            return name.Remove(name.Length - postfix.Length);
        }
    }
}