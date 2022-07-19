using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Encoding.Base;
using K4os.Text.BaseX;

namespace IT.Encoding.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base16_Decode_Benchmark
{
    private ITextEncoder _base16 = new HexEncoder_HexMate();
    internal String _data;

    //[Params(12, 100, 510, 1024, 510 * 1024, 2 * 1024 * 1024, 510 * 1024 * 1024)]

    //[Params(1, 2, 4, 8, 12, 15, 16, 31, 32, 49, 50, 63, 64, 128, 256, 512, 1024)]
    [Params(2 * 1024, 4 * 1024, 8 * 1024, 16 * 1024, 32 * 1024, 64 * 1024, 128 * 1024, 256 * 1024, 512 * 1024, 1024 * 1024)]
    //[Params(2 * 1024 * 1024, 4 * 1024 * 1024, 8 * 1024 * 1024, 16 * 1024 * 1024, 32 * 1024 * 1024, 64 * 1024 * 1024, 128 * 1024 * 1024, 256 * 1024 * 1024, 510 * 1024 * 1024)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var data = new byte[Length];
        Random.Shared.NextBytes(data);
        _data = Convert.ToHexString(data);
    }

    [Benchmark(Description = "IT_HexMate")]
    public Span<Byte> IT_Bench() => _base16.Decode(_data);

    [Benchmark(Description = "HexMate")]
    public Span<Byte> HexMate_Bench() => HexMate.Convert.FromHexString(_data);

    [Benchmark(Description = "K4os")]
    public Span<Byte> K4os_Bench() => Base16.Default.Decode(_data);

    [Benchmark(Description = "DR")]
    public Span<Byte> DR_Bench() => DRDigit.Fast.FromHexString(_data);

    [Benchmark(Description = "SimpleBase")]
    public Span<Byte> SimpleBase_Bench() => SimpleBase.Base16.UpperCase.Decode(_data);

    [Benchmark(Description = "Convert.FromHexString")]
    public Span<Byte> Convert_FromHexString() => Convert.FromHexString(_data);

    [Benchmark(Description = "Dodo")]
    public Span<Byte> Dodo_Bench() => Dodo.Primitives.Hex.GetBytes(_data)!;
}