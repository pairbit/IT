# IT.Mapping.AutoMapper
[![NuGet version (IT.Mapping.AutoMapper)](https://img.shields.io/nuget/v/IT.Mapping.AutoMapper.svg)](https://www.nuget.org/packages/IT.Mapping.AutoMapper)
[![NuGet pre version (IT.Mapping.AutoMapper)](https://img.shields.io/nuget/vpre/IT.Mapping.AutoMapper.svg)](https://www.nuget.org/packages/IT.Mapping.AutoMapper)

Implementation of mapping via AutoMapper

## IMapper

```csharp
    private static IT.Mapping.IMapper GetAutoMapper()
    {
        var autoMapper = new global::AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.CreateMap<City, City2>();
            cfg.CreateMap<City2, City>();

            cfg.CreateMap<Address, Address2>();
            cfg.CreateMap<Address2, Address>();

            cfg.CreateMap<Person, Person2>();
            cfg.CreateMap<Person2, Person>();
        }).CreateMapper();

        return new IT.Mapping.AutoMapper.Mapper(autoMapper);
    }
```