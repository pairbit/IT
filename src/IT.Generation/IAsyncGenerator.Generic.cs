using System;
using System.Threading.Tasks;

namespace IT.Generation;

public interface IAsyncGenerator<T>
{
    Task<T> GenerateAsync(String? rule = null);
}