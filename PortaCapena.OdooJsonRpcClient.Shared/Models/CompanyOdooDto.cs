using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("res.company")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class CompanyOdooDto : IOdooModel
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sequence")]
        public int? Sequence { get; set; }

        // res.company
        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

        // res.company
        [JsonProperty("child_ids")]
        public long[] ChildIds { get; set; }

        // res.partner
        [JsonProperty("partner_id")]
        public long PartnerId { get; set; }

        [JsonProperty("report_header")]
        public string ReportHeader { get; set; }

        [JsonProperty("report_footer")]
        public string ReportFooter { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("logo_web")]
        public string LogoWeb { get; set; }

        // res.currency
        [JsonProperty("currency_id")]
        public long CurrencyId { get; set; }

        // res.users
        [JsonProperty("user_ids")]
        public long[] UserIds { get; set; }

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

        // res.partner.bank
        [JsonProperty("bank_ids")]
        public long[] BankIds { get; set; }

        // res.country
        [JsonProperty("country_id")]
        public long? CountryId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("vat")]
        public string Vat { get; set; }

        [JsonProperty("company_registry")]
        public string CompanyRegistry { get; set; }

        // report.paperformat
        [JsonProperty("paperformat_id")]
        public long? PaperformatId { get; set; }

        // ir.ui.view
        [JsonProperty("external_report_layout_id")]
        public long? ExternalReportLayoutId { get; set; }

        [JsonProperty("base_onboarding_company_state")]
        public string BaseOnboardingCompanyState { get; set; }

        [JsonProperty("favicon")]
        public string Favicon { get; set; }

        [JsonProperty("font")]
        public string Font { get; set; }

        [JsonProperty("primary_color")]
        public string PrimaryColor { get; set; }

        [JsonProperty("secondary_color")]
        public string SecondaryColor { get; set; }

        [JsonProperty("social_twitter")]
        public string SocialTwitter { get; set; }

        [JsonProperty("social_facebook")]
        public string SocialFacebook { get; set; }

        [JsonProperty("social_github")]
        public string SocialGithub { get; set; }

        [JsonProperty("social_linkedin")]
        public string SocialLinkedin { get; set; }

        [JsonProperty("social_youtube")]
        public string SocialYoutube { get; set; }

        [JsonProperty("social_instagram")]
        public string SocialInstagram { get; set; }

        // barcode.nomenclature
        [JsonProperty("nomenclature_id")]
        public long? NomenclatureId { get; set; }

        // resource.calendar
        [JsonProperty("resource_calendar_ids")]
        public long[] ResourceCalendarIds { get; set; }

        // resource.calendar
        [JsonProperty("resource_calendar_id")]
        public long? ResourceCalendarId { get; set; }

        [JsonProperty("catchall_email")]
        public string CatchallEmail { get; set; }

        [JsonProperty("catchall_formatted")]
        public string CatchallFormatted { get; set; }

        [JsonProperty("email_formatted")]
        public string EmailFormatted { get; set; }

        [JsonProperty("partner_gid")]
        public int? PartnerGid { get; set; }

        [JsonProperty("snailmail_color")]
        public bool? SnailmailColor { get; set; }

        [JsonProperty("snailmail_cover")]
        public bool? SnailmailCover { get; set; }

        [JsonProperty("snailmail_duplex")]
        public bool? SnailmailDuplex { get; set; }

        [JsonProperty("fiscalyear_last_day")]
        public int FiscalyearLastDay { get; set; }

        [JsonProperty("fiscalyear_last_month")]
        public string FiscalyearLastMonth { get; set; }

        [JsonProperty("period_lock_date")]
        public DateTime? PeriodLockDate { get; set; }

        [JsonProperty("fiscalyear_lock_date")]
        public DateTime? FiscalyearLockDate { get; set; }

        [JsonProperty("tax_lock_date")]
        public DateTime? TaxLockDate { get; set; }

        // account.account
        [JsonProperty("transfer_account_id")]
        public long? TransferAccountId { get; set; }

        [JsonProperty("expects_chart_of_accounts")]
        public bool? ExpectsChartOfAccounts { get; set; }

        // account.chart.template
        [JsonProperty("chart_template_id")]
        public long? ChartTemplateId { get; set; }

        [JsonProperty("bank_account_code_prefix")]
        public string BankAccountCodePrefix { get; set; }

        [JsonProperty("cash_account_code_prefix")]
        public string CashAccountCodePrefix { get; set; }

        // account.account
        [JsonProperty("default_cash_difference_income_account_id")]
        public long? DefaultCashDifferenceIncomeAccountId { get; set; }

        // account.account
        [JsonProperty("default_cash_difference_expense_account_id")]
        public long? DefaultCashDifferenceExpenseAccountId { get; set; }

        // account.account
        [JsonProperty("account_journal_suspense_account_id")]
        public long? AccountJournalSuspenseAccountId { get; set; }

        [JsonProperty("transfer_account_code_prefix")]
        public string TransferAccountCodePrefix { get; set; }

        // account.tax
        [JsonProperty("account_sale_tax_id")]
        public long? AccountSaleTaxId { get; set; }

        // account.tax
        [JsonProperty("account_purchase_tax_id")]
        public long? AccountPurchaseTaxId { get; set; }

        [JsonProperty("tax_calculation_rounding_method")]
        public string TaxCalculationRoundingMethod { get; set; }

        // account.journal
        [JsonProperty("currency_exchange_journal_id")]
        public long? CurrencyExchangeJournalId { get; set; }

        // account.account
        [JsonProperty("income_currency_exchange_account_id")]
        public long? IncomeCurrencyExchangeAccountId { get; set; }

        // account.account
        [JsonProperty("expense_currency_exchange_account_id")]
        public long? ExpenseCurrencyExchangeAccountId { get; set; }

        [JsonProperty("anglo_saxon_accounting")]
        public bool? AngloSaxonAccounting { get; set; }

        // account.account
        [JsonProperty("property_stock_account_input_categ_id")]
        public long? PropertyStockAccountInputCategId { get; set; }

        // account.account
        [JsonProperty("property_stock_account_output_categ_id")]
        public long? PropertyStockAccountOutputCategId { get; set; }

        // account.account
        [JsonProperty("property_stock_valuation_account_id")]
        public long? PropertyStockValuationAccountId { get; set; }

        // account.journal
        [JsonProperty("bank_journal_ids")]
        public long[] BankJournalIds { get; set; }

        [JsonProperty("tax_exigibility")]
        public bool? TaxExigibility { get; set; }

        // res.country
        [JsonProperty("account_tax_fiscal_country_id")]
        public long? AccountTaxFiscalCountryId { get; set; }

        // account.incoterms
        [JsonProperty("incoterm_id")]
        public long? IncotermId { get; set; }

        [JsonProperty("qr_code")]
        public bool? QrCode { get; set; }

        [JsonProperty("invoice_is_email")]
        public bool? InvoiceIsEmail { get; set; }

        [JsonProperty("invoice_is_print")]
        public bool? InvoiceIsPrint { get; set; }

        // account.move
        [JsonProperty("account_opening_move_id")]
        public long? AccountOpeningMoveId { get; set; }

        // account.journal
        [JsonProperty("account_opening_journal_id")]
        public long? AccountOpeningJournalId { get; set; }

        [JsonProperty("account_opening_date")]
        public DateTime AccountOpeningDate { get; set; }

        [JsonProperty("account_setup_bank_data_state")]
        public string AccountSetupBankDataState { get; set; }

        [JsonProperty("account_setup_fy_data_state")]
        public string AccountSetupFyDataState { get; set; }

        [JsonProperty("account_setup_coa_state")]
        public string AccountSetupCoaState { get; set; }

        [JsonProperty("account_onboarding_invoice_layout_state")]
        public string AccountOnboardingInvoiceLayoutState { get; set; }

        [JsonProperty("account_onboarding_create_invoice_state")]
        public string AccountOnboardingCreateInvoiceState { get; set; }

        [JsonProperty("account_onboarding_sale_tax_state")]
        public string AccountOnboardingSaleTaxState { get; set; }

        [JsonProperty("account_invoice_onboarding_state")]
        public string AccountInvoiceOnboardingState { get; set; }

        [JsonProperty("account_dashboard_onboarding_state")]
        public string AccountDashboardOnboardingState { get; set; }

        [JsonProperty("invoice_terms")]
        public string InvoiceTerms { get; set; }

        [JsonProperty("account_setup_bill_state")]
        public string AccountSetupBillState { get; set; }

        // account.account
        [JsonProperty("account_default_pos_receivable_account_id")]
        public long? AccountDefaultPosReceivableAccountId { get; set; }

        // account.account
        [JsonProperty("expense_accrual_account_id")]
        public long? ExpenseAccrualAccountId { get; set; }

        // account.account
        [JsonProperty("revenue_accrual_account_id")]
        public long? RevenueAccrualAccountId { get; set; }

        // account.journal
        [JsonProperty("automatic_entry_default_journal_id")]
        public long? AutomaticEntryDefaultJournalId { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        // account.journal
        [JsonProperty("tax_cash_basis_journal_id")]
        public long? TaxCashBasisJournalId { get; set; }

        // account.account
        [JsonProperty("account_cash_basis_base_account_id")]
        public long? AccountCashBasisBaseAccountId { get; set; }

        // stock.location
        [JsonProperty("internal_transit_location_id")]
        public long? InternalTransitLocationId { get; set; }

        [JsonProperty("stock_move_email_validation")]
        public bool? StockMoveEmailValidation { get; set; }

        // mail.template
        [JsonProperty("stock_mail_confirmation_template_id")]
        public long? StockMailConfirmationTemplateId { get; set; }

        [JsonProperty("website_theme_onboarding_done")]
        public bool? WebsiteThemeOnboardingDone { get; set; }

        [JsonProperty("invoicing_switch_threshold")]
        public DateTime? InvoicingSwitchThreshold { get; set; }

        [JsonProperty("extract_show_ocr_option_selection")]
        public string ExtractShowOcrOptionSelection { get; set; }

        [JsonProperty("extract_single_line_per_tax")]
        public bool? ExtractSingleLinePerTax { get; set; }

        [JsonProperty("vat_check_vies")]
        public bool? VatCheckVies { get; set; }

        [JsonProperty("currency_interval_unit")]
        public string CurrencyIntervalUnit { get; set; }

        [JsonProperty("currency_next_execution_date")]
        public DateTime? CurrencyNextExecutionDate { get; set; }

        [JsonProperty("currency_provider")]
        public string CurrencyProvider { get; set; }

        [JsonProperty("documents_product_settings")]
        public bool? DocumentsProductSettings { get; set; }

        // documents.folder
        [JsonProperty("product_folder")]
        public long? ProductFolder { get; set; }

        // documents.tag
        [JsonProperty("product_tags")]
        public long[] ProductTags { get; set; }

        [JsonProperty("payment_acquirer_onboarding_state")]
        public string PaymentAcquirerOnboardingState { get; set; }

        [JsonProperty("payment_onboarding_payment_method")]
        public string PaymentOnboardingPaymentMethod { get; set; }

        [JsonProperty("po_lead")]
        public double PoLead { get; set; }

        [JsonProperty("po_lock")]
        public string PoLock { get; set; }

        [JsonProperty("po_double_validation")]
        public string PoDoubleValidation { get; set; }

        [JsonProperty("po_double_validation_amount")]
        public decimal? PoDoubleValidationAmount { get; set; }

        [JsonProperty("invoice_is_snailmail")]
        public bool? InvoiceIsSnailmail { get; set; }

        [JsonProperty("stock_move_sms_validation")]
        public bool? StockMoveSmsValidation { get; set; }

        // sms.template
        [JsonProperty("stock_sms_confirmation_template_id")]
        public long? StockSmsConfirmationTemplateId { get; set; }

        [JsonProperty("has_received_warning_stock_sms")]
        public bool? HasReceivedWarningStockSms { get; set; }

        [JsonProperty("totals_below_sections")]
        public bool? TotalsBelowSections { get; set; }

        [JsonProperty("account_tax_periodicity")]
        public string AccountTaxPeriodicity { get; set; }

        [JsonProperty("account_tax_periodicity_reminder_day")]
        public int? AccountTaxPeriodicityReminderDay { get; set; }

        [JsonProperty("account_tax_original_periodicity_reminder_day")]
        public int? AccountTaxOriginalPeriodicityReminderDay { get; set; }

        // account.journal
        [JsonProperty("account_tax_periodicity_journal_id")]
        public long? AccountTaxPeriodicityJournalId { get; set; }

        // mail.activity.type
        [JsonProperty("account_tax_next_activity_type")]
        public long? AccountTaxNextActivityType { get; set; }

        // account.journal
        [JsonProperty("account_revaluation_journal_id")]
        public long? AccountRevaluationJournalId { get; set; }

        // account.account
        [JsonProperty("account_revaluation_expense_provision_account_id")]
        public long? AccountRevaluationExpenseProvisionAccountId { get; set; }

        // account.account
        [JsonProperty("account_revaluation_income_provision_account_id")]
        public long? AccountRevaluationIncomeProvisionAccountId { get; set; }

        [JsonProperty("yodlee_access_token")]
        public string YodleeAccessToken { get; set; }

        [JsonProperty("yodlee_user_login")]
        public string YodleeUserLogin { get; set; }

        [JsonProperty("yodlee_user_password")]
        public string YodleeUserPassword { get; set; }

        [JsonProperty("yodlee_user_access_token")]
        public string YodleeUserAccessToken { get; set; }

        [JsonProperty("days_to_purchase")]
        public double? DaysToPurchase { get; set; }

        [JsonProperty("portal_confirmation_sign")]
        public bool? PortalConfirmationSign { get; set; }

        [JsonProperty("portal_confirmation_pay")]
        public bool? PortalConfirmationPay { get; set; }

        [JsonProperty("quotation_validity_days")]
        public int? QuotationValidityDays { get; set; }

        [JsonProperty("sale_quotation_onboarding_state")]
        public string SaleQuotationOnboardingState { get; set; }

        [JsonProperty("sale_onboarding_order_confirmation_state")]
        public string SaleOnboardingOrderConfirmationState { get; set; }

        [JsonProperty("sale_onboarding_sample_quotation_state")]
        public string SaleOnboardingSampleQuotationState { get; set; }

        [JsonProperty("sale_onboarding_payment_method")]
        public string SaleOnboardingPaymentMethod { get; set; }

        // account.account
        [JsonProperty("gain_account_id")]
        public long? GainAccountId { get; set; }

        // account.account
        [JsonProperty("loss_account_id")]
        public long? LossAccountId { get; set; }

        [JsonProperty("sepa_orgid_id")]
        public string SepaOrgidId { get; set; }

        [JsonProperty("sepa_orgid_issr")]
        public string SepaOrgidIssr { get; set; }

        [JsonProperty("sepa_initiating_party_name")]
        public string SepaInitiatingPartyName { get; set; }

        [JsonProperty("sepa_pain_version")]
        public string SepaPainVersion { get; set; }

        [JsonProperty("documents_account_settings")]
        public bool? DocumentsAccountSettings { get; set; }

        // documents.folder
        [JsonProperty("account_folder")]
        public long? AccountFolder { get; set; }

        // sale.order.template
        [JsonProperty("sale_order_template_id")]
        public long? SaleOrderTemplateId { get; set; }

        [JsonProperty("extra_hour")]
        public double? ExtraHour { get; set; }

        [JsonProperty("extra_day")]
        public double? ExtraDay { get; set; }

        [JsonProperty("min_extra_hour")]
        public int? MinExtraHour { get; set; }

        // product.product
        [JsonProperty("extra_product")]
        public long? ExtraProduct { get; set; }

        [JsonProperty("security_lead")]
        public double SecurityLead { get; set; }

        // stock.location
        [JsonProperty("rental_loc_id")]
        public long? RentalLocId { get; set; }

        [JsonProperty("padding_time")]
        public double? PaddingTime { get; set; }

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