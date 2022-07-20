using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Encoding.Base;
using K4os.Text.BaseX;

namespace IT.Encoding.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base85Z_Encode_Benchmark
{
    private ITextEncoder _k4os = new Base85Encoder_K4os(true);
    private ITextEncoder _coenM = new Base85ZEncoder_CoenM();
    private Base85Codec _z85 = new("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#", '\0');
    internal byte[] _data;

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
        _data = data;
    }

    [Benchmark(Description = "IT_K4os")]
    public String ITK4os_Bench() => _k4os.EncodeToText(_data);

    [Benchmark(Description = "K4os")]
    public String K4os_Bench() => _z85.Encode(_data);

    [Benchmark(Description = "IT_CoenM")]
    public String ITCoenM_Bench() => _coenM.EncodeToText(_data);

    [Benchmark(Description = "CoenM")]
    public String CoenM_Bench() => CoenM.Encoding.Z85.Encode(_data);

    [Benchmark(Description = "SimpleBase")]
    public String SimpleBase_Bench() => SimpleBase.Base85.Z85.Encode(_data);

    //[Benchmark(Description = "Fs.Binary.Codecs")]
    public String FsBinaryCodecs_Bench() => Fs.Binary.Codecs.BinaryCodecs.Z85.GetString(_data);
}