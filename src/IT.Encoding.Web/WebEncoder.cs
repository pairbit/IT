using System;
using System.Buffers;
using System.Text.Encodings.Web;

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

    public override Int32 MaxDataLength => _maxDataLengthTo;

    public override Int32 GetMaxEncodedLength(Int32 dataLength)
    {
        var max = (long)dataLength * _textEncoder.MaxOutputCharactersPerInputCharacter;
        return max >= _maxDataLengthTo ? _maxDataLengthTo : (Int32)max;
    }

    public override OperationStatus Encode(ReadOnlySpan<byte> data, Span<byte> encoded, out int consumed, out int written, bool isFinal = true)
        => _textEncoder.EncodeUtf8(data, encoded, out consumed, out written, isFinal);

    public override OperationStatus Encode(ReadOnlySpan<char> data, Span<char> encoded, out int consumed, out int written, bool isFinal = true)
        => _textEncoder.Encode(data, encoded, out consumed, out written, isFinal);

    public override String EncodeToText(ReadOnlySpan<Char> data) => _textEncoder.Encode(data);

    public override String EncodeToText(String data) => _textEncoder.Encode(data);
}