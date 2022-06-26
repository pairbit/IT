using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncFieldCreator<TId>
{
    Task<Boolean> CreateFieldByIdAsync<TField>(String fieldName, TField fieldValue, TId id);

    Task<TId> CreateFieldAsync<TField>(String fieldName, TField fieldValue);

    Task<Boolean> CreateViewByIdAsync<TView>(TView value, TId id);

    Task<TId> CreateViewAsync<TView>(TView value);
}