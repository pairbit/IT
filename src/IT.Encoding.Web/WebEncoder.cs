using System;
using System.Buffers;

namespace IT.Encoding.Web;

public class WebEncoder : TextEncoder
{
    private const Int32 _maxDataLengthTo = (1024 * 1024 * 1024) - 33;//1 073 741 791
    private readonly Int32 _maxDataLengthFrom;
    private readonly System.Text.Encodings.Web.TextEncoder _textEncoder;

    public WebEncoder(System.Text.Encodings.Web.TextEncoder textEncoder)
    {
        _textEncoder = textEncoder;
        _maxDataLengthFrom = (_maxDataLengthTo / _textEncoder.MaxOutputCharactersPerInputCharacter) - 90;
    }

    public override Int32 MaxDataLength => _maxDataLengthFrom;

    public override Int32 GetMaxEncodedLength(Int32 dataLength) => dataLength * _textEncoder.MaxOutputCharactersPerInputCharacter;

    public override OperationStatus Encode(ReadOnlySpan<byte> data, Span<byte> encoded, out int consumed, out int written, bool isFinal = true)
        => _textEncoder.EncodeUtf8(data, encoded, out consumed, out written, isFinal);

    public override OperationStatus Encode(ReadOnlySpan<char> data, Span<char> encoded, out int consumed, out int written, bool isFinal = true)
        => _textEncoder.Encode(data, encoded, out consumed, out written, isFinal);
}