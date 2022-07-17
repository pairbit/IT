using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Text.Encoders;
using K4os.Text.BaseX;

namespace IT.Text.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base64_Encode_Benchmark
{
    private IEncoder _base64;
    internal byte[] _data;

    [Params(100, 510, 1024, 510 * 1024, 2 * 1024 * 1024, 510 * 1024 * 1024)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _base64 = new Base64Encoder();
        var data = new byte[Length];
        Random.Shared.NextBytes(data);
        _data = data;
    }

    [Benchmark(Description = "gfoidl")]
    public String gfoidl_Bench() => gfoidl.Base64.Base64.Default.Encode(_data);

    [Benchmark(Description = "IT")]
    public String IT_Bench() => _base64.Encode(_data);

    [Benchmark(Description = "K4os")]
    public String K4os_Bench() => Base64.Default.Encode(_data);

    [Benchmark(Description = "Convert.ToBase64String")]
    public String ToBase64String() => Convert.ToBase64String(_data);

    //[Benchmark(Description = "Multiformats")]
    public String Multiformats_Bench() => Multiformats.Base.Multibase.EncodeRaw(Multiformats.Base.MultibaseEncoding.Base64Padded, _data);
}