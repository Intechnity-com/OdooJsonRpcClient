using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Tests.Models
{
    [OdooTableName("product.template")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class OdooProductTemplate : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("description_purchase")]
        public string DescriptionPurchase { get; set; }

        [JsonProperty("description_sale")]
        public string DescriptionSale { get; set; }

        [JsonProperty("rental")]
        public bool Rental { get; set; }

        [JsonProperty("categ_id")]
        public int? CategId { get; set; }

        [JsonProperty("currency_id")]
        public int? CurrencyId { get; set; }

        [JsonProperty("cost_currency_id")]
        public int? CostCurrencyId { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("list_price")]
        public double ListPrice { get; set; }

        [JsonProperty("lst_price")]
        public double LstPrice { get; set; }

        [JsonProperty("standard_price")]
        public double StandardPrice { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("volume_uom_name")]
        public string VolumeUomName { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("weight_uom_name")]
        public string WeightUomName { get; set; }

        [JsonProperty("sale_ok")]
        public bool SaleOk { get; set; }

        [JsonProperty("purchase_ok")]
        public bool PurchaseOk { get; set; }

        [JsonProperty("pricelist_id")]
        public bool PricelistId { get; set; }

        [JsonProperty("uom_id")]
        public int? UomId { get; set; }

        [JsonProperty("uom_name")]
        public string UomName { get; set; }

        [JsonProperty("uom_po_id")]
        public int? UomPoId { get; set; }

        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        [JsonProperty("packaging_ids")]
        public int[] PackagingIds { get; set; }

        [JsonProperty("seller_ids")]
        public int[] SellerIds { get; set; }

        [JsonProperty("variant_seller_ids")]
        public int[] VariantSellerIds { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("color")]
        public int Color { get; set; }

        [JsonProperty("is_product_variant")]
        public bool IsProductVariant { get; set; }

        [JsonProperty("attribute_line_ids")]
        public int[] AttributeLineIds { get; set; }

        [JsonProperty("valid_product_template_attribute_line_ids")]
        public int[] ValidProductTemplateAttributeLineIds { get; set; }

        [JsonProperty("product_variant_ids")]
        public int[] ProductVariantIds { get; set; }

        [JsonProperty("product_variant_id")]
        public int? ProductVariantId { get; set; }

        [JsonProperty("product_variant_count")]
        public int ProductVariantCount { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("default_code")]
        public string DefaultCode { get; set; }

        [JsonProperty("pricelist_item_count")]
        public int PricelistItemCount { get; set; }

        [JsonProperty("can_image_1024_be_zoomed")]
        public bool CanImage1024BeZoomed { get; set; }

        [JsonProperty("has_configurable_attributes")]
        public bool HasConfigurableAttributes { get; set; }

        [JsonProperty("responsible_id")]
        public int? ResponsibleId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("property_stock_production")]
        public int? PropertyStockProduction { get; set; }

        [JsonProperty("property_stock_inventory")]
        public int? PropertyStockInventory { get; set; }

        [JsonProperty("sale_delay")]
        public double SaleDelay { get; set; }

        [JsonProperty("tracking")]
        public string Tracking { get; set; }

        [JsonProperty("description_picking")]
        public bool DescriptionPicking { get; set; }

        [JsonProperty("description_pickingout")]
        public bool DescriptionPickingout { get; set; }

        [JsonProperty("description_pickingin")]
        public bool DescriptionPickingin { get; set; }

        [JsonProperty("qty_available")]
        public double QtyAvailable { get; set; }

        [JsonProperty("virtual_available")]
        public double VirtualAvailable { get; set; }

        [JsonProperty("incoming_qty")]
        public double IncomingQty { get; set; }

        [JsonProperty("outgoing_qty")]
        public double OutgoingQty { get; set; }

        [JsonProperty("location_id")]
        public bool LocationId { get; set; }

        [JsonProperty("warehouse_id")]
        public bool WarehouseId { get; set; }

        [JsonProperty("nbr_reordering_rules")]
        public int NbrReorderingRules { get; set; }

        [JsonProperty("reordering_min_qty")]
        public double ReorderingMinQty { get; set; }

        [JsonProperty("reordering_max_qty")]
        public double ReorderingMaxQty { get; set; }

        [JsonProperty("route_from_categ_ids")]
        public int? RouteFromCategIds { get; set; }

        [JsonProperty("taxes_id")]
        public int[] TaxesId { get; set; }

        [JsonProperty("supplier_taxes_id")]
        public int[] SupplierTaxesId { get; set; }

        [JsonProperty("property_account_income_id")]
        public bool PropertyAccountIncomeId { get; set; }

        [JsonProperty("property_account_expense_id")]
        public bool PropertyAccountExpenseId { get; set; }

        [JsonProperty("property_account_creditor_price_difference")]
        public bool PropertyAccountCreditorPriceDifference { get; set; }

        [JsonProperty("purchased_product_qty")]
        public double PurchasedProductQty { get; set; }

        [JsonProperty("purchase_method")]
        public string PurchaseMethod { get; set; }

        [JsonProperty("purchase_line_warn")]
        public string PurchaseLineWarn { get; set; }

        [JsonProperty("purchase_line_warn_msg")]
        public bool PurchaseLineWarnMsg { get; set; }

        [JsonProperty("cost_method")]
        public string CostMethod { get; set; }

        [JsonProperty("valuation")]
        public string Valuation { get; set; }

        [JsonProperty("route_ids")]
        public int[] RouteIds { get; set; }

        [JsonProperty("service_type")]
        public string ServiceType { get; set; }

        [JsonProperty("sale_line_warn")]
        public string SaleLineWarn { get; set; }

        [JsonProperty("sale_line_warn_msg")]
        public bool SaleLineWarnMsg { get; set; }

        [JsonProperty("expense_policy")]
        public string ExpensePolicy { get; set; }

        [JsonProperty("visible_expense_policy")]
        public bool VisibleExpensePolicy { get; set; }

        [JsonProperty("sales_count")]
        public double SalesCount { get; set; }

        [JsonProperty("invoice_policy")]
        public string InvoicePolicy { get; set; }

        [JsonProperty("service_to_purchase")]
        public bool ServiceToPurchase { get; set; }

        [JsonProperty("image_1920")]
        public string Image1920 { get; set; }

        [JsonProperty("image_1024")]
        public string Image1024 { get; set; }

        [JsonProperty("image_512")]
        public string Image512 { get; set; }

        [JsonProperty("image_256")]
        public string Image256 { get; set; }

        [JsonProperty("image_128")]
        public string Image128 { get; set; }

        [JsonProperty("activity_ids")]
        public int[] ActivityIds { get; set; }

        [JsonProperty("activity_state")]
        public string ActivityState { get; set; }

        [JsonProperty("activity_user_id")]
        public int? ActivityUserId { get; set; }

        [JsonProperty("activity_type_id")]
        public int? ActivityTypeId { get; set; }

        [JsonProperty("activity_date_deadline")]
        public DateTime? ActivityDateDeadline { get; set; }

        [JsonProperty("activity_summary")]
        public bool ActivitySummary { get; set; }

        [JsonProperty("activity_exception_decoration")]
        public string ActivityExceptionDecoration { get; set; }

        [JsonProperty("activity_exception_icon")]
        public string ActivityExceptionIcon { get; set; }

        [JsonProperty("message_is_follower")]
        public bool MessageIsFollower { get; set; }

        [JsonProperty("message_follower_ids")]
        public int[] MessageFollowerIds { get; set; }

        [JsonProperty("message_partner_ids")]
        public int[] MessagePartnerIds { get; set; }

        [JsonProperty("message_channel_ids")]
        public int[] MessageChannelIds { get; set; }

        [JsonProperty("message_ids")]
        public int[] MessageIds { get; set; }

        [JsonProperty("message_unread")]
        public bool MessageUnread { get; set; }

        [JsonProperty("message_unread_counter")]
        public int MessageUnreadCounter { get; set; }

        [JsonProperty("message_needaction")]
        public bool MessageNeedaction { get; set; }

        [JsonProperty("message_needaction_counter")]
        public int MessageNeedactionCounter { get; set; }

        [JsonProperty("message_has_error")]
        public bool MessageHasError { get; set; }

        [JsonProperty("message_has_error_counter")]
        public int MessageHasErrorCounter { get; set; }

        [JsonProperty("message_attachment_count")]
        public int MessageAttachmentCount { get; set; }

        [JsonProperty("message_main_attachment_id")]
        public bool MessageMainAttachmentId { get; set; }

        [JsonProperty("website_message_ids")]
        public int[] WebsiteMessageIds { get; set; }

        [JsonProperty("message_has_sms_error")]
        public bool MessageHasSmsError { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("create_uid")]
        public int? CreateUid { get; set; }

        [JsonProperty("create_date")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("write_uid")]
        public int? WriteUid { get; set; }

        [JsonProperty("write_date")]
        public DateTime WriteDate { get; set; }

        [JsonProperty("__last_update")]
        public DateTime? LastUpdate { get; set; }
    }
}