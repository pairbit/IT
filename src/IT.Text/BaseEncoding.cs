using System;
using System.Text;

namespace IT.Text;

public class BaseEncoding : IEncoding
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
        //Encoding.Default.GetChars()
        throw new NotImplementedException();
    }
}