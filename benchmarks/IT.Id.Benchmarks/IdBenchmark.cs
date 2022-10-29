using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace IT.Id.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class IdBenchmark
{
    private readonly String _idHexUpper;
    private readonly String _idHexLower;
    private readonly String _idBase64Url;
    private readonly String _idBase85;

    public IdBenchmark()
    {
        var id = System.Id.New();
        _idBase85 = id.ToString(Idf.Base85);
        _idHexUpper = id.ToString(Idf.HexUpper);
        _idHexLower = id.ToString(Idf.Hex);
        _idBase64Url = id.ToString();
    }

    [Benchmark]
    public System.Id Id_Parse_Base64Url() => System.Id.Parse(_idBase64Url);

    [Benchmark]
    public System.Id Id_Parse_Base85() => System.Id.Parse(_idBase85);

    [Benchmark]
    public System.Id Id_Parse_HexUpper() => System.Id.Parse(_idHexUpper);

    [Benchmark]
    public System.Id Id_Parse_HexLower() => System.Id.Parse(_idHexLower);

    [Benchmark]
    public System.Id Id_Parse_HexUpper_OLD() => System.Id.Parse(_idHexUpper, Idf.HexUpper);

    [Benchmark]
    public System.Id Id_Parse_HexLower_OLD() => System.Id.Parse(_idHexLower, Idf.Hex);
}