using System.Runtime.CompilerServices;

namespace System;

internal static unsafe class XXH64
{
    private const ulong PRIME64_1 = 11400714785074694791ul;
    private const ulong PRIME64_2 = 14029467366897019727ul;
    private const ulong PRIME64_3 = 1609587929392839161ul;
    private const ulong PRIME64_4 = 9650029242287828579ul;
    private const ulong PRIME64_5 = 2870177450012600261ul;

    #region Public

    public static unsafe ulong DigestOf(void* bytes, int length) => XXH64_hash(bytes, length, 0);

    public static unsafe ulong DigestOf(byte[] bytes, int offset, int length)
    {
        if (bytes == null || offset < 0 || length < 0 || offset + length > bytes.Length)
            throw new ArgumentException("Invalid buffer boundaries");

        fixed (byte* bytes0 = bytes)
            return DigestOf(bytes0 + offset, length);
    }

    #endregion

    #region Private

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint XXH_read32(void* p) => *(uint*)p;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH_read64(void* p) => *(ulong*)p;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH_rotl64(ulong x, int r) => x << r | x >> 64 - r;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH64_round(ulong acc, ulong input) => XXH_rotl64(acc + input * PRIME64_2, 31) * PRIME64_1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH64_mergeRound(ulong acc, ulong val) => (acc ^ XXH64_round(0, val)) * PRIME64_1 + PRIME64_4;

#if NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    private static ulong XXH64_hash(void* input, int len, ulong seed)
    {
        var p = (byte*)input;
        var bEnd = p + len;
        ulong h64;

        if (len >= 32)
        {
            var limit = bEnd - 32;
            var v1 = seed + PRIME64_1 + PRIME64_2;
            var v2 = seed + PRIME64_2;
            var v3 = seed + 0;
            var v4 = seed - PRIME64_1;

            do
            {
                v1 = XXH64_round(v1, XXH_read64(p + 0));
                v2 = XXH64_round(v2, XXH_read64(p + 8));
                v3 = XXH64_round(v3, XXH_read64(p + 16));
                v4 = XXH64_round(v4, XXH_read64(p + 24));
                p += 32;
            }
            while (p <= limit);

            h64 = XXH_rotl64(v1, 1) + XXH_rotl64(v2, 7) + XXH_rotl64(v3, 12) + XXH_rotl64(v4, 18);
            h64 = XXH64_mergeRound(h64, v1);
            h64 = XXH64_mergeRound(h64, v2);
            h64 = XXH64_mergeRound(h64, v3);
            h64 = XXH64_mergeRound(h64, v4);
        }
        else
        {
            h64 = seed + PRIME64_5;
        }

        h64 += (ulong)len;

        while (p + 8 <= bEnd)
        {
            h64 ^= XXH64_round(0, XXH_read64(p));
            h64 = XXH_rotl64(h64, 27) * PRIME64_1 + PRIME64_4;
            p += 8;
        }

        if (p + 4 <= bEnd)
        {
            h64 ^= XXH_read32(p) * PRIME64_1;
            h64 = XXH_rotl64(h64, 23) * PRIME64_2 + PRIME64_3;
            p += 4;
        }

        while (p < bEnd)
        {
            h64 ^= *p * PRIME64_5;
            h64 = XXH_rotl64(h64, 11) * PRIME64_1;
            p++;
        }

        h64 ^= h64 >> 33;
        h64 *= PRIME64_2;
        h64 ^= h64 >> 29;
        h64 *= PRIME64_3;
        h64 ^= h64 >> 32;

        return h64;
    }

    #endregion Private
}