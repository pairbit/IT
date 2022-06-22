using System;
using System.Threading.Tasks;

namespace IT.Generation;

public interface IAsyncRangeGenerator<T> : IAsyncGenerator<T>
{
    Task<T> GenerateAsync(T min, T max, String? rule = null);
}