using System;
using System.Collections;
using System.Collections.Generic;

namespace IT.Generation.Generators;

public class Generator : IGenerator
{
    private readonly static Random _random = new();
    private readonly IServiceProvider _serviceProvider;

    public Generator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T Generate<T>(String? rule = null)
    {
        var generator = (IGenerator<T>?)_serviceProvider.GetService(typeof(IGenerator<T>));
        if (generator != null && !generator.GetType().GetGenericTypeDefinition().Equals(typeof(Generator<>))) return generator.Generate();
        
        var type = typeof(T);
        var underlyingType = Nullable.GetUnderlyingType(type);

        return GenerateByType(underlyingType ?? type);
    }

    public T Generate<T>(T min, T max, String? rule = null)
    {
        throw new NotImplementedException();
    }

    public Object Generate(Type type, String? rule = null)
    {
        if (type == null) return GenerateRandom();

        var generatorType = typeof(IGenerator<>).MakeGenericType(type);
        var generator = _serviceProvider.GetService(generatorType);
        if (generator != null)
        {
            var method = generatorType.GetMethod("Generate");
            return method.Invoke(generator, Array.Empty<Object>());
        }

        return GenerateByType(Nullable.GetUnderlyingType(type) ?? type);
    }

    public Object Generate(Object min, Object max, Type? type = null)
    {
        throw new NotImplementedException();
    }

    private dynamic GenerateRandom() => _random.Next(0, 14) switch
    {
        0 => GenerateBoolean(),
        1 => GenerateChar(),
        2 => GenerateByte(),
        3 => GenerateDateTime(),
        4 => GenerateDecimal(),
        5 => GenerateDouble(),
        6 => GenerateInt16(),
        7 => GenerateInt32(),
        8 => GenerateInt64(),
        9 => GenerateSByte(),
        10 => GenerateSingle(),
        11 => GenerateString(),
        12 => GenerateUInt16(),
        13 => GenerateUInt32(),
        14 => GenerateUInt64(),
        _ => throw new InvalidOperationException()
    };

    private dynamic GenerateByType(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.Boolean => GenerateBoolean(),
        TypeCode.Char => GenerateChar(),
        TypeCode.Byte => GenerateByte(),
        TypeCode.DateTime => GenerateDateTime(),
        TypeCode.DBNull => throw new InvalidOperationException(),
        TypeCode.Decimal => GenerateDecimal(),
        TypeCode.Double => GenerateDouble(),
        TypeCode.Empty => throw new InvalidOperationException(),
        TypeCode.Int16 => GenerateInt16(),
        TypeCode.Int32 => GenerateInt32(),
        TypeCode.Int64 => GenerateInt64(),
        TypeCode.Object => GenerateObject(type),
        TypeCode.SByte => GenerateSByte(),
        TypeCode.Single => GenerateSingle(),
        TypeCode.String => GenerateString(),
        TypeCode.UInt16 => GenerateUInt16(),
        TypeCode.UInt32 => GenerateUInt32(),
        TypeCode.UInt64 => GenerateUInt64(),
        _ => throw new NotImplementedException()
    };

    protected virtual dynamic GenerateObject(Type type)
    {
        if (type == typeof(Guid)) return GenerateGuid();

        if (type == typeof(Byte[])) return GenerateBytes();

        if (type.IsAssignableFrom(typeof(IEnumerable))) return GenerateIEnumerable();

        if (type.IsAssignableFrom(typeof(IEnumerable<>))) return GenerateIEnumerable();

        throw new NotImplementedException($"Generator for '{type.FullName}' not implemented");
    }

    protected virtual IEnumerable GenerateIEnumerable()
    {
        var array = new Object[0];
        return array;
    }

    protected virtual String GenerateString() => Guid.NewGuid().ToString();

    protected virtual Guid GenerateGuid() => Guid.NewGuid();

    protected virtual Boolean GenerateBoolean() => _random.Next(0, 1) == 1;

    protected virtual SByte GenerateSByte() => (SByte)_random.Next(SByte.MinValue, SByte.MaxValue);

    protected virtual Byte GenerateByte() => (Byte)_random.Next(Byte.MinValue, Byte.MaxValue);

    protected virtual Int16 GenerateInt16() => (Int16)_random.Next(Int16.MinValue, Int16.MaxValue);

    protected virtual UInt16 GenerateUInt16() => (UInt16)_random.Next(0, Int16.MaxValue);

    protected virtual Int32 GenerateInt32() => _random.Next(Int32.MinValue, Int32.MaxValue);

    protected virtual UInt32 GenerateUInt32() => (UInt32)_random.Next(0, Int32.MaxValue);

    protected virtual Int64 GenerateInt64() => _random.Next(Int32.MinValue, Int32.MaxValue);

    protected virtual UInt64 GenerateUInt64() => (UInt64)_random.Next(0, Int32.MaxValue);

    protected virtual Single GenerateSingle() => _random.Next(Int32.MinValue, Int32.MaxValue);

    protected virtual Double GenerateDouble() => _random.Next(Int32.MinValue, Int32.MaxValue);

    protected virtual Decimal GenerateDecimal() => _random.Next(Int32.MinValue, Int32.MaxValue);

    protected virtual Char GenerateChar() => (Char)_random.Next(Char.MinValue, Char.MaxValue);

    protected virtual DateTime GenerateDateTime() => new(
        _random.Next(1900, DateTime.Today.Year + 10),
        _random.Next(1, 12), _random.Next(1, 28), _random.Next(0, 23), _random.Next(0, 59), _random.Next(0, 59));

    protected virtual Byte[] GenerateBytes()
    {
        var len = _random.Next(Byte.MinValue, Byte.MaxValue);
        var bytes = new Byte[len];
        _random.NextBytes(bytes);
        return bytes;
    }
}