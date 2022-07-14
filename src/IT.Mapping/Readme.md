# IT.Mapping
[![NuGet version (IT.Mapping)](https://img.shields.io/nuget/v/IT.Mapping.svg)](https://www.nuget.org/packages/IT.Mapping)
[![NuGet pre version (IT.Mapping)](https://img.shields.io/nuget/vpre/IT.Mapping.svg)](https://www.nuget.org/packages/IT.Mapping)

Interfaces of mapping

## Map

```csharp
    public Person2 Map(IMapper mapper, Person person)
    {
        return mapper.Map<Person2>(person);
    }

    public Person2 Map2(IMapper mapper, Person person)
    {
        return mapper.Map<Person, Person2>(person);
    }

    public Person2 Map3(IMapper mapper, Person person)
    {
        var person2 = new Person2();
        return mapper.Map(person, person2);
    }

    public Person2 Map(IMapper<Person, Person2> mapper, Person person)
    {
        return mapper.Map(person);
    }

    public Person2 Map2(IMapper<Person, Person2> mapper, Person person)
    {
        var person2 = new Person2();
        return mapper.Map(person, person2);
    }
```