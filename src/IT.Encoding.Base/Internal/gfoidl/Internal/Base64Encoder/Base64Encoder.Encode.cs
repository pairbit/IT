﻿using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Scalar based on https://github.com/dotnet/corefx/tree/ec34e99b876ea1119f37986ead894f4eded1a19a/src/System.Memory/src/System/Buffers/Text
// SSE2   based on https://github.com/aklomp/base64/tree/a27c565d1b6c676beaf297fe503c4518185666f7/lib/arch/ssse3
// AVX2   based on https://github.com/aklomp/base64/tree/a27c565d1b6c676beaf297fe503c4518185666f7/lib/arch/avx2

namespace gfoidl.Base64.Internal
{
    internal partial class Base64Encoder
    {
        public override int GetEncodedLength(int sourceLength) => GetBase64EncodedLength(sourceLength);
        //---------------------------------------------------------------------
        // PERF: can't be in base class due to inlining (generic virtual)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe string Encode(ReadOnlySpan<byte> data)
        {
            if (data.IsEmpty) return string.Empty;

#if NETCOREAPP || NETSTANDARD2_1
            // Threshould found by testing -- may not be ideal on all targets
#if CAN_USE_CONVERT_BASE64
            if (data.Length < 16)
                return Convert.ToBase64String(data);

            if (data.Length < 50)
                return EncodeWithNewString(data);

            return EncodeWithStringCreate(data);
#else
            return data.Length < 50
                ? EncodeWithNewString(data)
                : EncodeWithStringCreate(data);
#endif
            //-----------------------------------------------------------------
            string EncodeWithNewString(ReadOnlySpan<byte> data)
            {
                // stackallocing a power of 2 is preferred, as the JIT can produce better code,
                // especially if `locals init` is skipped, so it's just a pointer move `sub rsp, 256`
                char* ptr              = stackalloc char[MaxStackallocBytes / sizeof(char)];
                ref char encoded       = ref Unsafe.AsRef<char>(ptr);
                ref byte srcBytes      = ref MemoryMarshal.GetReference(data);
                int encodedLength      = this.GetEncodedLength(data.Length);
                OperationStatus status = this.EncodeImpl(ref srcBytes, data.Length, ref encoded, encodedLength, encodedLength, out int consumed, out int written);

                Debug.Assert(status        == OperationStatus.Done);
                Debug.Assert(data.Length   == consumed);
                Debug.Assert(encodedLength == written);

                return new string(ptr, 0, written);
            }
            //-----------------------------------------------------------------
            string EncodeWithStringCreate(ReadOnlySpan<byte> data)
            {
                fixed (byte* ptr = data)
                {
                    int encodedLength = this.GetEncodedLength(data.Length);

                    return string.Create(encodedLength, (Ptr: (IntPtr)ptr, data.Length), (encoded, state) =>
                    {
                        ref byte srcBytes      = ref Unsafe.AsRef<byte>(state.Ptr.ToPointer());
                        ref char dest          = ref MemoryMarshal.GetReference(encoded);
                        OperationStatus status = this.EncodeImpl(ref srcBytes, state.Length, ref dest, encoded.Length, encoded.Length, out int consumed, out int written);

                        Debug.Assert(status         == OperationStatus.Done);
                        Debug.Assert(state.Length   == consumed);
                        Debug.Assert(encoded.Length == written);
                    });
                }
            }
#else
            return EncodeWithNewString(data);
            //-----------------------------------------------------------------
            string EncodeWithNewString(ReadOnlySpan<byte> data)
            {
                int encodedLength           = this.GetEncodedLength(data.Length);
                char[]? arrayToReturnToPool = null;
                Span<char> encoded          = encodedLength <= MaxStackallocBytes / sizeof(char)
                    ? stackalloc char[MaxStackallocBytes / sizeof(char)]
                    : ArrayPool<char>.Shared.Rent(encodedLength);

                try
                {
                    OperationStatus status = this.EncodeImpl(data, encoded, out int consumed, out int written, encodedLength);

                    Debug.Assert(status        == OperationStatus.Done);
                    Debug.Assert(data.Length   == consumed);
                    Debug.Assert(encodedLength == written);

                    fixed (char* ptr = &MemoryMarshal.GetReference(encoded))
                        return new string(ptr, 0, written);
                }
                finally
                {
                    if (arrayToReturnToPool != null)
                    {
                        encoded.Clear();
                        ArrayPool<char>.Shared.Return(arrayToReturnToPool);
                    }
                }
            }
#endif
        }
        //---------------------------------------------------------------------
        // PERF: can't be in base class due to inlining (generic virtual)
        protected override OperationStatus EncodeCore(
            ReadOnlySpan<byte> data,
            Span<byte> encoded,
            out int consumed,
            out int written,
            int encodedLength = -1,
            bool isFinalBlock = true)
            => this.EncodeImpl(data, encoded, out consumed, out written, encodedLength, isFinalBlock);
        //---------------------------------------------------------------------
        // PERF: can't be in base class due to inlining (generic virtual)
        protected override OperationStatus EncodeCore(
            ReadOnlySpan<byte> data,
            Span<char> encoded,
            out int consumed,
            out int written,
            int encodedLength = -1,
            bool isFinalBlock = true)
            => this.EncodeImpl(data, encoded, out consumed, out written, encodedLength, isFinalBlock);
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private OperationStatus EncodeImpl<T>(
            ReadOnlySpan<byte> data,
            Span<T> encoded,
            out int consumed,
            out int written,
            int encodedLength = -1,
            bool isFinalBlock = true)
            where T : unmanaged
        {
            if (data.IsEmpty)
            {
                consumed = 0;
                written  = 0;
                return OperationStatus.Done;
            }

            int srcLength     = data.Length;
            ref byte srcBytes = ref MemoryMarshal.GetReference(data);
            ref T dest        = ref MemoryMarshal.GetReference(encoded);

            if (encodedLength == -1)
                encodedLength = this.GetEncodedLength(srcLength);

            return this.EncodeImpl(ref srcBytes, srcLength, ref dest, encoded.Length, encodedLength, out consumed, out written, isFinalBlock);
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EncodeTwoBytes<T>(ref byte twoBytes, ref T encoded, ref byte encodingMap)
        {
            uint i = (uint)twoBytes << 16
                | (uint)Unsafe.Add(ref twoBytes, 1) << 8;

            uint i0 = Unsafe.Add(ref encodingMap, (IntPtr)(i >> 18));
            uint i1 = Unsafe.Add(ref encodingMap, (IntPtr)((i >> 12) & 0x3F));
            uint i2 = Unsafe.Add(ref encodingMap, (IntPtr)((i >> 6) & 0x3F));

            if (typeof(T) == typeof(byte))
            {
                i = i0 | (i1 << 8) | (i2 << 16) | (EncodingPad << 24);
                Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref encoded), i);
            }
            else if (typeof(T) == typeof(char))
            {
                ref char enc = ref Unsafe.As<T, char>(ref encoded);
                Unsafe.Add(ref enc, 0) = (char)i0;
                Unsafe.Add(ref enc, 1) = (char)i1;
                Unsafe.Add(ref enc, 2) = (char)i2;
                Unsafe.Add(ref enc, 3) = (char)EncodingPad;
            }
            else
            {
                throw new NotSupportedException();  // just in case new types are introduced in the future
            }
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EncodeOneByte<T>(ref byte oneByte, ref T encoded, ref byte encodingMap)
        {
            uint i = (uint)oneByte << 8;

            uint i0 = Unsafe.Add(ref encodingMap, (IntPtr)(i >> 10));
            uint i1 = Unsafe.Add(ref encodingMap, (IntPtr)((i >> 4) & 0x3F));

            if (typeof(T) == typeof(byte))
            {
                i = i0 | (i1 << 8) | (EncodingPad << 16) | (EncodingPad << 24);
                Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref encoded), i);
            }
            else if (typeof(T) == typeof(char))
            {
                ref char enc = ref Unsafe.As<T, char>(ref encoded);
                Unsafe.Add(ref enc, 0) = (char)i0;
                Unsafe.Add(ref enc, 1) = (char)i1;
                Unsafe.Add(ref enc, 2) = (char)EncodingPad;
                Unsafe.Add(ref enc, 3) = (char)EncodingPad;
            }
            else
            {
                throw new NotSupportedException();  // just in case new types are introduced in the future
            }
        }
        //---------------------------------------------------------------------
        // internal because tests use this map too
        internal static ReadOnlySpan<byte> EncodingMap
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ReadOnlySpan<byte> map = new byte[64 + 1] {
                    0,      // https://github.com/dotnet/coreclr/issues/23194
                    65, 66, 67, 68, 69, 70, 71, 72,         //A..H
                    73, 74, 75, 76, 77, 78, 79, 80,         //I..P
                    81, 82, 83, 84, 85, 86, 87, 88,         //Q..X
                    89, 90, 97, 98, 99, 100, 101, 102,      //Y..Z, a..f
                    103, 104, 105, 106, 107, 108, 109, 110, //g..n
                    111, 112, 113, 114, 115, 116, 117, 118, //o..v
                    119, 120, 121, 122, 48, 49, 50, 51,     //w..z, 0..3
                    52, 53, 54, 55, 56, 57, 43, 47          //4..9, +, /
                };

                // Slicing is necessary to "unlink" the ref and let the JIT keep it in a register
                return map.Slice(1);
            }
        }
    }
}
