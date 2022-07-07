namespace IT.Mapping;

public interface IMapper<TFrom, TTo>
{
    void Map(TFrom from, TTo to);

    TTo Map(TFrom from);
}