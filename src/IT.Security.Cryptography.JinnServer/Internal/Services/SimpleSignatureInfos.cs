using IT.Serialization.Converters;
using Newtonsoft.Json;

namespace IT.Security.Cryptography.JinnServer.Internal.Services
{
    internal class SimpleSignatureInfos
    {
        [JsonConverter(typeof(ReadArrayJsonConverter))]
        public SimpleSignatureInfo[] SignatureInfo { get; set; }
    }
}