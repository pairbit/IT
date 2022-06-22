using KGySoft.CoreLibraries;
using System;

namespace IT.Generation.KGySoft;

public class Generator : IGenerator
{
    private GenerateObjectSettings? _settings;
    private Random _random = new();

    public Generator(GenerateObjectSettings? settings = null)
    {
        _settings = settings;
    }

    public T Generate<T>(String? rule = null)
    {
        if (rule is not null) throw new NotSupportedException("rule not supported");

        var value = _random.NextObject<T>(_settings);

        if (value is null) throw new NotImplementedException($"No generator found for type '{typeof(T).FullName}'");

        return value;
    }

    public Object Generate(Type type, String? rule = null)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        if (rule is not null) throw new NotSupportedException("rule not supported");

        var value = _random.NextObject(type, _settings);

        if (value is null) throw new NotImplementedException($"No generator found for type '{type.FullName}'");

        return value;
    }
}