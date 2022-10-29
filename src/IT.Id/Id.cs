using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;

namespace System;

[Serializable]
[DebuggerDisplay("{ToString(),nq}")]
public readonly struct Id : IComparable<Id>, IEquatable<Id>, IFormattable
#if NET6_0
, ISpanFormattable
#endif
{
    private static readonly DateTime _unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly Int64 _unixEpochTicks = _unixEpoch.Ticks;

    private static readonly Int64 _random = CalculateRandomValue();
    private static Int32 _staticIncrement = new Random().Next();
    public static readonly Id Empty = default;
    public static readonly Id MinValue = new(0, 0, 0);
    public static readonly Id MaxValue = new(-1, -1, -1);

    internal readonly Int32 _timestamp;
    internal readonly Int32 _b;
    internal readonly Int32 _c;

    #region Ctors

    public Id(ReadOnlySpan<Byte> bytes)
    {
        if (bytes == null) throw new ArgumentNullException(nameof(bytes));

        if (bytes.Length != 12) throw new ArgumentException("Byte array must be 12 bytes long", nameof(bytes));

        FromByteArray(bytes, 0, out _timestamp, out _b, out _c);
    }

    //public Id(Byte[] bytes, Int32 index)
    //{
    //    FromByteArray(bytes, index, out _timestamp, out _b, out _c);
    //}

    //public Id(DateTime timestamp, Int32 machine, Int16 pid, Int32 increment)
    //    : this(GetTimestampFromDateTime(timestamp), machine, pid, increment)
    //{
    //}

    //public Id(Int32 timestamp, Int32 machine, Int16 pid, Int32 increment)
    //{
    //    if ((machine & 0xff000000) != 0) throw new ArgumentOutOfRangeException(nameof(machine), "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");

    //    if ((increment & 0xff000000) != 0) throw new ArgumentOutOfRangeException(nameof(increment), "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");

    //    _timestamp = timestamp;
    //    _b = (machine << 8) | (((int)pid >> 8) & 0xff);
    //    _c = ((int)pid << 24) | increment;
    //}

    public Id(Int32 timestamp, Int32 b, Int32 c)
    {
        _timestamp = timestamp;
        _b = b;
        _c = c;
    }

    #endregion Ctors

    #region Props

    public Int32 Timestamp => _timestamp;

    public Int32 B => _b;

    public Int32 C => _c;

    //public Int32 Machine => (_b >> 8) & 0xffffff;

    //public Int16 Pid => (short)(((_b << 8) & 0xff00) | ((_c >> 24) & 0x00ff));

    //public Int32 Increment => _c & 0xffffff;

    public DateTime Created => _unixEpoch.AddSeconds((uint)_timestamp);

    #endregion Props

    #region Operators

    public static Boolean operator <(Id left, Id right) => left.CompareTo(right) < 0;

    public static Boolean operator <=(Id left, Id right) => left.CompareTo(right) <= 0;

    public static Boolean operator ==(Id left, Id right) => left.Equals(right);

    public static Boolean operator !=(Id left, Id right) => !(left == right);

    public static Boolean operator >=(Id left, Id right) => left.CompareTo(right) >= 0;

    public static Boolean operator >(Id left, Id right) => left.CompareTo(right) > 0;

    #endregion Operators

    #region Public Methods

    //[DllImport(Interop.Libraries.SystemNative, EntryPoint = "SystemNative_GetSystemTimeAsTicks")]
    //internal static extern long GetSystemTimeAsTicks();
    //https://github.com/dotnet/runtime/blob/4aeec6397c7dd6198129219ed806f93d6ed675c0/src/libraries/Common/src/Interop/Unix/System.Native/Interop.GetSystemTimeAsTicks.cs

    #region New

    public static Id New()
    {
        //var ticks = ((ulong)(Interop.Sys.GetSystemTimeAsTicks() + UnixEpochTicks)) | KindUtc;

        var totalSeconds = (double)(DateTime.UtcNow.Ticks - _unixEpochTicks) / TimeSpan.TicksPerSecond;

        var timestamp = (int)(uint)(long)Math.Floor(totalSeconds);

        // only use low order 3 bytes
        int increment = Interlocked.Increment(ref _staticIncrement) & 0x00ffffff;

        var random = _random;

        var b = (int)(random >> 8); // first 4 bytes of random
        var c = (int)(random << 24) | increment; // 5th byte of random and 3 byte increment

        return new Id(timestamp, b, c);
    }

    public static Id New(DateTime timestamp) => New(GetTimestampFromDateTime(timestamp));

    public static Id New(Int32 timestamp)
    {
        // only use low order 3 bytes
        int increment = Interlocked.Increment(ref _staticIncrement) & 0x00ffffff;
        return Create(timestamp, _random, increment);
    }

    #endregion New

    //public static Byte[] Pack(Int32 timestamp, Int32 machine, Int16 pid, Int32 increment)
    //{
    //    if ((machine & 0xff000000) != 0) throw new ArgumentOutOfRangeException(nameof(machine), "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");

    //    if ((increment & 0xff000000) != 0) throw new ArgumentOutOfRangeException(nameof(increment), "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");

    //    byte[] bytes = new byte[12];
    //    bytes[0] = (byte)(timestamp >> 24);
    //    bytes[1] = (byte)(timestamp >> 16);
    //    bytes[2] = (byte)(timestamp >> 8);
    //    bytes[3] = (byte)(timestamp);
    //    bytes[4] = (byte)(machine >> 16);
    //    bytes[5] = (byte)(machine >> 8);
    //    bytes[6] = (byte)(machine);
    //    bytes[7] = (byte)(pid >> 8);
    //    bytes[8] = (byte)(pid);
    //    bytes[9] = (byte)(increment >> 16);
    //    bytes[10] = (byte)(increment >> 8);
    //    bytes[11] = (byte)(increment);
    //    return bytes;
    //}

    public static Id Parse(ReadOnlySpan<Char> value) => value.Length switch
    {
        15 => ParseBase85(value),
        16 => ParseBase64(value),
        18 => ParsePath2(value),
        19 => ParsePath3(value),
        24 => ParseHex(value),
        _ => throw new FormatException()
    };

    public static Id Parse(ReadOnlySpan<Char> value, Idf format) => format switch
    {
        Idf.Hex or Idf.HexUpper => ParseHex(value),
        Idf.Base64 or Idf.Base64Url => ParseBase64(value),
        Idf.Base85 => ParseBase85(value),
        Idf.Path2 => ParsePath2(value),
        Idf.Path3 => ParsePath3(value),
        _ => throw new FormatException()
    };

    //public static void Unpack(Byte[] bytes, out Int32 timestamp, out Int32 machine, out Int16 pid, out Int32 increment)
    //{
    //    if (bytes == null) throw new ArgumentNullException(nameof(bytes));
    //    if (bytes.Length != 12) throw new ArgumentOutOfRangeException(nameof(bytes), "Byte array must be 12 bytes long.");

    //    timestamp = (bytes[0] << 24) + (bytes[1] << 16) + (bytes[2] << 8) + bytes[3];
    //    machine = (bytes[4] << 16) + (bytes[5] << 8) + bytes[6];
    //    pid = (short)((bytes[7] << 8) + bytes[8]);
    //    increment = (bytes[9] << 16) + (bytes[10] << 8) + bytes[11];
    //}

    public Byte[] ToByteArray() => new[] {
        (byte)(_timestamp >> 24),
        (byte)(_timestamp >> 16),
        (byte)(_timestamp >> 8),
        (byte)(_timestamp),
        (byte)(_b >> 24),
        (byte)(_b >> 16),
        (byte)(_b >> 8),
        (byte)(_b),
        (byte)(_c >> 24),
        (byte)(_c >> 16),
        (byte)(_c >> 8),
        (byte)(_c)
    };

    public void ToByteArray(Span<Byte> destination, Int32 offset)
    {
        if (offset + 12 > destination.Length) throw new ArgumentException("Not enough room in destination buffer.", nameof(offset));

        destination[offset + 0] = (byte)(_timestamp >> 24);
        destination[offset + 1] = (byte)(_timestamp >> 16);
        destination[offset + 2] = (byte)(_timestamp >> 8);
        destination[offset + 3] = (byte)(_timestamp);
        destination[offset + 4] = (byte)(_b >> 24);
        destination[offset + 5] = (byte)(_b >> 16);
        destination[offset + 6] = (byte)(_b >> 8);
        destination[offset + 7] = (byte)(_b);
        destination[offset + 8] = (byte)(_c >> 24);
        destination[offset + 9] = (byte)(_c >> 16);
        destination[offset + 10] = (byte)(_c >> 8);
        destination[offset + 11] = (byte)(_c);
    }

    public Int32 CompareTo(Id other)
    {
        int result = ((uint)_timestamp).CompareTo((uint)other._timestamp);

        if (result != 0) return result;

        result = ((uint)_b).CompareTo((uint)other._b);
        if (result != 0) return result;

        return ((uint)_c).CompareTo((uint)other._c);
    }

    public Boolean Equals(Id id) => _timestamp == id._timestamp && _b == id._b && _c == id._c;

    public override String ToString() => ToBase64Url();

    public String ToString(String? format, IFormatProvider? formatProvider = null) => format switch
    {
        "h" or "b16" or "16" => ToHexLower(),
        "H" or "B16" => ToHexUpper(),
        "b64" or "64" => ToBase64(),
        "u64" or null => ToBase64Url(),
        "85" => ToBase85(),
        "p2" => ToPath2(),
        "p3" => ToPath3(),
        _ => throw new FormatException($"The '{format}' format string is not supported."),
    };

    public String ToString(Idf format) => format switch
    {
        Idf.Hex => ToHexLower(),
        Idf.HexUpper => ToHexUpper(),
        Idf.Base64 => ToBase64(),
        Idf.Base64Url => ToBase64Url(),
        Idf.Base85 => ToBase85(),
        Idf.Path2 => ToPath2(),
        Idf.Path3 => ToPath3(),
        _ => throw new FormatException($"The '{format}' format string is not supported."),
    };

#if NET6_0

    public Boolean TryFormat(Span<Char> destination, out Int32 charsWritten, ReadOnlySpan<Char> format, IFormatProvider? provider)
    {
        if (format.IsEmpty || format.SequenceEqual("u64"))
        {
            charsWritten = 16;
            ToBase64(destination, Base64.tableUrl);
            return true;
        }

        if (format.SequenceEqual("b64") || format.SequenceEqual("64"))
        {
            charsWritten = 16;
            ToBase64(destination, Base64.table);
            return true;
        }

        if (format[0] == 'h' || format.SequenceEqual("b16") || format.SequenceEqual("16"))
        {
            charsWritten = 24;
            unsafe
            {
                ToHex(destination, Hex._lowerLookup32UnsafeP);
            }

            return true;
        }

        if (format[0] == 'H' || format.SequenceEqual("B16"))
        {
            charsWritten = 24;
            unsafe
            {
                ToHex(destination, Hex._upperLookup32UnsafeP);
            }

            return true;
        }

        if (format.SequenceEqual("85"))
        {
            charsWritten = 15;
            ToBase85(destination);

            return true;
        }

        if (format.SequenceEqual("p2"))
        {
            charsWritten = 18;
            ToPath2(destination);

            return true;
        }

        if (format.SequenceEqual("p3"))
        {
            charsWritten = 19;
            ToPath3(destination);

            return true;
        }

        charsWritten = 0;
        return false;
    }

#endif

    public UInt32 Hash32()
    {
        var bytes = ToByteArray();
        return XXH32.DigestOf(bytes, 0, bytes.Length);
    }

    public UInt64 Hash64()
    {
        var bytes = ToByteArray();
        return XXH64.DigestOf(bytes, 0, bytes.Length);
    }

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

    public override Boolean Equals(Object? obj) => obj is Id id && Equals(id);

    public override Int32 GetHashCode()
    {
        int hash = 17;
        hash = 37 * hash + _timestamp.GetHashCode();
        hash = 37 * hash + _b.GetHashCode();
        hash = 37 * hash + _c.GetHashCode();
        return hash;
    }

    #endregion Public Methods

    #region Private Methods

    private String ToBase64Url()
    {
        var result = new string((char)0, 16);
        var table = Base64.tableUrl;

        unsafe
        {
            fixed (char* resultP = result)
            fixed (char* base64 = &table[0])
            {
                var byte0 = (byte)(_timestamp >> 24);
                var byte1 = (byte)(_timestamp >> 16);
                var byte2 = (byte)(_timestamp >> 8);

                resultP[0] = base64[(byte0 & 0xfc) >> 2];
                resultP[1] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
                resultP[2] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
                resultP[3] = base64[byte2 & 0x3f];

                var byte3 = (byte)(_timestamp);
                var byte4 = (byte)(_b >> 24);
                var byte5 = (byte)(_b >> 16);

                resultP[4] = base64[(byte3 & 0xfc) >> 2];
                resultP[5] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
                resultP[6] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
                resultP[7] = base64[byte5 & 0x3f];

                var byte6 = (byte)(_b >> 8);
                var byte7 = (byte)(_b);
                var byte8 = (byte)(_c >> 24);

                resultP[8] = base64[(byte6 & 0xfc) >> 2];
                resultP[9] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
                resultP[10] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
                resultP[11] = base64[byte8 & 0x3f];

                var byte9 = (byte)(_c >> 16);
                var byte10 = (byte)(_c >> 8);
                var byte11 = (byte)(_c);

                resultP[12] = base64[(byte9 & 0xfc) >> 2];
                resultP[13] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
                resultP[14] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
                resultP[15] = base64[byte11 & 0x3f];
            }
        }
        return result;
    }

    private String ToBase64()
    {
        var result = new string((char)0, 16);
        var table = Base64.table;

        unsafe
        {
            fixed (char* resultP = result)
            fixed (char* base64 = &table[0])
            {
                var byte0 = (byte)(_timestamp >> 24);
                var byte1 = (byte)(_timestamp >> 16);
                var byte2 = (byte)(_timestamp >> 8);

                resultP[0] = base64[(byte0 & 0xfc) >> 2];
                resultP[1] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
                resultP[2] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
                resultP[3] = base64[byte2 & 0x3f];

                var byte3 = (byte)(_timestamp);
                var byte4 = (byte)(_b >> 24);
                var byte5 = (byte)(_b >> 16);

                resultP[4] = base64[(byte3 & 0xfc) >> 2];
                resultP[5] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
                resultP[6] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
                resultP[7] = base64[byte5 & 0x3f];

                var byte6 = (byte)(_b >> 8);
                var byte7 = (byte)(_b);
                var byte8 = (byte)(_c >> 24);

                resultP[8] = base64[(byte6 & 0xfc) >> 2];
                resultP[9] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
                resultP[10] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
                resultP[11] = base64[byte8 & 0x3f];

                var byte9 = (byte)(_c >> 16);
                var byte10 = (byte)(_c >> 8);
                var byte11 = (byte)(_c);

                resultP[12] = base64[(byte9 & 0xfc) >> 2];
                resultP[13] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
                resultP[14] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
                resultP[15] = base64[byte11 & 0x3f];
            }
        }
        return result;
    }

    private String ToBase85()
    {
        return Base85.Encode(ToByteArray());
    }

    private String ToHexLower()
    {
        var result = new string((char)0, 24);
        unsafe
        {
            var lookupP = Hex._lowerLookup32UnsafeP;
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;
                resultP2[0] = lookupP[(byte)(_timestamp >> 24)];
                resultP2[1] = lookupP[(byte)(_timestamp >> 16)];
                resultP2[2] = lookupP[(byte)(_timestamp >> 8)];
                resultP2[3] = lookupP[(byte)(_timestamp)];
                resultP2[4] = lookupP[(byte)(_b >> 24)];
                resultP2[5] = lookupP[(byte)(_b >> 16)];
                resultP2[6] = lookupP[(byte)(_b >> 8)];
                resultP2[7] = lookupP[(byte)(_b)];
                resultP2[8] = lookupP[(byte)(_c >> 24)];
                resultP2[9] = lookupP[(byte)(_c >> 16)];
                resultP2[10] = lookupP[(byte)(_c >> 8)];
                resultP2[11] = lookupP[(byte)(_c)];
            }
        }
        return result;
    }

    private String ToHexUpper()
    {
        var result = new string((char)0, 24);
        unsafe
        {
            var lookupP = Hex._upperLookup32UnsafeP;
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;
                resultP2[0] = lookupP[(byte)(_timestamp >> 24)];
                resultP2[1] = lookupP[(byte)(_timestamp >> 16)];
                resultP2[2] = lookupP[(byte)(_timestamp >> 8)];
                resultP2[3] = lookupP[(byte)(_timestamp)];
                resultP2[4] = lookupP[(byte)(_b >> 24)];
                resultP2[5] = lookupP[(byte)(_b >> 16)];
                resultP2[6] = lookupP[(byte)(_b >> 8)];
                resultP2[7] = lookupP[(byte)(_b)];
                resultP2[8] = lookupP[(byte)(_c >> 24)];
                resultP2[9] = lookupP[(byte)(_c >> 16)];
                resultP2[10] = lookupP[(byte)(_c >> 8)];
                resultP2[11] = lookupP[(byte)(_c)];
            }
        }
        return result;
    }

    private String ToPath2()
    {
        var result = new string((char)0, 18);
        var table = Base64.tableUrl;
        var sep = Path.DirectorySeparatorChar;
        unsafe
        {
            fixed (char* resultP = result)
            fixed (char* base64 = &table[0])
            {
                var byte0 = (byte)(_timestamp >> 24);
                var byte1 = (byte)(_timestamp >> 16);
                var byte2 = (byte)(_timestamp >> 8);

                resultP[17] = base64[(byte0 & 0xfc) >> 2];
                resultP[16] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
                resultP[15] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
                resultP[14] = base64[byte2 & 0x3f];

                var byte3 = (byte)(_timestamp);
                var byte4 = (byte)(_b >> 24);
                var byte5 = (byte)(_b >> 16);

                resultP[13] = base64[(byte3 & 0xfc) >> 2];
                resultP[12] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
                resultP[11] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
                resultP[10] = base64[byte5 & 0x3f];

                var byte6 = (byte)(_b >> 8);
                var byte7 = (byte)(_b);
                var byte8 = (byte)(_c >> 24);

                resultP[9] = base64[(byte6 & 0xfc) >> 2];
                resultP[8] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
                resultP[7] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
                resultP[6] = base64[byte8 & 0x3f];

                var byte9 = (byte)(_c >> 16);
                var byte10 = (byte)(_c >> 8);
                var byte11 = (byte)(_c);

                resultP[5] = base64[(byte9 & 0xfc) >> 2];
                resultP[4] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
                resultP[3] = sep;
                resultP[2] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
                resultP[1] = sep;
                resultP[0] = base64[byte11 & 0x3f];
            }
        }
        return result;
    }

    private String ToPath3()
    {
        var result = new string((char)0, 19);
        var table = Base64.tableUrl;
        var sep = Path.DirectorySeparatorChar;
        unsafe
        {
            fixed (char* resultP = result)
            fixed (char* base64 = &table[0])
            {
                var byte0 = (byte)(_timestamp >> 24);
                var byte1 = (byte)(_timestamp >> 16);
                var byte2 = (byte)(_timestamp >> 8);

                resultP[18] = base64[(byte0 & 0xfc) >> 2];
                resultP[17] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
                resultP[16] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
                resultP[15] = base64[byte2 & 0x3f];

                var byte3 = (byte)(_timestamp);
                var byte4 = (byte)(_b >> 24);
                var byte5 = (byte)(_b >> 16);

                resultP[14] = base64[(byte3 & 0xfc) >> 2];
                resultP[13] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
                resultP[12] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
                resultP[11] = base64[byte5 & 0x3f];

                var byte6 = (byte)(_b >> 8);
                var byte7 = (byte)(_b);
                var byte8 = (byte)(_c >> 24);

                resultP[10] = base64[(byte6 & 0xfc) >> 2];
                resultP[9] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
                resultP[8] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
                resultP[7] = base64[byte8 & 0x3f];

                var byte9 = (byte)(_c >> 16);
                var byte10 = (byte)(_c >> 8);
                var byte11 = (byte)(_c);

                resultP[6] = base64[(byte9 & 0xfc) >> 2];
                resultP[5] = sep;
                resultP[4] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
                resultP[3] = sep;
                resultP[2] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
                resultP[1] = sep;
                resultP[0] = base64[byte11 & 0x3f];
            }
        }
        return result;
    }

#if NET6_0

    private void ToBase64(Span<Char> destination, Char[] table)
    {
        unsafe
        {
            fixed (char* resultP = destination)
            fixed (char* base64 = &table[0])
            {
                var byte0 = (byte)(_timestamp >> 24);
                var byte1 = (byte)(_timestamp >> 16);
                var byte2 = (byte)(_timestamp >> 8);

                resultP[0] = base64[(byte0 & 0xfc) >> 2];
                resultP[1] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
                resultP[2] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
                resultP[3] = base64[byte2 & 0x3f];

                var byte3 = (byte)(_timestamp);
                var byte4 = (byte)(_b >> 24);
                var byte5 = (byte)(_b >> 16);

                resultP[4] = base64[(byte3 & 0xfc) >> 2];
                resultP[5] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
                resultP[6] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
                resultP[7] = base64[byte5 & 0x3f];

                var byte6 = (byte)(_b >> 8);
                var byte7 = (byte)(_b);
                var byte8 = (byte)(_c >> 24);

                resultP[8] = base64[(byte6 & 0xfc) >> 2];
                resultP[9] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
                resultP[10] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
                resultP[11] = base64[byte8 & 0x3f];

                var byte9 = (byte)(_c >> 16);
                var byte10 = (byte)(_c >> 8);
                var byte11 = (byte)(_c);

                resultP[12] = base64[(byte9 & 0xfc) >> 2];
                resultP[13] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
                resultP[14] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
                resultP[15] = base64[byte11 & 0x3f];
            }
        }
    }

    private void ToBase85(Span<Char> destination)
    {
        Base85.Encode(ToByteArray(), destination);
    }

    private unsafe void ToHex(Span<Char> destination, uint* lookupP)
    {
        fixed (char* resultP = destination)
        {
            uint* resultP2 = (uint*)resultP;
            resultP2[0] = lookupP[(byte)(_timestamp >> 24)];
            resultP2[1] = lookupP[(byte)(_timestamp >> 16)];
            resultP2[2] = lookupP[(byte)(_timestamp >> 8)];
            resultP2[3] = lookupP[(byte)(_timestamp)];
            resultP2[4] = lookupP[(byte)(_b >> 24)];
            resultP2[5] = lookupP[(byte)(_b >> 16)];
            resultP2[6] = lookupP[(byte)(_b >> 8)];
            resultP2[7] = lookupP[(byte)(_b)];
            resultP2[8] = lookupP[(byte)(_c >> 24)];
            resultP2[9] = lookupP[(byte)(_c >> 16)];
            resultP2[10] = lookupP[(byte)(_c >> 8)];
            resultP2[11] = lookupP[(byte)(_c)];
        }
    }

    private void ToPath2(Span<Char> destination)
    {
        var table = Base64.tableUrl;
        var sep = Path.DirectorySeparatorChar;
        unsafe
        {
            fixed (char* resultP = destination)
            fixed (char* base64 = &table[0])
            {
                var byte0 = (byte)(_timestamp >> 24);
                var byte1 = (byte)(_timestamp >> 16);
                var byte2 = (byte)(_timestamp >> 8);

                resultP[17] = base64[(byte0 & 0xfc) >> 2];
                resultP[16] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
                resultP[15] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
                resultP[14] = base64[byte2 & 0x3f];

                var byte3 = (byte)(_timestamp);
                var byte4 = (byte)(_b >> 24);
                var byte5 = (byte)(_b >> 16);

                resultP[13] = base64[(byte3 & 0xfc) >> 2];
                resultP[12] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
                resultP[11] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
                resultP[10] = base64[byte5 & 0x3f];

                var byte6 = (byte)(_b >> 8);
                var byte7 = (byte)(_b);
                var byte8 = (byte)(_c >> 24);

                resultP[9] = base64[(byte6 & 0xfc) >> 2];
                resultP[8] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
                resultP[7] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
                resultP[6] = base64[byte8 & 0x3f];

                var byte9 = (byte)(_c >> 16);
                var byte10 = (byte)(_c >> 8);
                var byte11 = (byte)(_c);

                resultP[5] = base64[(byte9 & 0xfc) >> 2];
                resultP[4] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
                resultP[3] = sep;
                resultP[2] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
                resultP[1] = sep;
                resultP[0] = base64[byte11 & 0x3f];
            }
        }
    }

    private void ToPath3(Span<Char> destination)
    {
        var table = Base64.tableUrl;
        var sep = Path.DirectorySeparatorChar;
        unsafe
        {
            fixed (char* resultP = destination)
            fixed (char* base64 = &table[0])
            {
                var byte0 = (byte)(_timestamp >> 24);
                var byte1 = (byte)(_timestamp >> 16);
                var byte2 = (byte)(_timestamp >> 8);

                resultP[18] = base64[(byte0 & 0xfc) >> 2];
                resultP[17] = base64[((byte0 & 0x03) << 4) | ((byte1 & 0xf0) >> 4)];
                resultP[16] = base64[((byte1 & 0x0f) << 2) | ((byte2 & 0xc0) >> 6)];
                resultP[15] = base64[byte2 & 0x3f];

                var byte3 = (byte)(_timestamp);
                var byte4 = (byte)(_b >> 24);
                var byte5 = (byte)(_b >> 16);

                resultP[14] = base64[(byte3 & 0xfc) >> 2];
                resultP[13] = base64[((byte3 & 0x03) << 4) | ((byte4 & 0xf0) >> 4)];
                resultP[12] = base64[((byte4 & 0x0f) << 2) | ((byte5 & 0xc0) >> 6)];
                resultP[11] = base64[byte5 & 0x3f];

                var byte6 = (byte)(_b >> 8);
                var byte7 = (byte)(_b);
                var byte8 = (byte)(_c >> 24);

                resultP[10] = base64[(byte6 & 0xfc) >> 2];
                resultP[9] = base64[((byte6 & 0x03) << 4) | ((byte7 & 0xf0) >> 4)];
                resultP[8] = base64[((byte7 & 0x0f) << 2) | ((byte8 & 0xc0) >> 6)];
                resultP[7] = base64[byte8 & 0x3f];

                var byte9 = (byte)(_c >> 16);
                var byte10 = (byte)(_c >> 8);
                var byte11 = (byte)(_c);

                resultP[6] = base64[(byte9 & 0xfc) >> 2];
                resultP[5] = sep;
                resultP[4] = base64[((byte9 & 0x03) << 4) | ((byte10 & 0xf0) >> 4)];
                resultP[3] = sep;
                resultP[2] = base64[((byte10 & 0x0f) << 2) | ((byte11 & 0xc0) >> 6)];
                resultP[1] = sep;
                resultP[0] = base64[byte11 & 0x3f];
            }
        }
    }

#endif

    private static Id ParseBase64(ReadOnlySpan<Char> value)
    {
        if (value.Length != 16) throw new ArgumentException("String must be 16 characters long", nameof(value));

        Span<Byte> bytes = stackalloc Byte[12];

        Base64.Parse(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }

    private static Id ParseBase85(ReadOnlySpan<Char> value)
    {
        if (value.Length != 15) throw new ArgumentException("String must be 15 characters long", nameof(value));

        Span<Byte> bytes = stackalloc Byte[12];

        Base85.Decode(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }

    private static Id ParseHex(ReadOnlySpan<Char> value)
    {
        if (value.Length != 24) throw new ArgumentException("String must be 24 characters long", nameof(value));

        Span<Byte> bytes = stackalloc Byte[12];

        Hex.Decode(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }

    private static Id ParsePath2(ReadOnlySpan<Char> value)
    {
        if (value.Length != 18) throw new ArgumentException("String must be 18 characters long", nameof(value));

        var i1 = value[1];
        var i3 = value[3];

        if ((i1 != '\\' && i1 != '/') || (i3 != '\\' && i3 != '/')) throw new FormatException();

        //_\I\-TH145xA0ZPhqY

        Span<Byte> bytes = stackalloc Byte[12];

        Base64.ParsePath2(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }

    private static Id ParsePath3(ReadOnlySpan<Char> value)
    {
        if (value.Length != 19) throw new ArgumentException("String must be 19 characters long", nameof(value));

        var i1 = value[1];
        var i3 = value[3];
        var i5 = value[5];
        if (i1 != '\\' && i1 != '/' && i3 != '\\' && i3 != '/' && i5 != '\\' && i5 != '/') throw new FormatException();

        //_\I\-\TH145xA0ZPhqY

        Span<Byte> bytes = stackalloc Byte[12];

        Base64.ParsePath3(value, bytes);

        FromByteArray(bytes, 0, out var timestamp, out var b, out var c);

        return new Id(timestamp, b, c);
    }

    private static long CalculateRandomValue()
    {
        var seed = (int)DateTime.UtcNow.Ticks ^ GetMachineHash() ^ GetPid();
        var random = new Random(seed);
        var high = random.Next();
        var low = random.Next();
        var combined = (long)((ulong)(uint)high << 32 | (ulong)(uint)low);
        return combined & 0xffffffffff; // low order 5 bytes
    }

    private static Id Create(int timestamp, long random, int increment)
    {
        if (random < 0 || random > 0xffffffffff) throw new ArgumentOutOfRangeException(nameof(random), "The random value must be between 0 and 1099511627775 (it must fit in 5 bytes).");

        if (increment < 0 || increment > 0xffffff) throw new ArgumentOutOfRangeException(nameof(increment), "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");

        var b = (int)(random >> 8); // first 4 bytes of random
        var c = (int)(random << 24) | increment; // 5th byte of random and 3 byte increment
        return new Id(timestamp, b, c);
    }

    /// <summary>
    /// Gets the current process id.  This method exists because of how CAS operates on the call stack, checking
    /// for permissions before executing the method.  Hence, if we inlined this call, the calling method would not execute
    /// before throwing an exception requiring the try/catch at an even higher level that we don't necessarily control.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int GetCurrentProcessId() => Process.GetCurrentProcess().Id;

    private static int GetMachineHash()
    {
        // use instead of Dns.HostName so it will work offline
        var machineName = Environment.MachineName;
        return 0x00ffffff & machineName.GetHashCode(); // use first 3 bytes of hash
    }

    private static short GetPid()
    {
        try
        {
            return (short)GetCurrentProcessId(); // use low order two bytes only
        }
        catch (SecurityException)
        {
            return 0;
        }
    }

    private static int GetTimestampFromDateTime(DateTime timestamp)
    {
        var secondsSinceEpoch = (long)Math.Floor((ToUniversalTime(timestamp) - _unixEpoch).TotalSeconds);
        if (secondsSinceEpoch < uint.MinValue || secondsSinceEpoch > uint.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(timestamp));
        }
        return (int)(uint)secondsSinceEpoch;
    }

    private static void FromByteArray(ReadOnlySpan<byte> bytes, int offset, out int timestamp, out int b, out int c)
    {
        timestamp = (bytes[offset] << 24) | (bytes[offset + 1] << 16) | (bytes[offset + 2] << 8) | bytes[offset + 3];
        b = (bytes[offset + 4] << 24) | (bytes[offset + 5] << 16) | (bytes[offset + 6] << 8) | bytes[offset + 7];
        c = (bytes[offset + 8] << 24) | (bytes[offset + 9] << 16) | (bytes[offset + 10] << 8) | bytes[offset + 11];
    }

    private static DateTime ToUniversalTime(DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
        {
            return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        }
        else if (dateTime == DateTime.MaxValue)
        {
            return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
        }
        else
        {
            return dateTime.ToUniversalTime();
        }
    }

    #endregion Private Methods
}