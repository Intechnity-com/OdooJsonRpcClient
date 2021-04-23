using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("account.tax")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class AccountTaxOdooModel : IOdooModel
    {

        // required
        [JsonProperty("type_tax_use")]
        public TaxTypeAccountTaxOdooEnum TypeTaxUse { get; set; }

        [JsonProperty("tax_scope")]
        public TaxScopeAccountTaxOdooEnum? TaxScope { get; set; }

        // required
        [JsonProperty("amount_type")]
        public TaxComputationAccountTaxOdooEnum AmountType { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        // res.company
        // required
        [JsonProperty("company_id")]
        public long CompanyId { get; set; }

        // account.tax
        [JsonProperty("children_tax_ids")]
        public long[] ChildrenTaxIds { get; set; }

        // required
        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        // required
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("price_include")]
        public bool? PriceInclude { get; set; }

        [JsonProperty("include_base_amount")]
        public bool? IncludeBaseAmount { get; set; }

        [JsonProperty("analytic")]
        public bool? Analytic { get; set; }

        // account.tax.group
        // required
        [JsonProperty("tax_group_id")]
        public long TaxGroupId { get; set; }

        [JsonProperty("hide_tax_exigibility")]
        public bool? HideTaxExigibility { get; set; }

        [JsonProperty("tax_exigibility")]
        public TaxDueAccountTaxOdooEnum? TaxExigibility { get; set; }

        // account.account
        [JsonProperty("cash_basis_transition_account_id")]
        public long? CashBasisTransitionAccountId { get; set; }

        // account.tax.repartition.line
        [JsonProperty("invoice_repartition_line_ids")]
        public long[] InvoiceRepartitionLineIds { get; set; }

        // account.tax.repartition.line
        [JsonProperty("refund_repartition_line_ids")]
        public long[] RefundRepartitionLineIds { get; set; }

        // res.country
        [JsonProperty("tax_fiscal_country_id")]
        public long? TaxFiscalCountryId { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        // required
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

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


    // Determines where the tax is selectable. Note : 'None' means a tax can't be used by itself, however it can still be used in a group. 'adjustment' is used to perform tax adjustment.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaxTypeAccountTaxOdooEnum
    {
        [EnumMember(Value = "sale")]
        Sales = 1,

        [EnumMember(Value = "purchase")]
        Purchases = 2,

        [EnumMember(Value = "none")]
        None = 3,
    }


    // Restrict the use of taxes to a type of product.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaxScopeAccountTaxOdooEnum
    {
        [EnumMember(Value = "service")]
        Services = 1,

        [EnumMember(Value = "consu")]
        Goods = 2,
    }


    // 
    //     - Group of Taxes: The tax is a set of sub taxes.
    //     - Fixed: The tax amount stays the same whatever the price.
    //     - Percentage of Price: The tax amount is a % of the price:
    //         e.g 100 * (1 + 10%) = 110 (not price included)
    //         e.g 110 / (1 + 10%) = 100 (price included)
    //     - Percentage of Price Tax Included: The tax amount is a division of the price:
    //         e.g 180 / (1 - 10%) = 200 (not price included)
    //         e.g 200 * (1 - 10%) = 180 (price included)
    //         
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaxComputationAccountTaxOdooEnum
    {
        [EnumMember(Value = "group")]
        GroupOfTaxes = 1,

        [EnumMember(Value = "fixed")]
        Fixed = 2,

        [EnumMember(Value = "percent")]
        PercentageOfPrice = 3,

        [EnumMember(Value = "division")]
        PercentageOfPriceTaxIncluded = 4,
    }


    // Based on Invoice: the tax is due as soon as the invoice is validated.
    // Based on Payment: the tax is due as soon as the payment of the invoice is received.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaxDueAccountTaxOdooEnum
    {
        [EnumMember(Value = "on_invoice")]
        BasedOnInvoice = 1,

        [EnumMember(Value = "on_payment")]
        BasedOnPayment = 2,
    }

}