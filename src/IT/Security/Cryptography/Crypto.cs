using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace IT.Security.Cryptography
{
    public static class Crypto
    {
        private static readonly Type ConfigType = typeof(CryptoConfig);

        public static ReadOnlyDictionary<String, Object> Names { get; }

        public static ReadOnlyDictionary<String, String> Oids { get; }

        private static Dictionary<String, Object> DefaultNameHT => GetProp<Dictionary<String, Object>>();

        private static Dictionary<String, String> DefaultOidHT => GetProp<Dictionary<String, String>>();

        static Crypto()
        {
            //GostCryptoConfig.Initialize();
            Names = new ReadOnlyDictionary<String, Object>(DefaultNameHT);
            Oids = new ReadOnlyDictionary<String, String>(DefaultOidHT);
        }

        public static void Init() { }

        private static T GetProp<T>([CallerMemberName] String name = null) => (T)ConfigType.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
    }
}