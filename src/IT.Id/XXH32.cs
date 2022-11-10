using System.Runtime.CompilerServices;

namespace System;

internal static unsafe class XXH32
{
    private const uint PRIME32_1 = 2654435761u;
    private const uint PRIME32_2 = 2246822519u;
    private const uint PRIME32_3 = 3266489917u;
    private const uint PRIME32_4 = 668265263u;
    private const uint PRIME32_5 = 374761393u;

    #region Public

    public static unsafe uint DigestOf(void* bytes, int length) => XXH32_hash(bytes, length, 0);

    public static unsafe uint DigestOf(byte[] bytes)
    {
        if (bytes == null)
            throw new ArgumentException("Invalid buffer boundaries");

        fixed (byte* bytes0 = bytes)
            return DigestOf(bytes0, bytes.Length);
    }

    #endregion Public

    #region Private

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint XXH_read32(void* p) => *(uint*)p;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint XXH32_rotl(uint x, int r) => x << r | x >> 32 - r;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint XXH32_round(uint seed, uint input) =>
        XXH32_rotl(seed + input * PRIME32_2, 13) * PRIME32_1;

#if NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    private static uint XXH32_hash(void* input, int len, uint seed)
    {
        var p = (byte*)input;
        var bEnd = p + len;
        uint h32;

        if (len >= 16)
        {
            var limit = bEnd - 16;
            var v1 = seed + PRIME32_1 + PRIME32_2;
            var v2 = seed + PRIME32_2;
            var v3 = seed + 0;
            var v4 = seed - PRIME32_1;

            do
            {
                v1 = XXH32_round(v1, XXH_read32(p + 0));
                v2 = XXH32_round(v2, XXH_read32(p + 4));
                v3 = XXH32_round(v3, XXH_read32(p + 8));
                v4 = XXH32_round(v4, XXH_read32(p + 12));
                p += 16;
            }
            while (p <= limit);

            h32 = XXH32_rotl(v1, 1)
                + XXH32_rotl(v2, 7)
                + XXH32_rotl(v3, 12)
                + XXH32_rotl(v4, 18);
        }
        else
        {
            h32 = seed + PRIME32_5;
        }

        h32 += (uint)len;

        while (p + 4 <= bEnd)
        {
            h32 = XXH32_rotl(h32 + XXH_read32(p) * PRIME32_3, 17) * PRIME32_4;
            p += 4;
        }

        while (p < bEnd)
        {
            h32 = XXH32_rotl(h32 + *p * PRIME32_5, 11) * PRIME32_1;
            p++;
        }

        h32 ^= h32 >> 15;
        h32 *= PRIME32_2;
        h32 ^= h32 >> 13;
        h32 *= PRIME32_3;
        h32 ^= h32 >> 16;

        return h32;
    }

    #endregion Private
}