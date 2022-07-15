#if NETSTANDARD2_0 || NETSTANDARD2_1

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public sealed class CallerArgumentExpressionAttribute : Attribute
{
    public String ParameterName { get; }

    public CallerArgumentExpressionAttribute(String parameterName)
    {
        ParameterName = parameterName;
    }
}

#endif