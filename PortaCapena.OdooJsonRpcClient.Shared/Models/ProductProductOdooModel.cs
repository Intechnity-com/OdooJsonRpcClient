using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("product.product")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class ProductProductOdooModel : IOdooModel
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
        // required
        [JsonProperty("product_tmpl_id")]
        public long ProductTmplId { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        // product.template.attribute.value
        [JsonProperty("product_template_attribute_value_ids")]
        public long[] ProductTemplateAttributeValueIds { get; set; }

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
        public long[] PackagingIds { get; set; }

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

        [JsonProperty("purchased_product_qty")]
        public double? PurchasedProductQty { get; set; }

        [JsonProperty("sales_count")]
        public double? SalesCount { get; set; }

        // mail.activity
        [JsonProperty("activity_ids")]
        public long[] ActivityIds { get; set; }

        [JsonProperty("activity_state")]
        public ActivityStateProductProductOdooEnum? ActivityState { get; set; }

        // res.users
        [JsonProperty("activity_user_id")]
        public long? ActivityUserId { get; set; }

        // mail.activity.type
        [JsonProperty("activity_type_id")]
        public long? ActivityTypeId { get; set; }

        [JsonProperty("activity_type_icon")]
        public string ActivityTypeIcon { get; set; }

        [JsonProperty("activity_date_deadline")]
        public DateTime? ActivityDateDeadline { get; set; }

        [JsonProperty("activity_summary")]
        public string ActivitySummary { get; set; }

        [JsonProperty("activity_exception_decoration")]
        public ActivityExceptionDecorationProductProductOdooEnum? ActivityExceptionDecoration { get; set; }

        [JsonProperty("activity_exception_icon")]
        public string ActivityExceptionIcon { get; set; }

        [JsonProperty("message_is_follower")]
        public bool? MessageIsFollower { get; set; }

        // mail.followers
        [JsonProperty("message_follower_ids")]
        public long[] MessageFollowerIds { get; set; }

        // res.partner
        [JsonProperty("message_partner_ids")]
        public long[] MessagePartnerIds { get; set; }

        // mail.channel
        [JsonProperty("message_channel_ids")]
        public long[] MessageChannelIds { get; set; }

        // mail.message
        [JsonProperty("message_ids")]
        public long[] MessageIds { get; set; }

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
        public long? MessageMainAttachmentId { get; set; }

        // mail.message
        [JsonProperty("website_message_ids")]
        public long[] WebsiteMessageIds { get; set; }

        [JsonProperty("message_has_sms_error")]
        public bool? MessageHasSmsError { get; set; }

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

        // required
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

        // required
        [JsonProperty("type")]
        public ProductTypeProductProductOdooEnum Type { get; set; }

        // product.category
        // required
        [JsonProperty("categ_id")]
        public long CategId { get; set; }

        // res.currency
        [JsonProperty("currency_id")]
        public long? CurrencyId { get; set; }

        // res.currency
        [JsonProperty("cost_currency_id")]
        public long? CostCurrencyId { get; set; }

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
        public long? PricelistId { get; set; }

        // uom.uom
        // required
        [JsonProperty("uom_id")]
        public long UomId { get; set; }

        [JsonProperty("uom_name")]
        public string UomName { get; set; }

        // uom.uom
        // required
        [JsonProperty("uom_po_id")]
        public long UomPoId { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }

        // product.supplierinfo
        [JsonProperty("seller_ids")]
        public long[] SellerIds { get; set; }

        // product.supplierinfo
        [JsonProperty("variant_seller_ids")]
        public long[] VariantSellerIds { get; set; }

        [JsonProperty("color")]
        public int? Color { get; set; }

        // product.template.attribute.line
        [JsonProperty("attribute_line_ids")]
        public long[] AttributeLineIds { get; set; }

        // product.template.attribute.line
        [JsonProperty("valid_product_template_attribute_line_ids")]
        public long[] ValidProductTemplateAttributeLineIds { get; set; }

        // product.product
        // required
        [JsonProperty("product_variant_ids")]
        public long[] ProductVariantIds { get; set; }

        // product.product
        [JsonProperty("product_variant_id")]
        public long? ProductVariantId { get; set; }

        [JsonProperty("product_variant_count")]
        public int? ProductVariantCount { get; set; }

        [JsonProperty("has_configurable_attributes")]
        public bool? HasConfigurableAttributes { get; set; }

        // account.tax
        [JsonProperty("taxes_id")]
        public long[] TaxesId { get; set; }

        // account.tax
        [JsonProperty("supplier_taxes_id")]
        public long[] SupplierTaxesId { get; set; }

        // account.account
        [JsonProperty("property_account_income_id")]
        public long? PropertyAccountIncomeId { get; set; }

        // account.account
        [JsonProperty("property_account_expense_id")]
        public long? PropertyAccountExpenseId { get; set; }

        // account.account
        [JsonProperty("property_account_creditor_price_difference")]
        public long? PropertyAccountCreditorPriceDifference { get; set; }

        [JsonProperty("purchase_method")]
        public ControlPolicyProductProductOdooEnum? PurchaseMethod { get; set; }

        // required
        [JsonProperty("purchase_line_warn")]
        public PurchaseOrderLineWarningProductProductOdooEnum PurchaseLineWarn { get; set; }

        [JsonProperty("purchase_line_warn_msg")]
        public string PurchaseLineWarnMsg { get; set; }

        [JsonProperty("service_type")]
        public TrackServiceProductProductOdooEnum? ServiceType { get; set; }

        // required
        [JsonProperty("sale_line_warn")]
        public SalesOrderLineProductProductOdooEnum SaleLineWarn { get; set; }

        [JsonProperty("sale_line_warn_msg")]
        public string SaleLineWarnMsg { get; set; }

        [JsonProperty("expense_policy")]
        public ReInvoiceExpensesProductProductOdooEnum? ExpensePolicy { get; set; }

        [JsonProperty("visible_expense_policy")]
        public bool? VisibleExpensePolicy { get; set; }

        [JsonProperty("visible_qty_configurator")]
        public bool? VisibleQtyConfigurator { get; set; }

        [JsonProperty("invoice_policy")]
        public InvoicingPolicyProductProductOdooEnum? InvoicePolicy { get; set; }

        [JsonProperty("service_to_purchase")]
        public bool? ServiceToPurchase { get; set; }
    }


    // Status based on activities
    // Overdue: Due date is already passed
    // Today: Activity date is today
    // Planned: Future activities.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActivityStateProductProductOdooEnum
    {
        [EnumMember(Value = "overdue")]
        Overdue = 1,

        [EnumMember(Value = "today")]
        Today = 2,

        [EnumMember(Value = "planned")]
        Planned = 3,
    }


    // Type of the exception activity on record.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActivityExceptionDecorationProductProductOdooEnum
    {
        [EnumMember(Value = "warning")]
        Alert = 1,

        [EnumMember(Value = "danger")]
        Error = 2,
    }


    // A storable product is a product for which you manage stock. The Inventory app has to be installed.
    // A consumable product is a product for which stock is not managed.
    // A service is a non-material product you provide.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductTypeProductProductOdooEnum
    {
        [EnumMember(Value = "consu")]
        Consumable = 1,

        [EnumMember(Value = "service")]
        Service = 2,
    }


    // On ordered quantities: Control bills based on ordered quantities.
    // On received quantities: Control bills based on received quantities.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ControlPolicyProductProductOdooEnum
    {
        [EnumMember(Value = "purchase")]
        OnOrderedQuantities = 1,

        [EnumMember(Value = "receive")]
        OnReceivedQuantities = 2,
    }


    // Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PurchaseOrderLineWarningProductProductOdooEnum
    {
        [EnumMember(Value = "no-message")]
        NoMessage = 1,

        [EnumMember(Value = "warning")]
        Warning = 2,

        [EnumMember(Value = "block")]
        BlockingMessage = 3,
    }


    // Manually set quantities on order: Invoice based on the manually entered quantity, without creating an analytic account.
    // Timesheets on contract: Invoice based on the tracked hours on the related timesheet.
    // Create a task and track hours: Create a task on the sales order validation and track the work hours.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TrackServiceProductProductOdooEnum
    {
        [EnumMember(Value = "manual")]
        ManuallySetQuantitiesOnOrder = 1,
    }


    // Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SalesOrderLineProductProductOdooEnum
    {
        [EnumMember(Value = "no-message")]
        NoMessage = 1,

        [EnumMember(Value = "warning")]
        Warning = 2,

        [EnumMember(Value = "block")]
        BlockingMessage = 3,
    }


    // Expenses and vendor bills can be re-invoiced to a customer.With this option, a validated expense can be re-invoice to a customer at its cost or sales price.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReInvoiceExpensesProductProductOdooEnum
    {
        [EnumMember(Value = "no")]
        No = 1,

        [EnumMember(Value = "cost")]
        AtCost = 2,

        [EnumMember(Value = "sales_price")]
        SalesPrice = 3,
    }


    // Ordered Quantity: Invoice quantities ordered by the customer.
    // Delivered Quantity: Invoice quantities delivered to the customer.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InvoicingPolicyProductProductOdooEnum
    {
        [EnumMember(Value = "order")]
        OrderedQuantities = 1,

        [EnumMember(Value = "delivery")]
        DeliveredQuantities = 2,
    }
}