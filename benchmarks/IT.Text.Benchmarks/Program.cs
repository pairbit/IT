using IT.Text.Benchmarks;

var bench = new BaseCoderBenchmark();

var u1 = bench.Base16_Upper_Encode_IT();
var u2 = bench.Base16_Upper_Encode_SimpleBase();
var u3 = bench.Base16_Upper_Encode_K4os();

if (!u1.Equals(u2)) throw new InvalidOperationException();
if (!u1.Equals(u3)) throw new InvalidOperationException();

var l1 = bench.Base16_Lower_Encode_IT();
var l2 = bench.Base16_Lower_Encode_SimpleBase();
var l3 = bench.Base16_Lower_Encode_K4os();

if (!l1.Equals(l2)) throw new InvalidOperationException();
if (!l1.Equals(l3)) throw new InvalidOperationException();

BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(BaseCoderBenchmark));