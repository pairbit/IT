using System;
using System.Text;

namespace IT.Ext
{
    public static class xBase64
    {
        #region Base64

        public static String ToBase64(this Byte[] data, Base64FormattingOptions options = Base64FormattingOptions.None) 
            => Convert.ToBase64String(data, options);

        public static String ToBase64(this String value, Encoding encoding = null, Base64FormattingOptions options = Base64FormattingOptions.None)
            => (encoding ?? Encoding.UTF8).GetBytes(value).ToBase64(options);

        public static String FromBase64(this String value, Encoding encoding = null) 
            => (encoding ?? Encoding.UTF8).GetString(Convert.FromBase64String(value));

        public static String TryFromBase64(this String value, Encoding encoding = null)
        {
            try
            {
                return value.FromBase64(encoding);
            }
            catch (FormatException)
            {
                return value;
            }
        }

        public static Boolean IsBase64(this String value)
        {
            try
            {
                return Convert.FromBase64String(value) != null;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static String TryToBase64(this String value, Encoding encoding = null, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            if (!value.IsBase64()) return value.ToBase64(encoding, options);

            var isLineBreaks = value.Contains("\n");

            if (isLineBreaks && options == Base64FormattingOptions.None || !isLineBreaks && options == Base64FormattingOptions.InsertLineBreaks)
            {
                value = Convert.FromBase64String(value).ToBase64(options);
            }

            return value;
        }

        #endregion Base64

        #region Hex

        /// <summary>
        /// value.ToString("X2")
        /// </summary>
        public static String ToHex(this Byte value) => value.ToString("X2");

        public static String ToHex(this Byte[] value)
        {
            var hex = new StringBuilder(value.Length * 2);
            foreach (var b in value) hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        /// <summary>
        /// Convert.FromHexString(value)
        /// </summary>
        public static Byte[] FromHex(this String value)
        {
            var bytes = new byte[value.Length / 2];
            for (var i = 0; i < value.Length; i += 2)
                bytes[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);
            return bytes;
        }

        /// <summary>
        /// Convert.FromBase64String(value).ToHex()
        /// </summary>
        public static String FromBase64ToHex(this String value) => Convert.FromBase64String(value).ToHex();

        /// <summary>
        /// value.FromHex().ToBase64()
        /// </summary>
        public static String FromHexToBase64(this String value) => value.FromHex().ToBase64();

        #endregion
    }
}