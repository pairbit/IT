namespace System;

public readonly partial struct Id
{
    public UInt32 Hash32() => XXH32.DigestOf(ToByteArray());

    public UInt64 Hash64() => XXH64.DigestOf(ToByteArray());

    //public unsafe UInt64 Hash64_Fast()
    //{
    //    ulong h64 = XXH64.PRIME64_5 + 12;

    //    fixed (byte* p64 = new[] {
    //        (byte)(_timestamp >> 24),
    //        (byte)(_timestamp >> 16),
    //        (byte)(_timestamp >> 8),
    //        (byte)(_timestamp),
    //        (byte)(_b >> 24),
    //        (byte)(_b >> 16),
    //        (byte)(_b >> 8),
    //        (byte)(_b)
    //    })
    //    {
    //        var uint64 = XXH.XXH_read64(p64);//7773248848437546850
    //        h64 ^= XXH64.XXH64_round(0, uint64);
    //    }

    //    h64 = XXH64.XXH_rotl64(h64, 27) * XXH64.PRIME64_1 + XXH64.PRIME64_4;

    //    fixed (byte* p32 = new[] {
    //        (byte)(_c >> 24),
    //        (byte)(_c >> 16),
    //        (byte)(_c >> 8),
    //        (byte)(_c)
    //    })
    //    {
    //        var uint32 = XXH.XXH_read32(p32);//2928914477
    //        h64 ^= uint32 * XXH64.PRIME64_1;
    //    }

    //    h64 = XXH64.XXH_rotl64(h64, 23) * XXH64.PRIME64_2 + XXH64.PRIME64_3;

    //    h64 ^= h64 >> 33;
    //    h64 *= XXH64.PRIME64_2;
    //    h64 ^= h64 >> 29;
    //    h64 *= XXH64.PRIME64_3;
    //    h64 ^= h64 >> 32;

    //    return h64;
    //}
}