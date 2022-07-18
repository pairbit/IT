using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Encoding.Encoders;
using K4os.Text.BaseX;

namespace IT.Encoding.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base64_Encode_Benchmark
{
    private ITextEncoder _base64Utf8;
    private ITextEncoder _base64gfoidl;
    internal byte[] _data;

    //[Params(14, 100, 510, 1024, 510 * 1024, 2 * 1024 * 1024, 510 * 1024 * 1024)]
    [Params(1, 2, 4, 8, 15, 16, 31, 32, 49, 50, 63, 64)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _base64Utf8 = new Base64Encoder_Utf8();
        _base64gfoidl = new Base64Encoder_gfoidl();
        var data = new byte[Length];
        Random.Shared.NextBytes(data);
        _data = data;
    }

    [Benchmark(Description = "gfoidl")]
    public String gfoidl_Bench() => gfoidl.Base64.Base64.Default.Encode(_data);

    [Benchmark(Description = "IT_gfoidl")]
    public String IT_gfoidl_Bench() => _base64gfoidl.Encode(_data);

    [Benchmark(Description = "System.Buffers.Text.Base64")]
    public String IT_Utf8_Bench() => _base64Utf8.Encode(_data);

    //[Benchmark(Description = "K4os")]
    public String K4os_Bench() => Base64.Default.Encode(_data);

    [Benchmark(Description = "Convert.ToBase64String")]
    public String ToBase64String() => Convert.ToBase64String(_data);

    //[Benchmark(Description = "Multiformats")]
    public String Multiformats_Bench() => Multiformats.Base.Multibase.EncodeRaw(Multiformats.Base.MultibaseEncoding.Base64Padded, _data);

    //[Benchmark(Description = "Exyll")]
    public String Exyll_Bench() => Exyll.Base64Encoder.Default.ToBase(_data);
}