using System;
using System.Runtime.InteropServices;

namespace IT.Text.Encoders;

public class HexEncoder : IEncoder
{
    //https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
    private static readonly uint[] _lowerLookup32Unsafe = CreateLookup32Unsafe("x2");
    private static readonly uint[] _upperLookup32Unsafe = CreateLookup32Unsafe("X2");

    internal static readonly unsafe uint* _lowerLookup32UnsafeP = (uint*)GCHandle.Alloc(_lowerLookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();
    internal static readonly unsafe uint* _upperLookup32UnsafeP = (uint*)GCHandle.Alloc(_upperLookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();

    private readonly HexMate.HexFormattingOptions _options;

    public HexEncoder(Boolean isUpper = true)
    {
        _options = isUpper ? HexMate.HexFormattingOptions.None : HexMate.HexFormattingOptions.Lowercase;
    }

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

    public int GetMaxEncodedLength(ReadOnlySpan<byte> bytes)
        => bytes.Length * 2;

    public int Encode(ReadOnlySpan<byte> bytes, Span<char> chars)
    {
        var len = bytes.Length;

        var encoded = len * 2;

        if (chars.Length < encoded) throw new ArgumentException($"{chars.Length} < {encoded}", nameof(chars));

        if (len < 32)
        {
            unsafe
            {
                var lookupP = _options == HexMate.HexFormattingOptions.Lowercase ? _lowerLookup32UnsafeP : _upperLookup32UnsafeP;
                fixed (byte* bytesP = &MemoryMarshal.GetReference(bytes))
                fixed (char* resultP = &MemoryMarshal.GetReference(chars))
                {
                    uint* resultP2 = (uint*)resultP;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        resultP2[i] = lookupP[bytesP[i]];
                    }
                }
            }
            return encoded;
        }
        //https://ndportmann.com/breaking-records-with-core-3-0/
        else
        {
            return HexMate.Convert.TryToHexChars(bytes, chars, out var written, _options) ? written : 0;
        }
    }

    public string Encode(ReadOnlySpan<byte> bytes)
    {
        var len = bytes.Length;
        if (len == 0) return String.Empty;
        if (len < 32)
        {
            var result = new string((char)0, bytes.Length * 2);
            unsafe
            {
                var lookupP = _options == HexMate.HexFormattingOptions.Lowercase ? _lowerLookup32UnsafeP : _upperLookup32UnsafeP;
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
        //https://ndportmann.com/breaking-records-with-core-3-0/
        else
        {
            return HexMate.Convert.ToHexStringInternal(bytes, _options);
        }
    }

    public int GetMaxDecodedLength(ReadOnlySpan<char> chars)
    {
        var len = chars.Length;
        if ((len & 0x01) != 0) throw new ArgumentException("Even number of digits expected");
        return len / 2;
    }

    public int Decode(ReadOnlySpan<char> chars, Span<byte> bytes)
        => HexMate.Convert.TryFromHexChars(chars, bytes, out var written) ? written : 0;

    public byte[] Decode(ReadOnlySpan<char> chars) => HexMate.Convert.FromHexString(chars);
}