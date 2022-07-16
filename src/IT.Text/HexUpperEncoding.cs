using System;

namespace IT.Text;

public class HexUpperEncoding : HexEncoding
{
    public override Int32 GetChars(ReadOnlySpan<Byte> bytes, Span<Char> chars)
    {
        var len = bytes.Length * 2;
        if (chars.Length < len) throw new ArgumentException($"(chars.Length == {chars.Length}) < (bytes.Length == {len})", nameof(chars));

        unsafe
        {
            var lookupP = _upperLookup32UnsafeP;
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

    public override String GetString(ReadOnlySpan<Byte> bytes)
    {
        var result = new string((char)0, bytes.Length * 2);
        unsafe
        {
            var lookupP = _upperLookup32UnsafeP;
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
