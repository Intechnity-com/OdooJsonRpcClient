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
        public static OdooFilter ToOdooExpresion<T>(string propertyname, OdooOperator odooOperator, object value) where T : IOdooModel, new()
        {
            return new OdooFilter { OdooExtensions.GetOdooPropertyName<T>(propertyname), odooOperator.Description(), value };
        }

        public static OdooFilter ToOdooExpresion<T>(object left, OdooOperator odooOperator, object right) where T : IOdooModel, new()
        {
            OdooFilter result;

            if (left is string member && right is object value)
                result = ToOdooExpresion<T>(member, odooOperator, value);

            else if (left is object constant && right is string memberExpression)
                result = ToOdooExpresion<T>(memberExpression, odooOperator, constant);

            else if (left.GetType().IsArray && right.GetType().IsArray)
            {
                var leftArray = left as object[];
                var rightArray = right as object[];
                if ((leftArray != null && leftArray.Length == 3 && leftArray.All(x => x != null) && leftArray[1] is OdooOperator leftOperator) &&
                    (rightArray != null && rightArray.Length == 3 && rightArray.All(x => x != null) && rightArray[1] is OdooOperator rightOperator))
                {
                    result = new OdooFilter
                    {
                        ToOdooExpresion<T>(leftArray[0], leftOperator, leftArray[2]),
                        odooOperator.Description(),
                        ToOdooExpresion<T>(rightArray[0], rightOperator, rightArray[2])
                    };
                }
                else
                    throw new ArgumentException("Invalid expresion");
            }
            else
                throw new ArgumentException("Incalid Expresion");
         
            return result;
        }
    }
}