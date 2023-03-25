using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Converters
{
    public static class OdooModelMapper
    {
        private const string OdooModelSuffix = "OdooModel";
        private const string OdooEnumSuffix = "OdooEnum";

        public static bool ConverOdooPropertyToDotNet(Type dotnetType, JToken value, out object result)
        {
            result = null;

            switch (value.Type)
            {
                case JTokenType.Boolean when dotnetType != typeof(bool) && dotnetType != typeof(bool?):
                    return false;

                case JTokenType.Boolean:
                    result = value.ToObject(dotnetType);
                    return true;

                case JTokenType.Integer when dotnetType == typeof(bool) || dotnetType == typeof(bool?):
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

                case JTokenType.String when dotnetType == typeof(DateTime) || dotnetType == typeof(DateTime?):
                    var stringTime = value.ToObject(typeof(string)) as string;
                    result = DateTime.Parse(stringTime);
                    return true;

                case JTokenType.String when dotnetType.IsEnum:
                    result = ConvertToDotNetEnum(dotnetType, value.ToString());
                    return true;

                case JTokenType.String when dotnetType.IsGenericType && dotnetType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                                            dotnetType.GenericTypeArguments.Length == 1 && dotnetType.GenericTypeArguments[0].IsEnum:
                    var nullableType = Nullable.GetUnderlyingType(dotnetType);
                    result = ConvertToDotNetEnum(nullableType, value);
                    return true;

                case JTokenType.Array when dotnetType.IsArray:
                    if (!value.HasValues)
                    {
                        result = Activator.CreateInstance(dotnetType, 0);
                        return true;
                    }

                    result = value.ToObject(dotnetType);
                    return true;

                case JTokenType.Array when !dotnetType.IsArray:
                    if (!value.HasValues)
                        return false;

                    if (dotnetType == typeof(string) && value.Children().All(x => x.Type == JTokenType.String))
                    {
                        var stringArray = value.ToObject(typeof(string[])) as string[];
                        result = string.Join(".", stringArray);
                        return true;
                    }

                    if (value.Count() > 2 || dotnetType != typeof(long) && dotnetType != typeof(long?) && dotnetType != typeof(int) && dotnetType != typeof(int?) || value.First.Type != JTokenType.Integer)
                        throw new Exception($"Not implemented json mapping '${value.Parent}'");

                    result = value.First.ToObject(dotnetType);
                    return true;

                default:
                    throw new Exception($"Not implemented json mapping value: '${value.Parent}' to {dotnetType.Name}");
            }
        }


        public static string GetDotNetModel(string tableName, Dictionary<string, OdooPropertyInfo> properties, bool addSummary = true)
        {
            var existing = new Dictionary<string, bool>();

            var builder = new StringBuilder();
            builder.AppendLine($"[OdooTableName(\"{tableName}\")]");
            builder.AppendLine($"[JsonConverter(typeof({nameof(OdooModelConverter)}))]");
            builder.AppendLine($"public partial class {ConvertOdooNameToDotNet(tableName, existing)}{OdooModelSuffix} : IOdooModel");
            builder.AppendLine("{");


            existing.Clear();
            foreach (var property in properties.OrderBy(p => p.Value.Name))
            {
                builder.AppendLine(string.Empty);

                if (addSummary)
                {
                    builder.AppendLine($"/// <summary>");

                    var relationField = !string.IsNullOrEmpty(property.Value.RelationField) ? $" ({property.Value.RelationField})" : string.Empty;
                    var relation = !string.IsNullOrEmpty(property.Value.Relation) ? $"- {property.Value.Relation}{relationField}" : string.Empty;
                    builder.AppendLine($"/// {property.Key} - {property.Value.Type} {relation} <br />");

                    builder.AppendLine($"/// Required: {property.Value.ResultRequired}, {nameof(property.Value.Readonly)}: {property.Value.Readonly}, {nameof(property.Value.Store)}: {property.Value.Store}, {nameof(property.Value.Sortable)}: {property.Value.Sortable} <br />");

                    if (!string.IsNullOrEmpty(property.Value.Help))
                        builder.AppendLine($"/// {nameof(property.Value.Help)}: {property.Value.Help.Replace("\n", "; ")} <br />");

                    builder.AppendLine($"/// </summary>");
                }

                builder.AppendLine($"[JsonProperty(\"{property.Key}\")]");
                builder.AppendLine($"public {ConvertToDotNetTypeName(property, tableName)}{ConvertToDotNetNullable(property.Value)} {ConvertOdooNameToDotNet(property.Key, existing)} {{ get; set; }}");
            }
            builder.AppendLine("}");

            var selectionsProps = properties.Where(x => x.Value.PropertyValueType == OdooValueTypeEnum.Selection).ToList();

            foreach (var property in selectionsProps)
            {
                existing.Clear();


                builder.AppendLine(string.Empty);
                builder.AppendLine(string.Empty);

                if (addSummary)
                {
                    builder.AppendLine($"/// <summary>");

                    if (!string.IsNullOrEmpty(property.Value.Help))
                        builder.AppendLine($"/// {nameof(property.Value.Help)}: {property.Value.Help.Replace("\n", " <br />\n /// ")}");

                    builder.AppendLine($"/// </summary>");
                }

                builder.AppendLine($"[JsonConverter(typeof(StringEnumConverter))]");
                builder.AppendLine($"public enum {ConvertToDotNetTypeName(property, tableName)}");
                builder.AppendLine("{");

                existing.Clear();
                for (int i = 0; i < property.Value.Selection.Length; i++)
                {
                    string[] item = property.Value.Selection[i];

                    if (i != 0)
                        builder.AppendLine(string.Empty);
                    builder.AppendLine($"[EnumMember(Value = \"{item[0]}\")]");
                    builder.AppendLine($"{ConvertOdooNameToDotNet(item[1], existing)} = {i + 1},");
                }
                builder.AppendLine("}");
            }
            return builder.ToString();
        }

       
        public static string ConvertToDotNetTypeName(KeyValuePair<string, OdooPropertyInfo> property, string tableName)
        {
            switch (property.Value.PropertyValueType)
            {
                case OdooValueTypeEnum.Binary:
                    return "string";
                case OdooValueTypeEnum.Char:
                    return "string";
                case OdooValueTypeEnum.Selection:
                    return $"{ConvertOdooNameToDotNet(property.Value.Name)}{ConvertOdooNameToDotNet(tableName)}{OdooEnumSuffix}";
                case OdooValueTypeEnum.Text:
                    return "string";
                case OdooValueTypeEnum.Html:
                    return "string";

                case OdooValueTypeEnum.Boolean:
                    return "bool";

                case OdooValueTypeEnum.Monetary:
                    return "decimal";

                case OdooValueTypeEnum.Float:
                    return "double";
                case OdooValueTypeEnum.Integer:
                    if (property.Key.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                        return "long";
                    return "int";

                case OdooValueTypeEnum.Date:
                    return "DateTime";
                case OdooValueTypeEnum.Datetime:
                    return "DateTime";

                case OdooValueTypeEnum.Many2One:
                    return "long";
                case OdooValueTypeEnum.Many2OneReference:
                    return "long";
                case OdooValueTypeEnum.Many2Many:
                    return "long[]";
                case OdooValueTypeEnum.One2Many:
                    return "long[]";
                case OdooValueTypeEnum.One2One:
                    return "long";

                case OdooValueTypeEnum.Reference:
                    return ConvertOdooNameToDotNet(property.Value.RelationField) + OdooModelSuffix;

                default:
                    throw new ArgumentException($"Not expected Property Value Type: '{property.Value.PropertyValueType}'");
            }
        }

        public static string ConvertToDotNetNullable(OdooPropertyInfo value)
        {
            if (false
                // required field should not be nullable
                || value.ResultRequired
                // id are mandatory even if they're not marked as required
                || value.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase)
                // one2many and many2many are array even if there are zero relations
                || value.PropertyValueType == OdooValueTypeEnum.Many2Many
                || value.PropertyValueType == OdooValueTypeEnum.One2Many
            )
            {
                return "";
            }
            else
                return "?";
        }

        public static string ConvertOdooNameToDotNet(string odooName, IDictionary<string, bool> existing = null)
        {
            var odooNameCleaned = Regex.Replace(odooName, "[^A-Za-z0-9-]", "_");
            var dotnetKeys = odooNameCleaned
                .Split('_', '-', '.', ',', ' ', ':', ';', '/', '\\', '*', '+', '(', ')', '[', ']')
                .Select(x => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(x))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
            if (dotnetKeys.Count == 0)
                dotnetKeys.Add("None");
            var result = string.Join(string.Empty, dotnetKeys);

            if (existing == null) existing = new Dictionary<string, bool>();

            int i = 1;
            string dedupResult;
            do
            {
                dedupResult = $"{result}{(i < 2 ? "" : i.ToString())}";
                i++;
            } while (existing.ContainsKey(dedupResult));

            existing[dedupResult] = true;
            return dedupResult;
        }

        public static object ConvertToDotNetEnum(Type type, JToken value)
        {
            var field = type.GetFields()
              .Where(x => x.IsLiteral && GetOdooEnumName(x) == value.ToString())
              .FirstOrDefault();

            if (field != null)
                return Enum.Parse(type, field.Name);

            throw new ArgumentException($"Value: '{value}' not found in enum : '{type.FullName}'");
        }

        public static string GetOdooEnumName(FieldInfo fieldInfo)
        {
            var enumAttribute = Attribute.GetCustomAttributes(fieldInfo)
                .FirstOrDefault(x => x is EnumMemberAttribute) as EnumMemberAttribute;
            if (enumAttribute != null)
                return enumAttribute.Value;

            throw new ArgumentException($"Missing atrribute: '{nameof(EnumMemberAttribute)}' for enum '{fieldInfo.FieldType.Name}' - '{fieldInfo.Name}'");
        }
    }
}