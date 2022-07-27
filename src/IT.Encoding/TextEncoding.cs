using IT.Encoding.Internal;
using System;
using System.Buffers;

namespace IT.Encoding;

public abstract class TextEncoding : Encoding, ITextEncoding
{
    protected readonly System.Text.Encoding _encoding;

    public TextEncoding()
    {
        _encoding = System.Text.Encoding.ASCII;
    }

    public TextEncoding(System.Text.Encoding encoding)
    {
        _encoding = encoding;
    }

    #region Encoding

    public override OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        throw new NotImplementedException();
    }

    public override OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        throw new NotImplementedException();
    }

    #endregion Encoding

    #region ITextEncoder

    public virtual Int32 GetEncodedLength(ReadOnlySpan<Char> data) => GetMaxEncodedLength(data.Length);

    public virtual OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => this.EncodeImpl(data, encoded, out consumed, out written, isFinal, _encoding);

    public virtual OperationStatus Encode(ReadOnlySpan<Char> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => throw new NotImplementedException();

    public virtual OperationStatus Encode(ReadOnlySpan<Char> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => throw new NotImplementedException();

    public virtual Byte[] Encode(ReadOnlySpan<Char> data) => throw new NotImplementedException();

    public virtual String EncodeToText(ReadOnlySpan<Byte> data) => this.EncodeToTextFromCharsFixLen(data);

    public virtual String EncodeToText(ReadOnlySpan<Char> data) => throw new NotImplementedException();

    #endregion ITextEncoder

    #region ITextDecoder

    public virtual Int32 GetDecodedLength(ReadOnlySpan<Char> encoded) => GetMaxDecodedLength(encoded.Length);

    public virtual OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => this.DecodeImpl(encoded, data, out consumed, out written, isFinal, _encoding);

    public virtual OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Char> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => throw new NotImplementedException();

    public virtual OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Char> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => throw new NotImplementedException();

    public virtual Byte[] Decode(ReadOnlySpan<Char> encoded) => this.DecodeImpl(encoded);

    public virtual String DecodeToText(ReadOnlySpan<Byte> encoded) => throw new NotImplementedException();

    public virtual String DecodeToText(ReadOnlySpan<Char> encoded) => throw new NotImplementedException();

    #endregion ITextDecoder
}