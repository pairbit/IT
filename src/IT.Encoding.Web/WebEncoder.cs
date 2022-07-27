using System;
using System.Buffers;

namespace IT.Encoding.Web;

public class WebEncoder : TextEncoder
{
    private readonly System.Text.Encodings.Web.TextEncoder _textEncoder;

    public WebEncoder(System.Text.Encodings.Web.TextEncoder textEncoder)
    {
        _textEncoder = textEncoder;
    }

    public override int MaxDataLength => throw new NotImplementedException();

    public override int GetMaxEncodedLength(int dataLength)
    {
        throw new NotImplementedException();
    }

    public override OperationStatus Encode(ReadOnlySpan<byte> data, Span<byte> encoded, out int consumed, out int written, bool isFinal = true)
        => _textEncoder.EncodeUtf8(data, encoded, out consumed, out written, isFinal);

    public override OperationStatus Encode(ReadOnlySpan<char> data, Span<char> encoded, out int consumed, out int written, bool isFinal = true)
        => _textEncoder.Encode(data, encoded, out consumed, out written, isFinal);
}