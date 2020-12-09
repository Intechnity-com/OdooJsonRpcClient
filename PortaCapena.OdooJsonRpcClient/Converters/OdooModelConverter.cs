using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PortaCapena.OdooJsonRpcClient.Converters
{
    public class OdooModelConverter : JsonConverter
    {
        public override bool CanWrite { get { return false; } }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var propertiesMap = new Dictionary<string, string>();

            foreach (var propertyInfo in objectType.GetProperties())
            {
                var atributes = Attribute.GetCustomAttributes(propertyInfo);
                var jsonAttribute = atributes.FirstOrDefault(x => x is JsonPropertyAttribute) as JsonPropertyAttribute;
                if (jsonAttribute == null)
                {
                    if (atributes.Any(x => x is JsonIgnoreAttribute))
                        continue;
                    throw new ArgumentException($"Mising attribute '{nameof(JsonPropertyAttribute)}' for property '{propertyInfo.Name}' in model '{objectType.Name}'");
                }
                propertiesMap.Add(jsonAttribute.PropertyName, propertyInfo.Name);
            }

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
            return Attribute.GetCustomAttributes(objectType).Any(v => v is KnownTypeAttribute);
        }
    }
}