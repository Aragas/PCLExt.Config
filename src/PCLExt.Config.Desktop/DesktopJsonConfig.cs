using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;


namespace PCLExt.Config
{
    internal class DesktopJsonConfig : IConfig
    {
        private JsonSerializerSettings Settings { get; }


        public DesktopJsonConfig()
        {
            Settings = new JsonSerializerSettings
            {
                ContractResolver = new ConfigIgnoreContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new JsonConverter[] { new StringEnumConverter() }
            };
        }

        public string Serialize<T>(T target)
        {
            try { return JsonConvert.SerializeObject(target, Settings); }
            catch (JsonWriterException ex) { throw new ConfigSerializingException(string.Empty, ex); }
        }
        public T Deserialize<T>(string value)
        {
            try { return JsonConvert.DeserializeObject<T>(value, Settings); }
            catch (JsonReaderException ex) { throw new ConfigDeserializingException(string.Empty, ex); }
        }
        public void PopulateObject<T>(string value, T target)
        {
            try { JsonConvert.PopulateObject(value, target, Settings); }
            catch (JsonReaderException ex) { throw new ConfigDeserializingException(string.Empty, ex); }
        }
    }

    internal class ConfigIgnoreContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) =>
            type.GetProperties()
                .Where(p => p.GetCustomAttribute<ConfigIgnoreAttribute>() == null)
                .Select(p =>
                {
                    var descriptor = new JsonProperty()
                    {
                        PropertyName = p.Name,
                        PropertyType = p.PropertyType,
                        Readable = true,
                        Writable = true,
                        ValueProvider = CreateMemberValueProvider(p)
                    };

                    var member = p.GetCustomAttribute<ConfigNameAttribute>();
                    if (member != null)
                    {
                        if (!string.IsNullOrEmpty(member.Name))
                            descriptor.PropertyName = member.Name;
                    }

                    return descriptor;
                }).ToList();
    }
}
