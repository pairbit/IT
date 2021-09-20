using IT.Serialization.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IT.Serialization
{
    internal class ContractResolver : DefaultContractResolver
    {
        public JsonFields Fields { get; set; }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IEnumerable<JsonProperty> props = base.CreateProperties(type, memberSerialization);

            if (Fields == JsonFields.Writable)
            {
                props = props.Where(x => x.Writable);
            }

            if (Fields == JsonFields.Data)
            {
                props = props.Where(x => x.AttributeProvider.GetAttributes(typeof(DataAttribute), true).Any() == true);
            }

            if (Fields == JsonFields.MetaDataShorten)
            {
                foreach (var prop in props)
                {
                    var propData = prop.AttributeProvider.GetAttributes(typeof(DataAttribute), true).SingleOrDefault();
                    if (propData != null) prop.Converter = new WriteDataShortenJsonConverter();
                }
            }

            if (Fields == JsonFields.Meta)
            {
                props = props.Where(x => x.AttributeProvider.GetAttributes(typeof(DataAttribute), true).Any() == false);
            }
            return props.ToList();
        }
    }
}