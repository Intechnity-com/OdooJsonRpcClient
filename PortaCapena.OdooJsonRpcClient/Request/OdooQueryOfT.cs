using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Result;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooQuery<T> : OdooQuery where T : IOdooModel, new()
    {

        public static OdooQuery<T> Create()
        {
            return  new OdooQuery<T>();
        }


        #region Select

        public OdooQuery<T> Select(Func<T, object> selector)
        {
            var model = selector.Invoke(new T());
            PropertyInfo[] modelProps = model.GetType().GetProperties();

            foreach (var propertyInfo in modelProps)
            {
                var key = new T().OdooPropertyName(propertyInfo.Name);
                ReturnFields.Add(key);
            }
            return this;
        }
        public OdooQuery<T> Select(params string[] fields)
        {
            foreach (var f in fields)
                ReturnFields.Add(f);

            return this;
        }

        #endregion

        #region Where
        public OdooQuery<T> Where(OdooFilter filter)
        {
            Filters.AddRange(filter);
            return this;
        }
        public OdooQuery<T> Where(Expression<Func<T, object>> expression, OdooOperator odooOperator, object value)
        {
            Filters.Add(OdooFilterMapper.ToOdooExpresion<T>(OdooExtensions.GetPropertyName(expression), odooOperator, value));
            return this;
        }

        public OdooQuery<T> ById(long id)
        {
            Filters.EqualTo(OdooExtensions.GetOdooPropertyName<T>(nameof(IOdooModel.Id)), id);
            return this;
        }

        #endregion

        #region Skip

        public OdooQuery<T> Take(int limit)
        {
            this.Limit = limit;
            return this;
        }
        public OdooQuery<T> Skip(int offset)
        {
            Offset = offset;
            return this;
        }

        #endregion

        #region Order

        public OdooQuery<T> OrderBy(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExtensions.GetOdooPropertyName(expression);
            this.Order = $"{odooPropertyName} ASC";
            return this;
        }
        public OdooQuery<T> OrderByDescending(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExtensions.GetOdooPropertyName(expression);
            this.Order = $"{odooPropertyName} DESC";
            return this;
        }
        public OdooQuery<T> ThenOrderBy(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExtensions.GetOdooPropertyName(expression);
            this.Order += $", {odooPropertyName} ASC";
            return this;
        }
        public OdooQuery<T> ThenOrderByDescending(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExtensions.GetOdooPropertyName(expression);
            this.Order += $", {odooPropertyName} DESC";
            return this;
        }

        #endregion

    }
}