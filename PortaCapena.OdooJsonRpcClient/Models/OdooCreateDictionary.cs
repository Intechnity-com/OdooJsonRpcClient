using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooCreateDictionary : Dictionary<string, object>
    {
        public string TableName { get; private set; }

        public OdooCreateDictionary() { }

        public OdooCreateDictionary(string tableName)
        {
            TableName = tableName;
        }

        public static OdooCreateDictionary Create()
        {
            return new OdooCreateDictionary();
        }

        public static OdooCreateDictionary Create(string tableName)
        {
            return new OdooCreateDictionary(tableName);
        }

        public static OdooCreateDictionary Create<T>(Expression<Func<T>> expression) where T : IOdooModel, new()
        {
            if (expression.Body is MemberInitExpression body)
            {
                foreach (var memberExpression in body.Bindings)
                {
                    if (memberExpression is MemberAssignment memberExp && memberExp.Expression is ConstantExpression constantExpression)
                    {
                        var property = (PropertyInfo)memberExpression.Member;
                        var attribute = property.GetCustomAttributes<JsonPropertyAttribute>();

                        var odooName = attribute.FirstOrDefault()?.PropertyName;
                        var value = constantExpression.Value;

                        if (odooName != null)
                        {
                            var result = new OdooCreateDictionary { { odooName, value } };
                            if (TryGetOdooTableName(expression, out var tableName))
                                result.TableName = tableName;

                            return result;
                        }
                    }
                }
            }
            throw new ArgumentException("invalid func");
        }

        public OdooCreateDictionary Add<T>(Expression<Func<T>> expression, object value) where T : IOdooModel
        {
            if (TableName != null && TryGetOdooTableName(expression, out var tableName))
                TableName = tableName;
            Add(OdooExtensions.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooCreateDictionary Add<T>(Expression<Func<T>> expression) where T : IOdooModel, new()
        {
            if (TableName != null && TryGetOdooTableName(expression, out var tableName))
                TableName = tableName;

            if (expression.Body is MemberInitExpression body)
            {
                foreach (var memberExpression in body.Bindings)
                {
                    if (memberExpression is MemberAssignment memberExp && memberExp.Expression is ConstantExpression constantExpression)
                    {
                        var property = (PropertyInfo)memberExpression.Member;
                        var attribute = property.GetCustomAttributes<JsonPropertyAttribute>();

                        var odooName = attribute.FirstOrDefault()?.PropertyName;
                        var value = constantExpression.Value;

                        if (odooName != null)
                        {
                            Add(odooName, value);
                            return this;
                        }
                    }
                }
            }
            throw new ArgumentException("invalid func");
        }


        private static bool TryGetOdooTableName<T>(Expression<Func<T>> expression, out string result)
        {
            result = null;
            var tableNameAttribute = expression.ReturnType.GetCustomAttributes(typeof(OdooTableNameAttribute), true).FirstOrDefault() as OdooTableNameAttribute;
            if (tableNameAttribute == null) return false;

            result = tableNameAttribute.Name;
            return true;
        }
    }
}