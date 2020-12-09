using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("sale.order.line")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class SaleOrderLineOdooDto : IOdooModel
    {

        // sale.order
        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sequence")]
        public int? Sequence { get; set; }

        // account.move.line
        [JsonProperty("invoice_lines")]
        public long[] InvoiceLines { get; set; }

        [JsonProperty("invoice_status")]
        public string InvoiceStatus { get; set; }

        [JsonProperty("price_unit")]
        public double PriceUnit { get; set; }

        [JsonProperty("price_subtotal")]
        public decimal? PriceSubtotal { get; set; }

        [JsonProperty("price_tax")]
        public double? PriceTax { get; set; }

        [JsonProperty("price_total")]
        public decimal? PriceTotal { get; set; }

        [JsonProperty("price_reduce")]
        public double? PriceReduce { get; set; }

        // account.tax
        [JsonProperty("tax_id")]
        public long[] TaxId { get; set; }

        [JsonProperty("price_reduce_taxinc")]
        public decimal? PriceReduceTaxinc { get; set; }

        [JsonProperty("price_reduce_taxexcl")]
        public decimal? PriceReduceTaxexcl { get; set; }

        [JsonProperty("discount")]
        public double? Discount { get; set; }

        // product.product
        [JsonProperty("product_id")]
        public long? ProductId { get; set; }

        // product.template
        [JsonProperty("product_template_id")]
        public long? ProductTemplateId { get; set; }

        [JsonProperty("product_updatable")]
        public bool? ProductUpdatable { get; set; }

        [JsonProperty("product_uom_qty")]
        public double ProductUomQty { get; set; }

        // uom.uom
        [JsonProperty("product_uom")]
        public long? ProductUom { get; set; }

        // uom.category
        [JsonProperty("product_uom_category_id")]
        public long? ProductUomCategoryId { get; set; }

        [JsonProperty("product_uom_readonly")]
        public bool? ProductUomReadonly { get; set; }

        // product.attribute.custom.value
        [JsonProperty("product_custom_attribute_value_ids")]
        public long[] ProductCustomAttributeValueIds { get; set; }

        // product.template.attribute.value
        [JsonProperty("product_no_variant_attribute_value_ids")]
        public long[] ProductNoVariantAttributeValueIds { get; set; }

        [JsonProperty("qty_delivered")]
        public double? QtyDelivered { get; set; }

        [JsonProperty("qty_delivered_manual")]
        public double? QtyDeliveredManual { get; set; }

        [JsonProperty("qty_to_invoice")]
        public double? QtyToInvoice { get; set; }

        [JsonProperty("qty_invoiced")]
        public double? QtyInvoiced { get; set; }

        [JsonProperty("untaxed_amount_invoiced")]
        public decimal? UntaxedAmountInvoiced { get; set; }

        [JsonProperty("untaxed_amount_to_invoice")]
        public decimal? UntaxedAmountToInvoice { get; set; }

        // res.users
        [JsonProperty("salesman_id")]
        public long? SalesmanId { get; set; }

        // res.currency
        [JsonProperty("currency_id")]
        public long? CurrencyId { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }

        // res.partner
        [JsonProperty("order_partner_id")]
        public long? OrderPartnerId { get; set; }

        // account.analytic.tag
        [JsonProperty("analytic_tag_ids")]
        public long[] AnalyticTagIds { get; set; }

        // account.analytic.line
        [JsonProperty("analytic_line_ids")]
        public long[] AnalyticLineIds { get; set; }

        [JsonProperty("is_expense")]
        public bool? IsExpense { get; set; }

        [JsonProperty("is_downpayment")]
        public bool? IsDownpayment { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("customer_lead")]
        public double CustomerLead { get; set; }

        [JsonProperty("display_type")]
        public string DisplayType { get; set; }

        // sale.order.option
        [JsonProperty("sale_order_option_ids")]
        public long[] SaleOrderOptionIds { get; set; }

        // purchase.order.line
        [JsonProperty("purchase_line_ids")]
        public long[] PurchaseLineIds { get; set; }

        [JsonProperty("purchase_line_count")]
        public int? PurchaseLineCount { get; set; }

        [JsonProperty("is_rental")]
        public bool? IsRental { get; set; }

        [JsonProperty("qty_returned")]
        public double? QtyReturned { get; set; }

        [JsonProperty("pickup_date")]
        public DateTime? PickupDate { get; set; }

        [JsonProperty("return_date")]
        public DateTime? ReturnDate { get; set; }

        [JsonProperty("reservation_begin")]
        public DateTime? ReservationBegin { get; set; }

        [JsonProperty("is_late")]
        public bool? IsLate { get; set; }

        [JsonProperty("is_product_rentable")]
        public bool? IsProductRentable { get; set; }

        [JsonProperty("rental_updatable")]
        public bool? RentalUpdatable { get; set; }

        [JsonProperty("qty_delivered_method")]
        public string QtyDeliveredMethod { get; set; }

        // product.packaging
        [JsonProperty("product_packaging")]
        public long? ProductPackaging { get; set; }

        // stock.location.route
        [JsonProperty("route_id")]
        public long? RouteId { get; set; }

        // stock.move
        [JsonProperty("move_ids")]
        public long[] MoveIds { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("virtual_available_at_date")]
        public double? VirtualAvailableAtDate { get; set; }

        [JsonProperty("scheduled_date")]
        public DateTime? ScheduledDate { get; set; }

        [JsonProperty("forecast_expected_date")]
        public DateTime? ForecastExpectedDate { get; set; }

        [JsonProperty("free_qty_today")]
        public double? FreeQtyToday { get; set; }

        [JsonProperty("qty_available_today")]
        public double? QtyAvailableToday { get; set; }

        // stock.warehouse
        [JsonProperty("warehouse_id")]
        public long? WarehouseId { get; set; }

        [JsonProperty("qty_to_deliver")]
        public double? QtyToDeliver { get; set; }

        [JsonProperty("is_mto")]
        public bool? IsMto { get; set; }

        [JsonProperty("display_qty_widget")]
        public bool? DisplayQtyWidget { get; set; }

        [JsonProperty("tracking")]
        public string Tracking { get; set; }

        // stock.production.lot
        [JsonProperty("reserved_lot_ids")]
        public long[] ReservedLotIds { get; set; }

        // stock.production.lot
        [JsonProperty("pickedup_lot_ids")]
        public long[] PickedupLotIds { get; set; }

        // stock.production.lot
        [JsonProperty("returned_lot_ids")]
        public long[] ReturnedLotIds { get; set; }

        // stock.production.lot
        [JsonProperty("unavailable_lot_ids")]
        public long[] UnavailableLotIds { get; set; }

        // sale.subscription
        [JsonProperty("subscription_id")]
        public long? SubscriptionId { get; set; }

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