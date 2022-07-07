namespace IT.Mapping;

public interface IMapper<TFrom, TTo>
{
    TTo Map(TFrom from);

    TTo Map(TFrom from, TTo? to);
}