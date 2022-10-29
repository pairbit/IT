using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace IT.Id.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class IdBenchmark
{
    private readonly Ulid _ulid;
    private readonly String _ulidString;

    private readonly System.Id _id;
    private readonly String _idHexLower;
    private readonly String _idHexUpper;
    private readonly String _idBase64Url;
    private readonly String _idBase85;
    private readonly String _idPath2;
    private readonly String _idPath3;

    public IdBenchmark()
    {
        _id = System.Id.New();
        _idHexUpper = Id_Encode_HexUpper();
        _idHexLower = Id_Encode_HexLower();
        _idBase64Url = Id_Encode_Base64Url();
        _idBase85 = Id_Encode_Base85();
        _idPath2 = Id_Encode_Path2();
        _idPath3 = Id_Encode_Path3();

        _ulid = Ulid.NewUlid();
        _ulidString = Ulid_Encode();
    }

    [Benchmark]
    public String Id_Encode_HexLower() => _id.ToString(Idf.Hex);

    [Benchmark]
    public System.Id Id_Decode_HexLower() => System.Id.Parse(_idHexLower);

    [Benchmark]
    public String Id_Encode_HexUpper() => _id.ToString(Idf.HexUpper);

    [Benchmark]
    public System.Id Id_Decode_HexUpper() => System.Id.Parse(_idHexUpper);

    [Benchmark]
    public String Id_Encode_Base64Url() => _id.ToString();

    [Benchmark]
    public System.Id Id_Decode_Base64Url() => System.Id.Parse(_idBase64Url);

    [Benchmark]
    public String Id_Encode_Base85() => _id.ToString(Idf.Base85);

    [Benchmark]
    public System.Id Id_Decode_Base85() => System.Id.Parse(_idBase85);

    [Benchmark]
    public String Id_Encode_Path2() => _id.ToString(Idf.Path2);

    [Benchmark]
    public System.Id Id_Decode_Path2() => System.Id.Parse(_idPath2);

    [Benchmark]
    public String Id_Encode_Path3() => _id.ToString(Idf.Path3);

    [Benchmark]
    public System.Id Id_Decode_Path3() => System.Id.Parse(_idPath3);

    [Benchmark]
    public String Ulid_Encode() => _ulid.ToString();

    [Benchmark]
    public Ulid Ulid_Decode() => Ulid.Parse(_ulidString);
}