using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("sale.order")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class OdooSaleOrderModel : IOdooModel
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("client_order_ref")]
        public string ClientOrderRef { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("date_order")]
        public DateTime DateOrder { get; set; }

        [JsonProperty("validity_date")]
        public DateTime? ValidityDate { get; set; }

        [JsonProperty("is_expired")]
        public bool? IsExpired { get; set; }

        [JsonProperty("require_signature")]
        public bool? RequireSignature { get; set; }

        [JsonProperty("require_payment")]
        public bool? RequirePayment { get; set; }

        [JsonProperty("create_date")]
        public DateTime? CreateDate { get; set; }

        // res.users
        [JsonProperty("user_id")]
        public long? UserId { get; set; }

        // res.partner
        [JsonProperty("partner_id")]
        public long PartnerId { get; set; }

        // res.partner
        [JsonProperty("partner_invoice_id")]
        public long PartnerInvoiceId { get; set; }

        // res.partner
        [JsonProperty("partner_shipping_id")]
        public long PartnerShippingId { get; set; }

        // product.pricelist
        [JsonProperty("pricelist_id")]
        public long PricelistId { get; set; }

        // res.currency
        [JsonProperty("currency_id")]
        public long? CurrencyId { get; set; }

        // account.analytic.account
        [JsonProperty("analytic_account_id")]
        public long? AnalyticAccountId { get; set; }

        // sale.order.line
        [JsonProperty("order_line")]
        public long[] OrderLine { get; set; }

        [JsonProperty("invoice_count")]
        public int? InvoiceCount { get; set; }

        // account.move
        [JsonProperty("invoice_ids")]
        public long[] InvoiceIds { get; set; }

        [JsonProperty("invoice_status")]
        public string InvoiceStatus { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("amount_untaxed")]
        public double? AmountUntaxed { get; set; }

        [JsonProperty("amount_by_group")]
        public string AmountByGroup { get; set; }

        [JsonProperty("amount_tax")]
        public double? AmountTax { get; set; }

        [JsonProperty("amount_total")]
        public double? AmountTotal { get; set; }

        [JsonProperty("currency_rate")]
        public double? CurrencyRate { get; set; }

        // account.payment.term
        [JsonProperty("payment_term_id")]
        public long? PaymentTermId { get; set; }

        // account.fiscal.position
        [JsonProperty("fiscal_position_id")]
        public long? FiscalPositionId { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public long CompanyId { get; set; }

        // crm.team
        [JsonProperty("team_id")]
        public long? TeamId { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("signed_by")]
        public string SignedBy { get; set; }

        [JsonProperty("signed_on")]
        public DateTime? SignedOn { get; set; }

        [JsonProperty("commitment_date")]
        public DateTime? CommitmentDate { get; set; }

        [JsonProperty("amount_undiscounted")]
        public double? AmountUndiscounted { get; set; }

        [JsonProperty("type_name")]
        public string TypeName { get; set; }

        // payment.transaction
        [JsonProperty("transaction_ids")]
        public long[] TransactionIds { get; set; }

        // payment.transaction
        [JsonProperty("authorized_transaction_ids")]
        public long[] AuthorizedTransactionIds { get; set; }

        [JsonProperty("show_update_pricelist")]
        public bool? ShowUpdatePricelist { get; set; }

        // crm.tag
        [JsonProperty("tag_ids")]
        public long[] TagIds { get; set; }

        // sale.order.template
        [JsonProperty("sale_order_template_id")]
        public long? SaleOrderTemplateId { get; set; }

        // sale.order.option
        [JsonProperty("sale_order_option_ids")]
        public long[] SaleOrderOptionIds { get; set; }

        [JsonProperty("purchase_order_count")]
        public int? PurchaseOrderCount { get; set; }

        [JsonProperty("is_rental_order")]
        public bool? IsRentalOrder { get; set; }

        [JsonProperty("rental_status")]
        public string RentalStatus { get; set; }

        [JsonProperty("has_pickable_lines")]
        public bool? HasPickableLines { get; set; }

        [JsonProperty("has_returnable_lines")]
        public bool? HasReturnableLines { get; set; }

        [JsonProperty("next_action_date")]
        public DateTime? NextActionDate { get; set; }

        [JsonProperty("has_late_lines")]
        public bool? HasLateLines { get; set; }

        // account.incoterms
        [JsonProperty("incoterm")]
        public long? Incoterm { get; set; }

        [JsonProperty("picking_policy")]
        public string PickingPolicy { get; set; }

        // stock.warehouse
        [JsonProperty("warehouse_id")]
        public long WarehouseId { get; set; }

        // stock.picking
        [JsonProperty("picking_ids")]
        public long[] PickingIds { get; set; }

        [JsonProperty("delivery_count")]
        public int? DeliveryCount { get; set; }

        // procurement.group
        [JsonProperty("procurement_group_id")]
        public long? ProcurementGroupId { get; set; }

        [JsonProperty("effective_date")]
        public DateTime? EffectiveDate { get; set; }

        [JsonProperty("expected_date")]
        public DateTime? ExpectedDate { get; set; }

        [JsonProperty("json_popover")]
        public string JsonPopover { get; set; }

        [JsonProperty("show_json_popover")]
        public bool? ShowJsonPopover { get; set; }

        // crm.lead
        [JsonProperty("opportunity_id")]
        public long? OpportunityId { get; set; }

        [JsonProperty("subscription_management")]
        public string SubscriptionManagement { get; set; }

        [JsonProperty("subscription_count")]
        public int? SubscriptionCount { get; set; }

        // utm.campaign
        [JsonProperty("campaign_id")]
        public long? CampaignId { get; set; }

        // utm.source
        [JsonProperty("source_id")]
        public long? SourceId { get; set; }

        // utm.medium
        [JsonProperty("medium_id")]
        public long? MediumId { get; set; }

        // mail.activity
        [JsonProperty("activity_ids")]
        public long[] ActivityIds { get; set; }

        [JsonProperty("activity_state")]
        public string ActivityState { get; set; }

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
        public string ActivityExceptionDecoration { get; set; }

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

        [JsonProperty("access_url")]
        public string AccessUrl { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("access_warning")]
        public string AccessWarning { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        // res.users
        [JsonProperty("create_uid")]
        public long? CreateUid { get; set; }

        // res.users
        [JsonProperty("write_uid")]
        public long? WriteUid { get; set; }

        [JsonProperty("write_date")]
        public DateTime? WriteDate { get; set; }

        [JsonProperty("__last_update")]
        public DateTime? LastUpdate { get; set; }
    }

}