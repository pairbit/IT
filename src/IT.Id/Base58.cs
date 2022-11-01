﻿namespace System;

internal static class Base58
{
    private const int AlphabetLength = 58;
    private const int AlphabetMaxLength = 127;
    private const int reductionFactor = 733; // https://github.com/bitcoin/bitcoin/blob/master/src/base58.cpp#L48

    private static readonly String _alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
    private static readonly char _zeroChar;
    private static readonly byte[] _lookupTable;

    static Base58()
    {
        _zeroChar = _alphabet[0];

        var lookupTable = new byte[AlphabetMaxLength];
        for (short i = 0; i < _alphabet.Length; i++)
        {
            var c = _alphabet[i];

            //Debug.Assert(c < AlphabetMaxLength, $"Alphabet contains character above {AlphabetMaxLength}");

            lookupTable[c] = (byte)(i + 1);
        }

        _lookupTable = lookupTable;
    }

    public static int GetSafeByteCountForDecoding(ReadOnlySpan<char> text)
    {
        int textLen = text.Length;
        return GetSafeByteCountForDecoding(textLen, getPrefixCount(text, textLen, _zeroChar));
    }

    public static int GetSafeByteCountForDecoding(int textLen, int numZeroes)
    {
        //Debug.Assert(textLen >= numZeroes, "Number of zeroes cannot be longer than text length");
        return numZeroes + ((textLen - numZeroes + 1) * reductionFactor / 1000) + 1;
    }

    public static int GetSafeCharCountForEncoding(ReadOnlySpan<byte> bytes)
    {
        int bytesLen = bytes.Length;
        int numZeroes = getZeroCount(bytes, bytesLen);

        return getSafeCharCountForEncoding(bytesLen, numZeroes);
    }

    public static unsafe string Encode(ReadOnlySpan<byte> bytes)
    {
        int bytesLen = bytes.Length;
        if (bytesLen == 0)
        {
            return string.Empty;
        }

        int numZeroes = getZeroCount(bytes, bytesLen);
        int outputLen = getSafeCharCountForEncoding(bytesLen, numZeroes);
        string output = new string('\0', outputLen);

        // 29.70µs (64.9x slower)   | 31.63µs (40.8x slower)
        // 30.93µs (first tryencode impl)
        // 29.36µs (single pass translation/copy + shift over multiply)
        // 31.04µs (70x slower)     | 24.71µs (34.3x slower)
        fixed (byte* inputPtr = bytes)
        fixed (char* outputPtr = output)
        {
            return internalEncode(inputPtr, bytesLen, outputPtr, outputLen, numZeroes, out int length)
                ? output.Substring(0, length)
                : throw new InvalidOperationException("Output buffer with insufficient size generated");
        }
    }

    public static unsafe Span<byte> Decode(ReadOnlySpan<char> text)
    {
        int textLen = text.Length;
        if (textLen == 0)
        {
            return Array.Empty<byte>();
        }

        char zeroChar = _zeroChar;
        int numZeroes = getPrefixCount(text, textLen, zeroChar);
        int outputLen = GetSafeByteCountForDecoding(textLen, numZeroes);
        byte[] output = new byte[outputLen];
        fixed (char* inputPtr = text)
        fixed (byte* outputPtr = output)
        {
#pragma warning disable IDE0046 // Convert to conditional expression - prefer clarity
            if (!internalDecode(
                inputPtr,
                textLen,
                outputPtr,
                outputLen,
                numZeroes))
            {
                throw new InvalidOperationException("Output buffer was too small while decoding Base58");
            }

            return output.AsSpan().Slice(0, 12);
#pragma warning restore IDE0046 // Convert to conditional expression
        }
    }

    public static unsafe bool Encode(ReadOnlySpan<byte> input, Span<char> output, out int numCharsWritten)
    {
        fixed (byte* inputPtr = input)
        fixed (char* outputPtr = output)
        {
            int inputLen = input.Length;
            int numZeroes = getZeroCount(input, inputLen);
            return internalEncode(inputPtr, inputLen, outputPtr, output.Length, numZeroes, out numCharsWritten);
        }
    }

    /// <inheritdoc/>
    public static unsafe bool Decode(ReadOnlySpan<char> input, Span<byte> output)
    {
        int inputLen = input.Length;
        if (inputLen == 0)
        {
            return true;
        }

        int zeroCount = getPrefixCount(input, inputLen, _zeroChar);
        fixed (char* inputPtr = input)
        fixed (byte* outputPtr = output)
        {
            return internalDecode(
                inputPtr,
                input.Length,
                outputPtr,
                output.Length,
                zeroCount);
        }
    }

    private static unsafe bool internalDecode(
        char* inputPtr,
        int inputLen,
        byte* outputPtr,
        int outputLen,
        int numZeroes)
    {
        char* pInputEnd = inputPtr + inputLen;
        char* pInput = inputPtr + numZeroes;
        if (pInput == pInputEnd)
        {
            if (numZeroes > outputLen)
            {
                return false;
            }

            byte* pOutput = outputPtr;
            for (int i = 0; i < numZeroes; i++)
            {
                *pOutput++ = 0;
            }

            return true;
        }

        var table = _lookupTable;
        byte* pOutputEnd = outputPtr + outputLen - 1;
        byte* pMinOutput = pOutputEnd;
        while (pInput != pInputEnd)
        {
            char c = *pInput;
            int carry = table[c] - 1;
            if (carry < 0)
            {
                throw new ArgumentException($"Invalid character: {c}");
            }

            for (byte* pOutput = pOutputEnd; pOutput >= outputPtr; pOutput--)
            {
                carry += 58 * (*pOutput);
                *pOutput = (byte)carry;
                if (pMinOutput > pOutput && carry != 0)
                {
                    pMinOutput = pOutput;
                }

                carry >>= 8;
            }

            pInput++;
        }

        int startIndex = (int)(pMinOutput - numZeroes - outputPtr);
        var numBytesWritten = outputLen - startIndex;
        Buffer.MemoryCopy(outputPtr + startIndex, outputPtr, numBytesWritten, numBytesWritten);
        return true;
    }

    private static unsafe bool internalEncode(
        byte* inputPtr,
        int inputLen,
        char* outputPtr,
        int outputLen,
        int numZeroes,
        out int numCharsWritten)
    {
        if (inputLen == 0)
        {
            numCharsWritten = 0;
            return true;
        }

        fixed (char* alphabetPtr = _alphabet)
        {
            byte* pInput = inputPtr + numZeroes;
            byte* pInputEnd = inputPtr + inputLen;
            char zeroChar = alphabetPtr[0];

            // optimized path for an all zero buffer
            if (pInput == pInputEnd)
            {
                if (outputLen < numZeroes)
                {
                    numCharsWritten = 0;
                    return false;
                }

                for (int i = 0; i < numZeroes; i++)
                {
                    *outputPtr++ = zeroChar;
                }

                numCharsWritten = numZeroes;
                return true;
            }

            int length = 0;
            char* pOutput = outputPtr;
            char* pLastChar = pOutput + outputLen - 1;
            while (pInput != pInputEnd)
            {
                int carry = *pInput;
                int i = 0;
                for (char* pDigit = pLastChar; (carry != 0 || i < length)
                    && pDigit >= outputPtr; pDigit--, i++)
                {
                    carry += *pDigit << 8;
                    carry = Math.DivRem(carry, 58, out int remainder);
                    *pDigit = (char)remainder;
                }

                length = i;
                pInput++;
            }

            var pOutputEnd = pOutput + outputLen;

            // copy the characters to the beginning of the buffer
            // and translate them at the same time. if no copying
            // is needed, this only acts as the translation phase.
            for (char* a = outputPtr + numZeroes, b = pOutputEnd - length;
                b != pOutputEnd;
                a++, b++)
            {
                *a = alphabetPtr[*b];
            }

            // translate the zeroes at the start
            while (pOutput != pOutputEnd)
            {
                char c = *pOutput;
                if (c != '\0')
                {
                    break;
                }

                *pOutput = alphabetPtr[c];
                pOutput++;
            }

            int actualLen = numZeroes + length;

            numCharsWritten = actualLen;
            return true;
        }
    }

    private static unsafe int getZeroCount(ReadOnlySpan<byte> bytes, int bytesLen)
    {
        if (bytesLen == 0)
        {
            return 0;
        }

        int numZeroes = 0;
        fixed (byte* inputPtr = bytes)
        {
            var pInput = inputPtr;
            while (*pInput == 0 && numZeroes < bytesLen)
            {
                numZeroes++;
                pInput++;
            }
        }

        return numZeroes;
    }

    private static unsafe int getPrefixCount(ReadOnlySpan<char> input, int length, char value)
    {
        if (length == 0)
        {
            return 0;
        }

        int numZeroes = 0;
        fixed (char* inputPtr = input)
        {
            var pInput = inputPtr;
            while (*pInput == value && numZeroes < length)
            {
                numZeroes++;
                pInput++;
            }
        }

        return numZeroes;
    }

    private static int getSafeCharCountForEncoding(int bytesLen, int numZeroes)
    {
        const int growthPercentage = 138;

        return numZeroes + ((bytesLen - numZeroes) * growthPercentage / 100) + 1;
    }
}