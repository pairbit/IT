using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace IT.Id.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class IdBenchmark
{
    private readonly String _idBase64Url;

    public IdBenchmark()
    {
        _idBase64Url = System.Id.New().ToString();
    }

    [Benchmark]
    public System.Id Id_Parse() => System.Id.Parse(_idBase64Url);

    [Benchmark]
    public System.Id Id_Parse_Old() => System.Id.Parse(_idBase64Url, Idf.Base64Url);
}