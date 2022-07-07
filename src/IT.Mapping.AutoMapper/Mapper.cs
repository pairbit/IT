using System;

namespace IT.Mapping.AutoMapper;

public class Mapper : IMapper
{
    private readonly global::AutoMapper.IMapperBase _mapper;

    public Mapper(global::AutoMapper.IMapperBase mapper)
    {
        _mapper = mapper;
    }

    public TTo Map<TTo>(Object from) => _mapper.Map<TTo>(from);

    public TTo Map<TFrom, TTo>(TFrom from) => _mapper.Map<TFrom, TTo>(from);

    public TTo Map<TFrom, TTo>(TFrom from, TTo to) => _mapper.Map(from, to);

    public Object Map(Type fromType, Object from, Type toType) => _mapper.Map(from, fromType, toType);

    public Object Map(Type fromType, Object from, Type toType, Object to) => _mapper.Map(from, to, fromType, toType);
}