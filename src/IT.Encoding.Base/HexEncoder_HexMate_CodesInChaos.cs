using System;

namespace IT.Encoding.Base;

public class HexEncoder_HexMate_CodesInChaos : HexEncoder_HexMate
{
    public HexEncoder_HexMate_CodesInChaos(Boolean isLower = false) : base(isLower) { }

    public override String EncodeToText(ReadOnlySpan<Byte> data)
    {
        var len = data.Length;
        if (len < 32)
        {
            return _options == HexMate.HexFormattingOptions.Lowercase
                ? CodesInChaos.Hex.EncodeLower(data)
                : CodesInChaos.Hex.EncodeUpper(data);
        }
        return base.EncodeToText(data);
    }
}