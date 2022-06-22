namespace IT.Mapping;

internal interface IMapper<T, T2>
{
    T2 Map(T value);
}