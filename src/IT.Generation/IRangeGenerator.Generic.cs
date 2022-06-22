using System;

namespace IT.Generation;

public interface IRangeGenerator<T> : IGenerator<T>
{
    T Generate(T min, T max, String? rule = null);
}