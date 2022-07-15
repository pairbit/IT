using System;
using System.Collections.Generic;

namespace IT.Samples;

record MyRecord(String FirstName, Int32 Age);

class MyClass
{
    public String Field { get; init; }
}

public class RecordSample
{
    public static void Run()
    {
        Console.WriteLine("\nRecordSample");

        List<MyClass> list = new()
        {
            new() { Field = "a1" },
            new() { Field = "a2" }
        };

        //Error
        //list[0].Field = "b1";

        var r1 = new MyRecord("Ivan", 30);

        //Error
        //r1.FirstName = "John";

        //MyRecord { FirstName = Ivan, Age = 30 }
        Console.WriteLine(r1);
    }
}