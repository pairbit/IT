using System;

namespace IT.Samples;

public class IndexRangeSample
{
    public static void Run()
    {
        Console.WriteLine("\nIndexRangeSample");

        var tes = "login(displayName)";

        var subs = tes[(tes.IndexOf("(") + 1)..tes.IndexOf(")")];

        //displayName
        Console.WriteLine(subs);
    }
}