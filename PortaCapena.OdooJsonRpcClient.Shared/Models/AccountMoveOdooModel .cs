using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("account.move")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class AccountMoveOdooModel : IOdooModel
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("highest_name")]
        public string HighestName { get; set; }

        [JsonProperty("show_name_warning")]
        public bool? ShowNameWarning { get; set; }

        // required
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("narration")]
        public string Narration { get; set; }

        // required
        [JsonProperty("state")]
        public StatusAccountMoveOdooEnum State { get; set; }

        [JsonProperty("posted_before")]
        public bool? PostedBefore { get; set; }

        // required
        [JsonProperty("move_type")]
        public TypeAccountMoveOdooEnum MoveType { get; set; }

        [JsonProperty("type_name")]
        public string TypeName { get; set; }

        [JsonProperty("to_check")]
        public bool? ToCheck { get; set; }

        // account.journal
        // required
        [JsonProperty("journal_id")]
        public long JournalId { get; set; }

        // account.journal
        [JsonProperty("suitable_journal_ids")]
        public long[] SuitableJournalIds { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }

        // res.currency
        [JsonProperty("company_currency_id")]
        public long? CompanyCurrencyId { get; set; }

        // res.currency
        // required
        [JsonProperty("currency_id")]
        public long CurrencyId { get; set; }

        // account.move.line
        [JsonProperty("line_ids")]
        public long[] LineIds { get; set; }

        // res.partner
        [JsonProperty("partner_id")]
        public long? PartnerId { get; set; }

        // res.partner
        [JsonProperty("commercial_partner_id")]
        public long? CommercialPartnerId { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        // res.users
        [JsonProperty("user_id")]
        public long? UserId { get; set; }

        [JsonProperty("is_move_sent")]
        public bool? IsMoveSent { get; set; }

        // res.partner.bank
        [JsonProperty("partner_bank_id")]
        public long? PartnerBankId { get; set; }

        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        // account.payment
        [JsonProperty("payment_id")]
        public long? PaymentId { get; set; }

        // account.bank.statement.line
        [JsonProperty("statement_line_id")]
        public long? StatementLineId { get; set; }

        [JsonProperty("amount_untaxed")]
        public decimal? AmountUntaxed { get; set; }

        [JsonProperty("amount_tax")]
        public decimal? AmountTax { get; set; }

        [JsonProperty("amount_total")]
        public decimal? AmountTotal { get; set; }

        [JsonProperty("amount_residual")]
        public decimal? AmountResidual { get; set; }

        [JsonProperty("amount_untaxed_signed")]
        public decimal? AmountUntaxedSigned { get; set; }

        [JsonProperty("amount_tax_signed")]
        public decimal? AmountTaxSigned { get; set; }

        [JsonProperty("amount_total_signed")]
        public decimal? AmountTotalSigned { get; set; }

        [JsonProperty("amount_residual_signed")]
        public decimal? AmountResidualSigned { get; set; }

        [JsonProperty("amount_by_group")]
        public string AmountByGroup { get; set; }

        [JsonProperty("payment_state")]
        public PaymentStatusAccountMoveOdooEnum? PaymentState { get; set; }

        // account.partial.reconcile
        [JsonProperty("tax_cash_basis_rec_id")]
        public long? TaxCashBasisRecId { get; set; }

        // account.move
        [JsonProperty("tax_cash_basis_move_id")]
        public long? TaxCashBasisMoveId { get; set; }

        [JsonProperty("auto_post")]
        public bool? AutoPost { get; set; }

        // account.move
        [JsonProperty("reversed_entry_id")]
        public long? ReversedEntryId { get; set; }

        // account.fiscal.position
        [JsonProperty("fiscal_position_id")]
        public long? FiscalPositionId { get; set; }

        // res.users
        [JsonProperty("invoice_user_id")]
        public long? InvoiceUserId { get; set; }

        [JsonProperty("invoice_date")]
        public DateTime? InvoiceDate { get; set; }

        [JsonProperty("invoice_date_due")]
        public DateTime? InvoiceDateDue { get; set; }

        [JsonProperty("invoice_origin")]
        public string InvoiceOrigin { get; set; }

        // account.payment.term
        [JsonProperty("invoice_payment_term_id")]
        public long? InvoicePaymentTermId { get; set; }

        // account.move.line
        [JsonProperty("invoice_line_ids")]
        public long[] InvoiceLineIds { get; set; }

        // account.incoterms
        [JsonProperty("invoice_incoterm_id")]
        public long? InvoiceIncotermId { get; set; }

        [JsonProperty("display_qr_code")]
        public bool? DisplayQrCode { get; set; }

        [JsonProperty("qr_code_method")]
        public PaymentQRCodeAccountMoveOdooEnum? QrCodeMethod { get; set; }

        [JsonProperty("invoice_outstanding_credits_debits_widget")]
        public string InvoiceOutstandingCreditsDebitsWidget { get; set; }

        [JsonProperty("invoice_has_outstanding")]
        public bool? InvoiceHasOutstanding { get; set; }

        [JsonProperty("invoice_payments_widget")]
        public string InvoicePaymentsWidget { get; set; }

        // account.move
        [JsonProperty("invoice_vendor_bill_id")]
        public long? InvoiceVendorBillId { get; set; }

        [JsonProperty("invoice_source_email")]
        public string InvoiceSourceEmail { get; set; }

        [JsonProperty("invoice_partner_display_name")]
        public string InvoicePartnerDisplayName { get; set; }

        // account.cash.rounding
        [JsonProperty("invoice_cash_rounding_id")]
        public long? InvoiceCashRoundingId { get; set; }

        [JsonProperty("invoice_filter_type_domain")]
        public string InvoiceFilterTypeDomain { get; set; }

        // res.partner
        [JsonProperty("bank_partner_id")]
        public long? BankPartnerId { get; set; }

        [JsonProperty("invoice_has_matching_suspense_amount")]
        public bool? InvoiceHasMatchingSuspenseAmount { get; set; }

        [JsonProperty("tax_lock_date_message")]
        public string TaxLockDateMessage { get; set; }

        [JsonProperty("has_reconciled_entries")]
        public bool? HasReconciledEntries { get; set; }

        [JsonProperty("show_reset_to_draft_button")]
        public bool? ShowResetToDraftButton { get; set; }

        [JsonProperty("restrict_mode_hash_table")]
        public bool? RestrictModeHashTable { get; set; }

        [JsonProperty("secure_sequence_number")]
        public int? SecureSequenceNumber { get; set; }

        [JsonProperty("inalterable_hash")]
        public string InalterableHash { get; set; }

        [JsonProperty("string_to_hash")]
        public string StringToHash { get; set; }

        // ir.attachment
        [JsonProperty("attachment_ids")]
        public long[] AttachmentIds { get; set; }

        [JsonProperty("payment_state_before_switch")]
        public string PaymentStateBeforeSwitch { get; set; }

        // account.edi.document
        [JsonProperty("edi_document_ids")]
        public long[] EdiDocumentIds { get; set; }

        [JsonProperty("edi_state")]
        public ElectronicInvoicingAccountMoveOdooEnum? EdiState { get; set; }

        [JsonProperty("edi_error_count")]
        public int? EdiErrorCount { get; set; }

        [JsonProperty("edi_web_services_to_process")]
        public string EdiWebServicesToProcess { get; set; }

        [JsonProperty("edi_show_cancel_button")]
        public bool? EdiShowCancelButton { get; set; }

        [JsonProperty("duplicated_vendor_ref")]
        public string DuplicatedVendorRef { get; set; }

        // required
        [JsonProperty("extract_state")]
        public ExtractStateAccountMoveOdooEnum ExtractState { get; set; }

        [JsonProperty("extract_status_code")]
        public int? ExtractStatusCode { get; set; }

        [JsonProperty("extract_error_message")]
        public string ExtractErrorMessage { get; set; }

        [JsonProperty("extract_remote_id")]
        public int? ExtractRemoteId { get; set; }

        // account.invoice_extract.words
        [JsonProperty("extract_word_ids")]
        public long[] ExtractWordIds { get; set; }

        [JsonProperty("extract_can_show_resend_button")]
        public bool? ExtractCanShowResendButton { get; set; }

        [JsonProperty("extract_can_show_send_button")]
        public bool? ExtractCanShowSendButton { get; set; }

        // payment.transaction
        [JsonProperty("transaction_ids")]
        public long[] TransactionIds { get; set; }

        // payment.transaction
        [JsonProperty("authorized_transaction_ids")]
        public long[] AuthorizedTransactionIds { get; set; }

        // purchase.bill.union
        [JsonProperty("purchase_vendor_bill_id")]
        public long? PurchaseVendorBillId { get; set; }

        // purchase.order
        [JsonProperty("purchase_id")]
        public long? PurchaseId { get; set; }

        // account.transfer.model
        [JsonProperty("transfer_model_id")]
        public long? TransferModelId { get; set; }

        [JsonProperty("tax_closing_end_date")]
        public DateTime? TaxClosingEndDate { get; set; }

        [JsonProperty("tax_report_control_error")]
        public bool? TaxReportControlError { get; set; }

        // crm.team
        [JsonProperty("team_id")]
        public long? TeamId { get; set; }

        // res.partner
        [JsonProperty("partner_shipping_id")]
        public long? PartnerShippingId { get; set; }

        // account.asset
        [JsonProperty("asset_id")]
        public long? AssetId { get; set; }

        [JsonProperty("asset_asset_type")]
        public AssetTypeAccountMoveOdooEnum? AssetAssetType { get; set; }

        [JsonProperty("asset_remaining_value")]
        public decimal? AssetRemainingValue { get; set; }

        [JsonProperty("asset_depreciated_value")]
        public decimal? AssetDepreciatedValue { get; set; }

        [JsonProperty("asset_manually_modified")]
        public bool? AssetManuallyModified { get; set; }

        [JsonProperty("asset_value_change")]
        public bool? AssetValueChange { get; set; }

        // account.asset
        [JsonProperty("asset_ids")]
        public long[] AssetIds { get; set; }

        [JsonProperty("asset_ids_display_name")]
        public string AssetIdsDisplayName { get; set; }

        [JsonProperty("asset_id_display_name")]
        public string AssetIdDisplayName { get; set; }

        [JsonProperty("number_asset_ids")]
        public int? NumberAssetIds { get; set; }

        [JsonProperty("draft_asset_ids")]
        public bool? DraftAssetIds { get; set; }

        // account.move
        [JsonProperty("reversal_move_id")]
        public long[] ReversalMoveId { get; set; }

        // account.account
        [JsonProperty("l10n_de_datev_main_account_id")]
        public long? L10nDeDatevMainAccountId { get; set; }

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
        public ActivityStateAccountMoveOdooEnum? ActivityState { get; set; }

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
        public ActivityExceptionDecorationAccountMoveOdooEnum? ActivityExceptionDecoration { get; set; }

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

        [JsonProperty("sequence_prefix")]
        public string SequencePrefix { get; set; }

        [JsonProperty("sequence_number")]
        public int? SequenceNumber { get; set; }

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

        [JsonProperty("x_studio_is_sent_to_exact")]
        public bool? XStudioIsSentToExact { get; set; }

        [JsonProperty("x_studio_char_field_paYL8")]
        public string XStudioCharFieldPayl8 { get; set; }

        [JsonProperty("x_studio_is300")]
        public bool? XStudioIs300 { get; set; }

        [JsonProperty("x_studio_char_field_M658o")]
        public string XStudioCharFieldM658o { get; set; }

        [JsonProperty("x_studio_char_field_uGHuy")]
        public string XStudioCharFieldUghuy { get; set; }
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusAccountMoveOdooEnum
    {
        [EnumMember(Value = "draft")]
        Draft = 1,

        [EnumMember(Value = "posted")]
        Posted = 2,

        [EnumMember(Value = "cancel")]
        Cancelled = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeAccountMoveOdooEnum
    {
        [EnumMember(Value = "entry")]
        JournalEntry = 1,

        [EnumMember(Value = "out_invoice")]
        CustomerInvoice = 2,

        [EnumMember(Value = "out_refund")]
        CustomerCreditNote = 3,

        [EnumMember(Value = "in_invoice")]
        VendorBill = 4,

        [EnumMember(Value = "in_refund")]
        VendorCreditNote = 5,

        [EnumMember(Value = "out_receipt")]
        SalesReceipt = 6,

        [EnumMember(Value = "in_receipt")]
        PurchaseReceipt = 7,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentStatusAccountMoveOdooEnum
    {
        [EnumMember(Value = "not_paid")]
        NotPaid = 1,

        [EnumMember(Value = "in_payment")]
        InPayment = 2,

        [EnumMember(Value = "paid")]
        Paid = 3,

        [EnumMember(Value = "partial")]
        PartiallyPaid = 4,

        [EnumMember(Value = "reversed")]
        Reversed = 5,

        [EnumMember(Value = "invoicing_legacy")]
        InvoicingAppLegacy = 6,
    }


    // Type of QR-code to be generated for the payment of this invoice, when printing it. If left blank, the first available and usable method will be used.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentQRCodeAccountMoveOdooEnum
    {
        [EnumMember(Value = "sct_qr")]
        SEPACreditTransferQR = 1,
    }


    // The aggregated state of all the EDIs of this move
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ElectronicInvoicingAccountMoveOdooEnum
    {
        [EnumMember(Value = "to_send")]
        ToSend = 1,

        [EnumMember(Value = "sent")]
        Sent = 2,

        [EnumMember(Value = "to_cancel")]
        ToCancel = 3,

        [EnumMember(Value = "cancelled")]
        Cancelled = 4,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExtractStateAccountMoveOdooEnum
    {
        [EnumMember(Value = "no_extract_requested")]
        NoExtractRequested = 1,

        [EnumMember(Value = "not_enough_credit")]
        NotEnoughCredit = 2,

        [EnumMember(Value = "error_status")]
        AnErrorOccurred = 3,

        [EnumMember(Value = "waiting_extraction")]
        WaitingExtraction = 4,

        [EnumMember(Value = "extract_not_ready")]
        WaitingExtractionButItIsNotReady = 5,

        [EnumMember(Value = "waiting_validation")]
        WaitingValidation = 6,

        [EnumMember(Value = "done")]
        CompletedFlow = 7,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum AssetTypeAccountMoveOdooEnum
    {
        [EnumMember(Value = "sale")]
        SaleRevenueRecognition = 1,

        [EnumMember(Value = "purchase")]
        PurchaseAsset = 2,

        [EnumMember(Value = "expense")]
        DeferredExpense = 3,
    }


    // Status based on activities
    // Overdue: Due date is already passed
    // Today: Activity date is today
    // Planned: Future activities.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActivityStateAccountMoveOdooEnum
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
    public enum ActivityExceptionDecorationAccountMoveOdooEnum
    {
        [EnumMember(Value = "warning")]
        Alert = 1,

        [EnumMember(Value = "danger")]
        Error = 2,
    }

}