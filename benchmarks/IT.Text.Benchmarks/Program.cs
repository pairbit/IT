using IT.Text.Benchmarks;

Base16Test();

BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Base16_Encode_Benchmark));
//BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Base16_Decode_Benchmark));

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
    var b1 = decoder.Lower_K4os();
    var b2 = decoder.Lower_DR();
    var b3 = decoder.Lower_SimpleBase();
    var b4 = decoder.Upper_K4os();
    var b5 = decoder.Upper_SimpleBase();
    var b6 = decoder.Upper_Convert();
    var b7 = decoder.HexMate_Convert();
    var b8 = decoder.IT_HexMate();

    if (!b.SequenceEqual(b1)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b2)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b3)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b4)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b5)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b6)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b7)) throw new InvalidOperationException();
    if (!b.SequenceEqual(b8)) throw new InvalidOperationException();
}