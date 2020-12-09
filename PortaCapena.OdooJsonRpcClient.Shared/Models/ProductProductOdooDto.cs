using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("product.product")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class ProductProductOdooDto : IOdooModel
    {
        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("price_extra")]
        public double? PriceExtra { get; set; }

        [JsonProperty("lst_price")]
        public double? LstPrice { get; set; }

        [JsonProperty("default_code")]
        public string DefaultCode { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("partner_ref")]
        public string PartnerRef { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        // product.template
        [JsonProperty("product_tmpl_id")]
        public int ProductTmplId { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        // product.template.attribute.value
        [JsonProperty("product_template_attribute_value_ids")]
        public int[] ProductTemplateAttributeValueIds { get; set; }

        [JsonProperty("combination_indices")]
        public string CombinationIndices { get; set; }

        [JsonProperty("is_product_variant")]
        public bool? IsProductVariant { get; set; }

        [JsonProperty("standard_price")]
        public double? StandardPrice { get; set; }

        [JsonProperty("volume")]
        public double? Volume { get; set; }

        [JsonProperty("weight")]
        public double? Weight { get; set; }

        [JsonProperty("pricelist_item_count")]
        public int? PricelistItemCount { get; set; }

        // product.packaging
        [JsonProperty("packaging_ids")]
        public int[] PackagingIds { get; set; }

        [JsonProperty("image_variant_1920")]
        public string ImageVariant1920 { get; set; }

        [JsonProperty("image_variant_1024")]
        public string ImageVariant1024 { get; set; }

        [JsonProperty("image_variant_512")]
        public string ImageVariant512 { get; set; }

        [JsonProperty("image_variant_256")]
        public string ImageVariant256 { get; set; }

        [JsonProperty("image_variant_128")]
        public string ImageVariant128 { get; set; }

        [JsonProperty("can_image_variant_1024_be_zoomed")]
        public bool? CanImageVariant1024BeZoomed { get; set; }

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

        [JsonProperty("can_image_1024_be_zoomed")]
        public bool? CanImage1024BeZoomed { get; set; }

        // stock.quant
        [JsonProperty("stock_quant_ids")]
        public int[] StockQuantIds { get; set; }

        // stock.move
        [JsonProperty("stock_move_ids")]
        public int[] StockMoveIds { get; set; }

        [JsonProperty("qty_available")]
        public double? QtyAvailable { get; set; }

        [JsonProperty("virtual_available")]
        public double? VirtualAvailable { get; set; }

        [JsonProperty("free_qty")]
        public double? FreeQty { get; set; }

        [JsonProperty("incoming_qty")]
        public double? IncomingQty { get; set; }

        [JsonProperty("outgoing_qty")]
        public double? OutgoingQty { get; set; }

        // stock.warehouse.orderpoint
        [JsonProperty("orderpoint_ids")]
        public int[] OrderpointIds { get; set; }

        [JsonProperty("nbr_reordering_rules")]
        public int? NbrReorderingRules { get; set; }

        [JsonProperty("reordering_min_qty")]
        public double? ReorderingMinQty { get; set; }

        [JsonProperty("reordering_max_qty")]
        public double? ReorderingMaxQty { get; set; }

        // stock.putaway.rule
        [JsonProperty("putaway_rule_ids")]
        public int[] PutawayRuleIds { get; set; }

        [JsonProperty("purchased_product_qty")]
        public double? PurchasedProductQty { get; set; }

        [JsonProperty("value_svl")]
        public double? ValueSvl { get; set; }

        [JsonProperty("quantity_svl")]
        public double? QuantitySvl { get; set; }

        // stock.valuation.layer
        [JsonProperty("stock_valuation_layer_ids")]
        public int[] StockValuationLayerIds { get; set; }

        [JsonProperty("valuation")]
        public string Valuation { get; set; }

        [JsonProperty("cost_method")]
        public string CostMethod { get; set; }

        // purchase.order.line
        [JsonProperty("purchase_order_line_ids")]
        public int[] PurchaseOrderLineIds { get; set; }

        [JsonProperty("sales_count")]
        public double? SalesCount { get; set; }

        [JsonProperty("qty_in_rent")]
        public double? QtyInRent { get; set; }

        // mail.activity
        [JsonProperty("activity_ids")]
        public int[] ActivityIds { get; set; }

        [JsonProperty("activity_state")]
        public string ActivityState { get; set; }

        // res.users
        [JsonProperty("activity_user_id")]
        public int? ActivityUserId { get; set; }

        // mail.activity.type
        [JsonProperty("activity_type_id")]
        public int? ActivityTypeId { get; set; }

        [JsonProperty("activity_type_icon")]
        public string ActivityTypeIcon { get; set; }

        [JsonProperty("activity_date_deadline")]
        public DateTime? ActivityDateDeadline { get; set; }

        [JsonProperty("activity_summary")]
        public string ActivitySummary { get; set; }

        [JsonProperty("activity_exception_decoration")]
        public string ActivityExceptionDecoration { get; set; }

        [JsonProperty("activity_exception_icon")]
        public string ActivityExceptionIcon { get; set; }

        [JsonProperty("message_is_follower")]
        public bool? MessageIsFollower { get; set; }

        // mail.followers
        [JsonProperty("message_follower_ids")]
        public int[] MessageFollowerIds { get; set; }

        // res.partner
        [JsonProperty("message_partner_ids")]
        public int[] MessagePartnerIds { get; set; }

        // mail.channel
        [JsonProperty("message_channel_ids")]
        public int[] MessageChannelIds { get; set; }

        // mail.message
        [JsonProperty("message_ids")]
        public int[] MessageIds { get; set; }

        [JsonProperty("message_unread")]
        public bool? MessageUnread { get; set; }

        [JsonProperty("message_unread_counter")]
        public int? MessageUnreadCounter { get; set; }

        [JsonProperty("message_needaction")]
        public bool? MessageNeedaction { get; set; }

        [JsonProperty("message_needaction_counter")]
        public int? MessageNeedactionCounter { get; set; }

        [JsonProperty("message_has_error")]
        public bool? MessageHasError { get; set; }

        [JsonProperty("message_has_error_counter")]
        public int? MessageHasErrorCounter { get; set; }

        [JsonProperty("message_attachment_count")]
        public int? MessageAttachmentCount { get; set; }

        // ir.attachment
        [JsonProperty("message_main_attachment_id")]
        public int? MessageMainAttachmentId { get; set; }

        // mail.message
        [JsonProperty("website_message_ids")]
        public int[] WebsiteMessageIds { get; set; }

        [JsonProperty("message_has_sms_error")]
        public bool? MessageHasSmsError { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        // res.users
        [JsonProperty("create_uid")]
        public int? CreateUid { get; set; }

        [JsonProperty("create_date")]
        public DateTime? CreateDate { get; set; }

        // res.users
        [JsonProperty("write_uid")]
        public int? WriteUid { get; set; }

        [JsonProperty("write_date")]
        public DateTime? WriteDate { get; set; }

        [JsonProperty("__last_update")]
        public DateTime? LastUpdate { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sequence")]
        public int? Sequence { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("description_purchase")]
        public string DescriptionPurchase { get; set; }

        [JsonProperty("description_sale")]
        public string DescriptionSale { get; set; }

        // product.category
        [JsonProperty("categ_id")]
        public int CategId { get; set; }

        // res.currency
        [JsonProperty("currency_id")]
        public int? CurrencyId { get; set; }

        // res.currency
        [JsonProperty("cost_currency_id")]
        public int? CostCurrencyId { get; set; }

        [JsonProperty("list_price")]
        public double? ListPrice { get; set; }

        [JsonProperty("volume_uom_name")]
        public string VolumeUomName { get; set; }

        [JsonProperty("weight_uom_name")]
        public string WeightUomName { get; set; }

        [JsonProperty("sale_ok")]
        public bool? SaleOk { get; set; }

        [JsonProperty("purchase_ok")]
        public bool? PurchaseOk { get; set; }

        // product.pricelist
        [JsonProperty("pricelist_id")]
        public int? PricelistId { get; set; }

        // uom.uom
        [JsonProperty("uom_id")]
        public int UomId { get; set; }

        [JsonProperty("uom_name")]
        public string UomName { get; set; }

        // uom.uom
        [JsonProperty("uom_po_id")]
        public int UomPoId { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }

        // product.supplierinfo
        [JsonProperty("seller_ids")]
        public int[] SellerIds { get; set; }

        // product.supplierinfo
        [JsonProperty("variant_seller_ids")]
        public int[] VariantSellerIds { get; set; }

        [JsonProperty("color")]
        public int? Color { get; set; }

        // product.template.attribute.line
        [JsonProperty("attribute_line_ids")]
        public int[] AttributeLineIds { get; set; }

        // product.template.attribute.line
        [JsonProperty("valid_product_template_attribute_line_ids")]
        public int[] ValidProductTemplateAttributeLineIds { get; set; }

        // product.product
        [JsonProperty("product_variant_ids")]
        public int[] ProductVariantIds { get; set; }

        // product.product
        [JsonProperty("product_variant_id")]
        public int? ProductVariantId { get; set; }

        [JsonProperty("product_variant_count")]
        public int? ProductVariantCount { get; set; }

        [JsonProperty("has_configurable_attributes")]
        public bool? HasConfigurableAttributes { get; set; }

        // account.tax
        [JsonProperty("taxes_id")]
        public int[] TaxesId { get; set; }

        // account.tax
        [JsonProperty("supplier_taxes_id")]
        public int[] SupplierTaxesId { get; set; }

        // account.account
        [JsonProperty("property_account_income_id")]
        public int? PropertyAccountIncomeId { get; set; }

        // account.account
        [JsonProperty("property_account_expense_id")]
        public int? PropertyAccountExpenseId { get; set; }

        // res.users
        [JsonProperty("responsible_id")]
        public int? ResponsibleId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        // stock.location
        [JsonProperty("property_stock_production")]
        public int? PropertyStockProduction { get; set; }

        // stock.location
        [JsonProperty("property_stock_inventory")]
        public int? PropertyStockInventory { get; set; }

        [JsonProperty("sale_delay")]
        public double? SaleDelay { get; set; }

        [JsonProperty("tracking")]
        public string Tracking { get; set; }

        [JsonProperty("description_picking")]
        public string DescriptionPicking { get; set; }

        [JsonProperty("description_pickingout")]
        public string DescriptionPickingout { get; set; }

        [JsonProperty("description_pickingin")]
        public string DescriptionPickingin { get; set; }

        // stock.location
        [JsonProperty("location_id")]
        public int? LocationId { get; set; }

        // stock.warehouse
        [JsonProperty("warehouse_id")]
        public int? WarehouseId { get; set; }

        [JsonProperty("has_available_route_ids")]
        public bool? HasAvailableRouteIds { get; set; }

        // stock.location.route
        [JsonProperty("route_from_categ_ids")]
        public int[] RouteFromCategIds { get; set; }

        // account.account
        [JsonProperty("property_account_creditor_price_difference")]
        public int? PropertyAccountCreditorPriceDifference { get; set; }

        [JsonProperty("purchase_method")]
        public string PurchaseMethod { get; set; }

        [JsonProperty("purchase_line_warn")]
        public string PurchaseLineWarn { get; set; }

        [JsonProperty("purchase_line_warn_msg")]
        public string PurchaseLineWarnMsg { get; set; }

        // stock.location.route
        [JsonProperty("route_ids")]
        public int[] RouteIds { get; set; }

        [JsonProperty("service_type")]
        public string ServiceType { get; set; }

        [JsonProperty("sale_line_warn")]
        public string SaleLineWarn { get; set; }

        [JsonProperty("sale_line_warn_msg")]
        public string SaleLineWarnMsg { get; set; }

        [JsonProperty("expense_policy")]
        public string ExpensePolicy { get; set; }

        [JsonProperty("visible_expense_policy")]
        public bool? VisibleExpensePolicy { get; set; }

        [JsonProperty("visible_qty_configurator")]
        public bool? VisibleQtyConfigurator { get; set; }

        [JsonProperty("invoice_policy")]
        public string InvoicePolicy { get; set; }

        [JsonProperty("service_to_purchase")]
        public bool? ServiceToPurchase { get; set; }

        [JsonProperty("rent_ok")]
        public bool? RentOk { get; set; }

        // rental.pricing
        [JsonProperty("rental_pricing_ids")]
        public int[] RentalPricingIds { get; set; }

        [JsonProperty("display_price")]
        public string DisplayPrice { get; set; }

        [JsonProperty("extra_hourly")]
        public double? ExtraHourly { get; set; }

        [JsonProperty("extra_daily")]
        public double? ExtraDaily { get; set; }

        [JsonProperty("preparation_time")]
        public double? PreparationTime { get; set; }

        [JsonProperty("recurring_invoice")]
        public bool? RecurringInvoice { get; set; }

        // sale.subscription.template
        [JsonProperty("subscription_template_id")]
        public int? SubscriptionTemplateId { get; set; }
    }
}