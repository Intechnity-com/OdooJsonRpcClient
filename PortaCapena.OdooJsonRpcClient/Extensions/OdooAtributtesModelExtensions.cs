using System;
using System.Linq;
using System.Linq.Expressions;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Extensions
{
    public static class OdooAtributtesModelExtensions
    {
        public static string OdooTableName(this IOdooAtributtesModel model)
        {
            if (!(model.GetType().GetCustomAttributes(typeof(OdooTableNameAttribute), true).FirstOrDefault() is OdooTableNameAttribute attribute))
                throw new ArgumentException($"Mising attribute '{nameof(OdooTableNameAttribute)}' for model '{model.GetType().Name}'");
            return attribute.Name;
        }

        public static string OdooPropertyName<T>(this T model, string name) where T : IOdooAtributtesModel
        {
            return OdooExtensions.GetOdooPropertyName<T>(name);
        }

        public static string OdooPropertyName<T>(this T model, Expression<Func<T, object>> expression) where T : IOdooAtributtesModel
        {
            return OdooExpresionMapper.GetOdooPropertyName(expression);
        }
    }
}