using System;
using System.Reflection;

namespace IT.Ext
{
    public static class xPropertyInfo
    {
        public static Object TargetGetValue(this PropertyInfo property, Object obj, Object[] index = null)
        {
            try
            {
                return property.GetValue(obj, index);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}