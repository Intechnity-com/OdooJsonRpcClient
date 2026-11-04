using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    /// <summary>
    /// Small product.template model without selection enums - works with Odoo 15 and 19 in integration tests.
    /// </summary>
    [OdooTableName("product.template")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationProductOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("list_price")]
        public double ListPrice { get; set; }

        [JsonProperty("categ_id")]
        public long CategId { get; set; }
    }
}
