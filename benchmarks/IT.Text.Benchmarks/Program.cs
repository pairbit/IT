using IT.Text.Benchmarks;
using System.Text;

Base16Test();

//BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Base16_Encode_Benchmark));
//BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Base16_Decode_Benchmark));

Base64Test();

//BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Base64_Encode_Benchmark));
//BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Base64_Decode_Benchmark));

Base85Test();

BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Base85_Encode_Benchmark));
//BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Base85_Decode_Benchmark));

static void Base16Test()
{
    var encoder = new Base16_Encode_Benchmark();

    encoder.Length = 16;
    encoder.Setup();

    var u1 = encoder.Upper_IT_old();
    var u2 = encoder.Upper_SimpleBase();
    var u3 = encoder.Upper_K4os();
    var u4 = encoder.Upper_DR();
    var u5 = encoder.Upper_Convert();
    var u6 = encoder.Upper_HexMate();
    var u7 = encoder.Upper_IT_HexMate();

    if (!u1.Equals(u2)) throw new InvalidOperationException();
    if (!u1.Equals(u3)) throw new InvalidOperationException();
    if (!u1.Equals(u4)) throw new InvalidOperationException();
    if (!u1.Equals(u5)) throw new InvalidOperationException();
    if (!u1.Equals(u6)) throw new InvalidOperationException();
    if (!u1.Equals(u7)) throw new InvalidOperationException();

    var l1 = encoder.Lower_IT_old();
    var l2 = encoder.Lower_SimpleBase();
    var l3 = encoder.Lower_K4os();
    var l4 = encoder.Lower_HexMate();
    var l5 = encoder.Lower_IT_HexMate();
    var l6 = encoder.Lower_Dodo();

    if (!l1.Equals(l2)) throw new InvalidOperationException();
    if (!l1.Equals(l3)) throw new InvalidOperationException();
    if (!l1.Equals(l4)) throw new InvalidOperationException();
    if (!l1.Equals(l5)) throw new InvalidOperationException();
    if (!l1.Equals(l6)) throw new InvalidOperationException();

    var decoder = new Base16_Decode_Benchmark();
    decoder._data = l1;

    var b = encoder._data.AsSpan();
    var b1 = decoder.K4os_Bench();
    var b2 = decoder.DR_Bench();
    var b3 = decoder.SimpleBase_Bench();
    var b4 = decoder.Convert_FromHexString();
    var b5 = decoder.HexMate_Bench();
    var b6 = decoder.IT_Bench();
    var b7 = decoder.Dodo_Bench();

    if (!b.SequenceEqual(b1)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b2)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b3)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b4)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b5)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b6)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b7)) throw new InvalidOperationException();

    var count = (l1.Length / 2) - 1;
    var sb = new StringBuilder(l1, l1.Length + count);

    for (int i = l1.Length - 2; i >= 2; i -= 2)
    {
        sb.Insert(i, ' ');
    }

    decoder._data = sb.ToString();

    b1 = decoder.IT_Bench();
    if (!b.SequenceEqual(b1)) throw new InvalidOperationException();
}

static void Base64Test()
{
    var encoder = new Base64_Encode_Benchmark();

    encoder.Length = 16;
    encoder.Setup();

    var u1 = encoder.ToBase64String();
    var u2 = encoder.K4os_Bench();
    var u3 = encoder.gfoidl_Bench();
    var u4 = encoder.IT_Utf8_Bench();
    var u5 = encoder.Multiformats_Bench();
    var u6 = encoder.Exyll_Bench();
    var u7 = encoder.IT_gfoidl_Bench();

    if (!u1.Equals(u2)) throw new InvalidOperationException();
    if (!u1.Equals(u3)) throw new InvalidOperationException();
    if (!u1.Equals(u4)) throw new InvalidOperationException();
    if (!u1.Equals(u5)) throw new InvalidOperationException();
    if (!u1.Equals(u6)) throw new InvalidOperationException();
    if (!u1.Equals(u7)) throw new InvalidOperationException();

    var decoder = new Base64_Decode_Benchmark();
    decoder._data = u1;

    var b = encoder._data.AsSpan();
    var b1 = decoder.FromBase64String();
    var b2 = decoder.K4os_Bench();
    var b3 = decoder.gfoidl_Bench();
    var b4 = decoder.Multiformats_Bench();
    var b5 = decoder.Exyll_Bench();
    var b6 = decoder.IT_gfoidl_Bench();
    //var b7 = decoder.IT_Utf8_Bench();

    if (!b.SequenceEqual(b1)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b2)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b3)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b4)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b5)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b6)) throw new InvalidOperationException();
    //if (!b.SequenceEqual(b7)) throw new InvalidOperationException();
}

static void Base85Test()
{
    var encoder = new Base85_Encode_Benchmark();

    encoder.Length = 16;
    encoder.Setup();

    var u1 = encoder.K4os_Bench();
    var u2 = encoder.SimpleBase_Bench();
    var u3 = encoder.Logos_Bench();
    var u4 = encoder.FsBinaryCodecs_Bench();
    //var u5 = encoder.ConfluxAddress_Bench();

    if (!u1.Equals(u2)) throw new InvalidOperationException();
    if (!u1.Equals(u3)) throw new InvalidOperationException();
    if (!u1.Equals(u4)) throw new InvalidOperationException();
    //if (!u1.Equals(u5)) throw new InvalidOperationException();
}