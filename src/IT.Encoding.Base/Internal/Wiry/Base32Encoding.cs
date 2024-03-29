﻿// Copyright (c) Dmitry Razumikhin, 2016-2019.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Wiry.Base32
{
    /// <summary>
    /// Generic Base32 implementation
    /// </summary>
    internal abstract class Base32Encoding
    {
        private const string ErrorMessageInvalidLength = "Invalid length";
        private const string ErrorMessageInvalidPadding = "Invalid padding";
        private const string ErrorMessageInvalidCharacter = "Invalid character";

        /// <summary>
        /// Base32 alphabet length.
        /// </summary>
        protected const int AlphabetLength = 32;

        private static volatile Base32Encoding _standard;
        private static volatile Base32Encoding _zBase32;

        /// <summary>
        /// Represent standard encoding defined in RFC 4648
        /// </summary>
        public static Base32Encoding Standard => _standard ?? (_standard = new StandardBase32Encoding());

        /// <summary>
        /// Represent z-base-32 encoding by Zooko O'Whielacronx
        /// </summary>
        public static Base32Encoding ZBase32 => _zBase32 ?? (_zBase32 = new ZBase32Encoding());

        private volatile LookupTable _lookupTable;

        /// <summary>
        /// Alphabet of a concrete Base32 encoding.
        /// </summary>
        protected abstract string Alphabet { get; }

        /// <summary>
        /// Padding symbol of a concrete Base32 encoding.
        /// </summary>
        protected abstract char? PadSymbol { get; }

        public Base32Encoding()
        {
            _lookupTable = BuildLookupTable(Alphabet);
        }

        /// <summary>
        /// Get encoded string
        /// </summary>
        public virtual string GetString(ReadOnlySpan<Byte> bytes)
        {
            return ToBase32(bytes, Alphabet, PadSymbol);
        }

        /// <summary>
        /// When overridden in a derived class, decodes string data to bytes.
        /// </summary>
        public virtual byte[] ToBytes(ReadOnlySpan<Char> encoded)
        {
            return ToBytes(encoded, PadSymbol, _lookupTable);
        }


        /// <summary>
        /// Validate input data
        /// </summary>
        public virtual ValidationResult Validate(ReadOnlySpan<Char> encoded)
        {
            return Validate(encoded, PadSymbol, _lookupTable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetSymbolsCount(int bytesCount)
        {
            return (bytesCount * 8 + 4) / 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetBytesCount(int symbolsCount)
        {
            return symbolsCount * 5 / 8;
        }

        private const int LookupTableNullItem = -1;

        private static LookupTable BuildLookupTable(string alphabet)
        {
            int[] codes = alphabet.Select(ch => (int)ch).ToArray();
            int min = codes.Min();
            int max = codes.Max();
            int size = max - min + 1;
            var table = new int[size];

            for (int i = 0; i < table.Length; i++)
                table[i] = LookupTableNullItem;

            foreach (int code in codes)
                table[code - min] = alphabet.IndexOf((char)code);

            return new LookupTable(min, table);
        }

        private static unsafe void ToBase32GroupsUnsafe(byte* pInput, char* pOutput, char* pAlphabet,
            int inputGroupsCount)
        {
            for (int i = 0; i < inputGroupsCount; i++)
            {
                ulong value = *pInput++;
                for (int j = 0; j < 4; j++)
                {
                    value <<= 8;
                    value |= *pInput++;
                }

                pOutput += 7;
                char* pNextPos = pOutput + 1;
                for (int j = 0; j < 7; j++)
                {
                    *pOutput-- = pAlphabet[value & 0x1F];
                    value >>= 5;
                }

                *pOutput = pAlphabet[value];
                pOutput = pNextPos;
            }
        }

        private static unsafe int ToBase32RemainderUnsafe(byte* pInput, char* pOutput, char* pAlphabet, int remainder)
        {
            ulong value = *pInput++;
            for (int j = 1; j < remainder; j++)
            {
                value <<= 8;
                value |= *pInput++;
            }

            int symbols = GetSymbolsCount(remainder);
            value <<= (5 - remainder) * 8 - (8 - symbols) * 5;

            pOutput += symbols - 1;
            for (int j = 1; j < symbols; j++)
            {
                *pOutput-- = pAlphabet[value & 0x1F];
                value >>= 5;
            }

            *pOutput = pAlphabet[value];

            return symbols;
        }

        private static unsafe void ToBase32Unsafe(ReadOnlySpan<Byte> input, Span<Char> output,
            int inputGroupsCount, int remainder, string alphabet, char? padSymbol)
        {
            fixed (byte* pInput = input)
            fixed (char* pOutput = output)
            fixed (char* pAlphabet = alphabet)
            {
                if (inputGroupsCount > 0)
                    ToBase32GroupsUnsafe(pInput, pOutput, pAlphabet, inputGroupsCount);

                if (remainder <= 0)
                    return;

                byte* pInputRemainder = pInput + inputGroupsCount * 5;
                char* pOutputRemainder = pOutput + inputGroupsCount * 8;
                int symbols = ToBase32RemainderUnsafe(pInputRemainder, pOutputRemainder, pAlphabet, remainder);
                if (padSymbol == null)
                    return;

                char padChar = padSymbol.Value;
                pOutputRemainder += symbols;
                for (int i = symbols; i < 8; i++)
                {
                    *pOutputRemainder++ = padChar;
                }
            }
        }

        private static unsafe void ToBytesGroupsUnsafe(char* pEncoded, byte* pOutput, int encodedGroupsCount,
            int* pLookup, int lookupSize, int lowCode)
        {
            ulong value = 0;
            for (int i = encodedGroupsCount; i != 0; i--)
            {
                for (int j = 8; j != 0; j--)
                {
                    int lookupIndex = *pEncoded - lowCode;
                    if (lookupIndex < 0 || lookupIndex >= lookupSize)
                        throw new FormatException(ErrorMessageInvalidCharacter);

                    int item = *(pLookup + lookupIndex);
                    if (item == LookupTableNullItem)
                        throw new FormatException(ErrorMessageInvalidCharacter);

                    value <<= 5;
                    value |= (byte)item;
                    pEncoded++;
                }

                pOutput += 4;
                byte* pNextPos = pOutput + 1;
                for (int j = 4; j != 0; j--)
                {
                    *pOutput-- = (byte)value;
                    value >>= 8;
                }

                *pOutput = (byte)value;
                pOutput = pNextPos;
            }
        }

        private static unsafe void ToBytesRemainderUnsafe(char* pEncoded, byte* pOutput, int remainder,
            int* pLookup, int lookupSize, int lowCode)
        {
            ulong value = 0;
            for (int j = remainder; j != 0; j--)
            {
                int lookupIndex = *pEncoded - lowCode;
                if (lookupIndex < 0 || lookupIndex >= lookupSize)
                    throw new FormatException(ErrorMessageInvalidCharacter);

                int item = *(pLookup + lookupIndex);
                if (item == LookupTableNullItem)
                    throw new FormatException(ErrorMessageInvalidCharacter);

                value <<= 5;
                value |= (byte)item;
                pEncoded++;
            }

            int bytesCount = GetBytesCount(remainder);
            value >>= (5 - bytesCount) * 8 - (8 - remainder) * 5;

            bytesCount--;
            pOutput += bytesCount;
            for (int j = bytesCount; j != 0; j--)
            {
                *pOutput-- = (byte)value;
                value >>= 8;
            }

            *pOutput = (byte)value;
        }

        private static unsafe void ToBytesUnsafe(ReadOnlySpan<Char> encoded, byte[] output,
            int encodedGroupsCount, int remainder, LookupTable lookupTable)
        {
            int[] lookupValues = lookupTable.Values;
            int lowCode = lookupTable.LowCode;
            fixed (char* pEncoded = encoded)
            fixed (byte* pOutput = output)
            fixed (int* pLookup = lookupValues)
            {
                if (encodedGroupsCount > 0)
                {
                    ToBytesGroupsUnsafe(pEncoded, pOutput, encodedGroupsCount, pLookup,
                        lookupValues.Length, lowCode);
                }

                if (remainder <= 1)
                    return;

                char* pEncodedRemainder = pEncoded + encodedGroupsCount * 8;
                byte* pOutputRemainder = pOutput + encodedGroupsCount * 5;
                ToBytesRemainderUnsafe(pEncodedRemainder, pOutputRemainder, remainder, pLookup,
                    lookupValues.Length, lowCode);
            }
        }

        internal static string ToBase32(ReadOnlySpan<Byte> bytes, string alphabet, char? padSymbol)
        {
            var count = bytes.Length;

            if (count == 0)
                return string.Empty;

            int groupsCount = count / 5;
            int remainder = count % 5;

            int symbolsCount = groupsCount * 8;
            if (padSymbol == null)
            {
                symbolsCount += GetSymbolsCount(remainder);
            }
            else if (remainder != 0)
            {
                symbolsCount += 8;
            }

#if NETSTANDARD2_0
            unsafe
            {
                fixed (byte* dataPtr = bytes)
                {
                    return _String.Create(symbolsCount, (Ptr: (IntPtr)dataPtr, bytes.Length, groupsCount, remainder, alphabet, padSymbol), (encoded, state) =>
                    {
                        var data = new ReadOnlySpan<Byte>((Byte*)state.Ptr, state.Length);

                        ToBase32Unsafe(data, encoded, state.groupsCount, state.remainder, state.alphabet, state.padSymbol);
                    });
                }
            }
#else
        unsafe
        {
            fixed (byte* dataPtr = bytes)
            {
                return String.Create(symbolsCount, (Ptr: (IntPtr)dataPtr, bytes.Length, groupsCount, remainder, alphabet, padSymbol), (encoded, state) =>
                {
                    var data = new ReadOnlySpan<Byte>((Byte*)state.Ptr, state.Length);

                    ToBase32Unsafe(data, encoded, state.groupsCount, state.remainder, state.alphabet, state.padSymbol);
                });
            }
        }
#endif
        }


        private static int GetRemainderWithChecks(ReadOnlySpan<Char> encoded, char? padSymbol)
        {
            if (padSymbol == null)
                return encoded.Length % 8;

            if (encoded.Length % 8 != 0)
                throw new FormatException(ErrorMessageInvalidLength);

            int remainder = 8;
            char padChar = padSymbol.Value;
            for (int i = encoded.Length - 1; i >= 0; i--)
            {
                if (encoded[i] != padChar)
                    break;

                if (--remainder <= 0)
                    throw new FormatException(ErrorMessageInvalidPadding);
            }

            return remainder;
        }

        internal static byte[] ToBytes(ReadOnlySpan<Char> encoded, char? padSymbol, LookupTable lookupTable)
        {
            var length = encoded.Length;
            if (length == 0) return Array.Empty<Byte>();

            int remainder = GetRemainderWithChecks(encoded, padSymbol);

            int groupsCount = length / 8;

            int bytesCount = 0;
            if (remainder > 0)
            {
                if (padSymbol != null)
                {
                    // groupsCount always >= 1 here because of "length % 8 != 0" as checked before
                    groupsCount--;
                }

                bytesCount = GetBytesCount(remainder);
            }

            bytesCount += groupsCount * 5;

            var bytes = new byte[bytesCount];
            if (bytesCount > 0)
            {
                ToBytesUnsafe(encoded, bytes, groupsCount, remainder, lookupTable);
            }

            return bytes;
        }

        internal static ValidationResult Validate(ReadOnlySpan<Char> encoded, char? padSymbol,
            LookupTable lookupTable)
        {
            try
            {
                var length = encoded.Length;
                int bytesCount = GetBytesCount(length);
                int symbolsCount = GetSymbolsCount(bytesCount);
                if (symbolsCount != length)
                    return ValidationResult.InvalidLength;

                int remainder;
                try
                {
                    remainder = GetRemainderWithChecks(encoded, padSymbol);
                }
                catch (FormatException fex)
                {
                    switch (fex.Message)
                    {
                        case ErrorMessageInvalidLength:
                            return ValidationResult.InvalidLength;

                        case ErrorMessageInvalidPadding:
                            return ValidationResult.InvalidPadding;

                        default:
                            throw;
                    }
                }

                if (padSymbol != null)
                {
                    length -= 8 - remainder; // ignore padding
                }

                if (!CheckAlphabet(encoded, lookupTable))
                    return ValidationResult.InvalidCharacter;

                return ValidationResult.Ok;
            }
            catch
            {
                return ValidationResult.InvalidArguments;
            }
        }

        private static unsafe bool CheckAlphabet(ReadOnlySpan<Char> encoded, LookupTable lookupTable)
        {
            int[] lookupValues = lookupTable.Values;
            int lowCode = lookupTable.LowCode;
            int lookupSize = lookupValues.Length;
            fixed (char* pEncodedBegin = encoded)
            fixed (int* pLookup = lookupValues)
            {
                char* pEncoded = pEncodedBegin;
                char* pEnd = pEncoded + encoded.Length;
                while (pEncoded < pEnd)
                {
                    int lookupIndex = *pEncoded - lowCode;
                    if (lookupIndex < 0 || lookupIndex >= lookupSize)
                        return false;

                    int item = *(pLookup + lookupIndex);
                    if (item == LookupTableNullItem)
                        return false;

                    pEncoded++;
                }
            }

            return true;
        }
    }
}