using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    /// <summary>
    /// Small account.move.line model - works with Odoo 15 and 19 in integration tests.
    /// </summary>
    [OdooTableName("account.move.line")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationMoveLineOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("move_id")]
        public long? MoveId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("account_id")]
        public long? AccountId { get; set; }

        [JsonProperty("debit")]
        public decimal? Debit { get; set; }

        [JsonProperty("credit")]
        public decimal? Credit { get; set; }
    }
}
