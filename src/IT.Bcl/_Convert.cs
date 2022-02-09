using System.Text;

namespace System
{
    public static class _Convert
    {

#if NETSTANDARD2_0 || NETSTANDARD2_1

        public static String ToHexString(Byte[] bytes)
        {
            var hex = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes)
                hex.Append(b.ToString("X2"));

            return hex.ToString();
        }

        public static Byte[] FromHexString(String hex)
        {
            int len = hex.Length;
            byte[] bytes = new byte[len / 2];

            for (int i = 0; i < len; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return bytes;
        }

#endif
    }
}