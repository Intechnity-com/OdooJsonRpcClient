using System;
using System.Linq.Expressions;
using PortaCapena.OdooJsonRpcClient.Extensions;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="OdooDictionaryModelOfT"]/*'/>
    public class OdooDictionaryModel<T> : OdooDictionaryModel where T : IOdooAtributtesModel, new()
    {
        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="OfT.ctor"]/*'/>
        public OdooDictionaryModel()
        {
            TableName = OdooExtensions.GetOdooTableName<T>();
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Create.Initializer"]/*'/>
        public static OdooDictionaryModel<T> Create(Expression<Func<T>> expression)
        {
            return new OdooDictionaryModel<T>().Add(expression);
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Create.Property"]/*'/>
        public static OdooDictionaryModel<T> Create(Expression<Func<T, object>> expression, object value)
        {
            return new OdooDictionaryModel<T>().Add(expression, value);
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Add.Property"]/*'/>
        public OdooDictionaryModel<T> Add(Expression<Func<T, object>> expression, object value)
        {
            Add<T>(expression, value);
            return this;
        }

        /// <include file='../Docs/OdooDictionaryModel.xml' path='docs/member[@name="Add.Initializer"]/*'/>
        public OdooDictionaryModel<T> Add(Expression<Func<T>> expression)
        {
            AddFromExpression(expression);
            return this;
        }
    }
}
