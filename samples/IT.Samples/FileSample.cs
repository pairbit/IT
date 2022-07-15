using System;
using System.IO;
using System.Threading.Tasks;

namespace IT.Samples;

public class FileSample
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\nFileSample");

        var content = $"content {Guid.NewGuid()}";
        var path = Path.Combine(Environment.CurrentDirectory, "Files", "file.txt");

        var dir = Path.GetDirectoryName(path);

        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        await WriteAllTextAsync(path, content).ConfigureAwait(false);

        var readed = await ReadAllTextAsync(path).ConfigureAwait(false);

        if (!readed.Equals(content)) throw new InvalidOperationException();

        Console.WriteLine(readed);
    }

    private static Task WriteAllTextAsync(String path, String contents)
    {
#if NETSTANDARD2_0
        Console.WriteLine("_File");
        return _File.WriteAllTextAsync(path, contents);
#else
        return File.WriteAllTextAsync(path, contents);
#endif
    }

    private static Task<String> ReadAllTextAsync(String path)
    {
#if NETSTANDARD2_0
        Console.WriteLine("_File");
        return _File.ReadAllTextAsync(path);
#else
        return File.ReadAllTextAsync(path);
#endif
    }
}
