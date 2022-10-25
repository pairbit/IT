
var idb = new IT.Id.Benchmarks.IdBenchmark();

var id1 = idb.Id_Parse();
var id2 = idb.Id_Parse_Old();

if (!id1.Equals(id2))
    throw new InvalidOperationException();

Console.WriteLine("Ok");

//var id2 = id.ToString(Idf.Base64Url);
//var id3 = id.ToString("b64");
//var id4 = $"{id:b64}";

//if (id1.Equals(id2) && id1.Equals(id3) && id1.Equals(id4))
//{
//    Console.WriteLine("Ok");
//}

//var idparsed = Id.Parse(id3);

//if (idparsed.Equals(id))
//{
//    Console.WriteLine("Ok");
//}

BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(IT.Id.Benchmarks.IdBenchmark));