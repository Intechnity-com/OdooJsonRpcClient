using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("purchase.order")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class PurchaseOrderOdooModel : IOdooModel
    {

        // required
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("priority")]
        public PriorityPurchaseOrderOdooEnum? Priority { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("partner_ref")]
        public string PartnerRef { get; set; }

        // required
        [JsonProperty("date_order")]
        public DateTime DateOrder { get; set; }

        [JsonProperty("date_approve")]
        public DateTime? DateApprove { get; set; }

        // res.partner
        // required
        [JsonProperty("partner_id")]
        public long PartnerId { get; set; }

        // res.partner
        [JsonProperty("dest_address_id")]
        public long? DestAddressId { get; set; }

        // res.currency
        // required
        [JsonProperty("currency_id")]
        public long CurrencyId { get; set; }

        [JsonProperty("state")]
        public StatusPurchaseOrderOdooEnum? State { get; set; }

        // purchase.order.line
        [JsonProperty("order_line")]
        public long[] OrderLine { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("invoice_count")]
        public int? InvoiceCount { get; set; }

        // account.move
        [JsonProperty("invoice_ids")]
        public long[] InvoiceIds { get; set; }

        [JsonProperty("invoice_status")]
        public BillingStatusPurchaseOrderOdooEnum? InvoiceStatus { get; set; }

        [JsonProperty("date_planned")]
        public DateTime? DatePlanned { get; set; }

        [JsonProperty("date_calendar_start")]
        public DateTime? DateCalendarStart { get; set; }

        [JsonProperty("amount_untaxed")]
        public decimal? AmountUntaxed { get; set; }

        [JsonProperty("amount_tax")]
        public decimal? AmountTax { get; set; }

        [JsonProperty("amount_total")]
        public decimal? AmountTotal { get; set; }

        // account.fiscal.position
        [JsonProperty("fiscal_position_id")]
        public long? FiscalPositionId { get; set; }

        // account.payment.term
        [JsonProperty("payment_term_id")]
        public long? PaymentTermId { get; set; }

        // account.incoterms
        [JsonProperty("incoterm_id")]
        public long? IncotermId { get; set; }

        // product.product
        [JsonProperty("product_id")]
        public long? ProductId { get; set; }

        // res.users
        [JsonProperty("user_id")]
        public long? UserId { get; set; }

        // res.company
        // required
        [JsonProperty("company_id")]
        public long CompanyId { get; set; }

        [JsonProperty("currency_rate")]
        public double? CurrencyRate { get; set; }

        [JsonProperty("mail_reminder_confirmed")]
        public bool? MailReminderConfirmed { get; set; }

        [JsonProperty("mail_reception_confirmed")]
        public bool? MailReceptionConfirmed { get; set; }

        [JsonProperty("receipt_reminder_email")]
        public bool? ReceiptReminderEmail { get; set; }

        [JsonProperty("reminder_date_before_receipt")]
        public int? ReminderDateBeforeReceipt { get; set; }

        [JsonProperty("sale_order_count")]
        public int? SaleOrderCount { get; set; }

        // mail.activity
        [JsonProperty("activity_ids")]
        public long[] ActivityIds { get; set; }

        [JsonProperty("activity_state")]
        public ActivityStatePurchaseOrderOdooEnum? ActivityState { get; set; }

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
        public ActivityExceptionDecorationPurchaseOrderOdooEnum? ActivityExceptionDecoration { get; set; }

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

        [JsonProperty("create_date")]
        public DateTime? CreateDate { get; set; }

        // res.users
        [JsonProperty("write_uid")]
        public long? WriteUid { get; set; }

        [JsonProperty("write_date")]
        public DateTime? WriteDate { get; set; }

        [JsonProperty("__last_update")]
        public DateTime? LastUpdate { get; set; }

        [JsonProperty("x_studio_pickup_date")]
        public DateTime? XStudioPickupDate { get; set; }

        [JsonProperty("x_studio_sent_to_exact")]
        public bool? XStudioSentToExact { get; set; }

        [JsonProperty("x_studio_pickup_address")]
        public string XStudioPickupAddress { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum PriorityPurchaseOrderOdooEnum
    {
        [EnumMember(Value = "0")]
        Normal = 1,

        [EnumMember(Value = "1")]
        Urgent = 2,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusPurchaseOrderOdooEnum
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


    [JsonConverter(typeof(StringEnumConverter))]
    public enum BillingStatusPurchaseOrderOdooEnum
    {
        [EnumMember(Value = "no")]
        NothingToBill = 1,

        [EnumMember(Value = "to invoice")]
        WaitingBills = 2,

        [EnumMember(Value = "invoiced")]
        FullyBilled = 3,
    }


    // Status based on activities
    // Overdue: Due date is already passed
    // Today: Activity date is today
    // Planned: Future activities.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActivityStatePurchaseOrderOdooEnum
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
    public enum ActivityExceptionDecorationPurchaseOrderOdooEnum
    {
        [EnumMember(Value = "warning")]
        Alert = 1,

        [EnumMember(Value = "danger")]
        Error = 2,
    }


}