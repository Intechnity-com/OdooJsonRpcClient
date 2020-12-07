using System;
using System.Linq.Expressions;
using System.Reflection;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient
{
    public class OdooQueryBuilder<T> where T : IOdooModel, new()
    {
        private readonly OdooQuery _query;

        protected OdooQueryBuilder()
        {
            _query = new OdooQuery();
        }

        public static OdooQueryBuilder<T> Create()
        {
            return new OdooQueryBuilder<T>();
        }

        public OdooQueryBuilder<T> Select(Func<T, object> selector)
        {
            var model = selector.Invoke(new T());
            PropertyInfo[] modelProps = model.GetType().GetProperties();

            foreach (var propertyInfo in modelProps)
            {
                var key = new T().OdooPropertyName(propertyInfo.Name);
                _query.ReturnFields.Add(key);
            }
            return this;
        }

        #region Where
        public OdooQueryBuilder<T> Where(OdooFilter filter)
        {
            _query.Filters.Add(filter);
            return this;
        }
        public OdooQueryBuilder<T> Where(Expression<Func<T, object>> expression, OdooOperator odooOperator, object value)
        {
            Where(OdooExtensions.GetPropertyName(expression), odooOperator, value);
            return this;
        }
        public OdooQueryBuilder<T> Where(string propertyname, OdooOperator odooOperator, object value)
        {
            _query.Filters.Add(OdooFilterMapper.ToOdooExpresion<T>(propertyname, odooOperator, value));
            return this;
        }
        public OdooQueryBuilder<T> Where(object[] exp1, OdooOperator odooOperator, object[] exp2)
        {
            _query.Filters.Add(OdooFilterMapper.ToOdooExpresion<T>(exp1, odooOperator, exp2));
            return this;
        }
        public OdooQueryBuilder<T> ById(long id)
        {
            _query.Filters.Equal(OdooExtensions.GetOdooPropertyName<T>(nameof(IOdooModel.Id)), id);
            return this;
        }

        #endregion

        #region Skip

        public OdooQueryBuilder<T> Take(int limit)
        {
            _query.Take = limit;
            return this;
        }
        public OdooQueryBuilder<T> Skip(int offset)
        {
            _query.Skip = offset;
            return this;
        }

        #endregion

        #region Order

        public OdooQueryBuilder<T> OrderBy(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExtensions.GetOdooPropertyName(expression);
            this._query.Order = $"{odooPropertyName} ASC";
            return this;
        }
        public OdooQueryBuilder<T> OrderByDescending(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExtensions.GetOdooPropertyName(expression);
            this._query.Order = $"{odooPropertyName} DESC";
            return this;
        }
        public OdooQueryBuilder<T> ThenOrderBy(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExtensions.GetOdooPropertyName(expression);
            this._query.Order += $", {odooPropertyName} ASC";
            return this;
        }
        public OdooQueryBuilder<T> ThenOrderByDescending(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExtensions.GetOdooPropertyName(expression);
            this._query.Order += $", {odooPropertyName} DESC";
            return this;
        }

        #endregion

        public OdooQuery Build()
        {
            return _query;
        }
    }
}