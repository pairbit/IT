using System;

namespace IT.Security.Cryptography.Models;

public static class xSignFormat
{
    public static String GetCode(this SignFormat format) => format switch
    {
        SignFormat.CadesBES => "cades-bes",
        SignFormat.CadesC => "cades-c",
        SignFormat.XadesBES => "xades-bes",
        SignFormat.XadesC => "xades-c",
        _ => throw new NotImplementedException()
    };
}