using System;

namespace IT.Samples;

public class CallerSample
{
    public static void Run()
    {
        Console.WriteLine("\nCallerSample");

        var local = "myName";
        ArgumentExpression(local);
    }

    static void ArgumentExpression(String name, [System.Runtime.CompilerServices.CallerArgumentExpression("name")] String? nameExp = null)
    {
        //local = "myName"
        Console.WriteLine($"{nameExp} = \"{name}\"");
    }
}