
var random = new Random(123);
var high = random.Next();
var low = random.Next();

Console.WriteLine($"{high} - {low}");

var idb = new IT.Id.Benchmarks.IdBenchmark();

var id1 = idb.Id_Parse_HexLower();
var id2 = idb.Id_Parse_HexUpper();
var id3 = idb.Id_Parse_HexLower_OLD();
var id4 = idb.Id_Parse_HexUpper_OLD();

if (!id1.Equals(id2) || !id1.Equals(id3) || !id1.Equals(id4)) throw new InvalidOperationException();

Console.WriteLine("Ok");

var id = Id.New();

var f1 = id.ToString(Idf.Base64Url);
var f2 = id.ToString("b64");
var f3 = $"{id:b64}";
var f4 = id.ToString();

if (!f1.Equals(f2) || !f1.Equals(f3) || !f1.Equals(f4) || !id.Equals(Id.Parse(f4)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.Base64);
f2 = id.ToString("B64");
f3 = $"{id:B64}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.Path2);
f2 = id.ToString("p2");
f3 = $"{id:p2}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.Path3);
f2 = id.ToString("p3");
f3 = $"{id:p3}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.HexLower);
f2 = id.ToString("h");
f3 = $"{id:h}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)) || !id.Equals(Id.Parse(f3, Idf.HexUpper)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.HexUpper);
f2 = id.ToString("H");
f3 = $"{id:H}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)) || !id.Equals(Id.Parse(f3, Idf.HexUpper)))
    throw new InvalidOperationException();

Console.WriteLine("Ok");

BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(IT.Id.Benchmarks.IdBenchmark));