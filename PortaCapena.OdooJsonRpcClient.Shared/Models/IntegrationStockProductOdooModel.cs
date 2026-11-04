using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    /// <summary>
    /// Small product.product model for stock - works with Odoo 15 and 19 in integration tests (is_storable only in Odoo 18+).
    /// </summary>
    [OdooTableName("product.product")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationStockProductOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("default_code")]
        public string DefaultCode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("is_storable")]
        public bool? IsStorable { get; set; }

        [JsonProperty("qty_available")]
        public double? QtyAvailable { get; set; }
    }
}
