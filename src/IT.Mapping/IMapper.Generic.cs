namespace IT.Mapping;

public interface IMapper<TFrom, TTo>
{
    void Map(TFrom source, TTo destination);

    TTo Map(TFrom source);
}