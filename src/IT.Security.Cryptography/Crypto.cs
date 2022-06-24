using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Security.Cryptography;

namespace IT.Security.Cryptography;

public static class Crypto
{
    private static readonly Type _configType = typeof(CryptoConfig);

    public static ReadOnlyDictionary<String, Object>? DefaultNames { get; }

    public static ReadOnlyDictionary<String, Type>? Names { get; }

    public static ReadOnlyDictionary<String, String>? DefaultOids { get; }

    public static ReadOnlyDictionary<String, String>? Oids { get; }

    static Crypto()
    {
        var defaultNameHT = GetProperty<Dictionary<String, Object>>("DefaultNameHT");

        if (defaultNameHT is not null)
            DefaultNames = new ReadOnlyDictionary<String, Object>(defaultNameHT);

        var defaultOidHT = GetProperty<Dictionary<String, String>>("DefaultOidHT");

        if (defaultOidHT is not null)
            DefaultOids = new ReadOnlyDictionary<String, String>(defaultOidHT);

        var appNameHT = GetField<ConcurrentDictionary<String, Type>>("appNameHT");

        if (appNameHT is not null)
            Names = new ReadOnlyDictionary<String, Type>(appNameHT);

        var appOidHT = GetField<ConcurrentDictionary<String, String>>("appOidHT");

        if (appOidHT is not null)
            Oids = new ReadOnlyDictionary<String, String>(appOidHT);
    }

    public static void Init() { }

    public static SignatureDescription? CreateSignatureDescription(String name) => (SignatureDescription?)CryptoConfig.CreateFromName(name);

    public static Object? Create(String name) => CryptoConfig.CreateFromName(name);

    private static T? GetProperty<T>(String name) => (T?)_configType.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);

    private static T? GetField<T>(String name) => (T?)_configType.GetField(name, BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
}