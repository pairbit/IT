using IT.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace IT.Ext
{
    public static class xICustomAttributeProvider
    {
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this ICustomAttributeProvider member, bool inherit = false) where TAttribute : Attribute
        {
            Arg.NotNull(member, nameof(member));
            var type = typeof(TAttribute);
            var attrs = member.GetCustomAttributes(type, inherit);
            return attrs.Cast<TAttribute>();
        }

        public static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider member, bool inherit = false) where TAttribute : Attribute
        {
            try
            {
                return member.GetAttributes<TAttribute>(inherit).SingleOrDefault();
            }
            catch (Exception ex)
            {
                ex.Data.Add("Method", This.Method().GetContract(typeof(TAttribute), member, inherit));
                throw;
            }
        }

        public static DisplayAttribute GetDisplay(this ICustomAttributeProvider member, bool inherit = false)
        {
            var displayName = member.GetAttribute<DisplayNameAttribute>(inherit);
            var description = member.GetAttribute<DescriptionAttribute>(inherit);
            var display = member.GetAttribute<DisplayAttribute>(inherit) ?? new DisplayAttribute();
            if (displayName != null) display.Name = displayName.DisplayName;
            if (description != null) display.Description = description.Description;
            return display;
        }
    }
}