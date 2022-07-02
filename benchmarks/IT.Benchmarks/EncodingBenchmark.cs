using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Buffers.Text;
using System.Text;

namespace IT.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class EncodingBenchmark
{
    private readonly byte[] _data;

    public EncodingBenchmark()
    {
        _data = File.ReadAllBytes(@"S:\Videos\grip_legend\David_Train.mp4");
    }

    //[Benchmark(Description = "Base64.EncodeToUtf8InPlace")]
    //public ReadOnlySpan<byte> EncodeToUtf8InPlace()
    //{
    //    var bytes = _data.AsSpan();

    //    var len = bytes.Length;

    //    var utf8len = (len + 2) / 3 * 4;

    //    var utf8 = new byte[utf8len].AsSpan();

    //    var status = Base64.EncodeToUtf8InPlace(bytes,);

    //    if (status != System.Buffers.OperationStatus.Done) throw new InvalidOperationException(status.ToString());


    //    return utf8;
    //}

    //https://github.com/dotnet/runtime/blob/main/src/libraries/System.Memory/src/System/Buffers/Text/Base64Encoder.cs
    [Benchmark(Description = "Base64.EncodeToUtf8")]
    public ReadOnlySpan<byte> EncodeToUtf8()
    {
        ReadOnlySpan<byte> bytes = _data;

        var len = bytes.Length;

        var utf8len = (len + 2) / 3 * 4;

        Span<byte> utf8 = new byte[utf8len];

        var status = Base64.EncodeToUtf8(bytes, utf8, out var consumed, out var written);

        if (status != System.Buffers.OperationStatus.Done) throw new InvalidOperationException(status.ToString());

        if (consumed != len) throw new InvalidOperationException();

        if (written != utf8len) throw new InvalidOperationException();

        return utf8;
    }

    [Benchmark(Description = "ASCII.GetChars(EncodeToUtf8())")]
    public ReadOnlySpan<char> ASCIIEncodeToUtf8Chars()
    {
        var bytes = EncodeToUtf8();

        var len = bytes.Length;

        Span<char> chars = new char[len];

        var count = Encoding.ASCII.GetChars(bytes, chars);

        if (count != len) throw new InvalidOperationException();

        return chars;
    }

    [Benchmark(Description = "ASCII.GetString(EncodeToUtf8())")]
    public string ASCIIEncodeToUtf8String() => Encoding.ASCII.GetString(EncodeToUtf8());

    [Benchmark(Description = "Convert.TryToBase64Chars")]
    public ReadOnlySpan<char> TryToBase64Chars()
    {
        ReadOnlySpan<byte> bytes = _data;

        var len = (bytes.Length + 2) / 3 * 4;

        Span<char> chars = new char[len];

        var result = Convert.TryToBase64Chars(bytes, chars, out var written);

        if (!result || written != len) throw new InvalidOperationException();

        return chars;
    }

    //https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Convert.cs
    [Benchmark(Description = "Convert.ToBase64String")]
    public string ConvertToBase64String() => Convert.ToBase64String(_data);
}