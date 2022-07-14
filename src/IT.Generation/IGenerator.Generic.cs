using System;

namespace IT.Generation;

public interface IGenerator<T>
{
    T Generate(String? rule = null);

    //void Populate(T value, String? rule = null);
}