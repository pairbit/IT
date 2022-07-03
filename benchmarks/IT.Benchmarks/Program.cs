using BenchmarkDotNet.Running;
using IT.Benchmarks;

//var b = new EncodingBenchmark();
//var base64_1 = b.ConvertToBase64String();
//var base64_2 = new string(b.TryToBase64Chars());
//var base64_3 = b.EncodeToUtf8String();
//var base64_4 = b.EncodeToUtf8InPlaceString();

//var base64_bytes = b.EncodeToUtf8();
//var base64_chars = b.EncodeToUtf8Chars();

//if (!base64_1.Equals(base64_2)) throw new InvalidOperationException();
//if (!base64_1.Equals(base64_3)) throw new InvalidOperationException();
//if (!base64_1.Equals(base64_4)) throw new InvalidOperationException();

//Console.WriteLine("Ok");


//var finder = new TagFinderBenchmark();

//var obj = finder.ParseEnvelope();

//var obj1 = finder.NewEtalon();

BenchmarkRunner.Run(typeof(TagFinderBenchmark));