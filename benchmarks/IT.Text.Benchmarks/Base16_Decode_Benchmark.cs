using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Text.Encoders;
using K4os.Text.BaseX;

namespace IT.Text.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base16_Decode_Benchmark
{
    private IEncoder _base16Mate = new HexMateEncoder();
    internal String _data;

    [Params(16, 1337, 1024 * 1024)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var data = new byte[Length];
        Random.Shared.NextBytes(data);
        _data = Convert.ToHexString(data);
    }

    [Benchmark]
    public Span<Byte> IT_HexMate() => _base16Mate.Decode(_data);

    [Benchmark]
    public Span<Byte> HexMate_Convert() => HexMate.Convert.FromHexString(_data);

    [Benchmark]
    public Span<Byte> Lower_K4os() => Base16.Lower.Decode(_data);

    [Benchmark]
    public Span<Byte> Upper_K4os() => Base16.Upper.Decode(_data);

    [Benchmark]
    public Span<Byte> Lower_DR() => DRDigit.Fast.FromHexString(_data);

    [Benchmark]
    public Span<Byte> Lower_SimpleBase() => SimpleBase.Base16.LowerCase.Decode(_data);

    [Benchmark]
    public Span<Byte> Upper_SimpleBase() => SimpleBase.Base16.UpperCase.Decode(_data);

    [Benchmark]
    public Span<Byte> Upper_Convert() => Convert.FromHexString(_data);
}