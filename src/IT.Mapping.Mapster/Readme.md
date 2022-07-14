# IT.Mapping.Mapster
[![NuGet version (IT.Mapping.Mapster)](https://img.shields.io/nuget/v/IT.Mapping.Mapster.svg)](https://www.nuget.org/packages/IT.Mapping.Mapster)
[![NuGet pre version (IT.Mapping.Mapster)](https://img.shields.io/nuget/vpre/IT.Mapping.Mapster.svg)](https://www.nuget.org/packages/IT.Mapping.Mapster)

Implementation of mapping via Mapster

## IMapper

```csharp
    private static IT.Mapping.IMapper GetMapsterMapper()
    {
        var config = new global::Mapster.TypeAdapterConfig();
        var mapper = new MapsterMapper.Mapper(config);
        return new IT.Mapping.Mapster.Mapper(mapper);
    }
```