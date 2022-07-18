namespace gfoidl.Base64.Internal
{
    internal sealed partial class Base64Encoder : Base64EncoderImpl
    {
        internal const byte EncodingPad = (byte)'=';     // '=', for padding
    }
}
