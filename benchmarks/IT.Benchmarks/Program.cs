using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using IT.Benchmarks;
using Perfolizer.Horology;

var config = DefaultConfig.Instance.AddJob(
                Job.Default
                    .WithIterationTime(TimeInterval.FromMilliseconds(250)) // each iteration should last no longer than 250ms
                    .WithWarmupCount(1) // one warmup should be enough
                    .WithMaxIterationCount(20) // we don't need more than 20 iterations
                    .AsDefault());

BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(FileWriteBenchmark), config, args);

//var b = new EncodingBenchmark();
//var base64_1 = b.ConvertToBase64String();
//var base64_2 = new string(b.TryToBase64Chars());
//var base64_3 = b.EncodeToUtf8String();
//var base64_4 = b.EncodeToUtf8InPlaceString();

//var base64_bytes = b.EncodeToUtf8();
//var base64_chars = b.EncodeToUtf8Chars();

//var str = new string(base64_chars);

//if (!base64_1.Equals(str)) throw new InvalidOperationException();

//Console.WriteLine("Ok");

//if (!base64_1.Equals(base64_2)) throw new InvalidOperationException();
//if (!base64_1.Equals(base64_3)) throw new InvalidOperationException();
//if (!base64_1.Equals(base64_4)) throw new InvalidOperationException();

//Console.WriteLine("Ok");


//var finder = new TagFinderBenchmark();

//var obj = finder.ParseEnvelope();

//var obj1 = finder.NewEtalon();

//var test = new

//BenchmarkRunner.Run(typeof(TagFinderBenchmark));
