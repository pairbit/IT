namespace IT.Data;

public interface IDataReaderDeleter<TId, TValue> : IReadOnlyId<TId>
{
    TValue? GetDelete();
}