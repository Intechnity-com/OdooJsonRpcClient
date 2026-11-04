using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    /// <summary>
    /// Stock quantity of a product in a location - works with Odoo 15 and 19 in integration tests.
    /// </summary>
    [OdooTableName("stock.quant")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationStockQuantOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("product_id")]
        public long? ProductId { get; set; }

        [JsonProperty("location_id")]
        public long? LocationId { get; set; }

        [JsonProperty("quantity")]
        public double? Quantity { get; set; }

        [JsonProperty("inventory_quantity")]
        public double? InventoryQuantity { get; set; }
    }
}
