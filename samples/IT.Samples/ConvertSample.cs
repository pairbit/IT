using System;
using System.Linq;
using System.Text;

namespace IT.Samples;

public class ConvertSample
{
    public static void Run()
    {
        Console.WriteLine("\nConvertSample");

        var oBytes = Encoding.UTF8.GetBytes("Hello World");

        var hexString = ToHexString(oBytes);

        //48656C6C6F20576F726C64
        Console.WriteLine(hexString);

        var bytes = FromHexString(hexString);

        if (!oBytes.SequenceEqual(bytes)) throw new InvalidOperationException();
    }

    private static String ToHexString(Byte[] bytes)
    {
#if NET6_0
        return Convert.ToHexString(bytes);
#else
        Console.WriteLine("_Convert");
        return _Convert.ToHexString(bytes);
#endif
    }

    private static Byte[] FromHexString(String hex)
    {
#if NET6_0
        return Convert.FromHexString(hex);
#else
        Console.WriteLine("_Convert");
        return _Convert.FromHexString(hex);
#endif
    }
}