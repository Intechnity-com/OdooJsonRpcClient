using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Converters;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooPropertyInfo
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("change_default")]
        public bool ChangeDefault { get; set; }

        [JsonProperty("company_dependent")]
        public bool CompanyDependent { get; set; }

        [JsonProperty("depends")]
        public string[] Depends { get; set; }

        [JsonProperty("manual")]
        public bool Manual { get; set; }

        [JsonProperty("readonly")]
        public bool Readonly { get; set; }

        [JsonProperty("required")]
        public bool ResultRequired { get; set; }

        [JsonProperty("searchable")]
        public bool Searchable { get; set; }

        [JsonProperty("sortable")]
        public bool Sortable { get; set; }

        [JsonProperty("store")]
        public bool Store { get; set; }

        [JsonProperty("string")]
        public string String { get; set; }

        [JsonProperty("help")]
        public string Help { get; set; }

        [JsonProperty("groups")]
        public string Groups { get; set; }

        [JsonProperty("selection")]
        public string[][] Selection { get; set; }

        [JsonProperty("translate")]
        public bool? Translate { get; set; }

        [JsonProperty("trim")]
        public bool? Trim { get; set; }


        [JsonProperty("relation")]
        public string Relation { get; set; }

        [JsonProperty("relation_field")]
        public string RelationField { get; set; }

        [JsonProperty("related")]
        public string[] Related { get; set; }

        [JsonProperty("group_operator")]
        public string GroupOperator { get; set; }

        [JsonProperty("digits")]
        public long[] Digits { get; set; }

        [JsonProperty("attachment")]
        public bool? Attachment { get; set; }

        [JsonIgnore]
        public OdooValueTypeEnum PropertyValueType => ToOdooValueTypeEnum(this.Type);

        public static OdooValueTypeEnum ToOdooValueTypeEnum(string value)
        {
            switch (value)
            {
                case "binary":
                    return OdooValueTypeEnum.Binary;
                case "boolean":
                    return OdooValueTypeEnum.Boolean;
                case "char":
                    return OdooValueTypeEnum.Char;
                case "date":
                    return OdooValueTypeEnum.Date;
                case "datetime":
                    return OdooValueTypeEnum.Datetime;
                case "float":
                    return OdooValueTypeEnum.Float;
                case "integer":
                    return OdooValueTypeEnum.Integer;
                case "many2many":
                    return OdooValueTypeEnum.Many2Many;
                case "many2one":
                    return OdooValueTypeEnum.Many2One;
                case "one2many":
                    return OdooValueTypeEnum.One2Many;
                case "selection":
                    return OdooValueTypeEnum.Selection;
                case "text":
                    return OdooValueTypeEnum.Text;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }
    }


    public enum OdooValueTypeEnum
    {
        Binary,
        Boolean,
        Char,
        Date,
        Datetime,
        Float,
        Integer,
        Many2Many,
        Many2One,
        One2Many,
        Selection,
        Text
    };
}