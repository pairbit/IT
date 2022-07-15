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
        for (int i = 0; i < result.Length; i++)
        {
            string s = i.ToString(format);
            if (BitConverter.IsLittleEndian)
                result[i] = s[0] + ((uint)s[1] << 16);
            else
                result[i] = s[1] + ((uint)s[0] << 16);
        }
        return result;
    }

    public Int32 GetByteCount(ReadOnlySpan<Char> chars)
        => chars.Length / 2;

    public Int32 GetBytes(ReadOnlySpan<Char> chars, Span<Byte> bytes)
    {
        throw new NotImplementedException();
    }

    public Int32 GetCharCount(ReadOnlySpan<Byte> bytes)
        => bytes.Length * 2;

    public virtual Int32 GetChars(ReadOnlySpan<Byte> bytes, Span<Char> chars)
    {
        var len = bytes.Length * 2;
        if (chars.Length < len) throw new ArgumentException($"(chars.Length == {chars.Length}) < (bytes.Length == {len})", nameof(chars));

        unsafe
        {
            var lookupP = _lowerLookup32UnsafeP;
            fixed (byte* bytesP = bytes)
            fixed (char* resultP = chars)
            {
                uint* resultP2 = (uint*)resultP;
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultP2[i] = lookupP[bytesP[i]];
                }
            }
        }
        return len;
    }

    public virtual String GetString(ReadOnlySpan<Byte> bytes)
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