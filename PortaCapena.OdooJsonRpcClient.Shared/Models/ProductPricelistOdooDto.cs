using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("product.pricelist")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class ProductPriceListOdooDto : IOdooModel
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        // product.pricelist.item
        [JsonProperty("item_ids")]
        public long[] ItemIds { get; set; }

        // res.currency
        [JsonProperty("currency_id")]
        public long CurrencyId { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }

        [JsonProperty("sequence")]
        public int? Sequence { get; set; }

        // res.country.group
        [JsonProperty("country_group_ids")]
        public long[] CountryGroupIds { get; set; }

        [JsonProperty("discount_policy")]
        public string DiscountPolicy { get; set; }

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

}