
//var idb = new IT.Id.Benchmarks.IdBenchmark();

//var id1 = idb.Id_Parse();
//var id2 = idb.Id_Parse_Old();

//if (!id1.Equals(id2))
//    throw new InvalidOperationException();

//Console.WriteLine("Ok");

var id = Id.New();

var id1 = id.ToString();
var id2 = id.ToString(Idf.Base64Url);
var id3 = id.ToString("b64");
var id4 = $"{id:b64}";

if (!id1.Equals(id2) || !id1.Equals(id3) || !id1.Equals(id4) || !id.Equals(Id.Parse(id4)))
    throw new InvalidOperationException();

id1 = id.ToString(Idf.Path2);
id2 = id.ToString("p2");
id3 = $"{id:p2}";

if (!id1.Equals(id2) || !id1.Equals(id3) || !id.Equals(Id.Parse(id3)))
    throw new InvalidOperationException();

Console.WriteLine("Ok");

//BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(IT.Id.Benchmarks.IdBenchmark));