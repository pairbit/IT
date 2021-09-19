using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace IT.Ext
{
    public static class xMethodBase
    {
        public static Boolean IsAsync(this MethodBase method)
        {
            return method.Name == nameof(IAsyncStateMachine.MoveNext) && typeof(IAsyncStateMachine).IsAssignableFrom(method.DeclaringType);
        }

        public static Object TargetInvoke(this MethodBase method, Object obj, Object[] parameters)
        {
            try
            {
                return method.Invoke(obj, parameters);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public static String GetContract(this MethodBase method, params Object[] args)
        {
            var sb = new StringBuilder(method.GetFullName());
            var genArgs = method.GetGenericArguments();
            var index = 0;
            if (genArgs.Length > 0)
            {
                sb.Append("<");
                for (; index < genArgs.Length; index++)
                {
                    sb.Append(args[index].ToString()).Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(">");
            }
            sb.Append("(");

            for (; index < args.Length; index++)
            {
                sb.Append(args[index]?.ToString() ?? "null").Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")");
            return sb.ToString();
        }
    }
}