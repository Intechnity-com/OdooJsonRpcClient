using System;
using System.Linq.Expressions;
using PortaCapena.OdooJsonRpcClient.Extensions;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Converters
{
    public static class OdooExpressionMapper
    {
        /// <summary>
        /// Returns odoo field name (value of <c>[JsonProperty]</c>) of property selected by expression, e.g. <c>x => x.PartnerId</c> gives <c>partner_id</c>.
        /// </summary>
        internal static string GetOdooPropertyName<T>(LambdaExpression expression) where T : IOdooAtributtesModel
        {
            var propertyName = GetPropertyName(expression);
            if (propertyName == null)
                throw new ArgumentException($"Expression '{expression}' must select a property of '{typeof(T).Name}', e.g. x => x.Name", nameof(expression));

            var odooName = OdooExtensions.GetOdooPropertyName<T>(propertyName);
            if (odooName == null)
                throw new ArgumentException($"Property '{typeof(T).Name}.{propertyName}' has no [JsonProperty] attribute with odoo field name", nameof(expression));

            return odooName;
        }

        internal static string GetOdooPropertyName<T>(Expression<Func<T, object>> expression) where T : IOdooAtributtesModel
        {
            return GetOdooPropertyName<T>((LambdaExpression)expression);
        }

        /// <summary>
        /// Returns .NET property name selected by expression (also through a boxing/nullable conversion), or null.
        /// </summary>
        internal static string GetPropertyName(LambdaExpression expression)
        {
            var body = expression.Body;

            if (body is UnaryExpression unary && (unary.NodeType == ExpressionType.Convert || unary.NodeType == ExpressionType.ConvertChecked))
                body = unary.Operand;

            return (body as MemberExpression)?.Member.Name;
        }
    }
}
