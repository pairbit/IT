using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace IT.Encoding.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base32_Decode_Benchmark
{
    internal string _data;

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
        _data = SimpleBase.Base32.Rfc4648.Encode(data, true);
    }

    [Benchmark(Description = "Wiry")]
    public Byte[] Wiry_Bench() => Wiry.Base32.Base32Encoding.Standard.ToBytes(_data);

    [Benchmark(Description = "KodeAid")]
    public Byte[] KodeAid_Bench() => KodeAid.Base32Encoder.DecodeBytes(_data);

    //[Benchmark(Description = "SimpleBase")]
    public Span<Byte> SimpleBase_Bench() => SimpleBase.Base32.Rfc4648.Decode(_data);

    //[Benchmark(Description = "deniszykov")]
    public Byte[] deniszykov_Bench() => deniszykov.BaseN.Base32Convert.ToBytes(_data);

    //[Benchmark(Description = "MikValSor")]
    public Byte[] MikValSor_Bench() => MikValSor.Encoding.Base32Decoder.Decode(_data);

    //[Benchmark(Description = "Multiformats")]
    public Byte[] Multiformats_Bench() => Multiformats.Base.Multibase.DecodeRaw(Multiformats.Base.MultibaseEncoding.Base32PaddedUpper, _data);

    //[Benchmark(Description = "Albireo")]
    public Byte[] Albireo_Bench() => Albireo.Base32.Base32.Decode(_data);

}