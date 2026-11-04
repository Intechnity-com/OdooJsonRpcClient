using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    /// <summary>
    /// Small res.partner model without selection enums - works with Odoo 15 and 19 in integration tests.
    /// </summary>
    [OdooTableName("res.partner")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationPartnerOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("is_company")]
        public bool IsCompany { get; set; }

        [JsonProperty("country_id")]
        public long? CountryId { get; set; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

        [JsonProperty("category_id")]
        public long[] CategoryId { get; set; }
    }
}
