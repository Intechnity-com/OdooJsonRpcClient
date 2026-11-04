using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models.Odoo19
{
    /// <summary>
    /// Invoice "Send &amp; Print" wizard (Odoo 17+) - integration tests.
    /// </summary>
    [OdooTableName("account.move.send.wizard")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationInvoiceSendWizardOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("move_id")]
        public long? MoveId { get; set; }

        [JsonProperty("sending_methods")]
        public string[] SendingMethods { get; set; }
    }
}
