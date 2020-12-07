using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PortaCapena.OdooJsonRpcClient.Extensions
{
    public static class EnumExtension
    {
        public static string Description(this Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0) return attributes[0].Description;
                else return value.ToString();
            }
            catch (Exception)
            {
                return value.ToString();
            }
        }
    }
}