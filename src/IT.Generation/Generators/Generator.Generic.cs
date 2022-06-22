using System;

namespace IT.Generation.Generators;

public class Generator<T> : IGenerator<T>
{
    private readonly IGenerator _generator;

    public Generator(IGenerator generator)
    {
        _generator = generator;
    }

    public T Generate(String? rule = null) => _generator.Generate<T>();

    //public T Generate(T min, T max, String? rule = null) => _generator.Generate<T>(min, max, rule);
}