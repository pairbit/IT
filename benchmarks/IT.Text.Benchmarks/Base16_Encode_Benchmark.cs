using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Text.Encoders;
using K4os.Text.BaseX;

namespace IT.Text.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base16_Encode_Benchmark
{
    private IEncoder _base16lower;
    private IEncoder _base16upper;
    private IEncoder _base16lowerOld;
    private IEncoder _base16upperOld;
    private IEncoder _base16lowerMate;
    private IEncoder _base16upperMate;
    internal byte[] _data;

    [Params(100, 510, 1024, 510 * 1024, 2 * 1024 * 1024, 510 * 1024 * 1024)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _base16lower = new HexEncoder(false);
        _base16upper = new HexEncoder(true);
        _base16lowerOld = new HexOldEncoder(false);
        _base16upperOld = new HexOldEncoder(true);
        _base16lowerMate = new HexMateEncoder(false);
        _base16upperMate = new HexMateEncoder(true);
        var data = new byte[Length];
        Random.Shared.NextBytes(data);
        _data = data;
    }

    [Benchmark]
    public String Upper_IT_new() => _base16upper.Encode(_data);

    //[Benchmark]
    public String Lower_IT_new() => _base16lower.Encode(_data);

    //[Benchmark]
    public String Upper_IT_HexMate() => _base16upperMate.Encode(_data);

    //[Benchmark]
    public String Lower_IT_HexMate() => _base16lowerMate.Encode(_data);

    [Benchmark]
    public String Upper_IT_old() => _base16upperOld.Encode(_data);

    //[Benchmark]
    public String Lower_IT_old() => _base16lowerOld.Encode(_data);

    //[Benchmark]
    public String Upper_HexMate() => HexMate.Convert.ToHexString(_data);

    //[Benchmark]
    public String Lower_HexMate() => HexMate.Convert.ToHexString(_data, HexMate.HexFormattingOptions.Lowercase);

    //[Benchmark]
    public String Upper_K4os() => Base16.Upper.Encode(_data);

    //[Benchmark]
    public String Lower_K4os() => Base16.Lower.Encode(_data);

    //[Benchmark]
    public String Upper_DR() => DRDigit.Fast.ToHexString(_data);

    [Benchmark(Description = "System.Convert.ToHexString")]
    public String Upper_Convert() => Convert.ToHexString(_data);

    //[Benchmark]
    public String Upper_SimpleBase() => SimpleBase.Base16.UpperCase.Encode(_data);

    //[Benchmark]
    public String Lower_SimpleBase() => SimpleBase.Base16.LowerCase.Encode(_data);

    [Benchmark]
    public String Lower_Dodo() => Dodo.Primitives.Hex.GetString(_data)!;
}