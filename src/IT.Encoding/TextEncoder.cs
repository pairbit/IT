using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;

namespace IT.Encoding;

public abstract class TextEncoder : Encoder, ITextEncoder
{
    private readonly System.Text.Encoding _encoding;

    public TextEncoder()
    {
        _encoding = System.Text.Encoding.ASCII;
    }

    public TextEncoder(System.Text.Encoding encoding)
    {
        _encoding = encoding;
    }

    #region Encoder

    public override OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        throw new NotImplementedException();
    }

    public override OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        throw new NotImplementedException();
    }

    #endregion Encoder

    public virtual Int32 GetEncodedLength(ReadOnlySpan<Char> data)
        => GetMaxEncodedLength(data.Length);

    public virtual Int32 GetDecodedLength(ReadOnlySpan<Char> encoded)
        => GetMaxDecodedLength(encoded.Length);

    #region ByteToText

    public virtual OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        var encodedLength = GetEncodedLength(data);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(encodedLength);

        Span<Byte> encodedBytes = rented;

        try
        {
            var status = Encode(data, encodedBytes, out consumed, out written, isFinal);

            if (status == OperationStatus.Done || status == OperationStatus.NeedMoreData)
            {
                _encoding.GetChars(encodedBytes[..written], encoded);
            }

            return status;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public virtual OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        var encodedLength = encoded.Length;

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(encodedLength);

        Span<Byte> encodedBytes = rented;

        try
        {
            var count = _encoding.GetBytes(encoded, encodedBytes);

            if (count != encodedLength) throw new InvalidOperationException();

            return Decode(encodedBytes[..encodedLength], data, out consumed, out written);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public virtual String EncodeToText(ReadOnlySpan<Byte> data) => EncodeToTextFromChars(data);

    private String EncodeToTextFromChars(ReadOnlySpan<Byte> data)
    {
        var encodedLength = GetEncodedLength(data);

#if NETSTANDARD2_0
        unsafe
        {
            fixed (byte* dataPtr = data)
            {
                return _String.Create(encodedLength, (Ptr: (IntPtr)dataPtr, data.Length), (encoded, state) =>
                {
                    var data = new ReadOnlySpan<Byte>((Byte*)state.Ptr, state.Length);

                    var status = Encode(data, encoded, out var consumed, out var written);

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

                    var status = Encode(data, encoded, out var consumed, out var written);

                    if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

                    if (consumed != data.Length) throw new InvalidOperationException();

                    if (written != encoded.Length) throw new InvalidOperationException();
                });
            }
        }
#endif
    }

    private String EncodeToTextFromBytes(ReadOnlySpan<Byte> data)
    {
        var encodedLength = GetEncodedLength(data);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(encodedLength);

        Span<Byte> encoded = rented;

        try
        {
            var status = Encode(data, encoded, out var consumed, out var written);

            if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

            if (consumed != data.Length) throw new InvalidOperationException();

            if (written != encodedLength) throw new InvalidOperationException();

            return _encoding.GetString(encoded[..encodedLength]);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public virtual Byte[] Decode(ReadOnlySpan<Char> encoded)
    {
        if (encoded.IsEmpty) return Array.Empty<Byte>();

        var decodedLength = GetDecodedLength(encoded);

#if NET6_0
        var dataArray = GC.AllocateUninitializedArray<Byte>(decodedLength);
#else
        var dataArray = new Byte[decodedLength];
#endif

        var data = dataArray.AsSpan();

        var status = Decode(encoded, data, out var consumed, out var written);

        if (status != OperationStatus.Done) throw new InvalidOperationException(status.ToString());

        if (consumed != encoded.Length) throw new InvalidOperationException($"(consumed == {consumed}) != (encoded.Length == {encoded.Length})");

        return written < data.Length ? data[..written].ToArray() : dataArray;
    }

    #endregion ByteToText

    #region TextToByte

    public virtual OperationStatus Encode(ReadOnlySpan<Char> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        throw new NotImplementedException();
    }

    public virtual OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Char> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        throw new NotImplementedException();
    }

    public virtual Byte[] Encode(ReadOnlySpan<Char> data)
    {
        throw new NotImplementedException();
    }

    public virtual String DecodeToText(ReadOnlySpan<Byte> encoded)
    {
        throw new NotImplementedException();
    }

    #endregion TextToByte

    #region TextToText

    public virtual OperationStatus Encode(ReadOnlySpan<Char> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        throw new NotImplementedException();
    }

    public virtual OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Char> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        throw new NotImplementedException();
    }

    public virtual String EncodeToText(ReadOnlySpan<Char> data)
    {
        throw new NotImplementedException();
    }

    public virtual String DecodeToText(ReadOnlySpan<Char> encoded)
    {
        throw new NotImplementedException();
    }

    #endregion TextToText
}