namespace IT.Mapping;

public class Mapper<TFrom, TTo> : IMapper<TFrom, TTo>
{
    private readonly IMapper _mapper;

    public Mapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TTo Map(TFrom from) => _mapper.Map<TFrom, TTo>(from);

    public TTo Map(TFrom from, TTo? to) => _mapper.Map(from, to);
}