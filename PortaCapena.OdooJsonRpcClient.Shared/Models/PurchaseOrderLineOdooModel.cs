using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("purchase.order.line")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class PurchaseOrderLineOdooModel : IOdooModel
    {

        // required
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sequence")]
        public int? Sequence { get; set; }

        // required
        [JsonProperty("product_qty")]
        public double ProductQty { get; set; }

        [JsonProperty("product_uom_qty")]
        public double? ProductUomQty { get; set; }

        [JsonProperty("date_planned")]
        public DateTime? DatePlanned { get; set; }

        // account.tax
        [JsonProperty("taxes_id")]
        public long[] TaxesId { get; set; }

        // uom.uom
        [JsonProperty("product_uom")]
        public long? ProductUom { get; set; }

        // uom.category
        [JsonProperty("product_uom_category_id")]
        public long? ProductUomCategoryId { get; set; }

        // product.product
        [JsonProperty("product_id")]
        public long? ProductId { get; set; }

        [JsonProperty("product_type")]
        public ProductTypePurchaseOrderLineOdooEnum? ProductType { get; set; }

        // required
        [JsonProperty("price_unit")]
        public double PriceUnit { get; set; }

        [JsonProperty("price_subtotal")]
        public decimal? PriceSubtotal { get; set; }

        [JsonProperty("price_total")]
        public decimal? PriceTotal { get; set; }

        [JsonProperty("price_tax")]
        public double? PriceTax { get; set; }

        // purchase.order
        // required
        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        // account.analytic.account
        [JsonProperty("account_analytic_id")]
        public long? AccountAnalyticId { get; set; }

        // account.analytic.tag
        [JsonProperty("analytic_tag_ids")]
        public long[] AnalyticTagIds { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }

        [JsonProperty("state")]
        public StatusPurchaseOrderLineOdooEnum? State { get; set; }

        // account.move.line
        [JsonProperty("invoice_lines")]
        public long[] InvoiceLines { get; set; }

        [JsonProperty("qty_invoiced")]
        public double? QtyInvoiced { get; set; }

        [JsonProperty("qty_received_method")]
        public ReceivedQtyMethodPurchaseOrderLineOdooEnum? QtyReceivedMethod { get; set; }

        [JsonProperty("qty_received")]
        public double? QtyReceived { get; set; }

        [JsonProperty("qty_received_manual")]
        public double? QtyReceivedManual { get; set; }

        [JsonProperty("qty_to_invoice")]
        public double? QtyToInvoice { get; set; }

        // res.partner
        [JsonProperty("partner_id")]
        public long? PartnerId { get; set; }

        // res.currency
        [JsonProperty("currency_id")]
        public long? CurrencyId { get; set; }

        [JsonProperty("date_order")]
        public DateTime? DateOrder { get; set; }

        [JsonProperty("display_type")]
        public DisplayTypePurchaseOrderLineOdooEnum? DisplayType { get; set; }

        // sale.order
        [JsonProperty("sale_order_id")]
        public long? SaleOrderId { get; set; }

        // sale.order.line
        [JsonProperty("sale_line_id")]
        public long? SaleLineId { get; set; }

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


    // A storable product is a product for which you manage stock. The Inventory app has to be installed.
    // A consumable product is a product for which stock is not managed.
    // A service is a non-material product you provide.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductTypePurchaseOrderLineOdooEnum
    {
        [EnumMember(Value = "consu")]
        Consumable = 1,

        [EnumMember(Value = "service")]
        Service = 2,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusPurchaseOrderLineOdooEnum
    {
        [EnumMember(Value = "draft")]
        RFQ = 1,

        [EnumMember(Value = "sent")]
        RFQSent = 2,

        [EnumMember(Value = "to approve")]
        ToApprove = 3,

        [EnumMember(Value = "purchase")]
        PurchaseOrder = 4,

        [EnumMember(Value = "done")]
        Locked = 5,

        [EnumMember(Value = "cancel")]
        Cancelled = 6,
    }


    // According to product configuration, the received quantity can be automatically computed by mechanism :
    //   - Manual: the quantity is set manually on the line
    //   - Stock Moves: the quantity comes from confirmed pickings
    // 
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReceivedQtyMethodPurchaseOrderLineOdooEnum
    {
        [EnumMember(Value = "manual")]
        Manual = 1,
    }


    // Technical field for UX purpose.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DisplayTypePurchaseOrderLineOdooEnum
    {
        [EnumMember(Value = "line_section")]
        Section = 1,

        [EnumMember(Value = "line_note")]
        Note = 2,
    }

}