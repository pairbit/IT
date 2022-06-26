using System;

namespace IT.Data;

public interface IDataRepositoryUpdaterConditional<TId, TValue>
{
    Boolean UpdateByIdIfEqual(TValue value, TId id, TValue equalValue);

    Boolean UpdateByIdIfNotEqual(TValue value, TId id, TValue notEqualValue);

    //Boolean UpdateByIdIfContains(TValue value, TId id, params TValue[] contains);

    //Boolean UpdateByIdIfNotContains(TValue value, TId id, params TValue[] contains);
}