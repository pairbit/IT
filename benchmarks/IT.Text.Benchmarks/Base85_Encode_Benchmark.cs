using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using K4os.Text.BaseX;

namespace IT.Text.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base85_Encode_Benchmark
{
    internal byte[] _data;

    [Params(14, 100, 510, 1024, 510 * 1024, 2 * 1024 * 1024, 400 * 1024 * 1024)]
    //[Params(1, 2, 4, 8, 15, 16, 31, 32, 49, 50, 63, 64)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var data = new byte[Length];
        Random.Shared.NextBytes(data);
        _data = data;
    }

    [Benchmark(Description = "K4os")]
    public String K4os_Bench() => Base85.Default.Encode(_data);

    [Benchmark(Description = "SimpleBase")]
    public String SimpleBase_Bench() => SimpleBase.Base85.Ascii85.Encode(_data);

    //[Benchmark(Description = "Logos")]
    public String Logos_Bench() => ReferenceCodec.Logos.Ascii85.Encode(_data);

    //[Benchmark(Description = "Fs.Binary.Codecs")]
    public String FsBinaryCodecs_Bench() => Fs.Binary.Codecs.BinaryCodecs.Base85Standard.GetString(_data);
}