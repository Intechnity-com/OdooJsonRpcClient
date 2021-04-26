using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("res.company")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class ResCompanyOdooModel : IOdooModel
    {

        // required
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
        // required
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
        // required
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
        public StateOfTheOnboardingCompanyStepResCompanyOdooEnum? BaseOnboardingCompanyState { get; set; }

        [JsonProperty("favicon")]
        public string Favicon { get; set; }

        [JsonProperty("font")]
        public FontResCompanyOdooEnum? Font { get; set; }

        [JsonProperty("primary_color")]
        public string PrimaryColor { get; set; }

        [JsonProperty("secondary_color")]
        public string SecondaryColor { get; set; }

        [JsonProperty("street_name")]
        public string StreetName { get; set; }

        [JsonProperty("street_number")]
        public string StreetNumber { get; set; }

        [JsonProperty("street_number2")]
        public string StreetNumber2 { get; set; }

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

        [JsonProperty("background_image")]
        public string BackgroundImage { get; set; }

        // required
        [JsonProperty("fiscalyear_last_day")]
        public int FiscalyearLastDay { get; set; }

        // required
        [JsonProperty("fiscalyear_last_month")]
        public FiscalyearLastMonthResCompanyOdooEnum FiscalyearLastMonth { get; set; }

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
        public TaxCalculationRoundingMethodResCompanyOdooEnum? TaxCalculationRoundingMethod { get; set; }

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

        // required
        [JsonProperty("account_opening_date")]
        public DateTime AccountOpeningDate { get; set; }

        [JsonProperty("account_setup_bank_data_state")]
        public StateOfTheOnboardingBankDataStepResCompanyOdooEnum? AccountSetupBankDataState { get; set; }

        [JsonProperty("account_setup_fy_data_state")]
        public StateOfTheOnboardingFiscalYearStepResCompanyOdooEnum? AccountSetupFyDataState { get; set; }

        [JsonProperty("account_setup_coa_state")]
        public StateOfTheOnboardingChartsOfAccountStepResCompanyOdooEnum? AccountSetupCoaState { get; set; }

        [JsonProperty("account_onboarding_invoice_layout_state")]
        public StateOfTheOnboardingInvoiceLayoutStepResCompanyOdooEnum? AccountOnboardingInvoiceLayoutState { get; set; }

        [JsonProperty("account_onboarding_create_invoice_state")]
        public StateOfTheOnboardingCreateInvoiceStepResCompanyOdooEnum? AccountOnboardingCreateInvoiceState { get; set; }

        [JsonProperty("account_onboarding_sale_tax_state")]
        public StateOfTheOnboardingSaleTaxStepResCompanyOdooEnum? AccountOnboardingSaleTaxState { get; set; }

        [JsonProperty("account_invoice_onboarding_state")]
        public StateOfTheAccountInvoiceOnboardingPanelResCompanyOdooEnum? AccountInvoiceOnboardingState { get; set; }

        [JsonProperty("account_dashboard_onboarding_state")]
        public StateOfTheAccountDashboardOnboardingPanelResCompanyOdooEnum? AccountDashboardOnboardingState { get; set; }

        [JsonProperty("invoice_terms")]
        public string InvoiceTerms { get; set; }

        [JsonProperty("account_setup_bill_state")]
        public StateOfTheOnboardingBillStepResCompanyOdooEnum? AccountSetupBillState { get; set; }

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

        [JsonProperty("invoicing_switch_threshold")]
        public DateTime? InvoicingSwitchThreshold { get; set; }

        [JsonProperty("extract_show_ocr_option_selection")]
        public SendModeOnInvoicesAttachmentsResCompanyOdooEnum? ExtractShowOcrOptionSelection { get; set; }

        [JsonProperty("extract_single_line_per_tax")]
        public bool? ExtractSingleLinePerTax { get; set; }

        [JsonProperty("vat_check_vies")]
        public bool? VatCheckVies { get; set; }

        [JsonProperty("currency_interval_unit")]
        public IntervalUnitResCompanyOdooEnum? CurrencyIntervalUnit { get; set; }

        [JsonProperty("currency_next_execution_date")]
        public DateTime? CurrencyNextExecutionDate { get; set; }

        [JsonProperty("currency_provider")]
        public ServiceProviderResCompanyOdooEnum? CurrencyProvider { get; set; }

        [JsonProperty("payment_acquirer_onboarding_state")]
        public StateOfTheOnboardingPaymentAcquirerStepResCompanyOdooEnum? PaymentAcquirerOnboardingState { get; set; }

        [JsonProperty("payment_onboarding_payment_method")]
        public SelectedOnboardingPaymentMethodResCompanyOdooEnum? PaymentOnboardingPaymentMethod { get; set; }

        // required
        [JsonProperty("po_lead")]
        public double PoLead { get; set; }

        [JsonProperty("po_lock")]
        public PurchaseOrderModificationResCompanyOdooEnum? PoLock { get; set; }

        [JsonProperty("po_double_validation")]
        public LevelsOfApprovalsResCompanyOdooEnum? PoDoubleValidation { get; set; }

        [JsonProperty("po_double_validation_amount")]
        public decimal? PoDoubleValidationAmount { get; set; }

        [JsonProperty("invoice_is_snailmail")]
        public bool? InvoiceIsSnailmail { get; set; }

        [JsonProperty("totals_below_sections")]
        public bool? TotalsBelowSections { get; set; }

        [JsonProperty("account_tax_periodicity")]
        public DelayUnitsResCompanyOdooEnum? AccountTaxPeriodicity { get; set; }

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

        // ir.sequence
        [JsonProperty("l10n_fr_closing_sequence_id")]
        public long? L10nFrClosingSequenceId { get; set; }

        [JsonProperty("siret")]
        public string Siret { get; set; }

        [JsonProperty("ape")]
        public string Ape { get; set; }

        [JsonProperty("portal_confirmation_sign")]
        public bool? PortalConfirmationSign { get; set; }

        [JsonProperty("portal_confirmation_pay")]
        public bool? PortalConfirmationPay { get; set; }

        [JsonProperty("quotation_validity_days")]
        public int? QuotationValidityDays { get; set; }

        [JsonProperty("sale_quotation_onboarding_state")]
        public StateOfTheSaleOnboardingPanelResCompanyOdooEnum? SaleQuotationOnboardingState { get; set; }

        [JsonProperty("sale_onboarding_order_confirmation_state")]
        public StateOfTheOnboardingConfirmationOrderStepResCompanyOdooEnum? SaleOnboardingOrderConfirmationState { get; set; }

        [JsonProperty("sale_onboarding_sample_quotation_state")]
        public StateOfTheOnboardingSampleQuotationStepResCompanyOdooEnum? SaleOnboardingSampleQuotationState { get; set; }

        [JsonProperty("sale_onboarding_payment_method")]
        public SaleOnboardingSelectedPaymentMethodResCompanyOdooEnum? SaleOnboardingPaymentMethod { get; set; }

        // account.account
        [JsonProperty("gain_account_id")]
        public long? GainAccountId { get; set; }

        // account.account
        [JsonProperty("loss_account_id")]
        public long? LossAccountId { get; set; }

        [JsonProperty("l10n_de_datev_consultant_number")]
        public string L10nDeDatevConsultantNumber { get; set; }

        [JsonProperty("l10n_de_datev_client_number")]
        public string L10nDeDatevClientNumber { get; set; }

        // sale.order.template
        [JsonProperty("sale_order_template_id")]
        public long? SaleOrderTemplateId { get; set; }

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


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingCompanyStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum FontResCompanyOdooEnum
    {
        [EnumMember(Value = "Lato")]
        Lato = 1,

        [EnumMember(Value = "Roboto")]
        Roboto = 2,

        [EnumMember(Value = "Open_Sans")]
        OpenSans = 3,

        [EnumMember(Value = "Montserrat")]
        Montserrat = 4,

        [EnumMember(Value = "Oswald")]
        Oswald = 5,

        [EnumMember(Value = "Raleway")]
        Raleway = 6,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum FiscalyearLastMonthResCompanyOdooEnum
    {
        [EnumMember(Value = "1")]
        January = 1,

        [EnumMember(Value = "2")]
        February = 2,

        [EnumMember(Value = "3")]
        March = 3,

        [EnumMember(Value = "4")]
        April = 4,

        [EnumMember(Value = "5")]
        May = 5,

        [EnumMember(Value = "6")]
        June = 6,

        [EnumMember(Value = "7")]
        July = 7,

        [EnumMember(Value = "8")]
        August = 8,

        [EnumMember(Value = "9")]
        September = 9,

        [EnumMember(Value = "10")]
        October = 10,

        [EnumMember(Value = "11")]
        November = 11,

        [EnumMember(Value = "12")]
        December = 12,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaxCalculationRoundingMethodResCompanyOdooEnum
    {
        [EnumMember(Value = "round_per_line")]
        RoundPerLine = 1,

        [EnumMember(Value = "round_globally")]
        RoundGlobally = 2,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingBankDataStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingFiscalYearStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingChartsOfAccountStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingInvoiceLayoutStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingCreateInvoiceStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingSaleTaxStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheAccountInvoiceOnboardingPanelResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,

        [EnumMember(Value = "closed")]
        Closed = 4,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheAccountDashboardOnboardingPanelResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,

        [EnumMember(Value = "closed")]
        Closed = 4,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingBillStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum SendModeOnInvoicesAttachmentsResCompanyOdooEnum
    {
        [EnumMember(Value = "no_send")]
        DoNotDigitalizeBills = 1,

        [EnumMember(Value = "manual_send")]
        DigitalizeBillsOnDemandOnly = 2,

        [EnumMember(Value = "auto_send")]
        DigitalizeAllBillsAutomatically = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum IntervalUnitResCompanyOdooEnum
    {
        [EnumMember(Value = "manually")]
        Manually = 1,

        [EnumMember(Value = "daily")]
        Daily = 2,

        [EnumMember(Value = "weekly")]
        Weekly = 3,

        [EnumMember(Value = "monthly")]
        Monthly = 4,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum ServiceProviderResCompanyOdooEnum
    {
        [EnumMember(Value = "ecb")]
        EuropeanCentralBank = 1,

        [EnumMember(Value = "fta")]
        FederalTaxAdministrationSwitzerland = 2,

        [EnumMember(Value = "banxico")]
        MexicanBank = 3,

        [EnumMember(Value = "boc")]
        BankOfCanada = 4,

        [EnumMember(Value = "xe_com")]
        XeCom = 5,

        [EnumMember(Value = "bnr")]
        NationalBankOfRomania = 6,

        [EnumMember(Value = "mindicador")]
        ChileanMindicadorCl = 7,

        [EnumMember(Value = "bcrp")]
        BankOfPeru = 8,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingPaymentAcquirerStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum SelectedOnboardingPaymentMethodResCompanyOdooEnum
    {
        [EnumMember(Value = "paypal")]
        Paypal = 1,

        [EnumMember(Value = "stripe")]
        Stripe = 2,

        [EnumMember(Value = "manual")]
        Manual = 3,

        [EnumMember(Value = "other")]
        Other = 4,
    }


    // Purchase Order Modification used when you want to purchase order editable after confirm
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PurchaseOrderModificationResCompanyOdooEnum
    {
        [EnumMember(Value = "edit")]
        AllowToEditPurchaseOrders = 1,

        [EnumMember(Value = "lock")]
        ConfirmedPurchaseOrdersAreNotEditable = 2,
    }


    // Provide a double validation mechanism for purchases
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LevelsOfApprovalsResCompanyOdooEnum
    {
        [EnumMember(Value = "one_step")]
        ConfirmPurchaseOrdersInOneStep = 1,

        [EnumMember(Value = "two_step")]
        Get2LevelsOfApprovalsToConfirmAPurchaseOrder = 2,
    }


    // Periodicity
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DelayUnitsResCompanyOdooEnum
    {
        [EnumMember(Value = "year")]
        Annually = 1,

        [EnumMember(Value = "semester")]
        SemiAnnually = 2,

        [EnumMember(Value = "4_months")]
        Every4Months = 3,

        [EnumMember(Value = "trimester")]
        Quarterly = 4,

        [EnumMember(Value = "2_months")]
        Every2Months = 5,

        [EnumMember(Value = "monthly")]
        Monthly = 6,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheSaleOnboardingPanelResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,

        [EnumMember(Value = "closed")]
        Closed = 4,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingConfirmationOrderStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateOfTheOnboardingSampleQuotationStepResCompanyOdooEnum
    {
        [EnumMember(Value = "not_done")]
        NotDone = 1,

        [EnumMember(Value = "just_done")]
        JustDone = 2,

        [EnumMember(Value = "done")]
        Done = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum SaleOnboardingSelectedPaymentMethodResCompanyOdooEnum
    {
        [EnumMember(Value = "digital_signature")]
        SignOnline = 1,

        [EnumMember(Value = "paypal")]
        Paypal = 2,

        [EnumMember(Value = "stripe")]
        Stripe = 3,

        [EnumMember(Value = "other")]
        PayWithAnotherPaymentAcquirer = 4,

        [EnumMember(Value = "manual")]
        ManualPayment = 5,
    }

}