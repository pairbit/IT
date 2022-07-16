using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using K4os.Text.BaseX;
using System.Diagnostics;

namespace IT.Text.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class BaseCoderBenchmark
{
    private IEncoding _base16lower = new HexEncoding();
    private IEncoding _base16upper = new HexUpperEncoding();
    private readonly byte[] _data;

    public BaseCoderBenchmark()
    {
        if (Debugger.IsAttached)
        {
            _data = new byte[1024];//1KB
        }
        else
        {
            _data = new byte[100 * 1024 * 1024];//10MB
        }
        Random.Shared.NextBytes(_data);
    }

    [Benchmark]
    public String Base16_Upper_Encode_SimpleBase() => SimpleBase.Base16.UpperCase.Encode(_data);

    [Benchmark]
    public String Base16_Lower_Encode_SimpleBase() => SimpleBase.Base16.LowerCase.Encode(_data);

    [Benchmark]
    public String Base16_Upper_Encode_K4os() => Base16.Upper.Encode(_data);

    [Benchmark]
    public String Base16_Lower_Encode_K4os() => Base16.Lower.Encode(_data);

    [Benchmark]
    public String Base16_Upper_Encode_IT() => _base16upper.GetString(_data);

    [Benchmark]
    public String Base16_Lower_Encode_IT() => _base16lower.GetString(_data);
}