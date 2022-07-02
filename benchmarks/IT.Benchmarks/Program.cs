using BenchmarkDotNet.Running;
using IT.Benchmarks;

//var b = new EncodingBenchmark();
//var base64_1 = b.ConvertToBase64String();
//var base64_2 = b.TryToBase64String();
//var base64_3 = b.EncodeToUtf8String();

//var base64_bytes = b.EncodeToUtf8();
//var base64_chars = b.EncodeToUtf8Chars();
//var base64_chars2 = b.EncodeToUtf8CharsWithGetCharCount();

//if (!base64_1.Equals(base64_2)) throw new InvalidOperationException();
//if (!base64_1.Equals(base64_3)) throw new InvalidOperationException();

//Console.WriteLine("Ok");


BenchmarkRunner.Run(typeof(EncodingBenchmark));