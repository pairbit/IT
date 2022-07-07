using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Generation;
using IT.Mapping.Benchmarks.Data;
using Mapster;

namespace IT.Mapping.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class MapperBenchmark
{
    private static readonly IGenerator _generator = new Generation.KGySoft.Generator();
    private static readonly Person _person = _generator.Generate<Person>();
    private static readonly IMapper _autoMapper;
    private static readonly IMapper _mapster;

    static MapperBenchmark()
    {
        _autoMapper = new AutoMapper.Mapper(new global::AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.CreateMap<City, City2>();
            cfg.CreateMap<City2, City>();

            cfg.CreateMap<Address, Address2>();
            cfg.CreateMap<Address2, Address>();

            cfg.CreateMap<Person, Person2>();
            cfg.CreateMap<Person2, Person>();
        }).CreateMapper());

        _mapster = new Mapster.Mapper(new MapsterMapper.Mapper(new TypeAdapterConfig()));
    }

    [Benchmark]
    public void AutoMapper()
    {
        var person2 = _autoMapper.Map<Person, Person2>(_person);
        if (person2.Id != _person.Id) throw new InvalidOperationException();
    }

    [Benchmark]
    public void Mapster()
    {
        var person2 = _mapster.Map<Person, Person2>(_person);
        if (person2.Id != _person.Id) throw new InvalidOperationException();
    }
}