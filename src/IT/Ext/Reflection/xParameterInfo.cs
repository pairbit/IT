using System;
using System.Reflection;

namespace IT.Ext
{
    public static class xParameterInfo
    {
        public static String GetFullName(this ParameterInfo parameter)
        {
            return parameter.Member.GetFullName() + (parameter.Name.IsEmpty() ? ":" + parameter.GetName() : "(" + parameter.Name + ")");
        }

        public static String GetName(this ParameterInfo parameter)
        {
            return parameter.Name.UnEmpty() ?? "return";
        }
    }
}