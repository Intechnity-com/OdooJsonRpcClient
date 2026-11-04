using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PortaCapena.OdooJsonRpcClient.Converters
{
    public class OdooModelConverter : JsonConverter
    {
        /// <summary>
        /// odoo field name -> .NET property, cached per model type.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> PropertiesMapCache = new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

        public override bool CanWrite { get { return false; } }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var propertiesMap = PropertiesMapCache.GetOrAdd(objectType, CreatePropertiesMap);

            var instance = Activator.CreateInstance(objectType);

            JObject jObject = JObject.Load(reader);

            foreach (var keyValuePair in jObject)
            {
                if (!propertiesMap.TryGetValue(keyValuePair.Key, out var property))
                    continue;

                if (OdooModelMapper.ConverOdooPropertyToDotNet(property.PropertyType, keyValuePair.Value, out var result))
                    property.SetValue(instance, result);
            }

            return instance;
        }

        public override bool CanConvert(Type objectType)
        {
            return Attribute.GetCustomAttributes(objectType).Any(v => v is KnownTypeAttribute);
        }

        private static Dictionary<string, PropertyInfo> CreatePropertiesMap(Type objectType)
        {
            var propertiesMap = new Dictionary<string, PropertyInfo>();

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

                if (propertyInfo.CanWrite)
                    propertiesMap.Add(jsonAttribute.PropertyName, propertyInfo);
            }

            return propertiesMap;
        }
    }
}
