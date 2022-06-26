namespace IT.Data;

public interface IId<T> : IReadOnlyId<T>
{
    new T Id { get; set; }
}