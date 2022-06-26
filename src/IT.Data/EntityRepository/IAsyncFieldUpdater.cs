using System;
using System.Threading.Tasks;

namespace IT.Data;

public interface IAsyncFieldUpdater<TId>
{
    Task<Boolean> UpdateFieldByIdAsync<TField>(String fieldName, TField fieldValue, TId id);

    Task<Boolean> UpdateViewByIdAsync<TView>(TView value, TId id);
}