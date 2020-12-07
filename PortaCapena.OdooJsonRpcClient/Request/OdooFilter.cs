using System;
using System.Collections;
using System.Collections.Generic;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooFilter : ArrayList
    { 
        public static OdooFilter Create()
        {
            return new OdooFilter();
        }

        public OdooFilter Equal(string fieldName, object value)
        {
            var field = new object[] { fieldName, "=", value };
            Add(field);
            return this;
        }

        public OdooFilter Or()
        {
            Add("|");
            return this;
        }

        public OdooFilter Not()
        {
            Add("!");
            return this;
        }

        public OdooFilter And()
        {
            Add("&");
            return this;
        }

        public OdooFilter ILike(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.CaseInsensitiveLike.Description(), value };
            Add(field);
            return this;
        }

        public OdooFilter Like(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.Like.Description(), value };
            Add(field);
            return this;
        }

        public OdooFilter NotLike(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.NotLike.Description(), value };
            Add(field);
            return this;
        }

        public OdooFilter NotEqual(string fieldName, object value)
        {
            var field = new object[] { fieldName, OdooOperator.NotEqualsTo.Description(), value };
            Add(field);
            return this;
        }

        public OdooFilter GreaterThan(string fieldName, object value)
        {
            var field = new object[] { fieldName, ">", value };
            Add(field);
            return this;
        }

        public OdooFilter GreaterThanOrEqual(string fieldName, object value)
        {
            var field = new object[] { fieldName, ">=", value };
            Add(field);
            return this;
        }

        public OdooFilter LessThan(string fieldName, object value)
        {
            var field = new object[] { fieldName, "<", value };
            Add(field);
            return this;
        }

        public OdooFilter LessThanOrEqual(string fieldName, object value)
        {
            var field = new object[] { fieldName, "<=", value };
            Add(field);
            return this;
        }

        public OdooFilter Between(string fieldName, object value1, object value2)
        {
            var field = new object[] { fieldName, "between", value1, "and", value2 };
            Add(field);
            return this;
        }

        public OdooFilter In(string fieldName, OdooFilterValue value)
        {
            var field = new object[] { fieldName, "in", value.ToArray() };
            Add(field);
            return this;
        }

        public OdooFilter In(string fieldName, object[] values)
        {
            var field = new object[] { fieldName, "in", values };
            Add(field);
            return this;
        }

        public OdooFilter In(string fieldName, List<object> values)
        {
            var field = new object[] { fieldName, "in", values.ToArray() };
            Add(field);
            return this;
        }

        public OdooFilter In(string fieldName, int[] values)
        {
            var field = new object[] { fieldName, "in", values };
            Add(field);
            return this;
        }

        public OdooFilter NotIn(string fieldName, OdooFilterValue value)
        {
            var field = new object[] { fieldName, "not in", value.ToArray() };
            Add(field);
            return this;
        }

        public OdooFilter NotIn(string fieldName, object[] values)
        {
            var field = new object[] { fieldName, "not in", values };
            Add(field);
            return this;
        }

        public OdooFilter NotIn(string fieldName, List<object> values)
        {
            var field = new object[] { fieldName, "not in", values.ToArray() };
            Add(field);
            return this;
        }

        public OdooFilter NotIn(string fieldName, int[] values)
        {
            var field = new object[] { fieldName, "not in", values };
            Add(field);
            return this;
        }

        public OdooFilter ChildOf(string fieldName, string values)
        {
            var field = new object[] { fieldName, OdooOperator.NotEqualsTo.Description(), "child_of", values };
            Add(field);
            return this;
        }

        public OdooFilter Build()
        {
            return this;
        }
    }

    public class OdooFilterValue : ArrayList
    {
        public OdooFilterValue AddValue(object value)
        {
            Add(value);
            return this;
        }
    }
}