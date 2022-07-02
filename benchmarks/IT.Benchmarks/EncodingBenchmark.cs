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
        _data = File.ReadAllBytes(@"S:\Videos\grip_legend\schwartz.mp4");
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
        var bytes = _data.AsSpan();

        var len = bytes.Length;

        var utf8len = (len + 2) / 3 * 4;

        var utf8 = new byte[utf8len].AsSpan();

        var status = Base64.EncodeToUtf8(bytes, utf8, out var consumed, out var written);

        if (status != System.Buffers.OperationStatus.Done) throw new InvalidOperationException(status.ToString());

        if (consumed != len) throw new InvalidOperationException();

        if (written != utf8len) throw new InvalidOperationException();

        return utf8;
    }

    //https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Convert.cs
    [Benchmark(Description = "Convert.ToBase64String")]
    public string ConvertToBase64String() => Convert.ToBase64String(_data);

    [Benchmark(Description = "Convert.TryToBase64Chars")]
    public ReadOnlySpan<char> TryToBase64Chars()
    {
        var bytes = _data.AsSpan();

        var len = (bytes.Length + 2) / 3 * 4;

        var chars = new char[len].AsSpan();

        var result = Convert.TryToBase64Chars(bytes, chars, out var written);

        if (!result || written != len) throw new InvalidOperationException();

        return chars;
    }

    [Benchmark(Description = "UTF8.GetChars(EncodeToUtf8())")]
    public ReadOnlySpan<char> EncodeToUtf8Chars()
    {
        var bytes = EncodeToUtf8();

        var len = bytes.Length;

        var chars = new char[len].AsSpan();

        var count = Encoding.UTF8.GetChars(bytes, chars);

        if (count != len) throw new InvalidOperationException();

        return chars;
    }

    [Benchmark(Description = "UTF8.GetString(EncodeToUtf8())")]
    public string EncodeToUtf8String() => Encoding.UTF8.GetString(EncodeToUtf8());
}