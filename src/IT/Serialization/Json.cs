using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace IT.Serialization
{
    public static class Json
    {
        public static String MIME { get; } = "application/json";

        public static JsonSerializerSettings Settings { get; set; } = Init();

        public static JsonSerializerSettings GetSettings(JsonFields fields) => AllSettings[fields];

        internal static Dictionary<JsonFields, JsonSerializerSettings> AllSettings { get; } = new Dictionary<JsonFields, JsonSerializerSettings>()
        {
            { JsonFields.All, Init(fields: JsonFields.All) },
            { JsonFields.Data, Init(fields: JsonFields.Data) },
            { JsonFields.Meta, Init(fields: JsonFields.Meta) },
            { JsonFields.MetaDataShorten, Init(fields: JsonFields.MetaDataShorten) },
            { JsonFields.Writable, Init(fields: JsonFields.Writable) },
        };

        public static Formatting Formatting
        {
            get
            {
#if DEBUG
                return Formatting.Indented;
#else
                return Formatting.None;
#endif
            }
        }

        public static JsonSerializerSettings Init(JsonSerializerSettings settings = null, JsonFields fields = JsonFields.All)
        {
            if (settings == null) settings = new JsonSerializerSettings();
            var namingStrategy = GetNamingStrategy();

            settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = Formatting;
            settings.ContractResolver = new ContractResolver
            {
                Fields = fields,
                NamingStrategy = namingStrategy
            };
            settings.Converters.Add(new StringEnumConverter(namingStrategy));

            return settings;
        }

        public static void InitDefaultSettings()
        {
            JsonConvert.DefaultSettings = () => Settings;
        }

        public static String Serialize(Object obj, JsonFields fields, Formatting? formatting = null)
        {
            return Serialize(obj, formatting, GetSettings(fields));
        }

        public static String Serialize(Object obj, Formatting? formatting = null, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(obj, formatting ?? Formatting, settings ?? Settings);
        }

        public static Object Deserialize(String value, Type type = null, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject(value, type, settings);
        }

        public static T Deserialize<T>(String value, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(value, settings ?? Settings);
        }

        private static NamingStrategy GetNamingStrategy()
        {
            //new DefaultNamingStrategy(),  //FieldName -> FieldName
            //new CamelCaseNamingStrategy(),//FieldName -> fieldName
            //new SnakeCaseNamingStrategy(),//FieldName -> field_Name
            //new KebabCaseNamingStrategy(),//FieldName -> field-Name
            return new CamelCaseNamingStrategy();
        }
    }
}