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
    private IEncoder _base16 = new HexEncoder();
    internal String _data;

    [Params(100, 510, 1024, 510 * 1024, 2 * 1024 * 1024, 510 * 1024 * 1024)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var data = new byte[Length];
        Random.Shared.NextBytes(data);
        _data = Convert.ToHexString(data);
    }

    [Benchmark]
    public Span<Byte> IT_Bench() => _base16.Decode(_data);

    [Benchmark]
    public Span<Byte> HexMate_Bench() => HexMate.Convert.FromHexString(_data);

    //[Benchmark]
    public Span<Byte> K4os_Bench() => Base16.Default.Decode(_data);

    //[Benchmark]
    public Span<Byte> DR_Bench() => DRDigit.Fast.FromHexString(_data);

    //[Benchmark]
    public Span<Byte> SimpleBase_Bench() => SimpleBase.Base16.UpperCase.Decode(_data);

    [Benchmark(Description = "System.Convert.FromHexString")]
    public Span<Byte> Convert_FromHexString() => Convert.FromHexString(_data);

    //[Benchmark]
    public Span<Byte> Dodo_Bench() => Dodo.Primitives.Hex.GetBytes(_data)!;
}