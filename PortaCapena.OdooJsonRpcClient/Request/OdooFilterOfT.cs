using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="OdooFilterOfT"]/*'/>
    public class OdooFilter<T> : OdooFilter where T : IOdooModel
    {
        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Create"]/*'/>
        public new static OdooFilter<T> Create()
        {
            return new OdooFilter<T>();
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Or"]/*'/>
        public new OdooFilter<T> Or()
        {
            base.Or();
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Not"]/*'/>
        public new OdooFilter<T> Not()
        {
            base.Not();
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="And"]/*'/>
        public new OdooFilter<T> And()
        {
            base.And();
            return this;
        }


        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="EqualTo"]/*'/>
        public OdooFilter<T> EqualTo(Expression<Func<T, object>> expression, object value)
        {
            EqualTo(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="ILike"]/*'/>
        public OdooFilter<T> ILike(Expression<Func<T, object>> expression, object value)
        {
            ILike(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Like"]/*'/>
        public OdooFilter<T> Like(Expression<Func<T, object>> expression, object value)
        {
            Like(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotLike"]/*'/>
        public OdooFilter<T> NotLike(Expression<Func<T, object>> expression, object value)
        {
            NotLike(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotEqual"]/*'/>
        public OdooFilter<T> NotEqual(Expression<Func<T, object>> expression, object value)
        {
            NotEqual(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="GreaterThan"]/*'/>
        public OdooFilter<T> GreaterThan(Expression<Func<T, object>> expression, object value)
        {
            GreaterThan(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="GreaterThanOrEqual"]/*'/>
        public OdooFilter<T> GreaterThanOrEqual(Expression<Func<T, object>> expression, object value)
        {
            GreaterThanOrEqual(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="LessThan"]/*'/>
        public OdooFilter<T> LessThan(Expression<Func<T, object>> expression, object value)
        {
            LessThan(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="LessThanOrEqual"]/*'/>
        public OdooFilter<T> LessThanOrEqual(Expression<Func<T, object>> expression, object value)
        {
            LessThanOrEqual(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Between"]/*'/>
        public OdooFilter<T> Between(Expression<Func<T, object>> expression, object value1, object value2)
        {
            Between(OdooExpresionMapper.GetOdooPropertyName(expression), value1, value2);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="In"]/*'/>
        public OdooFilter<T> In(Expression<Func<T, object>> expression, OdooFilterValue value)
        {
            In(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="In"]/*'/>
        public OdooFilter<T> In(Expression<Func<T, object>> expression, object[] values)
        {
            In(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="In"]/*'/>
        public OdooFilter<T> In(Expression<Func<T, object>> expression, List<object> values)
        {
            In(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="In"]/*'/>
        public OdooFilter<T> In(Expression<Func<T, object>> expression, long[] values)
        {
            In(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotIn"]/*'/>
        public OdooFilter<T> NotIn(Expression<Func<T, object>> expression, OdooFilterValue value)
        {
            NotIn(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotIn"]/*'/>
        public OdooFilter<T> NotIn(Expression<Func<T, object>> expression, object[] values)
        {
            NotIn(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotIn"]/*'/>
        public OdooFilter NotIn(Expression<Func<T, object>> expression, List<object> values)
        {
            NotIn(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotIn"]/*'/>
        public OdooFilter<T> NotIn(Expression<Func<T, object>> expression, long[] values)
        {
            NotIn(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="ChildOf"]/*'/>
        public OdooFilter<T> ChildOf(Expression<Func<T, object>> expression, string values)
        {
            ChildOf(OdooExpresionMapper.GetOdooPropertyName(expression), values);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="ChildOf"]/*'/>
        public OdooFilter<T> ChildOf(Expression<Func<T, object>> expression, object value)
        {
            ChildOf(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="ParentOf"]/*'/>
        public OdooFilter<T> ParentOf(Expression<Func<T, object>> expression, object value)
        {
            ParentOf(OdooExpresionMapper.GetOdooPropertyName(expression), value);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotBeNull"]/*'/>
        public OdooFilter<T> NotBeNull(Expression<Func<T, object>> expression)
        {
            NotBeNull(OdooExpresionMapper.GetOdooPropertyName(expression));
            return this;
        }
        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="BeNull"]/*'/>
        public OdooFilter<T> BeNull(Expression<Func<T, object>> expression)
        {
            BeNull(OdooExpresionMapper.GetOdooPropertyName(expression));
            return this;
        }
    }
}
