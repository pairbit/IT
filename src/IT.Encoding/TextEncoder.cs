using IT.Encoding.Impl;
using System;
using System.Buffers;

namespace IT.Encoding;

public abstract class TextEncoder : Encoder, ITextEncoder
{
    protected readonly System.Text.Encoding _encoding;

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

    #endregion Encoder

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

    public virtual String EncodeToText(ReadOnlySpan<Char> data) => this.EncodeToTextFromCharsVarLen(data);

    public virtual String EncodeToText(String data) => this.EncodeToTextFromCharsVarLen(data.AsSpan());

    #endregion ITextEncoder
}