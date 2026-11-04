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
    /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQueryOfT"]/*'/>
    public class OdooQuery<T> : OdooQuery where T : IOdooModel, new()
    {
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQueryOfT.ctor"]/*'/>
        public OdooQuery() : base()
        {
            base.Filters = new OdooFilter<T>();
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQueryOfT.Filters"]/*'/>
        public new OdooFilter<T> Filters => base.Filters as OdooFilter<T>;

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OdooQueryOfT.Create"]/*'/>
        public static OdooQuery<T> Create()
        {
            return new OdooQuery<T>();
        }


        #region Select

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Select.Selector"]/*'/>
        public OdooQuery<T> Select(Func<T, object> selector)
        {
            var model = selector.Invoke(new T());
            PropertyInfo[] modelProps = model.GetType().GetProperties();

            return Select(modelProps.Select(x => OdooExtensions.GetOdooPropertyName<T>(x.Name)).Where(x => x != null).ToArray());
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Select.Fields"]/*'/>
        public OdooQuery<T> Select(params string[] fields)
        {
            foreach (var f in fields)
                ReturnFields.Add(f);

            return this;
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="SelectSimplifiedModel"]/*'/>
        public OdooQuery<T> SelectSimplifiedModel()
        {
            PropertyInfo[] modelProps = typeof(T).GetProperties();
            return Select(modelProps.Select(x => OdooExtensions.GetOdooPropertyName<T>(x.Name)).Where(x => x != null).ToArray());
        }

        #endregion

        #region Where
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.Filter"]/*'/>
        public OdooQuery<T> Where(OdooFilter filter)
        {
            Filters.AddRange(filter);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.Expression"]/*'/>
        public OdooQuery<T> Where(Expression<Func<T, object>> expression, OdooOperator odooOperator, object value)
        {
            Filters.Add(new OdooFilter { OdooExpresionMapper.GetOdooPropertyName(expression), odooOperator.Description(), value });
            return this;
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel1"]/*'/>
        public OdooQuery<T> Where<TForeignKeyLevel1>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, object>> expressionForeignKeyLevel1, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new()
        {
            Filters.Add(OdooFilterMapper.ToOdooPathExpresion(odooOperator, value, OdooExpresionMapper.GetOdooPropertyName<T>(expression), OdooExpresionMapper.GetOdooPropertyName<TForeignKeyLevel1>(expressionForeignKeyLevel1)));
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel1"]/*'/>
        public OdooQuery<T> Where<TForeignKeyLevel1>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, object>> expressionForeignKeyLevel1, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new()
        {
            Filters.Add(OdooFilterMapper.ToOdooPathExpresion(odooOperator, value, OdooExpresionMapper.GetOdooPropertyName<T>(expression), OdooExpresionMapper.GetOdooPropertyName<TForeignKeyLevel1>(expressionForeignKeyLevel1)));
            return this;
        }

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel2"]/*'/>
        public OdooQuery<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, long>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            Filters.Add(OdooFilterMapper.ToOdooPathExpresion(odooOperator, value, OdooExpresionMapper.GetOdooPropertyName<T>(expression), OdooExpresionMapper.GetOdooPropertyName<TForeignKeyLevel1>(expressionForeignKeyLevel1), OdooExpresionMapper.GetOdooPropertyName<TForeignKeyLevel2>(expressionForeignKeyLevel2)));
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel2"]/*'/>
        public OdooQuery<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, long?>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            Filters.Add(OdooFilterMapper.ToOdooPathExpresion(odooOperator, value, OdooExpresionMapper.GetOdooPropertyName<T>(expression), OdooExpresionMapper.GetOdooPropertyName<TForeignKeyLevel1>(expressionForeignKeyLevel1), OdooExpresionMapper.GetOdooPropertyName<TForeignKeyLevel2>(expressionForeignKeyLevel2)));
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel2"]/*'/>
        public OdooQuery<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long>> expression, Expression<Func<TForeignKeyLevel1, long>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            Filters.Add(OdooFilterMapper.ToOdooPathExpresion(odooOperator, value, OdooExpresionMapper.GetOdooPropertyName<T>(expression), OdooExpresionMapper.GetOdooPropertyName<TForeignKeyLevel1>(expressionForeignKeyLevel1), OdooExpresionMapper.GetOdooPropertyName<TForeignKeyLevel2>(expressionForeignKeyLevel2)));
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Where.ForeignKeyLevel2"]/*'/>
        public OdooQuery<T> Where<TForeignKeyLevel1, TForeignKeyLevel2>(Expression<Func<T, long?>> expression, Expression<Func<TForeignKeyLevel1, long?>> expressionForeignKeyLevel1, Expression<Func<TForeignKeyLevel2, object>> expressionForeignKeyLevel2, OdooOperator odooOperator, object value) where TForeignKeyLevel1 : IOdooModel, new() where TForeignKeyLevel2 : IOdooModel, new()
        {
            Filters.Add(OdooFilterMapper.ToOdooPathExpresion(odooOperator, value, OdooExpresionMapper.GetOdooPropertyName<T>(expression), OdooExpresionMapper.GetOdooPropertyName<TForeignKeyLevel1>(expressionForeignKeyLevel1), OdooExpresionMapper.GetOdooPropertyName<TForeignKeyLevel2>(expressionForeignKeyLevel2)));
            return this;
        }


        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="ById"]/*'/>
        public OdooQuery<T> ById(long id)
        {
            Filters.EqualTo(x => x.Id, id);
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="ByIds"]/*'/>
        public OdooQuery<T> ByIds(params long[] ids)
        {
            if (ids.Any()) Filters.In(x => x.Id, ids);
            return this;
        }

        #endregion

        #region Skip

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Take"]/*'/>
        public OdooQuery<T> Take(int limit)
        {
            this.Limit = limit;
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="Skip"]/*'/>
        public OdooQuery<T> Skip(int offset)
        {
            Offset = offset;
            return this;
        }

        #endregion

        #region Order

        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OrderBy"]/*'/>
        public OdooQuery<T> OrderBy(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExpresionMapper.GetOdooPropertyName(expression);
            this.Order = $"{odooPropertyName} ASC";
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="OrderByDescending"]/*'/>
        public OdooQuery<T> OrderByDescending(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExpresionMapper.GetOdooPropertyName(expression);
            this.Order = $"{odooPropertyName} DESC";
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="ThenOrderBy"]/*'/>
        public OdooQuery<T> ThenOrderBy(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExpresionMapper.GetOdooPropertyName(expression);
            this.Order += $", {odooPropertyName} ASC";
            return this;
        }
        /// <include file='../Docs/OdooQuery.xml' path='docs/member[@name="ThenOrderByDescending"]/*'/>
        public OdooQuery<T> ThenOrderByDescending(Expression<Func<T, object>> expression)
        {
            var odooPropertyName = OdooExpresionMapper.GetOdooPropertyName(expression);
            this.Order += $", {odooPropertyName} DESC";
            return this;
        }

        #endregion

    }
}
