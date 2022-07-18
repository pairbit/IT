using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Text.Encoders;
using K4os.Text.BaseX;

namespace IT.Encoding.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base64_Decode_Benchmark
{
    private IEncoder _base64Utf8 = new Base64Utf8Encoder();
    private IEncoder _base64gfoidl = new Base64gfoidlEncoder();
    internal string _data;

    //[Params(14, 100, 510, 1024, 510 * 1024, 2 * 1024 * 1024, 510 * 1024 * 1024)]
    [Params(1, 2, 4, 8, 15, 16, 31, 32, 49, 50, 63, 64, 100, 510, 1024, 510 * 1024, 2 * 1024 * 1024, 510 * 1024 * 1024)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var data = new byte[Length];
        Random.Shared.NextBytes(data);
        _data = Convert.ToBase64String(data);
    }

    [Benchmark(Description = "gfoidl")]
    public Byte[] gfoidl_Bench() => gfoidl.Base64.Base64.Default.Decode(_data);

    [Benchmark(Description = "IT_gfoidl")]
    public Byte[] IT_gfoidl_Bench() => _base64gfoidl.Decode(_data);

    //[Benchmark(Description = "IT_Utf8")]
    //public Byte[] IT_Utf8_Bench() => _base64Utf8.Decode(_data);

    //[Benchmark(Description = "K4os")]
    public Byte[] K4os_Bench() => Base64.Default.Decode(_data);

    [Benchmark(Description = "Convert.ToBase64String")]
    public Byte[] FromBase64String() => Convert.FromBase64String(_data);

    //[Benchmark(Description = "Multiformats")]
    public Byte[] Multiformats_Bench() => Multiformats.Base.Multibase.DecodeRaw(Multiformats.Base.MultibaseEncoding.Base64Padded, _data);

    //[Benchmark(Description = "Exyll")]
    public Byte[] Exyll_Bench() => Exyll.Base64Encoder.Default.FromBase(_data);
}