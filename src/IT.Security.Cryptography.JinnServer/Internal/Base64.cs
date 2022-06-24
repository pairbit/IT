using System;
using System.Text;

namespace IT.Security.Cryptography.JinnServer.Internal;

internal static class Base64
{
    public static String ToBase64(this Byte[] data, Base64FormattingOptions options = Base64FormattingOptions.None)
    {
        return Convert.ToBase64String(data, options);
    }

    public static String ToBase64(this String value, Encoding? encoding = null, Base64FormattingOptions options = Base64FormattingOptions.None)
    {
        if (encoding == null) encoding = Encoding.UTF8;
        return encoding.GetBytes(value).ToBase64(options);
    }

    public static String TryToBase64(this String value, Encoding? encoding = null, Base64FormattingOptions options = Base64FormattingOptions.None)
    {
        try
        {
            var bytes = Convert.FromBase64String(value);

            var isLineBreaks = value.IndexOf('\n') > -1;

            if (isLineBreaks && options == Base64FormattingOptions.None || !isLineBreaks && options == Base64FormattingOptions.InsertLineBreaks)
            {
                value = bytes.ToBase64(options);
            }

            return value;
        }
        catch (FormatException)
        {
            return value.ToBase64(encoding, options);
        }
    }
}
