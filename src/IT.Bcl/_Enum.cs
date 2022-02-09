#if NETSTANDARD2_0

namespace System
{
    public static class _Enum
    {
        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result)
        {
            try
            {
                result = Enum.Parse(enumType, value, ignoreCase);
                return true;
            }
            catch (OverflowException)
            {
                result = null;
                return false;
            }
        }
    }
}
#endif
