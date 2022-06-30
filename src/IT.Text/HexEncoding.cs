using System;
using System.Runtime.InteropServices;

namespace IT.Text;

public class HexEncoding : IEncoding
{
    //https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
    private static readonly uint[] _lowerLookup32Unsafe = CreateLookup32Unsafe("x2");
    private static readonly uint[] _upperLookup32Unsafe = CreateLookup32Unsafe("X2");

    internal static readonly unsafe uint* _lowerLookup32UnsafeP = (uint*)GCHandle.Alloc(_lowerLookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();
    internal static readonly unsafe uint* _upperLookup32UnsafeP = (uint*)GCHandle.Alloc(_upperLookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();

    private static uint[] CreateLookup32Unsafe(string format)
    {
        var result = new uint[256];
        for (int i = 0; i < 256; i++)
        {
            string s = i.ToString(format);
            if (BitConverter.IsLittleEndian)
                result[i] = s[0] + ((uint)s[1] << 16);
            else
                result[i] = s[1] + ((uint)s[0] << 16);
        }
        return result;
    }

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
        var result = new string((char)0, bytes.Length * 2);
        unsafe
        {
            var lookupP = _lowerLookup32UnsafeP;
            fixed (byte* bytesP = bytes)
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultP2[i] = lookupP[bytesP[i]];
                }
            }
        }
        return result;
    }
}
