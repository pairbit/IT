# IT
[![NuGet version (IT)](https://img.shields.io/nuget/v/IT.svg)](https://www.nuget.org/packages/IT)
[![NuGet pre version (IT)](https://img.shields.io/nuget/vpre/IT.svg)](https://www.nuget.org/packages/IT)

Extensions of System to support latest language version

## Random

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