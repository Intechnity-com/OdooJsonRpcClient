using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models.Fake
{
    [OdooTableName("test.partner")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class TestPartnerOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_company")]
        public bool IsCompany { get; set; }

        [JsonProperty("credit_limit")]
        public double? CreditLimit { get; set; }

        [JsonProperty("balance")]
        public decimal? Balance { get; set; }

        [JsonProperty("color")]
        public int? Color { get; set; }

        [JsonProperty("country_id")]
        public long? CountryId { get; set; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

        [JsonProperty("category_id")]
        public long[] CategoryId { get; set; }

        [JsonProperty("create_date")]
        public DateTime? CreateDate { get; set; }

        [JsonProperty("type")]
        public TestPartnerTypeOdooEnum? Type { get; set; }

        [JsonProperty("properties")]
        public string Properties { get; set; }

        [JsonProperty("raw")]
        public object Raw { get; set; }

        [JsonIgnore]
        public string Computed => Name?.ToUpperInvariant();
    }
}
