using IT.Encoding.Impl;
using System;
using System.Buffers;

namespace IT.Encoding;

public abstract class TextDecoder : Decoder, ITextDecoder
{
    protected readonly System.Text.Encoding _encoding;

    public TextDecoder()
    {
        _encoding = System.Text.Encoding.ASCII;
    }

    public TextDecoder(System.Text.Encoding encoding)
    {
        _encoding = encoding;
    }

    #region Decoder

    public override OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        throw new NotImplementedException();
    }

    #endregion Decoder

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

    public virtual String DecodeToText(String encoded) => throw new NotImplementedException();

    #endregion ITextDecoder
}