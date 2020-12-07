using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Converters
{
    public class OdooModelConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var propertiesMap = objectType.GetProperties().Select(p =>
            {
                var jsonAttribute = Attribute.GetCustomAttributes(p).FirstOrDefault(x => x is JsonPropertyAttribute) as JsonPropertyAttribute;
                if (jsonAttribute == null)
                    throw new ArgumentException($"Mising attribute '{nameof(JsonPropertyAttribute)}' for property '{p.Name}' in model '{objectType.Name}'");

                return new KeyValuePair<string, string>(jsonAttribute.PropertyName, p.Name);
            }).ToDictionary(x => x.Key, x => x.Value);

            var instance = Activator.CreateInstance(objectType);

            JObject jObject = JObject.Load(reader);

            foreach (var keyValuePair in jObject)
            {
                if (!propertiesMap.TryGetValue(keyValuePair.Key, out var dotNetName))
                    continue;

                var dotNetType = objectType.GetProperty(dotNetName);

                if (OdooModelMapper.ConverOdooPropertyToDotNet(dotNetType.PropertyType, keyValuePair.Value, out var result))
                    dotNetType.SetValue(instance, result);
            }

            return instance;
        }

        public override bool CanConvert(Type objectType)
        {
            return System.Attribute.GetCustomAttributes(objectType).Any(v => v is KnownTypeAttribute);
        }
    }
}