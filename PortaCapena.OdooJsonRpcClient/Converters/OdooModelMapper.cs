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

            if (value == null)
                return false;

            // untyped targets get the raw json value (booleans skipped as before - odoo sends `false` for empty values)
            if ((dotnetType == typeof(object) || typeof(JToken).IsAssignableFrom(dotnetType)) && value.Type != JTokenType.Boolean)
            {
                result = dotnetType == typeof(object) || dotnetType.IsInstanceOfType(value) ? value : value.ToObject(dotnetType);
                return true;
            }

            switch (value.Type)
            {
                case JTokenType.Null:
                case JTokenType.Undefined:
                    return false;

                case JTokenType.Boolean when dotnetType != typeof(bool) && dotnetType != typeof(bool?):
                    return false;

                case JTokenType.Boolean:
                    result = value.ToObject(dotnetType);
                    return true;

                case JTokenType.Integer when dotnetType == typeof(bool) || dotnetType == typeof(bool?):
                    result = value.ToObject(dotnetType);
                    return true;

                case JTokenType.Integer when IsNumeric(dotnetType):
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
                    result = DateTime.Parse(stringTime, CultureInfo.InvariantCulture);
                    return true;

                // Enum part accept both string and integer to avoid issue #54
                case JTokenType.String when dotnetType.IsEnum:
                case JTokenType.Integer when dotnetType.IsEnum:
                    result = ConvertToDotNetEnum(dotnetType, value);
                    return true;

                case JTokenType.String when dotnetType.IsGenericType && dotnetType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                                            dotnetType.GenericTypeArguments.Length == 1 && dotnetType.GenericTypeArguments[0].IsEnum:
                case JTokenType.Integer when dotnetType.IsGenericType && dotnetType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
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

                    // many2one: [id, "display name"]
                    if (value.Count() <= 2 && IsInteger(dotnetType) && value.First.Type == JTokenType.Integer)
                    {
                        result = value.First.ToObject(dotnetType);
                        return true;
                    }

                    // many2one to a model with text ids, e.g. account.root in Odoo 18+: ["60", "60"]
                    if (value.Count() <= 2 && IsInteger(dotnetType) && value.First.Type == JTokenType.String
                        && long.TryParse(value.First.Value<string>(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var textId))
                    {
                        result = Convert.ChangeType(textId, Nullable.GetUnderlyingType(dotnetType) ?? dotnetType, CultureInfo.InvariantCulture);
                        return true;
                    }

                    // e.g. json / properties fields mapped to string
                    if (dotnetType == typeof(string))
                    {
                        result = value.ToString(Formatting.None);
                        return true;
                    }

                    throw new Exception($"Not implemented json mapping value: '{value.Path}' ({value.ToString(Formatting.None)}) to {dotnetType.Name}");

                case JTokenType.Object when dotnetType == typeof(string):
                    result = value.ToString(Formatting.None);
                    return true;

                default:
                    throw new Exception($"Not implemented json mapping value: '{value.Path}' ({value.Type}) to {dotnetType.Name}");
            }
        }


        private static bool IsInteger(Type type)
        {
            var underlying = Nullable.GetUnderlyingType(type) ?? type;
            return underlying == typeof(int) || underlying == typeof(long) || underlying == typeof(short);
        }

        private static bool IsNumeric(Type type)
        {
            var underlying = Nullable.GetUnderlyingType(type) ?? type;
            return IsInteger(underlying) || underlying == typeof(double) || underlying == typeof(decimal) || underlying == typeof(float);
        }

        public static string GetDotNetModel(string tableName, Dictionary<string, OdooPropertyInfo> properties, bool addSummary = true)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"[OdooTableName(\"{tableName}\")]");
            builder.AppendLine($"[JsonConverter(typeof({nameof(OdooModelConverter)}))]");
            builder.AppendLine($"public class {ConvertOdooNameToDotNet(tableName)}{OdooModelSuffix} : IOdooModel");
            builder.AppendLine("{");

            foreach (var property in properties)
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
                builder.AppendLine($"public {ConvertToDotNetPropertyTypeName(property, tableName)} {ConvertOdooNameToDotNet(property.Key)} {{ get; set; }}");
            }
            builder.AppendLine("}");

            var selectionsProps = properties.Where(x => x.Value.PropertyValueType == OdooValueTypeEnum.Selection).ToList();

            foreach (var property in selectionsProps)
            {
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
                builder.AppendLine($"public enum {ConvertOdooNameToDotNet(property.Value.String)}{ConvertOdooNameToDotNet(tableName)}{OdooEnumSuffix}");
                builder.AppendLine("{");

                var memberNames = new HashSet<string>();
                for (int i = 0; i < property.Value.Selection.Length; i++)
                {
                    string[] item = property.Value.Selection[i];

                    // C# names cannot start with a digit (e.g. labels '1', '2') - issue #122
                    var memberName = ConvertOdooNameToDotNet(item[1]);
                    if (char.IsDigit(memberName[0]))
                        memberName = "Value" + memberName;

                    // different labels can give the same name (e.g. 'Etc/GMT-0' and 'Etc/GMT0') - issue #50
                    var uniqueMemberName = memberName;
                    for (var number = 2; !memberNames.Add(uniqueMemberName); number++)
                        uniqueMemberName = memberName + number;

                    if (i != 0)
                        builder.AppendLine(string.Empty);
                    builder.AppendLine($"[EnumMember(Value = \"{item[0]}\")]");
                    builder.AppendLine($"{uniqueMemberName} = {i + 1},");
                }
                builder.AppendLine("}");
            }
            return builder.ToString();
        }

        public static string ConvertToDotNetPropertyTypeName(KeyValuePair<string, OdooPropertyInfo> property, string tableName)
        {
            switch (property.Value.PropertyValueType)
            {
                case OdooValueTypeEnum.Binary:
                    return "string";
                case OdooValueTypeEnum.Char:
                    return "string";
                case OdooValueTypeEnum.Selection:
                    return $"{ConvertOdooNameToDotNet(property.Value.String)}{ConvertOdooNameToDotNet(tableName)}{OdooEnumSuffix}{(property.Value.ResultRequired ? "" : "?")}";
                case OdooValueTypeEnum.Text:
                    return "string";
                case OdooValueTypeEnum.Html:
                    return "string";

                case OdooValueTypeEnum.Boolean:
                    return property.Value.ResultRequired ? "bool" : "bool?";

                case OdooValueTypeEnum.Monetary:
                    return property.Value.ResultRequired ? "decimal" : "decimal?";

                case OdooValueTypeEnum.Float:
                    return property.Value.ResultRequired ? "double" : "double?";
                case OdooValueTypeEnum.Integer:
                    if (property.Key.ToString().ToLower() == "id")
                        return "long";
                    return property.Value.ResultRequired ? "int" : "int?";

                case OdooValueTypeEnum.Date:
                    return property.Value.ResultRequired ? "DateTime" : "DateTime?";
                case OdooValueTypeEnum.Datetime:
                    return property.Value.ResultRequired ? "DateTime" : "DateTime?";

                case OdooValueTypeEnum.Many2One:
                    return property.Value.ResultRequired ? "long" : "long?";
                case OdooValueTypeEnum.Many2OneReference:
                    return property.Value.ResultRequired ? "long" : "long?";
                case OdooValueTypeEnum.Many2Many:
                    return "long[]";
                case OdooValueTypeEnum.One2Many:
                    return "long[]";
                case OdooValueTypeEnum.One2One:
                    return property.Value.ResultRequired ? "long" : "long?";

                case OdooValueTypeEnum.Reference:
                    return ConvertOdooNameToDotNet(property.Value.RelationField) + OdooModelSuffix;

                case OdooValueTypeEnum.Properties:
                    return "string";
                case OdooValueTypeEnum.PropertiesDefinition:
                    return "string";

                default:
                    throw new ArgumentException($"Not expected Property Value Type: '{property.Value.PropertyValueType}'");
            }
        }

        public static string ConvertOdooNameToDotNet(string odooName)
        {
            odooName = odooName.Replace("+", "Plus");
            var odooNameCleaned = Regex.Replace(odooName, "[^A-Za-z0-9-]", "_");
            var dotnetKeys = odooNameCleaned
                .Split('_', '-', '.', ',', ' ', ':', ';', '/', '\\', '*', '+', '(', ')', '[', ']')
                .Select(x => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(x))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
            if (dotnetKeys.Count == 0)
                dotnetKeys.Add("None");
            return string.Join(string.Empty, dotnetKeys);
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

            throw new ArgumentException($"Missing attribute: '{nameof(EnumMemberAttribute)}' for enum '{fieldInfo.FieldType.Name}' - '{fieldInfo.Name}'");
        }

        /// <summary>
        /// Maps values returned by Odoo as a list (e.g. <c>[id, "display name"]</c> from <c>name_search</c> / <c>name_create</c>)
        /// to a record: value at position N is stored under N-th key. Missing values are null, extra values are ignored.
        /// </summary>
        internal static OdooDictionaryModel ToOdooDictionaryModel(string tableName, string[] keys, object[] values)
        {
            var model = new OdooDictionaryModel(tableName);
            for (var i = 0; i < keys.Length; i++)
                model[keys[i]] = values != null && i < values.Length ? values[i] : null;
            return model;
        }
    }
}