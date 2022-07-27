using deniszykov.BaseN;
using System;
using System.Buffers;
using System.Text;

namespace IT.Encoding.Base;

public class Base32Encoder_deniszykov : TextEncoding
{
    private readonly BaseNEncoding _baseNEncoding;

    public Base32Encoder_deniszykov(Boolean isZ32 = false)
    {
        _baseNEncoding = isZ32 ? BaseNEncoding.ZBase32 : BaseNEncoding.Base32;
    }

    #region Encoder

    private const Int32 _MaxDataLength = Int32.MaxValue / 8 * 5; // 1 342 177 275

    public override Int32 MaxDataLength => _MaxDataLength;

    public override Int32 GetMaxEncodedLength(Int32 dataLength) => _baseNEncoding.GetMaxCharCount(dataLength);

    public override Int32 GetMaxDecodedLength(Int32 encodedLength) => _baseNEncoding.GetMaxByteCount(encodedLength);

    #endregion Encoder

    public override String EncodeToText(ReadOnlySpan<Byte> data) => _baseNEncoding.GetString(data);

    public override OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        written = _baseNEncoding.GetBytes(encoded, data);
        consumed = encoded.Length;
        
        return OperationStatus.Done;
    }
}