using IT.Ext;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace IT.Serialization.Converters
{
    internal class WriteDataShortenJsonConverter : BaseJsonConverter
    {
        public Int32 MaxLength { get; } = 100;

        public override Boolean CanRead => false;

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            if (value is Byte[] bytes && bytes.Length > MaxLength)
            {
                value = bytes.Take(MaxLength).ToArray().ToBase64();
            }
            if (value is String str && str.Length > MaxLength)
            {
                value = str.Shorten(MaxLength, true);
            }
            base.WriteJson(writer, value, serializer);
        }
    }
}