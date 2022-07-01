using System;

namespace IT.Security.Cryptography.JinnServer;

public record SigningOptions
{
    /// <example>http://195.230.101.103:8080/tccs/SigningService</example>
    public String SigningUrl { get; set; }

    /// <example>50MB</example>
    public String PartInBytes { get; set; }

    public Int64 PartInBytesValue => PartInBytes != null ? ParseBytes(PartInBytes) : 50 * 1024 * 1024;//50MB

    private static Int64 ParseBytes(String value)
    {
        if (value.EndsWith("KB")) return Int32.Parse(value.Substring(0, value.Length - 2)) * 1024;
        
        if (value.EndsWith("MB")) return Int32.Parse(value.Substring(0, value.Length - 2)) * 1024 * 1024;
        
        if (value.EndsWith("GB")) return Int32.Parse(value.Substring(0, value.Length - 2)) * 1024 * 1024 * 1024;

        if (value.EndsWith("B")) return Int32.Parse(value.Substring(0, value.Length - 1));

        if (Int32.TryParse(value, out int bytes)) return bytes;

        throw new FormatException($"Format 'PartInBytes' file size '{value}' not correct");
    }
}