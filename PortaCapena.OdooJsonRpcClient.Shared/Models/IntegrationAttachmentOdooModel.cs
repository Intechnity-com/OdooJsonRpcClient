using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    /// <summary>
    /// Small ir.attachment model - works with Odoo 15 and 19 in integration tests.
    /// </summary>
    [OdooTableName("ir.attachment")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationAttachmentOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// File content as base64.
        /// </summary>
        [JsonProperty("datas")]
        public string Datas { get; set; }

        [JsonProperty("mimetype")]
        public string Mimetype { get; set; }

        [JsonProperty("res_model")]
        public string ResModel { get; set; }

        [JsonProperty("res_id")]
        public long? ResId { get; set; }

        [JsonProperty("file_size")]
        public long? FileSize { get; set; }

        [JsonProperty("store_fname")]
        public string StoreFname { get; set; }
    }
}
