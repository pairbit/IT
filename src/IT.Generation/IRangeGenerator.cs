using System;

namespace IT.Generation;

public interface IRangeGenerator : IGenerator
{
    T Generate<T>(T min, T max, String? rule = null);

    Object Generate(Type type, Object min, Object max, String? rule = null);
}