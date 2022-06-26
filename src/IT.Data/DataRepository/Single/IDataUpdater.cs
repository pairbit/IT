using System;

namespace IT.Data;

public interface IDataUpdater<TId, TValue> : IReadOnlyId<TId>
{
    //Upsert
    Boolean Update(TValue value);

    //Update
    Boolean UpdateIfExists(TValue value);
}

//public static class xIUpdater
//{
//    public static Boolean Update<TId, TValue>(this IDataRepositoryUpdater<TId, TValue> repo, TValue value)
//    {
//        var id = default(TId); // TODO: Get Id from value
//        return repo.UpdateById(value, id);
//    }

//    public static Boolean UpdateMany<TId, TValue>(this IDataRepositoryUpdater<TId, TValue> repo, params TValue[] value)
//    {
//        throw new NotImplementedException();
//    }
//}