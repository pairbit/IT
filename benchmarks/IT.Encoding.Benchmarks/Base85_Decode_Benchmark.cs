using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Encoding.Base;
using K4os.Text.BaseX;

namespace IT.Encoding.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base85_Decode_Benchmark
{
    private ITextEncoder _k4os = new Base85Encoder_K4os();
    internal string _data;

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
        _data = SimpleBase.Base85.Ascii85.Encode(data);
    }

    [Benchmark(Description = "IT_K4os")]
    public Byte[] ITK4os_Bench() => _k4os.Decode(_data);

    [Benchmark(Description = "K4os")]
    public Byte[] K4os_Bench() => Base85.Default.Decode(_data);

    [Benchmark(Description = "SimpleBase")]
    public Span<Byte> SimpleBase_Bench() => SimpleBase.Base85.Ascii85.Decode(_data);

    [Benchmark(Description = "Logos")]
    public Byte[] Logos_Bench() => ReferenceCodec.Logos.Ascii85.Decode(_data);

    [Benchmark(Description = "Fs.Binary.Codecs")]
    public Byte[] FsBinaryCodecs_Bench() => Fs.Binary.Codecs.BinaryCodecs.BtoA.GetBytes(_data);
}