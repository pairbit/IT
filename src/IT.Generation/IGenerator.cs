using System;

namespace IT.Generation;

public interface IGenerator
{
    T Generate<T>(String? rule = null);

    //void Populate<T>(T value, String? rule = null);

    Object Generate(Type type, String? rule = null);
}