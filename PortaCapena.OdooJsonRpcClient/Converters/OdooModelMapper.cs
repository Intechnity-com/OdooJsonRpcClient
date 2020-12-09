using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Converters
{
    public static class OdooModelMapper
    {
        private const string OdooModelSuffix = "OdooDto";

        public static bool ConverOdooPropertyToDotNet(Type dotnetType, JToken value, out object result)
        {
            result = null;

            switch (value.Type)
            {
                case JTokenType.Boolean when dotnetType != typeof(bool):
                    return false;

                case JTokenType.Boolean:
                    result = value.ToObject(dotnetType);
                    return true;

                case JTokenType.Integer when dotnetType == typeof(int) || dotnetType == typeof(int?) || dotnetType == typeof(long) || dotnetType == typeof(long?):
                case JTokenType.Float:
                    result = value.ToObject(dotnetType);
                    return true;

                case JTokenType.Integer when dotnetType.IsArray && (int)value.ToObject(typeof(int)) == 0:
                    result = Activator.CreateInstance(dotnetType, 0);
                    return true;
                case JTokenType.String when dotnetType == typeof(string):
                    result = value.ToObject(dotnetType);
                    return true;

                case JTokenType.String when (dotnetType == typeof(DateTime) || dotnetType == typeof(DateTime?)):
                    {
                        var stringTime = value.ToObject(typeof(string)) as string;
                        result = DateTime.Parse(stringTime);
                        return true;
                    }

                case JTokenType.Array when dotnetType.IsArray:
                    {
                        if (!value.HasValues)
                        {
                            result = Activator.CreateInstance(dotnetType, 0);
                            return true;
                        }

                        result = value.ToObject(dotnetType);
                        return true;
                    }

                case JTokenType.Array when !dotnetType.IsArray:
                    {
                        if (!value.HasValues)
                            return false;

                        if (value.Count() > 2 || dotnetType != typeof(long) && dotnetType != typeof(long?) && dotnetType != typeof(int) && dotnetType != typeof(int?) || value.First.Type != JTokenType.Integer)
                            throw new Exception($"Not implemented json mapping '${value.Parent}'");

                        result = value.First.ToObject(dotnetType);
                        return true;
                    }

                default:
                    throw new Exception($"Not implemented json mapping value: '${value.Parent}' to {dotnetType.Name}");
            }
        }


        public static string GetDotNetModel(string tableName, Dictionary<string, OdooPropertyInfo> properties)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"[OdooTableName(\"{tableName}\")]");
            builder.AppendLine($"[JsonConverter(typeof({nameof(OdooModelConverter)}))]");
            builder.AppendLine($"public class {ConvertOdooNameToDotNet(tableName)}{OdooModelSuffix} : IOdooModel");
            builder.AppendLine("{");

            foreach (var property in properties)
            {
                builder.AppendLine(string.Empty);
                if (!string.IsNullOrEmpty(property.Value.Relation))
                    builder.AppendLine($"// {property.Value.Relation}");

                builder.AppendLine($"[JsonProperty(\"{property.Key}\")]");
                builder.AppendLine($"public {ConvertToDotNetPropertyTypeName(property)} {ConvertOdooNameToDotNet(property.Key)} {{ get; set; }}");
            }

            builder.AppendLine("}");

            return builder.ToString();
        }

        public static string ConvertToDotNetPropertyTypeName(KeyValuePair<string, OdooPropertyInfo> propery)
        {
            switch (propery.Value.PropertyValueType)
            {
                case OdooValueTypeEnum.Binary:
                    return "string";
                case OdooValueTypeEnum.Char:
                    return "string";
                case OdooValueTypeEnum.Selection:
                    return "string";
                case OdooValueTypeEnum.Text:
                    return "string";
                case OdooValueTypeEnum.Html:
                    return "string";

                case OdooValueTypeEnum.Boolean:
                    return propery.Value.ResultRequired ? "bool" : "bool?";

                case OdooValueTypeEnum.Monetary: 
                    return propery.Value.ResultRequired ? "decimal" : "decimal?";

                case OdooValueTypeEnum.Float:
                    return propery.Value.ResultRequired ? "double" : "double?";
                case OdooValueTypeEnum.Integer:
                    return propery.Value.ResultRequired ? "int" : "int?";

                case OdooValueTypeEnum.Date:
                    return propery.Value.ResultRequired ? "DateTime" : "DateTime?";
                case OdooValueTypeEnum.Datetime:
                    return propery.Value.ResultRequired ? "DateTime" : "DateTime?";

                case OdooValueTypeEnum.Many2One:
                    return propery.Value.ResultRequired ? "long" : "long?";
                case OdooValueTypeEnum.Many2Many:
                    return "long[]";
                case OdooValueTypeEnum.One2Many:
                    return "long[]";
                case OdooValueTypeEnum.One2One:
                    return propery.Value.ResultRequired ? "long" : "long?";

                case OdooValueTypeEnum.Reference:
                    return  ConvertOdooNameToDotNet(propery.Value.RelationField) + OdooModelSuffix;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string ConvertOdooNameToDotNet(string odooName)
        {
            var dotnetKeys = odooName.Split('.', '_').Select(x => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(x));
            return string.Join(string.Empty, dotnetKeys);
        }
    }
}