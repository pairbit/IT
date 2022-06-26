using System;

namespace IT.Data;

public interface IDataRepositoryUpdater<TId, TValue>
{
    Boolean UpdateById(TValue value, TId id);

    Boolean UpdateByIdIfExists(TValue value, TId id);

    void UpdateByIds(params IdValue<TId, TValue>[] data);

    void UpdateByIdsIfExists(params IdValue<TId, TValue>[] data);
}

public static class xIRepositoryUpdater
{
    public static Boolean Update<TId, TValue>(this IDataRepositoryUpdater<TId, TValue> repo, TValue value)
    {
        var id = default(TId); // TODO: Get Id from value
        return repo.UpdateById(value, id);
    }

    public static Boolean UpdateMany<TId, TValue>(this IDataRepositoryUpdater<TId, TValue> repo, params TValue[] value)
    {
        throw new NotImplementedException();
    }
}