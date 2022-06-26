using System;

namespace IT.Data;

public interface IDataDeleterConditional<TId, TValue> : IReadOnlyId<TId>
{
    Boolean DeleteIfEqual(TValue equalValue);

    Boolean DeleteIfNotEqual(TValue notEqualValue);

    //Boolean DeleteIfContains(TId id, params TValue[] contains);

    //Boolean DeleteIfNotContains(TId id, params TValue[] contains);
}