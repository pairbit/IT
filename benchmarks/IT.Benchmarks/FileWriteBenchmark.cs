using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace IT.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class FileWriteBenchmark
{
    internal static String _rootPath = "/var/IT";

    internal byte[] _data;

    static FileWriteBenchmark()
    {
        var location = Path.GetDirectoryName(typeof(FileWriteBenchmark).Assembly.Location);

        var path = Path.GetFullPath(Path.Combine(location!, "IT.Benchmarks.config"));

        if (File.Exists(path))
        {
            var config = File.ReadAllText(path);

            if (!string.IsNullOrWhiteSpace(config)) _rootPath = config.Trim();

            Console.WriteLine($"Config found '{path}'");
        }
        else
        {
            Console.WriteLine($"Config not found '{path}'");
        }

        Console.WriteLine($"RootPath '{_rootPath}'");
    }

    //[Params(1, 2, 4, 8, 12, 15, 16, 31, 32, 49, 50, 63, 64, 128, 256, 512, 1024)]
    [Params(1024)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var data = new byte[Length * 1024];

        Random.Shared.NextBytes(data);

        _data = data;
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        Console.WriteLine($"Deleting '{_rootPath}' ...");

        Directory.Delete(_rootPath, true);

        Console.WriteLine("Deleted");
    }

    [Benchmark]
    public async Task FileWrite()
    {
        var id = Id.New();

        var path = Path.Combine(_rootPath, id.ToString(IdCoding.Path2));

        var pathDir = Path.GetDirectoryName(path);

        if (pathDir is null) throw new InvalidOperationException($"Path.GetDirectoryName({path}) is null");

        Directory.CreateDirectory(pathDir);

        await File.WriteAllBytesAsync(path, _data).ConfigureAwait(false);
    }
}