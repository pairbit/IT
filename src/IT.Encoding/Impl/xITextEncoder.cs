using System;
using System.Buffers;
using System.Text;

namespace IT.Encoding.Impl;

public static class xITextEncoder
{
    public static OperationStatus EncodeImpl(this ITextEncoder textEncoder, ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal, System.Text.Encoding encoding)
    {
        var encodedLength = textEncoder.GetEncodedLength(data);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(encodedLength);

        Span<Byte> encodedBytes = rented;

        try
        {
            var status = textEncoder.Encode(data, encodedBytes, out consumed, out written, isFinal);

            if (status == OperationStatus.Done || status == OperationStatus.NeedMoreData)
            {
                encoding.GetChars(encodedBytes[..written], encoded);
            }

            return status;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public static String EncodeToTextFromCharsFixLen(this ITextEncoder textEncoder, ReadOnlySpan<Byte> data)
    {
        if (data.IsEmpty) return String.Empty;

        var encodedLength = textEncoder.GetEncodedLength(data);

#if NETSTANDARD2_0
        unsafe
        {
            fixed (byte* dataPtr = data)
            {
                return _String.Create(encodedLength, (Ptr: (IntPtr)dataPtr, data.Length), (encoded, state) =>
                {
                    var data = new ReadOnlySpan<Byte>((Byte*)state.Ptr, state.Length);

                    var status = textEncoder.Encode(data, encoded, out var consumed, out var written);

                    if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

                    if (consumed != data.Length) throw new InvalidOperationException();

                    if (written != encoded.Length) throw new InvalidOperationException();
                });
            }
        }
#else
        unsafe
        {
            fixed (byte* dataPtr = data)
            {
                return String.Create(encodedLength, (Ptr: (IntPtr)dataPtr, data.Length), (encoded, state) =>
                {
                    var data = new ReadOnlySpan<Byte>((Byte*)state.Ptr, state.Length);

                    var status = textEncoder.Encode(data, encoded, out var consumed, out var written);

                    if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

                    if (consumed != data.Length) throw new InvalidOperationException();

                    if (written != encoded.Length) throw new InvalidOperationException();
                });
            }
        }
#endif
    }

    public static String EncodeToTextFromCharsVarLen(this ITextEncoder textEncoder, ReadOnlySpan<Byte> data)
    {
        if (data.IsEmpty) return String.Empty;

        var encodedLength = textEncoder.GetEncodedLength(data);

        var pool = ArrayPool<Char>.Shared;

        var rented = pool.Rent(encodedLength);

        Span<Char> encoded = rented;

        try
        {
            var status = textEncoder.Encode(data, encoded, out var consumed, out var written);

            if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

            if (consumed != data.Length) throw new InvalidOperationException();

#if NETSTANDARD2_0
            encoded = encoded[..written];

            unsafe
            {
                fixed (char* encodedPtr = encoded)
                {
                    return new String(encodedPtr);
                }
            }
#else
            return new String(encoded[..written]);
#endif
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public static String EncodeToTextFromBytes(this ITextEncoder textEncoder, ReadOnlySpan<Byte> data, System.Text.Encoding encoding)
    {
        if (data.IsEmpty) return String.Empty;

        var encodedLength = textEncoder.GetEncodedLength(data);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(encodedLength);

        Span<Byte> encoded = rented;

        try
        {
            var status = textEncoder.Encode(data, encoded, out var consumed, out var written);

            if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

            if (consumed != data.Length) throw new InvalidOperationException();

            if (written != encodedLength) throw new InvalidOperationException();

            return encoding.GetString(encoded[..encodedLength]);
        }
        finally
        {
            pool.Return(rented);
        }
    }
}