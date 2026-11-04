using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    /// <summary>
    /// Small res.country model - works with Odoo 15 and 19 in integration tests.
    /// </summary>
    [OdooTableName("res.country")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationCountryOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
