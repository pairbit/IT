using System;

namespace IT.Exceptions
{
    public static class Ex
    {
        public static ErrorException Error(Error error)
        {
            return new ErrorException { Error = error };
        }

        public static Exception Invalid(String msg = null)
        {
            return new InvalidOperationException(WrapMsg(msg));
        }

        public static Exception Range(String arg, Object value, String msg = null)
        {
            return new ArgumentOutOfRangeException(arg, value, WrapMsg(msg));
        }

        public static Exception Null(String arg, String msg = null)
        {
            return new ArgumentNullException(arg, WrapMsg(msg) ?? "Field is null");
        }

        #region ArgumentException

        public static Exception NotEnum(Enum value)
        {
            return new ArgumentException();
        }

        public static Exception Arg(String arg, String msg)
        {
            return new ArgumentException(WrapMsg(msg), arg);
        }

        public static Exception NotNull(String arg, String msg = null)
        {
            return new ArgumentException(WrapMsg(msg) ?? "Field is not null", arg);
        }

        public static Exception Required(String arg, String msg = null)
        {
            return new ArgumentException(WrapMsg(msg) ?? "Field is required", arg);
        }

        public static Exception Space(String arg, String msg = null)
        {
            return new ArgumentException(WrapMsg(msg) ?? "Sequence is space", arg);
        }

        public static Exception Empty(String arg, String msg = null)
        {
            return new ArgumentException(WrapMsg(msg) ?? "Sequence is empty", arg);
        }

        public static Exception MoreOneMember(String arg, String msg = null, Exception exception = null)
        {
            return new ArgumentException(WrapMsg(msg) ?? "Sequence contains more than one element", arg, exception);
        }

        public static Exception Unique(String arg, String msg = null)
        {
            return new ArgumentException(WrapMsg(msg) ?? "Sequence is not unique", arg);
        }

        #endregion ArgumentException

        private static String WrapMsg(String value)
        {
            return value?.Replace("[newline]", Environment.NewLine);
        }
    }
}