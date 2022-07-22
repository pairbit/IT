using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace IT.Encoding.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base32_Encode_Benchmark
{
    internal byte[] _data;
    //private static readonly SimpleBase.Base32 _simpleBase = new SimpleBase.Base32(new SimpleBase.Base32Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", '='));

    //[Params(12, 100, 510, 1024, 510 * 1024, 2 * 1024 * 1024, 510 * 1024 * 1024)]

    //[Params(1, 2, 4, 8, 12, 15, 16, 31, 32, 49, 50, 63, 64, 128, 256, 512, 1024)]
    [Params(2 * 1024, 4 * 1024, 8 * 1024, 16 * 1024, 32 * 1024, 64 * 1024, 128 * 1024, 256 * 1024, 512 * 1024, 1024 * 1024)]
    //[Params(2 * 1024 * 1024, 4 * 1024 * 1024, 8 * 1024 * 1024, 16 * 1024 * 1024, 32 * 1024 * 1024, 64 * 1024 * 1024, 128 * 1024 * 1024, 256 * 1024 * 1024, 510 * 1024 * 1024)]
    //[Params(1024 * 1024)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var data = new byte[Length];
        Random.Shared.NextBytes(data);
        _data = data;
    }

    [Benchmark(Description = "deniszykov")]
    public String deniszykov_Bench() => deniszykov.BaseN.Base32Convert.ToString(_data);

    [Benchmark(Description = "KodeAid")]
    public String KodeAid_Bench() => KodeAid.Base32Encoder.EncodeBytes(_data);

    [Benchmark(Description = "Wiry")]
    public String Wiry_Bench() => Wiry.Base32.Base32Encoding.Standard.GetString(_data);

    //[Benchmark(Description = "SimpleBase")]
    public String SimpleBase_Bench() => SimpleBase.Base32.Rfc4648.Encode(_data, true);

    //[Benchmark(Description = "Albireo")]
    public String Albireo_Bench() => Albireo.Base32.Base32.Encode(_data);

    //[Benchmark(Description = "MikValSor")]
    public String MikValSor_Bench() => MikValSor.Encoding.Base32Encoder.Encode(_data);

    //[Benchmark(Description = "Multiformats")]
    public String Multiformats_Bench() => Multiformats.Base.Multibase.EncodeRaw(Multiformats.Base.MultibaseEncoding.Base32PaddedUpper, _data);

}