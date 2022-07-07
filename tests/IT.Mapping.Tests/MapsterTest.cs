using Mapster;

namespace IT.Mapping.Tests;

public class MapsterTest : MapperTest
{
    private static readonly IMapper Mapper = new Mapster.Mapper(GetMapsterMapper());

    public MapsterTest() : base(Mapper)
    {
    }

    private static MapsterMapper.IMapper GetMapsterMapper()
    {
        var config = new TypeAdapterConfig();
        return new MapsterMapper.Mapper(config);
    }
}