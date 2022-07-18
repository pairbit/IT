﻿using System;
using System.Buffers;

namespace gfoidl.Base64
{
    /// <summary>
    /// Base64 encoding / decoding.
    /// </summary>
    internal interface IBase64
    {
        /// <summary>
        /// Gets the length of the encoded data.
        /// </summary>
        /// <param name="sourceLength">The length of the data.</param>
        /// <returns>The base64 encoded length of <paramref name="sourceLength" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="sourceLength" /> is greater than <see cref="Base64.MaximumEncodeLength" />.
        /// </exception>
        int GetEncodedLength(int sourceLength);
        //---------------------------------------------------------------------
        /// <summary>
        /// Gets the maximum length of the decoded data.
        /// The result may not be the exact length due to padding.
        /// Use <see cref="GetDecodedLength(ReadOnlySpan{byte})" /> or <see cref="GetDecodedLength(ReadOnlySpan{char})" />
        /// for an accurate length.
        /// </summary>
        /// <param name="encodedLength">The length of the encoded data.</param>
        /// <returns>The maximum base64 decoded length of <paramref name="encodedLength" />.</returns>
        /// <remarks>
        /// This method can be used for buffer-chains, to get the size which is at least
        /// required for decoding.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="encodedLength" /> is negative.
        /// </exception>
        int GetMaxDecodedLength(int encodedLength);
        //---------------------------------------------------------------------
        /// <summary>
        /// Gets the length of the decoded data.
        /// </summary>
        /// <param name="encoded">The encoded data.</param>
        /// <returns>The base64 decoded length of <paramref name="encoded" />. Any padding is handled.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// For <see cref="Base64.Default" /> thrown when the length of <paramref name="encoded" /> is
        /// less than 4, as it is not a valid length according the base64 standard.
        /// </exception>
        int GetDecodedLength(ReadOnlySpan<byte> encoded);
        //---------------------------------------------------------------------
        /// <summary>
        /// Gets the length of the decoded data.
        /// </summary>
        /// <param name="encoded">The encoded data.</param>
        /// <returns>The base64 decoded length of <paramref name="encoded" />. Any padding is handled.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// For <see cref="Base64.Default" /> thrown when the length of <paramref name="encoded" /> is
        /// less than 4, as it is not a valid length according the base64 standard.
        /// </exception>
        int GetDecodedLength(ReadOnlySpan<char> encoded);
        //---------------------------------------------------------------------
        /// <summary>
        /// Base64 encodes <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data to be base64 encoded.</param>
        /// <param name="encoded">The base64 encoded data.</param>
        /// <param name="consumed">
        /// The number of input bytes consumed during the operation. This can be used to slice the input for
        /// subsequent calls, if necessary.
        /// </param>
        /// <param name="written">
        /// The number of bytes written into the output span. This can be used to slice the output for
        /// subsequent calls, if necessary.
        /// </param>
        /// <param name="isFinalBlock">
        /// <c>true</c> (default) when the input span contains the entire data to decode.
        /// Set to <c>false</c> only if it is known that the input span contains partial data with more data to follow.
        /// </param>
        /// <returns>
        /// It returns the OperationStatus enum values:
        /// <list type="bullet">
        /// <item><description>Done - on successful processing of the entire input span</description></item>
        /// <item><description>DestinationTooSmall - if there is not enough space in the output span to fit the decoded input</description></item>
        /// <item><description>
        /// NeedMoreData - only if isFinalBlock is false and the input is not a multiple of 4, otherwise the partial input
        /// would be considered as InvalidData
        /// </description></item>
        /// <item><description>
        /// InvalidData - if the input contains bytes outside of the expected base64 range, or if it contains invalid/more
        /// than two padding characters, or if the input is incomplete (i.e. not a multiple of 4) and isFinalBlock is true.
        /// </description></item>
        /// </list>
        /// </returns>
        OperationStatus Encode(
            ReadOnlySpan<byte> data,
            Span<byte>         encoded,
            out                int consumed,
            out                int written,
            bool               isFinalBlock = true);
        //---------------------------------------------------------------------
        /// <summary>
        /// Base64 encodes <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data to be base64 encoded.</param>
        /// <param name="encoded">The base64 encoded data.</param>
        /// <param name="consumed">
        /// The number of input bytes consumed during the operation. This can be used to slice the input for
        /// subsequent calls, if necessary.
        /// </param>
        /// <param name="written">
        /// The number of chars written into the output span. This can be used to slice the output for
        /// subsequent calls, if necessary.
        /// </param>
        /// <param name="isFinalBlock">
        /// <c>true</c> (default) when the input span contains the entire data to decode.
        /// Set to <c>false</c> only if it is known that the input span contains partial data with more data to follow.
        /// </param>
        /// <returns>
        /// It returns the OperationStatus enum values:
        /// <list type="bullet">
        /// <item><description>Done - on successful processing of the entire input span</description></item>
        /// <item><description>DestinationTooSmall - if there is not enough space in the output span to fit the decoded input</description></item>
        /// <item><description>
        /// NeedMoreData - only if isFinalBlock is false and the input is not a multiple of 4, otherwise the partial input
        /// would be considered as InvalidData
        /// </description></item>
        /// <item><description>
        /// InvalidData - if the input contains bytes outside of the expected base64 range, or if it contains invalid/more
        /// than two padding characters, or if the input is incomplete (i.e. not a multiple of 4) and isFinalBlock is true.
        /// </description></item>
        /// </list>
        /// </returns>
        OperationStatus Encode(
            ReadOnlySpan<byte> data,
            Span<char>         encoded,
            out                int consumed,
            out                int written,
            bool               isFinalBlock = true);
        //---------------------------------------------------------------------
        /// <summary>
        /// Base64 decodes <paramref name="encoded" />.
        /// </summary>
        /// <param name="encoded">The base64 encoded data.</param>
        /// <param name="data">The base64 encoded data to decode.</param>
        /// <param name="consumed">
        /// The number of input bytes consumed during the operation. This can be used to slice the input for
        /// subsequent calls, if necessary.
        /// </param>
        /// <param name="written">
        /// The number of bytes written into the output span. This can be used to slice the output for
        /// subsequent calls, if necessary.
        /// </param>
        /// <param name="isFinalBlock">
        /// <c>true</c> (default) when the input span contains the entire data to decode.
        /// Set to <c>false</c> only if it is known that the input span contains partial data with more data to follow.
        /// </param>
        /// <returns>
        /// It returns the OperationStatus enum values:
        /// <list type="bullet">
        /// <item><description>Done - on successful processing of the entire input span</description></item>
        /// <item><description>DestinationTooSmall - if there is not enough space in the output span to fit the decoded input</description></item>
        /// <item><description>
        /// NeedMoreData - only if isFinalBlock is false and the input is not a multiple of 4, otherwise the partial input
        /// would be considered as InvalidData
        /// </description></item>
        /// <item><description>
        /// InvalidData - if the input contains bytes outside of the expected base64 range, or if it contains invalid/more
        /// than two padding characters, or if the input is incomplete (i.e. not a multiple of 4) and isFinalBlock is true.
        /// </description></item>
        /// </list>
        /// </returns>
        /// <exception cref="FormatException">
        /// Thrown for <see cref="Base64.Url" /> when the length is not conforming the base64Url standard.
        /// <paramref name="isFinalBlock" /> set to <c>false</c> won't throw this exception.
        /// </exception>
        OperationStatus Decode(
            ReadOnlySpan<byte> encoded,
            Span<byte>         data,
            out                int consumed,
            out                int written,
            bool               isFinalBlock = true);
        //---------------------------------------------------------------------
        /// <summary>
        /// Base64 decodes <paramref name="encoded" />.
        /// </summary>
        /// <param name="encoded">The base64 encoded data.</param>
        /// <param name="data">The base64 decoded data.</param>
        /// <param name="consumed">
        /// The number of input chars consumed during the operation. This can be used to slice the input for
        /// subsequent calls, if necessary.
        /// </param>
        /// <param name="written">
        /// The number of bytes written into the output span. This can be used to slice the output for
        /// subsequent calls, if necessary.
        /// </param>
        /// <param name="isFinalBlock">
        /// <c>true</c> (default) when the input span contains the entire data to decode.
        /// Set to <c>false</c> only if it is known that the input span contains partial data with more data to follow.
        /// </param>
        /// <returns>
        /// It returns the OperationStatus enum values:
        /// <list type="bullet">
        /// <item><description>Done - on successful processing of the entire input span</description></item>
        /// <item><description>DestinationTooSmall - if there is not enough space in the output span to fit the decoded input</description></item>
        /// <item><description>
        /// NeedMoreData - only if isFinalBlock is false and the input is not a multiple of 4, otherwise the partial input
        /// would be considered as InvalidData
        /// </description></item>
        /// <item><description>
        /// InvalidData - if the input contains chars outside of the expected base64 range, or if it contains invalid/more
        /// than two padding characters, or if the input is incomplete (i.e. not a multiple of 4) and isFinalBlock is true.
        /// </description></item>
        /// </list>
        /// </returns>
        /// <exception cref="FormatException">
        /// Thrown for <see cref="Base64.Url" /> when the length is not conforming the base64Url standard.
        /// <paramref name="isFinalBlock" /> set to <c>false</c> won't throw this exception.
        /// </exception>
        OperationStatus Decode(
            ReadOnlySpan<char> encoded,
            Span<byte>         data,
            out                int consumed,
            out                int written,
            bool               isFinalBlock = true);
        //---------------------------------------------------------------------
        /// <summary>
        /// Base64 encoded <paramref name="data" /> to a <see cref="string" />.
        /// </summary>
        /// <param name="data">The data to be base64 encoded.</param>
        /// <returns>The base64 encoded <see cref="string" />.</returns>
        /// <remarks>
        /// For base64 encoding on .NET Full or .NET Standard perf-wise it may be
        /// better to use <see cref="Convert.ToBase64String(byte[])" /> if possible.<br />
        /// Please benchmark yourself.
        /// <para>
        /// base64Url or .NET Core is not affected by this.
        /// </para>
        /// </remarks>
        /// <seealso href="https://github.com/gfoidl/Base64/issues/137" />
        string Encode(ReadOnlySpan<byte> data);
        //---------------------------------------------------------------------
        /// <summary>
        /// Base64 decodes <paramref name="encoded" /> into a <see cref="byte" /> array.
        /// </summary>
        /// <param name="encoded">The base64 encoded data in string-form.</param>
        /// <returns>The base64 decoded data.</returns>
        /// <exception cref="FormatException">
        /// The input is not a valid Base64 string as it contains a non-base 64 character,
        /// more than two padding characters, or an illegal character among the padding characters.
        /// </exception>
        /// <remarks>
        /// For base64 decoding on .NET Full or .NET Standard perf-wise it may be
        /// better to use <see cref="Convert.FromBase64String(string)" /> or
        /// <see cref="Convert.FromBase64CharArray(char[], int, int)" /> if possible.<br />
        /// Please benchmark yourself.
        /// <para>
        /// base64Url or .NET Core is not affected by this.
        /// </para>
        /// </remarks>
        /// <seealso href="https://github.com/gfoidl/Base64/issues/137" />
        byte[] Decode(ReadOnlySpan<char> encoded);
    }
}
