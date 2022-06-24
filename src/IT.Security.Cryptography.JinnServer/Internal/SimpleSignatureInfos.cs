using Newtonsoft.Json;

namespace IT.Security.Cryptography.JinnServer.Internal;

using Converters;

internal class SimpleSignatureInfos
{
    [JsonConverter(typeof(ReadArrayJsonConverter))]
    public SimpleSignatureInfo[] SignatureInfo { get; set; }
}