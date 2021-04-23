using System;
using System.Linq.Expressions;
using PortaCapena.OdooJsonRpcClient.Extensions;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooDictionaryModel<T> : OdooDictionaryModel where T : IOdooAtributtesModel, new()
    {
        public OdooDictionaryModel()
        {
            TableName = OdooExtensions.GetOdooTableName<T>();
        }

        private OdooDictionaryModel(string tableName)
        {
            TableName = tableName;
        }

        public static OdooDictionaryModel<T> Create(Expression<Func<T>> expression)
        {
            return new OdooDictionaryModel<T>().Add(expression);
        }

        public static OdooDictionaryModel<T> Create(Expression<Func<T, object>> expression, object value)
        {
            return new OdooDictionaryModel<T>().Add(expression, value);
        }
        public static OdooDictionaryModel<T> Create(Expression<Func<T, Enum>> expression, Enum value)
        {
            return new OdooDictionaryModel<T>().Add(expression, value);
        }

        public OdooDictionaryModel<T> Add(Expression<Func<T, object>> expression, object value)
        {
            Add<T>(expression, value);
            return this;
        }

        public OdooDictionaryModel<T> Add(Expression<Func<T, Enum>> expression, Enum value)
        {
            Add<T>(expression, value);
            return this;
        }

        public OdooDictionaryModel<T> Add(Expression<Func<T>> expression, object value)
        {
            Add<T>(expression, value);
            return this;
        }

        public OdooDictionaryModel<T> Add(Expression<Func<T>> expression)
        {
            AddFromExpresion(expression);
            return this;
        }
    }
}