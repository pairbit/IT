# IT
[![NuGet version (IT)](https://img.shields.io/nuget/v/IT.svg)](https://www.nuget.org/packages/IT)
[![NuGet pre version (IT)](https://img.shields.io/nuget/vpre/IT.svg)](https://www.nuget.org/packages/IT)

Extensions of System to support latest language version

## Support System.Runtime.CompilerServices.IsExternalInit (init and record)

```csharp
class MyClass
{
    public String Field { get; init; }
}

record MyRecord(String FirstName, Int32 Age);
```

## Support System.Index and System.Range

```csharp
    static void IndexRange()
    {
        var tes = "login(displayName)";

        var subs = tes[(tes.IndexOf("(") + 1)..tes.IndexOf(")")];

        //displayName
        Console.WriteLine(subs);
    }
```

## Support System.Runtime.CompilerServices.CallerArgumentExpression

```csharp
class Caller
{
    static void ArgumentExpression(String name, [System.Runtime.CompilerServices.CallerArgumentExpression("name")] String? argumentExpression = null)
    {
        Console.WriteLine($"name: {name}");
        Console.WriteLine($"CallerArgumentExpression: {argumentExpression}");
    }
}
```

## Polyfill File.WriteAllTextAsync / File.ReadAllTextAsync

```csharp
    private static Task WriteAllTextAsync(String path, String contents)
    {
#if NETSTANDARD2_0
        return _File.WriteAllTextAsync(path, contents);
#else
        return File.WriteAllTextAsync(path, contents);
#endif
    }

    private static Task<String> ReadAllTextAsync(String path)
    {
#if NETSTANDARD2_0
        return _File.ReadAllTextAsync(path);
#else
        return File.ReadAllTextAsync(path);
#endif
    }
```

## Polyfill Convert.ToHexString / Convert.FromHexString

```csharp
    private static String ToHexString(Byte[] bytes)
    {
#if NET6_0
        return Convert.ToHexString(bytes);
#else
        return _Convert.ToHexString(bytes);
#endif
    }

    private static Byte[] FromHexString(String hex)
    {
#if NET6_0
        return Convert.FromHexString(hex);
#else
        return _Convert.FromHexString(hex);
#endif
    }
```

## Polyfill Random

```csharp
    private static Random GetRandom()
    {
#if NET6_0
        return Random.Shared;
#else
        return _Random.Shared;
#endif
    }
```