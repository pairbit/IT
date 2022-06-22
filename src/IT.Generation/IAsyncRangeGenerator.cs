using System;
using System.Threading.Tasks;

namespace IT.Generation;

public interface IAsyncRangeGenerator : IAsyncGenerator
{
    Task<T> GenerateAsync<T>(T min, T max, String? rule = null);

    Task<Object> GenerateAsync(Type type, Object min, Object max, String? rule = null);
}