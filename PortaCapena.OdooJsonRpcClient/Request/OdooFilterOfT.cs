using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooFilter<T> : OdooFilter where T : IOdooModel
    { 
        public static OdooFilter<T> Create()
        {
            return new OdooFilter<T>();
        }

        public OdooFilter<T> Or()
        {
            base.Or();
            return this;
        }

        public OdooFilter<T> Not()
        {
            base.Not();
            return this;
        }

        public OdooFilter<T> And()
        {
            base.And();
            return this;
        }


        public OdooFilter<T> EqualTo(Expression<Func<T, object>> expression, object value)
        {
            EqualTo(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> ILike(Expression<Func<T, object>> expression, object value)
        {
            ILike(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> Like(Expression<Func<T, object>> expression, object value)
        {
            Like(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> NotLike(Expression<Func<T, object>> expression, object value)
        {
            NotLike(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> NotEqual(Expression<Func<T, object>> expression, object value)
        {
            NotEqual(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> GreaterThan(Expression<Func<T, object>> expression, object value)
        {
            GreaterThan(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> GreaterThanOrEqual(Expression<Func<T, object>> expression, object value)
        {
            GreaterThanOrEqual(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> LessThan(Expression<Func<T, object>> expression, object value)
        {
            LessThan(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> LessThanOrEqual(Expression<Func<T, object>> expression, object value)
        {
            LessThanOrEqual(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> Between(Expression<Func<T, object>> expression, object value1, object value2)
        {
            Between(OdooExpresionMapper.GetOdooPropertyName(expression), value1, value2);
            return this;
        }

        public OdooFilter<T> In(Expression<Func<T, object>> expression, OdooFilterValue value)
        {
            In(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> In(Expression<Func<T, object>> expression, object[] values)
        {
            In(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        public OdooFilter<T> In(Expression<Func<T, object>> expression, List<object> values)
        {
            In(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        public OdooFilter<T> In(Expression<Func<T, object>> expression, long[] values)
        {
            In(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        public OdooFilter<T> NotIn(Expression<Func<T, object>> expression, OdooFilterValue value)
        {
            NotIn(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        public OdooFilter<T> NotIn(Expression<Func<T, object>> expression, object[] values)
        {
            NotIn(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        public OdooFilter NotIn(Expression<Func<T, object>> expression, List<object> values)
        {
            NotIn(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        public OdooFilter<T> NotIn(Expression<Func<T, object>> expression, long[] values)
        {
            NotIn(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        public OdooFilter<T> ChildOf(Expression<Func<T, object>> expression, string values)
        {
            ChildOf(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        public OdooFilter<T> NotBeNull(Expression<Func<T, object>> expression)
        {
            NotBeNull(OdooExpresionMapper.GetOdooPropertyName(expression));
            return this;
        }
        public OdooFilter<T> BeNull(Expression<Func<T, object>> expression)
        {
            BeNull(OdooExpresionMapper.GetOdooPropertyName(expression));
            return this;
        }
    }
}