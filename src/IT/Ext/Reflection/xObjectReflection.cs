using IT.Validation;
using System;

namespace IT.Ext
{
    public static class xObjectReflection
    {
        public static Object GetProperty(this Object obj, String name)
        {
            Arg.NotNull(obj, nameof(obj));
            Arg.NotNull(name, nameof(name));

            var type = obj.GetType();
            var property = type.GetProperty(name);

            Arg.NotNull(property, nameof(property));

            return property.GetValue(obj);
        }
    }
}