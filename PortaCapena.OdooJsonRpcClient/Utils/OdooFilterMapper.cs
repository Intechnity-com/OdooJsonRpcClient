using System;
using System.Linq;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Request;

namespace PortaCapena.OdooJsonRpcClient.Utils
{
    public class OdooFilterMapper
    {
        [Obsolete("Use " + nameof(ToOdooExpression))]
        public static OdooFilter ToOdooExpresion<T>(string propertyname, OdooOperator odooOperator, object value) where T : IOdooModel, new()
        {
            return ToOdooExpression<T>(propertyname, odooOperator, value);
        }

        [Obsolete("Use " + nameof(ToOdooExpression))]
        public static OdooFilter ToOdooExpresion<T>(object left, OdooOperator odooOperator, object right) where T : IOdooModel, new()
        {
            return ToOdooExpression<T>(left, odooOperator, right);
        }

        public static OdooFilter ToOdooExpression<T>(string propertyname, OdooOperator odooOperator, object value) where T : IOdooModel, new()
        {
            return new OdooFilter { OdooExtensions.GetOdooPropertyName<T>(propertyname), odooOperator.Description(), value };
        }

        public static OdooFilter ToOdooExpression<T>(object left, OdooOperator odooOperator, object right) where T : IOdooModel, new()
        {
            OdooFilter result;

            if (left is string member && right is object value)
                result = ToOdooExpression<T>(member, odooOperator, value);

            else if (left is object constant && right is string memberExpression)
                result = ToOdooExpression<T>(memberExpression, odooOperator, constant);

            else if (left.GetType().IsArray && right.GetType().IsArray)
            {
                var leftArray = left as object[];
                var rightArray = right as object[];
                if ((leftArray != null && leftArray.Length == 3 && leftArray.All(x => x != null) && leftArray[1] is OdooOperator leftOperator) &&
                    (rightArray != null && rightArray.Length == 3 && rightArray.All(x => x != null) && rightArray[1] is OdooOperator rightOperator))
                {
                    result = new OdooFilter
                    {
                        ToOdooExpression<T>(leftArray[0], leftOperator, leftArray[2]),
                        odooOperator.Description(),
                        ToOdooExpression<T>(rightArray[0], rightOperator, rightArray[2])
                    };
                }
                else
                    throw new ArgumentException("Invalid expression");
            }
            else
                throw new ArgumentException("Invalid expression");
         
            return result;
        }

        /// <summary>
        /// Creates condition on a related field path, e.g. <c>["partner_id.country_id.code", "=", "PL"]</c>.
        /// </summary>
        /// <param name="odooOperator">Comparison operator.</param>
        /// <param name="value">Compared value.</param>
        /// <param name="fieldPath">Odoo field names from the root model to the compared field.</param>
        [Obsolete("Use " + nameof(ToOdooPathExpression))]
        public static OdooFilter ToOdooPathExpresion(OdooOperator odooOperator, object value, params string[] fieldPath)
        {
            return ToOdooPathExpression(odooOperator, value, fieldPath);
        }

        /// <summary>
        /// Creates condition on a related field path, e.g. <c>["partner_id.country_id.code", "=", "PL"]</c>.
        /// </summary>
        /// <param name="odooOperator">Comparison operator.</param>
        /// <param name="value">Compared value.</param>
        /// <param name="fieldPath">Odoo field names from the root model to the compared field.</param>
        public static OdooFilter ToOdooPathExpression(OdooOperator odooOperator, object value, params string[] fieldPath)
        {
            if (fieldPath == null || fieldPath.Length == 0)
                throw new ArgumentException("At least one field name is required", nameof(fieldPath));

            return new OdooFilter { string.Join(".", fieldPath), odooOperator.Description(), value };
        }
    }
}