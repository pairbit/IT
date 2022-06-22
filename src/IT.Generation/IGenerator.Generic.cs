using System;

namespace IT.Generation;

public interface IGenerator<T>
{
    T Generate(String? rule = null);
}