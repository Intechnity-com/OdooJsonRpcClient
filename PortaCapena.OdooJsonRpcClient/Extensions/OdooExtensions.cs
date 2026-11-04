using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Result;

namespace PortaCapena.OdooJsonRpcClient.Extensions
{
    public static class OdooExtensions
    {
        private static readonly ConcurrentDictionary<Type, string> TableNameCache = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<(Type, string), string> PropertyNameCache = new ConcurrentDictionary<(Type, string), string>();

        public static string GetOdooTableName<T>() where T : IOdooAtributtesModel
        {
            return GetOdooTableName(typeof(T));
        }

        internal static string GetOdooTableName(Type type)
        {
            var tableName = TryGetOdooTableName(type);
            if (tableName == null)
                throw new ArgumentException($"Missing attribute '{nameof(OdooTableNameAttribute)}' for model '{type.Name}'");
            return tableName;
        }

        /// <summary>
        /// Returns value of <see cref="OdooTableNameAttribute"/> or null when type has no such attribute.
        /// </summary>
        internal static string TryGetOdooTableName(Type type)
        {
            return TableNameCache.GetOrAdd(type, t =>
                (t.GetCustomAttributes(typeof(OdooTableNameAttribute), true).FirstOrDefault() as OdooTableNameAttribute)?.Name);
        }

        /// <summary>
        /// Returns odoo field name (value of <see cref="JsonPropertyAttribute"/>) for .NET property name,
        /// or null when the property has no such attribute.
        /// </summary>
        public static string GetOdooPropertyName<T>(string name) where T : IOdooAtributtesModel
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            return PropertyNameCache.GetOrAdd((typeof(T), name), key =>
            {
                var property = key.Item1.GetProperty(key.Item2, BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    throw new ArgumentException($"Property '{key.Item2}' not found in model '{key.Item1.Name}'", nameof(name));

                return property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
            });
        }

        public static OdooResult<TResult> ToResult<T, TResult>(this OdooResult<T> result, TResult newValue)
        {
            return new OdooResult<TResult> { Id = result.Id, Jsonrpc = result.Jsonrpc, Value = newValue };
        }

        public static string ToOdooDateTimeString(this DateTime date)
        {
            return date.ToString(OdooConsts.DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        }
        public static string ToOdooDateString(this DateTime date)
        {
            return date.ToString(OdooConsts.DateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
