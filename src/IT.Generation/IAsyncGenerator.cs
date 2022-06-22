using System;
using System.Threading.Tasks;

namespace IT.Generation;

public interface IAsyncGenerator
{
    Task<T> GenerateAsync<T>(String? rule = null);

    Task<Object> GenerateAsync(Type type, String? rule = null);
}