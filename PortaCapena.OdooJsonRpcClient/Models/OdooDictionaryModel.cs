using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Extensions;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="OdooDictionaryModel"]/*'/>
    public class OdooDictionaryModel : Dictionary<string, object>
    {
        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="TableName"]/*'/>
        public string TableName { get; internal set; }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="ctor"]/*'/>
        public OdooDictionaryModel() { }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="ctor.TableName"]/*'/>
        public OdooDictionaryModel(string tableName)
        {
            TableName = tableName;
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Create"]/*'/>
        public static OdooDictionaryModel Create()
        {
            return new OdooDictionaryModel();
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Create.TableName"]/*'/>
        public static OdooDictionaryModel Create(string tableName)
        {
            return new OdooDictionaryModel(tableName);
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Create.Model"]/*'/>
        public static OdooDictionaryModel<T> Create<T>() where T : IOdooAtributtesModel, new()
        {
            return new OdooDictionaryModel<T>();
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Create.Initializer"]/*'/>
        public static OdooDictionaryModel<T> Create<T>(Expression<Func<T>> expression) where T : IOdooAtributtesModel, new()
        {
            return new OdooDictionaryModel<T>().Add(expression);
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Create.Property"]/*'/>
        public static OdooDictionaryModel Create<T>(Expression<Func<T, object>> expression, object value) where T : IOdooAtributtesModel, new()
        {
            return new OdooDictionaryModel<T>().Add(expression, value);
        }

        //TODO: Rename to set / addOrUpdate ?

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Add.Property"]/*'/>
        public OdooDictionaryModel Add<T>(Expression<Func<T, object>> expression, object value) where T : IOdooAtributtesModel
        {
            if (TableName == null && TryGetOdooTableName(expression, out var tableName))
                TableName = tableName;
            this[OdooExpressionMapper.GetOdooPropertyName(expression)] = value;
            return this;
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Add.Initializer"]/*'/>
        public OdooDictionaryModel Add<T>(Expression<Func<T>> expression) where T : IOdooAtributtesModel, new()
        {
            if (TableName == null && TryGetOdooTableName(expression, out var tableName))
                TableName = tableName;

            AddFromExpression(expression);

            return this;
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="AddFromExpression"]/*'/>
        [Obsolete("Use " + nameof(AddFromExpression))]
        protected void AddFromExpresion<T>(Expression<Func<T>> expression) where T : IOdooAtributtesModel, new()
        {
            AddFromExpression(expression);
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="AddFromExpression"]/*'/>
        protected void AddFromExpression<T>(Expression<Func<T>> expression) where T : IOdooAtributtesModel, new()
        {
            if (!(expression.Body is MemberInitExpression body))
                throw new ArgumentException($"Invalid Func - expected object initializer, e.g. () => new {typeof(T).Name} {{ Name = \"value\" }}", nameof(expression));

            foreach (var binding in body.Bindings)
            {
                if (!(binding is MemberAssignment memberAssignment) || !(binding.Member is PropertyInfo property))
                    throw new ArgumentException($"Invalid Func - only property assignments are supported ('{binding}')", nameof(expression));

                var odooName = property.GetCustomAttributes<JsonPropertyAttribute>().FirstOrDefault()?.PropertyName;
                if (odooName == null)
                    throw new ArgumentException($"Invalid Func - property '{typeof(T).Name}.{property.Name}' has no [JsonProperty] attribute with odoo field name", nameof(expression));

                this[odooName] = Evaluate(memberAssignment.Expression);
            }
        }

        private static object Evaluate(Expression expression)
        {
            if (expression is ConstantExpression constantExpression)
                return constantExpression.Value;

            // variables, method calls, conversions, arrays, arithmetic, conditionals...
            return Expression.Lambda(expression).Compile().DynamicInvoke();
        }


        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="TryGetOdooTableName"]/*'/>
        protected static bool TryGetOdooTableName<T>(Expression<Func<T>> expression, out string result)
        {
            result = null;
            var tableNameAttribute = expression.ReturnType.GetCustomAttributes(typeof(OdooTableNameAttribute), true).FirstOrDefault() as OdooTableNameAttribute;
            if (tableNameAttribute == null) return false;

            result = tableNameAttribute.Name;
            return true;
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="TryGetOdooTableName"]/*'/>
        protected static bool TryGetOdooTableName<T>(Expression<Func<T, object>> expression, out string result)
        {
            result = OdooExtensions.TryGetOdooTableName(typeof(T));
            return result != null;
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="TryGetOdooTableName"]/*'/>
        protected static bool TryGetOdooTableName<T>(Expression<Func<T, Enum>> expression, out string result)
        {
            result = OdooExtensions.TryGetOdooTableName(typeof(T));
            return result != null;
        }
    }
}
