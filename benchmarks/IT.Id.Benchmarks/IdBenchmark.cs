using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace IT.Id.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class IdBenchmark
{
    private readonly String _id;

    public IdBenchmark()
    {
        _id = System.Id.New().ToString(Idf.Base64Url);
    }

    [Benchmark]
    public String Id_Base64Url() => System.Id.New().ToString(Idf.Base64Url);
}