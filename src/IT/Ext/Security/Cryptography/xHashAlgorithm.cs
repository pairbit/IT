using System;
using System.Security.Cryptography;
using System.Text;

namespace IT.Ext
{
    public static class xHashAlgorithm
    {
        public static String ComputeHash(this HashAlgorithm alg, String data, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            return alg.ComputeHash(encoding.GetBytes(data)).ToHex();
        }
    }
}