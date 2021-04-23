using System;
using System.Linq.Expressions;
using PortaCapena.OdooJsonRpcClient.Extensions;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooCommandModel<T> : OdooCommandModel where T : IOdooAtributtesModel, new()
    {
        public OdooCommandModel()
        {
            TableName = OdooExtensions.GetOdooTableName<T>();
        }

        private OdooCommandModel(string tableName)
        {
            TableName = tableName;
        }

        public static OdooCommandModel<T> Create(Expression<Func<T>> expression)
        {
            return new OdooCommandModel<T>().Add(expression);
        }

        public static OdooCommandModel<T> Create(Expression<Func<T, object>> expression, object value)
        {
            return new OdooCommandModel<T>().Add(expression, value);
        }
        public static OdooCommandModel<T> Create(Expression<Func<T, Enum>> expression, Enum value)
        {
            return new OdooCommandModel<T>().Add(expression, value);
        }

        public OdooCommandModel<T> Add(Expression<Func<T, object>> expression, object value)
        {
            Add<T>(expression, value);
            return this;
        }

        public OdooCommandModel<T> Add(Expression<Func<T, Enum>> expression, Enum value)
        {
            Add<T>(expression, value);
            return this;
        }

        public OdooCommandModel<T> Add(Expression<Func<T>> expression, object value)
        {
            Add<T>(expression, value);
            return this;
        }

        public OdooCommandModel<T> Add(Expression<Func<T>> expression)
        {
            AddFromExpresion(expression);
            return this;
        }
    }
}