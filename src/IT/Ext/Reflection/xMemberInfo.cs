using System;
using System.Reflection;

namespace IT.Ext
{
    public static class xMemberInfo
    {
        public static String GetFullName(this MemberInfo member)
        {
            return member.DeclaringType == null ? member.ToString() :
                   member.DeclaringType?.FullName + "." + member.Name;
        }
    }
}