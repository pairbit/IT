namespace IT.Data;

public interface IReadOnlyId<T>
{
    T Id { get; }
}