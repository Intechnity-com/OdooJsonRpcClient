using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("res.partner")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class ResPartnerOdooModel : IOdooModel
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        // res.partner.title
        [JsonProperty("title")]
        public long? Title { get; set; }

        // res.partner
        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

        [JsonProperty("parent_name")]
        public string ParentName { get; set; }

        // res.partner
        [JsonProperty("child_ids")]
        public long[] ChildIds { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("lang")]
        public LanguageResPartnerOdooEnum? Lang { get; set; }

        [JsonProperty("active_lang_count")]
        public int? ActiveLangCount { get; set; }

        [JsonProperty("tz_offset")]
        public string TzOffset { get; set; }

        [JsonProperty("vat")]
        public string Vat { get; set; }

        // res.partner
        [JsonProperty("same_vat_partner_id")]
        public long? SameVatPartnerId { get; set; }

        // res.partner.bank
        [JsonProperty("bank_ids")]
        public long[] BankIds { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        // res.partner.category
        [JsonProperty("category_id")]
        public long[] CategoryId { get; set; }

        [JsonProperty("credit_limit")]
        public double? CreditLimit { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        [JsonProperty("employee")]
        public bool? Employee { get; set; }

        [JsonProperty("function")]
        public string Function { get; set; }

        [JsonProperty("type")]
        public AddressTypeResPartnerOdooEnum? Type { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("street2")]
        public string Street2 { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        // res.country.state
        [JsonProperty("state_id")]
        public long? StateId { get; set; }

        // res.country
        [JsonProperty("country_id")]
        public long? CountryId { get; set; }

        [JsonProperty("partner_latitude")]
        public double? PartnerLatitude { get; set; }

        [JsonProperty("partner_longitude")]
        public double? PartnerLongitude { get; set; }

        [JsonProperty("email_formatted")]
        public string EmailFormatted { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("is_company")]
        public bool? IsCompany { get; set; }

        // res.partner.industry
        [JsonProperty("industry_id")]
        public long? IndustryId { get; set; }

        [JsonProperty("company_type")]
        public CompanyTypeResPartnerOdooEnum? CompanyType { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }

        [JsonProperty("color")]
        public int? Color { get; set; }

        // res.users
        [JsonProperty("user_ids")]
        public long[] UserIds { get; set; }

        [JsonProperty("partner_share")]
        public bool? PartnerShare { get; set; }

        [JsonProperty("contact_address")]
        public string ContactAddress { get; set; }

        // res.partner
        [JsonProperty("commercial_partner_id")]
        public long? CommercialPartnerId { get; set; }

        [JsonProperty("commercial_company_name")]
        public string CommercialCompanyName { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        // res.partner
        [JsonProperty("self")]
        public long? Self { get; set; }

        [JsonProperty("im_status")]
        public string ImStatus { get; set; }

        // mail.activity
        [JsonProperty("activity_ids")]
        public long[] ActivityIds { get; set; }

        [JsonProperty("activity_state")]
        public ActivityStateResPartnerOdooEnum? ActivityState { get; set; }

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
        public ActivityExceptionDecorationResPartnerOdooEnum? ActivityExceptionDecoration { get; set; }

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

        [JsonProperty("email_normalized")]
        public string EmailNormalized { get; set; }

        [JsonProperty("is_blacklisted")]
        public bool? IsBlacklisted { get; set; }

        [JsonProperty("message_bounce")]
        public int? MessageBounce { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        // mail.channel
        [JsonProperty("channel_ids")]
        public long[] ChannelIds { get; set; }

        // res.users
        [JsonProperty("user_id")]
        public long? UserId { get; set; }

        [JsonProperty("contact_address_complete")]
        public string ContactAddressComplete { get; set; }

        [JsonProperty("image_medium")]
        public string ImageMedium { get; set; }

        [JsonProperty("signup_token")]
        public string SignupToken { get; set; }

        [JsonProperty("signup_type")]
        public string SignupType { get; set; }

        [JsonProperty("signup_expiration")]
        public DateTime? SignupExpiration { get; set; }

        [JsonProperty("signup_valid")]
        public bool? SignupValid { get; set; }

        [JsonProperty("signup_url")]
        public string SignupUrl { get; set; }

        [JsonProperty("phone_sanitized")]
        public string PhoneSanitized { get; set; }

        [JsonProperty("phone_sanitized_blacklisted")]
        public bool? PhoneSanitizedBlacklisted { get; set; }

        [JsonProperty("phone_blacklisted")]
        public bool? PhoneBlacklisted { get; set; }

        [JsonProperty("mobile_blacklisted")]
        public bool? MobileBlacklisted { get; set; }

        // product.pricelist
        [JsonProperty("property_product_pricelist")]
        public long? PropertyProductPricelist { get; set; }

        // crm.team
        [JsonProperty("team_id")]
        public long? TeamId { get; set; }

        [JsonProperty("ocn_token")]
        public string OcnToken { get; set; }

        [JsonProperty("partner_gid")]
        public int? PartnerGid { get; set; }

        [JsonProperty("additional_info")]
        public string AdditionalInfo { get; set; }

        // mail.message
        [JsonProperty("website_message_ids")]
        public long[] WebsiteMessageIds { get; set; }

        [JsonProperty("message_has_sms_error")]
        public bool? MessageHasSmsError { get; set; }

        [JsonProperty("credit")]
        public decimal? Credit { get; set; }

        [JsonProperty("debit")]
        public decimal? Debit { get; set; }

        [JsonProperty("debit_limit")]
        public decimal? DebitLimit { get; set; }

        [JsonProperty("total_invoiced")]
        public decimal? TotalInvoiced { get; set; }

        // res.currency
        [JsonProperty("currency_id")]
        public long? CurrencyId { get; set; }

        [JsonProperty("journal_item_count")]
        public int? JournalItemCount { get; set; }

        // account.account
        // required
        [JsonProperty("property_account_payable_id")]
        public long PropertyAccountPayableId { get; set; }

        // account.account
        // required
        [JsonProperty("property_account_receivable_id")]
        public long PropertyAccountReceivableId { get; set; }

        // account.fiscal.position
        [JsonProperty("property_account_position_id")]
        public long? PropertyAccountPositionId { get; set; }

        // account.payment.term
        [JsonProperty("property_payment_term_id")]
        public long? PropertyPaymentTermId { get; set; }

        // account.payment.term
        [JsonProperty("property_supplier_payment_term_id")]
        public long? PropertySupplierPaymentTermId { get; set; }

        // res.company
        [JsonProperty("ref_company_ids")]
        public long[] RefCompanyIds { get; set; }

        [JsonProperty("has_unreconciled_entries")]
        public bool? HasUnreconciledEntries { get; set; }

        [JsonProperty("last_time_entries_checked")]
        public DateTime? LastTimeEntriesChecked { get; set; }

        // account.move
        [JsonProperty("invoice_ids")]
        public long[] InvoiceIds { get; set; }

        // account.analytic.account
        [JsonProperty("contract_ids")]
        public long[] ContractIds { get; set; }

        [JsonProperty("bank_account_count")]
        public int? BankAccountCount { get; set; }

        [JsonProperty("trust")]
        public DegreeOfTrustYouHaveInThisDebtorResPartnerOdooEnum? Trust { get; set; }

        [JsonProperty("invoice_warn")]
        public InvoiceResPartnerOdooEnum? InvoiceWarn { get; set; }

        [JsonProperty("invoice_warn_msg")]
        public string InvoiceWarnMsg { get; set; }

        [JsonProperty("supplier_rank")]
        public int? SupplierRank { get; set; }

        [JsonProperty("customer_rank")]
        public int? CustomerRank { get; set; }

        [JsonProperty("online_partner_vendor_name")]
        public string OnlinePartnerVendorName { get; set; }

        [JsonProperty("online_partner_bank_account")]
        public string OnlinePartnerBankAccount { get; set; }

        // payment.token
        [JsonProperty("payment_token_ids")]
        public long[] PaymentTokenIds { get; set; }

        [JsonProperty("payment_token_count")]
        public int? PaymentTokenCount { get; set; }

        // res.currency
        [JsonProperty("property_purchase_currency_id")]
        public long? PropertyPurchaseCurrencyId { get; set; }

        [JsonProperty("purchase_order_count")]
        public int? PurchaseOrderCount { get; set; }

        [JsonProperty("supplier_invoice_count")]
        public int? SupplierInvoiceCount { get; set; }

        [JsonProperty("purchase_warn")]
        public PurchaseOrderResPartnerOdooEnum? PurchaseWarn { get; set; }

        [JsonProperty("purchase_warn_msg")]
        public string PurchaseWarnMsg { get; set; }

        [JsonProperty("receipt_reminder_email")]
        public bool? ReceiptReminderEmail { get; set; }

        [JsonProperty("reminder_date_before_receipt")]
        public int? ReminderDateBeforeReceipt { get; set; }

        [JsonProperty("online_partner_information")]
        public string OnlinePartnerInformation { get; set; }

        [JsonProperty("sale_order_count")]
        public int? SaleOrderCount { get; set; }

        // sale.order
        [JsonProperty("sale_order_ids")]
        public long[] SaleOrderIds { get; set; }

        [JsonProperty("sale_warn")]
        public SalesWarningsResPartnerOdooEnum? SaleWarn { get; set; }

        [JsonProperty("sale_warn_msg")]
        public string SaleWarnMsg { get; set; }

        [JsonProperty("payment_next_action_date")]
        public DateTime? PaymentNextActionDate { get; set; }

        // account.move.line
        [JsonProperty("unreconciled_aml_ids")]
        public long[] UnreconciledAmlIds { get; set; }

        // account.move
        [JsonProperty("unpaid_invoices")]
        public long[] UnpaidInvoices { get; set; }

        [JsonProperty("total_due")]
        public decimal? TotalDue { get; set; }

        [JsonProperty("total_overdue")]
        public decimal? TotalOverdue { get; set; }

        [JsonProperty("followup_status")]
        public FollowUpStatusResPartnerOdooEnum? FollowupStatus { get; set; }

        // account_followup.followup.line
        [JsonProperty("followup_level")]
        public long? FollowupLevel { get; set; }

        // res.users
        [JsonProperty("payment_responsible_id")]
        public long? PaymentResponsibleId { get; set; }

        [JsonProperty("citizen_identification")]
        public string CitizenIdentification { get; set; }

        [JsonProperty("form_file")]
        public string FormFile { get; set; }

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

        [JsonProperty("id")]
        public long Id { get; set; }

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


    // All the emails and documents sent to this contact will be translated in this language.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LanguageResPartnerOdooEnum
    {
        [EnumMember(Value = "en_US")]
        EnglishUS = 1,
    }

    // Invoice & Delivery addresses are used in sales orders. Private addresses are only visible by authorized users.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AddressTypeResPartnerOdooEnum
    {
        [EnumMember(Value = "contact")]
        Contact = 1,

        [EnumMember(Value = "invoice")]
        InvoiceAddress = 2,

        [EnumMember(Value = "delivery")]
        DeliveryAddress = 3,

        [EnumMember(Value = "other")]
        OtherAddress = 4,

        [EnumMember(Value = "private")]
        PrivateAddress = 5,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum CompanyTypeResPartnerOdooEnum
    {
        [EnumMember(Value = "person")]
        Individual = 1,

        [EnumMember(Value = "company")]
        Company = 2,
    }


    // Status based on activities
    // Overdue: Due date is already passed
    // Today: Activity date is today
    // Planned: Future activities.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActivityStateResPartnerOdooEnum
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
    public enum ActivityExceptionDecorationResPartnerOdooEnum
    {
        [EnumMember(Value = "warning")]
        Alert = 1,

        [EnumMember(Value = "danger")]
        Error = 2,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum DegreeOfTrustYouHaveInThisDebtorResPartnerOdooEnum
    {
        [EnumMember(Value = "good")]
        GoodDebtor = 1,

        [EnumMember(Value = "normal")]
        NormalDebtor = 2,

        [EnumMember(Value = "bad")]
        BadDebtor = 3,
    }


    // Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InvoiceResPartnerOdooEnum
    {
        [EnumMember(Value = "no-message")]
        NoMessage = 1,

        [EnumMember(Value = "warning")]
        Warning = 2,

        [EnumMember(Value = "block")]
        BlockingMessage = 3,
    }


    // Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PurchaseOrderResPartnerOdooEnum
    {
        [EnumMember(Value = "no-message")]
        NoMessage = 1,

        [EnumMember(Value = "warning")]
        Warning = 2,

        [EnumMember(Value = "block")]
        BlockingMessage = 3,
    }


    // Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SalesWarningsResPartnerOdooEnum
    {
        [EnumMember(Value = "no-message")]
        NoMessage = 1,

        [EnumMember(Value = "warning")]
        Warning = 2,

        [EnumMember(Value = "block")]
        BlockingMessage = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum FollowUpStatusResPartnerOdooEnum
    {
        [EnumMember(Value = "in_need_of_action")]
        InNeedOfAction = 1,

        [EnumMember(Value = "with_overdue_invoices")]
        WithOverdueInvoices = 2,

        [EnumMember(Value = "no_action_needed")]
        NoActionNeeded = 3,
    }
}
