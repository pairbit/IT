using System;

namespace IT.Mapping;

public interface IMapper
{
    TTo Map<TTo>(Object source);

    TTo Map<TFrom, TTo>(TFrom source);

    TTo Map<TFrom, TTo>(TFrom source, TTo destination);

    Object Map(Type sourceType, Object source, Type destinationType);

    Object Map(Type sourceType, Object source, Type destinationType, Object destination);
}