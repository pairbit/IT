﻿
var id = Id.Parse("Y1fvxj8PlIp6nO4h");

var id1 = id.ToString();
var id2 = id.ToString(Idf.Base64Url);
var id3 = id.ToString("b64");
var id4 = $"{id:b64}";

if (id1.Equals(id2) && id1.Equals(id3) && id1.Equals(id4))
{
    Console.WriteLine("Ok");
}

var idparsed = Id.Parse("Y1fvxj8PlIp6nO4h");

if (idparsed.Equals(id))
{
    Console.WriteLine("Ok");
}

//BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(IT.Id.Benchmarks.IdBenchmark));