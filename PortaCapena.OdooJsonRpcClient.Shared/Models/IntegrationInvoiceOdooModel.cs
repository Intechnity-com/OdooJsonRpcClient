using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    /// <summary>
    /// Small account.move model - works with Odoo 15 and 19 in integration tests (invoice_pdf_report_file only in Odoo 17+).
    /// </summary>
    [OdooTableName("account.move")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class IntegrationInvoiceOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("move_type")]
        public string MoveType { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("invoice_pdf_report_file")]
        public string InvoicePdfReportFile { get; set; }

        [JsonProperty("partner_id")]
        public long? PartnerId { get; set; }

        [JsonProperty("invoice_line_ids")]
        public long[] InvoiceLineIds { get; set; }

        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }
    }
}
