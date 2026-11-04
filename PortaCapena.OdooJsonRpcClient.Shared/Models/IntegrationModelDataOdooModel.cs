using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    /// <summary>
    /// External ids (xml ids) of records - integration tests.
    /// </summary>
    [OdooTableName("ir.model.data")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationModelDataOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("module")]
        public string Module { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("res_id")]
        public long ResId { get; set; }
    }
}
