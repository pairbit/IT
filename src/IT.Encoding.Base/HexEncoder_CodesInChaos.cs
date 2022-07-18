using System;

namespace IT.Encoding.Base;

public class HexEncoder_CodesInChaos : HexEncoder_HexMate
{
    public HexEncoder_CodesInChaos(Boolean isLower = false) : base(isLower) { }

    public override String Encode(ReadOnlySpan<Byte> data)
        => _options == HexMate.HexFormattingOptions.Lowercase ? CodesInChaos.Hex.EncodeLower(data) : CodesInChaos.Hex.EncodeUpper(data);
}