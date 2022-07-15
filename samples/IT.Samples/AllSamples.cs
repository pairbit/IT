using System.Threading.Tasks;

namespace IT.Samples;

public class AllSamples
{
    public static async Task RunAsync()
    {
        CallerSample.Run();

        ConvertSample.Run();

        IndexRangeSample.Run();

        RecordSample.Run();

        await FileSample.RunAsync();
    }
}