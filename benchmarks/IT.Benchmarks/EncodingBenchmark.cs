using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
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
        if (Debugger.IsAttached)
        {
            _data = File.ReadAllBytes(@"S:\Videos\grip_legend\schwartz.mp4");
        }
        else
        {
            _data = File.ReadAllBytes(@"S:\Videos\grip_legend\David_Train.mp4");
            //_data = new byte[60];
            //Random.Shared.NextBytes(_data);
        }
    }

    [Benchmark(Description = "EncodeToUtf8InPlace")]
    public ReadOnlySpan<byte> EncodeToUtf8InPlace()
    {
        var len = _data.Length;

        var utf8len = (len + 2) / 3 * 4;

        var bytes = CopyData(utf8len);

        var status = Base64.EncodeToUtf8InPlace(bytes, len, out var written);

        if (status != System.Buffers.OperationStatus.Done) throw new InvalidOperationException(status.ToString());

        if (written != utf8len) throw new InvalidOperationException();

        return bytes;
    }

    [Benchmark(Description = "ASCII.GetChars(EncodeToUtf8InPlace())")]
    public ReadOnlySpan<char> EncodeToUtf8InPlaceChars()
    {
        var bytes = EncodeToUtf8InPlace();

        var len = bytes.Length;

        Span<char> chars = new char[len];

        var count = Encoding.ASCII.GetChars(bytes, chars);

        if (count != len) throw new InvalidOperationException();

        return chars;
    }

    [Benchmark(Description = "ASCII.GetString(EncodeToUtf8InPlace())")]
    public string EncodeToUtf8InPlaceString() => Encoding.ASCII.GetString(EncodeToUtf8InPlace());

    //https://github.com/dotnet/runtime/blob/main/src/libraries/System.Memory/src/System/Buffers/Text/Base64Encoder.cs
    [Benchmark(Description = "EncodeToUtf8")]
    public ReadOnlySpan<byte> EncodeToUtf8()
    {
        ReadOnlySpan<byte> bytes = CopyData();

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
    public ReadOnlySpan<char> EncodeToUtf8Chars()
    {
        ReadOnlySpan<byte> bytes = CopyData();

        var len = bytes.Length;

        var utf8len = (len + 2) / 3 * 4;

        var pool = ArrayPool<byte>.Shared;

        var rented = pool.Rent(utf8len);

        Span<byte> utf8 = rented;

        try
        {
            var status = Base64.EncodeToUtf8(bytes, utf8, out var consumed, out var written);

            if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

            if (consumed != len) throw new InvalidOperationException();

            if (written != utf8len) throw new InvalidOperationException();

            Span<char> chars = new char[utf8len];

            var count = Encoding.ASCII.GetChars(utf8[..utf8len], chars);

            if (count != utf8len) throw new InvalidOperationException();

            return chars;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    [Benchmark(Description = "ASCII.GetString(EncodeToUtf8())")]
    public string EncodeToUtf8String()
    {
        ReadOnlySpan<byte> bytes = CopyData();

        var len = bytes.Length;

        var utf8len = (len + 2) / 3 * 4;

        var pool = ArrayPool<byte>.Shared;

        var rented = pool.Rent(utf8len);

        Span<byte> utf8 = rented;

        try
        {
            var status = Base64.EncodeToUtf8(bytes, utf8, out var consumed, out var written);

            if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

            if (consumed != len) throw new InvalidOperationException();

            if (written != utf8len) throw new InvalidOperationException();

            return Encoding.ASCII.GetString(utf8[..utf8len]);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    [Benchmark(Description = "Convert.TryToBase64Chars")]
    public ReadOnlySpan<char> TryToBase64Chars()
    {
        ReadOnlySpan<byte> bytes = CopyData();

        var len = (bytes.Length + 2) / 3 * 4;

        Span<char> chars = new char[len];

        var result = Convert.TryToBase64Chars(bytes, chars, out var written);

        if (!result || written != len) throw new InvalidOperationException();

        return chars;
    }

    //https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Convert.cs
    [Benchmark(Description = "Convert.ToBase64String")]
    public string ConvertToBase64String() => Convert.ToBase64String(CopyData());

    [Benchmark(Description = "CopyDataEtalon")]
    public Span<byte> CopyDataEtalon() => CopyData();

    private Span<byte> CopyData(Int32 len = 0)
    {
        ReadOnlySpan<byte> data = _data;

        if (len == 0) len = data.Length;

        Span<byte> span = new byte[len];

        data.CopyTo(span);

        return span;
    }
}