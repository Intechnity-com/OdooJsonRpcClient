using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("account.payment.term")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class AccountPaymentTermOdooModel : IOdooModel
    {

        // required
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        // account.payment.term.line
        [JsonProperty("line_ids")]
        public long[] LineIds { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }

        // required
        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        // res.users
        [JsonProperty("create_uid")]
        public long? CreateUid { get; set; }

        [JsonProperty("create_date")]
        public DateTime? CreateDate { get; set; }

        // res.users
        [JsonProperty("write_uid")]
        public long? WriteUid { get; set; }

        [JsonProperty("write_date")]
        public DateTime? WriteDate { get; set; }

        [JsonProperty("__last_update")]
        public DateTime? LastUpdate { get; set; }
    }

}