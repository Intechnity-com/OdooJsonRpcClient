using System;
using System.Linq.Expressions;
using PortaCapena.OdooJsonRpcClient.Extensions;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooCreateDictionary<T> : OdooCreateDictionary where T : IOdooAtributtesModel, new()
    {
        public OdooCreateDictionary()
        {
            TableName = OdooExtensions.GetOdooTableName<T>();
        }

        private OdooCreateDictionary(string tableName)
        {
            TableName = tableName;
        }

        public static OdooCreateDictionary<T> Create(Expression<Func<T>> expression)
        {
            return new OdooCreateDictionary<T>().Add(expression);
        }

        public static OdooCreateDictionary<T> Create(Expression<Func<T, object>> expression, object value)
        {
            return new OdooCreateDictionary<T>().Add(expression, value);
        }
        public static OdooCreateDictionary<T> Create(Expression<Func<T, Enum>> expression, Enum value)
        {
            return new OdooCreateDictionary<T>().Add(expression, value);
        }

        public OdooCreateDictionary<T> Add(Expression<Func<T, object>> expression, object value)
        {
            Add<T>(expression, value);
            return this;
        }

        public OdooCreateDictionary<T> Add(Expression<Func<T, Enum>> expression, Enum value)
        {
            Add<T>(expression, value);
            return this;
        }

        public OdooCreateDictionary<T> Add(Expression<Func<T>> expression, object value)
        {
            Add<T>(expression, value);
            return this;
        }

        public OdooCreateDictionary<T> Add(Expression<Func<T>> expression)
        {
            AddFromExpresion(expression);
            return this;
        }
    }
}