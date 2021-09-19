using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace IT.Ext
{
    public static class xEnum
    {
        public static MemberInfo GetMember(this Enum value) => value.GetType().GetMember(value.ToString()).Single();

        public static DisplayAttribute Display(this Enum value) => value.GetMember().GetAttribute<DisplayAttribute>();

        public static String DisplayName(this Enum value) => value.Display()?.Name;
    }
}