using System;
using System.Collections;
using System.Collections.Generic;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="OdooFilter"]/*'/>
    public class OdooFilter : ArrayList
    {
        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Create"]/*'/>
        public static OdooFilter Create()
        {
            return new OdooFilter();
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="EqualTo"]/*'/>
        public OdooFilter EqualTo(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.EqualsTo.Description(), value };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Or"]/*'/>
        public OdooFilter Or()
        {
            Add("|");
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Not"]/*'/>
        public OdooFilter Not()
        {
            Add("!");
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="And"]/*'/>
        public OdooFilter And()
        {
            Add("&");
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="ILike"]/*'/>
        public OdooFilter ILike(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.CaseInsensitiveLike.Description(), value };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Like"]/*'/>
        public OdooFilter Like(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.Like.Description(), value };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotLike"]/*'/>
        public OdooFilter NotLike(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.NotLike.Description(), value };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotEqual"]/*'/>
        public OdooFilter NotEqual(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.NotEqualsTo.Description(), value };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="GreaterThan"]/*'/>
        public OdooFilter GreaterThan(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.GreaterThan.Description(), value };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="GreaterThanOrEqual"]/*'/>
        public OdooFilter GreaterThanOrEqual(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.GreaterThanOrEqualTo.Description(), value };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="LessThan"]/*'/>
        public OdooFilter LessThan(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.LessThan.Description(), value };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="LessThanOrEqual"]/*'/>
        public OdooFilter LessThanOrEqual(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.LessThanOrEqualTo.Description(), value };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Between"]/*'/>
        public OdooFilter Between(string fieldName, object value1, object value2)
        {
            Add(OdooOperator.And.Description());
            Add(new object[] { fieldName, OdooOperator.GreaterThanOrEqualTo.Description(), value1 });
            Add(new object[] { fieldName, OdooOperator.LessThanOrEqualTo.Description(), value2 });
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="In"]/*'/>
        public OdooFilter In(string fieldName, OdooFilterValue value)
        {
            var field = new object[] { fieldName, OdooOperator.In.Description(), value.ToArray() };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="In"]/*'/>
        public OdooFilter In(string fieldName, object[] values)
        {
            var field = new object[] { fieldName, OdooOperator.In.Description(), values };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="In"]/*'/>
        public OdooFilter In(string fieldName, List<object> values)
        {
            var field = new object[] { fieldName, OdooOperator.In.Description(), values.ToArray() };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="In"]/*'/>
        public OdooFilter In(string fieldName, long[] values)
        {
            var field = new object[] { fieldName, OdooOperator.In.Description(), values };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="In"]/*'/>
        public OdooFilter In(string fieldName, int[] values)
        {
            var field = new object[] { fieldName, OdooOperator.In.Description(), values };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotIn"]/*'/>
        public OdooFilter NotIn(string fieldName, OdooFilterValue value)
        {
            var field = new object[] { fieldName, OdooOperator.NotIn.Description(), value.ToArray() };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotIn"]/*'/>
        public OdooFilter NotIn(string fieldName, object[] values)
        {
            var field = new object[] { fieldName, OdooOperator.NotIn.Description(), values };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotIn"]/*'/>
        public OdooFilter NotIn(string fieldName, List<object> values)
        {
            var field = new object[] { fieldName, OdooOperator.NotIn.Description(), values.ToArray() };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotIn"]/*'/>
        public OdooFilter NotIn(string fieldName, long[] values)
        {
            var field = new object[] { fieldName, OdooOperator.NotIn.Description(), values };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotIn"]/*'/>
        public OdooFilter NotIn(string fieldName, int[] values)
        {
            var field = new object[] { fieldName, OdooOperator.NotIn.Description(), values };
            Add(field);
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="ChildOf"]/*'/>
        public OdooFilter ChildOf(string fieldName, string values)
        {
            return ChildOf(fieldName, (object)values);
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="ChildOf"]/*'/>
        public OdooFilter ChildOf(string fieldName, object value)
        {
            Add(new object[] { fieldName, OdooOperator.ChildOf.Description(), value });
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="ParentOf"]/*'/>
        public OdooFilter ParentOf(string fieldName, object value)
        {
            Add(new object[] { fieldName, OdooOperator.ParentOf.Description(), value });
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="Build"]/*'/>
        public OdooFilter Build()
        {
            return this;
        }

        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="NotBeNull"]/*'/>
        public OdooFilter NotBeNull(string fieldName)
        {
            var field = new object[] { fieldName, OdooOperator.NotEqualsTo.Description(), false };
            Add(field);
            return this;
        }
        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="BeNull"]/*'/>
        public OdooFilter BeNull(string fieldName)
        {
            var field = new object[] { fieldName, OdooOperator.EqualsTo.Description(), false };
            Add(field);
            return this;
        }
    }

    /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="OdooFilterValue"]/*'/>
    public class OdooFilterValue : ArrayList
    {
        /// <include file='../Docs/OdooFilter.xml' path='docs/member[@name="OdooFilterValue.AddValue"]/*'/>
        public OdooFilterValue AddValue(object value)
        {
            Add(value);
            return this;
        }
    }
}
