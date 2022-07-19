using System;
using System.Buffers;

namespace IT.Encoding.Base;

/// <summary>
/// https://ndportmann.com/breaking-records-with-core-3-0/
/// </summary>
public class HexEncoder_HexMate : TextEncoder
{
    private const Int32 _MaxDataLength = int.MaxValue / 2;

    internal HexMate.HexFormattingOptions _options;

    public HexEncoder_HexMate(Boolean isLower = false, Boolean insertLineBreaks = false)
    {
        if (isLower) _options |= HexMate.HexFormattingOptions.Lowercase;
        if (insertLineBreaks) _options |= HexMate.HexFormattingOptions.InsertLineBreaks;
    }

    #region Encoder

    public override Int32 MaxDataLength => _MaxDataLength;

    public override Int32 GetMaxEncodedLength(Int32 dataLength) => dataLength * 2;

    public override Int32 GetMaxDecodedLength(Int32 encodedLength) => encodedLength / 2;

    public override OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Byte> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => HexMate.Hex.EncodeToUtf8(data, encoded, out consumed, out written, isFinal);

    public override OperationStatus Decode(ReadOnlySpan<Byte> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
        => HexMate.Hex.DecodeFromUtf8(encoded, data, out consumed, out written, isFinal);

    public override OperationStatus Encode(Span<Byte> buffer, Int32 dataLength, out Int32 written)
        => HexMate.Hex.EncodeToUtf8InPlace(buffer, dataLength, out written);

    public override OperationStatus Decode(Span<Byte> buffer, out Int32 written)
        => HexMate.Hex.DecodeFromUtf8InPlace(buffer, out written);

    #endregion Encoder

    #region TextEncoder

    public override OperationStatus Encode(ReadOnlySpan<Byte> data, Span<Char> encoded, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        //consumed = data.Length;
        //var status = HexMate.Convert.TryToHexChars(data, encoded, out written);
        throw new NotImplementedException();
    }

    public override OperationStatus Decode(ReadOnlySpan<Char> encoded, Span<Byte> data, out Int32 consumed, out Int32 written, Boolean isFinal = true)
    {
        throw new NotImplementedException();
    }

    public override String EncodeToText(ReadOnlySpan<Byte> data) => HexMate.Convert.ToHexString(data, _options);

    public override Byte[] Decode(ReadOnlySpan<Char> encoded) => HexMate.Convert.FromHexString(encoded);

    #endregion
}