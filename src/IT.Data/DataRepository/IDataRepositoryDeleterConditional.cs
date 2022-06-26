using System;

namespace IT.Data;

public interface IDataRepositoryDeleterConditional<TId, TValue>
{
    Boolean DeleteByIdIfEqual(TId id, TValue equalValue);

    Boolean DeleteByIdIfNotEqual(TId id, TValue notEqualValue);

    //Boolean DeleteByIdIfContains(TId id, params TValue[] contains);

    //Boolean DeleteByIdIfNotContains(TId id, params TValue[] contains);
}