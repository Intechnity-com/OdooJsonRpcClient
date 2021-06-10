using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooQuery<T> : OdooQuery where T : IOdooModel, new()
    {
        public OdooQuery() : base()
        {
            base.Filters = new OdooFilter<T>();
        }

        public new OdooFilter<T> Filters => base.Filters as OdooFilter<T>;

        public static OdooQuery<T> Create()
        {
            return new OdooQuery<T>();
        }


        #region Select

        public OdooQuery<T> Select(Func<T, object> selector)
        {
            var model = selector.Invoke(new T());
            PropertyInfo[] modelProps = model.GetType().GetProperties();

            return Select(modelProps.Select(x => OdooExtensions.GetOdooPropertyName<T>(x.Name)).ToArray());
        }
        public OdooQuery<T> Select(params string[] fields)
        {
            foreach (var f in fields)
                ReturnFields.Add(f);

            return this;
        }

        public OdooQuery<T> SelectSimplifiedModel()
        {
            PropertyInfo[] modelProps = typeof(T).GetProperties();
            return Select(modelProps.Select(x => OdooExtensions.GetOdooPropertyName<T>(x.Name)).ToArray());
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
            Filters.Add(OdooFilterMapper.ToOdooExpresion<T>(OdooExpresionMapper.GetPropertyName(expression), odooOperator, value));
            return this;
        }

        public OdooQuery<T> Where<TForeignKeyLevel1>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, object>> expressionForeignKeyLevel1, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new()
        {
            var fields = OdooExtensions.GetOdooPropertyName<T>(OdooExpresionMapper.GetPropertyName(expression)) + '.' +
                         OdooExtensions.GetOdooPropertyName<TForeignKeyLevel1>(OdooExpresionMapper.GetPropertyName(expressionForeignKeyLevel1));

            Filters.Add(new OdooFilter { fields, odooOperator.Description(), value });
            return this;
        }
        public OdooQuery<T> Where<TForeignKeyLevel1>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, object>> expressionForeignKeyLevel1, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new()
        {
            var fields = OdooExtensions.GetOdooPropertyName<T>(OdooExpresionMapper.GetPropertyName(expression)) + '.' +
                         OdooExtensions.GetOdooPropertyName<TForeignKeyLevel1>(OdooExpresionMapper.GetPropertyName(expressionForeignKeyLevel1));

            Filters.Add(new OdooFilter { fields, odooOperator.Description(), value });
            return this;
        }

        public OdooQuery<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, long>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            var fields = OdooExtensions.GetOdooPropertyName<T>(OdooExpresionMapper.GetPropertyName(expression)) + '.' +
                         OdooExtensions.GetOdooPropertyName<TForeignKeyLevel1>(OdooExpresionMapper.GetPropertyName(expressionForeignKeyLevel1)) + '.' +
                         OdooExtensions.GetOdooPropertyName<TForeignKeyLevel2>(OdooExpresionMapper.GetPropertyName(expressionForeignKeyLevel2));

            Filters.Add(new OdooFilter { fields, odooOperator.Description(), value });
            return this;
        }
        public OdooQuery<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, long?>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            var fields = OdooExtensions.GetOdooPropertyName<T>(OdooExpresionMapper.GetPropertyName(expression)) + '.' +
                         OdooExtensions.GetOdooPropertyName<TForeignKeyLevel1>(OdooExpresionMapper.GetPropertyName(expressionForeignKeyLevel1)) + '.' +
                         OdooExtensions.GetOdooPropertyName<TForeignKeyLevel2>(OdooExpresionMapper.GetPropertyName(expressionForeignKeyLevel2));

            Filters.Add(new OdooFilter { fields, odooOperator.Description(), value });
            return this;
        }
        public OdooQuery<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, long>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            var fields = OdooExtensions.GetOdooPropertyName<T>(OdooExpresionMapper.GetPropertyName(expression)) + '.' +
                         OdooExtensions.GetOdooPropertyName<TForeignKeyLevel1>(OdooExpresionMapper.GetPropertyName(expressionForeignKeyLevel1)) + '.' +
                         OdooExtensions.GetOdooPropertyName<TForeignKeyLevel2>(OdooExpresionMapper.GetPropertyName(expressionForeignKeyLevel2));

            Filters.Add(new OdooFilter { fields, odooOperator.Description(), value });
            return this;
        }
        public OdooQuery<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, long?>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            var fields = OdooExtensions.GetOdooPropertyName<T>(OdooExpresionMapper.GetPropertyName(expression)) + '.' +
                         OdooExtensions.GetOdooPropertyName<TForeignKeyLevel1>(OdooExpresionMapper.GetPropertyName(expressionForeignKeyLevel1)) + '.' +
                         OdooExtensions.GetOdooPropertyName<TForeignKeyLevel2>(OdooExpresionMapper.GetPropertyName(expressionForeignKeyLevel2));

            Filters.Add(new OdooFilter { fields, odooOperator.Description(), value });
            return this;
        }


        public OdooQuery<T> ById(long id)
        {
            Filters.EqualTo(x => x.Id, id);
            return this;
        }
        public OdooQuery<T> ByIds(params long[] ids)
        {
            if (ids.Any()) Filters.In(x => x.Id, ids);
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
            var odooPropertyName = OdooExpresionMapper.GetOdooPropertyName(expression);
            this.Order = $"{odooPropertyName} ASC";
            return this;
        }
        public OdooQuery<T> OrderByDescending(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExpresionMapper.GetOdooPropertyName(expression);
            this.Order = $"{odooPropertyName} DESC";
            return this;
        }
        public OdooQuery<T> ThenOrderBy(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExpresionMapper.GetOdooPropertyName(expression);
            this.Order += $", {odooPropertyName} ASC";
            return this;
        }
        public OdooQuery<T> ThenOrderByDescending(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExpresionMapper.GetOdooPropertyName(expression);
            this.Order += $", {odooPropertyName} DESC";
            return this;
        }

        #endregion

    }
}