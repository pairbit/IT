using System;

namespace IT.Mapping;

public interface IMapper
{
    TTo Map<TTo>(Object from);

    TTo Map<TFrom, TTo>(TFrom from);

    TTo Map<TFrom, TTo>(TFrom from, TTo? to);

    Object Map(Type fromType, Object from, Type toType);

    Object Map(Type fromType, Object from, Type toType, Object? to);
}