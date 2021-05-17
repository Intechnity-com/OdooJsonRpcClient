using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("res.currency")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class ResCurrencyOdooModel : IOdooModel
    {

        // required
        [JsonProperty("name")]
        public string Name { get; set; }

        // required
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("rate")]
        public double? Rate { get; set; }

        // res.currency.rate
        [JsonProperty("rate_ids")]
        public long[] RateIds { get; set; }

        [JsonProperty("rounding")]
        public double? Rounding { get; set; }

        [JsonProperty("decimal_places")]
        public int? DecimalPlaces { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        [JsonProperty("position")]
        public SymbolPositionResCurrencyOdooEnum? Position { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("currency_unit_label")]
        public string CurrencyUnitLabel { get; set; }

        [JsonProperty("currency_subunit_label")]
        public string CurrencySubunitLabel { get; set; }

        [JsonProperty("display_rounding_warning")]
        public bool? DisplayRoundingWarning { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        // res.users
        [JsonProperty("create_uid")]
        public long? CreateUid { get; set; }

        [JsonProperty("create_date")]
        public DateTime? CreateDate { get; set; }

        // res.users
        [JsonProperty("write_uid")]
        public long? WriteUid { get; set; }

        [JsonProperty("write_date")]
        public DateTime? WriteDate { get; set; }

        [JsonProperty("__last_update")]
        public DateTime? LastUpdate { get; set; }
    }


    // Determines where the currency symbol should be placed after or before the amount.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SymbolPositionResCurrencyOdooEnum
    {
        [EnumMember(Value = "after")]
        AfterAmount = 1,

        [EnumMember(Value = "before")]
        BeforeAmount = 2,
    }

}