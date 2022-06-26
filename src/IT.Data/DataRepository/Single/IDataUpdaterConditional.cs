using System;

namespace IT.Data;

public interface IDataUpdaterConditional<TId, TValue> : IReadOnlyId<TId>
{
    Boolean UpdateIfEqual(TValue value, TValue equalValue);

    Boolean UpdateIfNotEqual(TValue value, TValue notEqualValue);

    //Boolean UpdateByIdIfContains(TValue value, TId id, params TValue[] contains);

    //Boolean UpdateByIdIfNotContains(TValue value, TId id, params TValue[] contains);
}