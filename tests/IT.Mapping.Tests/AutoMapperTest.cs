using IT.Mapping.Tests.Data;

namespace IT.Mapping.Tests;

public class AutoMapperTest : MapperTest
{
    private static readonly IMapper Mapper = new AutoMapper.Mapper(GetAutoMapper());

    public AutoMapperTest() : base(Mapper) { }

    private static global::AutoMapper.IMapperBase GetAutoMapper() => new global::AutoMapper.MapperConfiguration(cfg =>
    {
        cfg.CreateMap<City, City2>();
        cfg.CreateMap<City2, City>();

        cfg.CreateMap<Address, Address2>();
        cfg.CreateMap<Address2, Address>();

        cfg.CreateMap<Person, Person2>();
        cfg.CreateMap<Person2, Person>();
    }).CreateMapper();
}