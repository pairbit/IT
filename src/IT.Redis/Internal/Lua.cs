using System.IO;
using System.Reflection;

namespace StackExchange.Redis;

internal static class Lua
{
    internal static readonly string StringSetIfEqual;//sete
    internal static readonly string StringSetIfNotEqual;//setne
    //internal static readonly String StringSetIfContains;
    //internal static readonly String StringSetIfNotContains;

    internal static readonly string StringDeleteIfEqual;//dele
    internal static readonly string StringDeleteIfNotEqual;//delne
    internal static readonly string StringDeleteIfContains;
    internal static readonly string StringDeleteIfNotContains;

    internal static readonly string HashGetSet;
    internal static readonly string HashGetDelete;

    internal static readonly string HashSetIfExists;//hsetx
    internal static readonly string HashSetIfEqual;//hsete
    internal static readonly string HashSetIfNotEqual;//hsetne

    internal static readonly string HashDeleteIfEqual;//hdele
    internal static readonly string HashDeleteIfNotEqual;//hdelne

    static Lua()
    {
        var assembly = Assembly.GetExecutingAssembly();

        StringSetIfEqual = assembly.GetLua(nameof(StringSetIfEqual));
        StringSetIfNotEqual = assembly.GetLua(nameof(StringSetIfNotEqual));

        StringDeleteIfEqual = assembly.GetLua(nameof(StringDeleteIfEqual));
        StringDeleteIfNotEqual = assembly.GetLua(nameof(StringDeleteIfNotEqual));
        StringDeleteIfContains = assembly.GetLua(nameof(StringDeleteIfContains));
        StringDeleteIfNotContains = assembly.GetLua(nameof(StringDeleteIfNotContains));

        HashGetSet = assembly.GetLua(nameof(HashGetSet));
        HashGetDelete = assembly.GetLua(nameof(HashGetDelete));

        HashSetIfExists = assembly.GetLua(nameof(HashSetIfExists));
        HashSetIfEqual = assembly.GetLua(nameof(HashSetIfEqual));
        HashSetIfNotEqual = assembly.GetLua(nameof(HashSetIfNotEqual));

        HashDeleteIfEqual = assembly.GetLua(nameof(HashDeleteIfEqual));
        HashDeleteIfNotEqual = assembly.GetLua(nameof(HashDeleteIfNotEqual));
    }

    private static string GetLua(this Assembly assembly, string name)
    {
        using var stream = assembly.GetManifestResourceStream($"IT.Redis.Lua.{name}.lua");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}