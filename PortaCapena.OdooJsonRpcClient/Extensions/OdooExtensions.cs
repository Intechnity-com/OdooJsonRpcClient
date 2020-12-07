using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Result;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Extensions
{
    public static class OdooExtensions
    {
        public static string GetOdooTableName<T>() where T : IOdooAtributtesModel
        {
            var attribute = typeof(T).GetCustomAttributes(typeof(OdooTableNameAttribute), true).FirstOrDefault() as OdooTableNameAttribute;
            if (attribute == null)
                throw new ArgumentException($"Mising attribute '{nameof(OdooTableNameAttribute)}' for model '{typeof(T).Name}'");
            return attribute.Name;
        }
        public static string OdooTableName(this IOdooAtributtesModel model)
        {
            var attribute = model.GetType().GetCustomAttributes(typeof(OdooTableNameAttribute), true).FirstOrDefault() as OdooTableNameAttribute;
            if (attribute == null)
                throw new ArgumentException($"Mising attribute '{nameof(OdooTableNameAttribute)}' for model '{model.GetType().Name}'");
            return attribute.Name;
        }

        public static string GetOdooPropertyName<T>(string name) where T : IOdooAtributtesModel
        {
            var property = typeof(T).GetProperties().First(x => string.Equals(x.Name, name));
            var attribute = property.GetCustomAttributes<JsonPropertyAttribute>();

            return attribute.FirstOrDefault()?.PropertyName;
        }
        public static string OdooPropertyName(this IOdooAtributtesModel model, string name)
        {
            var key = model.GetType().GetProperties().First(x => x.Name == name).GetCustomAttributes<JsonPropertyAttribute>().FirstOrDefault();
            if (key == null)
                throw new ArgumentException($"Mising attribute '{nameof(JsonPropertyAttribute)}' for property '{name}' in model '{model.GetType().Name}'");

            return key.PropertyName;
        }

        internal static string GetOdooPropertyName<T>(Expression<Func<T, object>> expression) where T : IOdooAtributtesModel
        {
            return GetOdooPropertyName<T>(GetPropertyName(expression));
        }

        internal static string GetOdooPropertyName<T>(Expression<Func<T>> expression) where T : IOdooAtributtesModel
        {
            return GetOdooPropertyName<T>(GetPropertyName(expression));
        }
        internal static string GetPropertyName<T>(Expression<Func<T>> expression) where T : IOdooAtributtesModel
        {
            if (expression.Body is MemberExpression body)
                return body.Member.Name;

            if (expression.Body is UnaryExpression unar && unar.Operand is MemberExpression member)
                return member.Member.Name;

            return null;
        }
        internal static string GetPropertyName<T>(Expression<Func<T, object>> expression) where T : IOdooAtributtesModel
        {
            if (expression.Body is MemberExpression body)
                return body.Member.Name;

            if (expression.Body is UnaryExpression unar && unar.Operand is MemberExpression member)
                return member.Member.Name;

            return null;
        }

        public static OdooResult<TResult> ToResult<T, TResult>(this OdooResult<T> result, TResult newValue)
        {
            return new OdooResult<TResult> { Id = result.Id, Jsonrpc = result.Jsonrpc, Value = newValue };
        }

        public static string ToOdooDateTimeString(this DateTime date)
        {
            return date.ToString(OdooConsts.DateTimeFormat);
        }
        public static string ToOdooDateString(this DateTime date)
        {
            return date.ToString(OdooConsts.DateFormat);
        }
    }
}