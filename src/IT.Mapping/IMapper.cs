namespace IT.Mapping;

internal interface IMapper
{
    T Map<T, T2>(T value);
}