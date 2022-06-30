using System;

namespace IT.Text;

public class Base64Encoding : IEncoding
{
    public int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
    {
        throw new NotImplementedException();
    }

    public byte[] GetBytes(ReadOnlySpan<char> chars)
    {
        throw new NotImplementedException();
    }

    public int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
    {
        throw new NotImplementedException();
    }

    public string GetString(ReadOnlySpan<byte> bytes)
    {
        //Convert.TryFromBase64String
        throw new NotImplementedException();
    }
}
