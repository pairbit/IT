﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Encoding.Base;
using K4os.Text.BaseX;

namespace IT.Encoding.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Base16_Encode_Benchmark
{
    private ITextEncoder _base16lower;
    private ITextEncoder _base16upper;
    private ITextEncoder _base16lowerOld;
    private ITextEncoder _base16upperOld;
    private ITextEncoder _base16lowerMate;
    private ITextEncoder _base16upperMate;
    internal byte[] _data;

    [Params(14, 100, 510, 1024, 510 * 1024, 2 * 1024 * 1024, 510 * 1024 * 1024)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _base16lower = new HexEncoder_HexMate_CodesInChaos(true);
        _base16upper = new HexEncoder_HexMate_CodesInChaos();
        _base16lowerOld = new HexEncoder_CodesInChaos(true);
        _base16upperOld = new HexEncoder_CodesInChaos();
        _base16lowerMate = new HexEncoder_HexMate(true);
        _base16upperMate = new HexEncoder_HexMate();
        var data = new byte[Length];
        Random.Shared.NextBytes(data);
        _data = data;
    }

    [Benchmark(Description = "IT")]
    public String Upper_IT_new() => _base16upper.Encode(_data);

    //[Benchmark]
    public String Lower_IT_new() => _base16lower.Encode(_data);

    [Benchmark(Description = "IT_HexMate")]
    public String Upper_IT_HexMate() => _base16upperMate.Encode(_data);

    //[Benchmark]
    public String Lower_IT_HexMate() => _base16lowerMate.Encode(_data);

    [Benchmark(Description = "CodesInChaos")]
    public String Upper_CodesInChaos() => _base16upperOld.Encode(_data);

    //[Benchmark]
    public String Lower_CodesInChaos() => _base16lowerOld.Encode(_data);

    [Benchmark(Description = "HexMate")]
    public String Upper_HexMate() => HexMate.Convert.ToHexString(_data);

    //[Benchmark]
    public String Lower_HexMate() => HexMate.Convert.ToHexString(_data, HexMate.HexFormattingOptions.Lowercase);

    [Benchmark(Description = "K4os")]
    public String Upper_K4os() => Base16.Upper.Encode(_data);

    //[Benchmark]
    public String Lower_K4os() => Base16.Lower.Encode(_data);

    [Benchmark(Description = "DR")]
    public String Upper_DR() => DRDigit.Fast.ToHexString(_data);

    [Benchmark(Description = "Convert.ToHexString")]
    public String Upper_Convert() => Convert.ToHexString(_data);

    [Benchmark(Description = "SimpleBase")]
    public String Upper_SimpleBase() => SimpleBase.Base16.UpperCase.Encode(_data);

    //[Benchmark]
    public String Lower_SimpleBase() => SimpleBase.Base16.LowerCase.Encode(_data);

    [Benchmark(Description = "Dodo")]
    public String Lower_Dodo() => Dodo.Primitives.Hex.GetString(_data)!;
}