using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("account.account")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class AccountAccountOdooModel : IOdooModel
    {

        // res.currency
        [JsonProperty("currency_id")]
        public long? CurrencyId { get; set; }

        // required
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("deprecated")]
        public bool? Deprecated { get; set; }

        [JsonProperty("used")]
        public bool? Used { get; set; }

        // account.account.type
        // required
        [JsonProperty("user_type_id")]
        public long UserTypeId { get; set; }

        [JsonProperty("internal_type")]
        public InternalTypeAccountAccountOdooEnum? InternalType { get; set; }

        [JsonProperty("internal_group")]
        public InternalGroupAccountAccountOdooEnum? InternalGroup { get; set; }

        [JsonProperty("reconcile")]
        public bool? Reconcile { get; set; }

        // account.tax
        [JsonProperty("tax_ids")]
        public long[] TaxIds { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        // res.company
        // required
        [JsonProperty("company_id")]
        public long CompanyId { get; set; }

        // account.account.tag
        [JsonProperty("tag_ids")]
        public long[] TagIds { get; set; }

        // account.group
        [JsonProperty("group_id")]
        public long? GroupId { get; set; }

        // account.root
        [JsonProperty("root_id")]
        public long? RootId { get; set; }

        // account.journal
        [JsonProperty("allowed_journal_ids")]
        public long[] AllowedJournalIds { get; set; }

        [JsonProperty("opening_debit")]
        public decimal? OpeningDebit { get; set; }

        [JsonProperty("opening_credit")]
        public decimal? OpeningCredit { get; set; }

        [JsonProperty("opening_balance")]
        public decimal? OpeningBalance { get; set; }

        [JsonProperty("is_off_balance")]
        public bool? IsOffBalance { get; set; }

        // required
        [JsonProperty("name")]
        public string Name { get; set; }

        // res.currency
        [JsonProperty("exclude_provision_currency_ids")]
        public long[] ExcludeProvisionCurrencyIds { get; set; }

        // account.asset
        [JsonProperty("asset_model")]
        public long? AssetModel { get; set; }

        // required
        [JsonProperty("create_asset")]
        public CreateAssetAccountAccountOdooEnum CreateAsset { get; set; }

        [JsonProperty("can_create_asset")]
        public bool? CanCreateAsset { get; set; }

        [JsonProperty("form_view_ref")]
        public string FormViewRef { get; set; }

        [JsonProperty("asset_type")]
        public AssetTypeAccountAccountOdooEnum? AssetType { get; set; }

        [JsonProperty("multiple_assets_per_line")]
        public bool? MultipleAssetsPerLine { get; set; }

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


    // The 'Internal Type' is used for features available on different types of accounts: liquidity type is for cash or bank accounts, payable/receivable is for vendor/customer accounts.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InternalTypeAccountAccountOdooEnum
    {
        [EnumMember(Value = "other")]
        Regular = 1,

        [EnumMember(Value = "receivable")]
        Receivable = 2,

        [EnumMember(Value = "payable")]
        Payable = 3,

        [EnumMember(Value = "liquidity")]
        Liquidity = 4,
    }


    // The 'Internal Group' is used to filter accounts based on the internal group set on the account type.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InternalGroupAccountAccountOdooEnum
    {
        [EnumMember(Value = "equity")]
        Equity = 1,

        [EnumMember(Value = "asset")]
        Asset = 2,

        [EnumMember(Value = "liability")]
        Liability = 3,

        [EnumMember(Value = "income")]
        Income = 4,

        [EnumMember(Value = "expense")]
        Expense = 5,

        [EnumMember(Value = "off_balance")]
        OffBalance = 6,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum CreateAssetAccountAccountOdooEnum
    {
        [EnumMember(Value = "no")]
        No = 1,

        [EnumMember(Value = "draft")]
        CreateInDraft = 2,

        [EnumMember(Value = "validate")]
        CreateAndValidate = 3,
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum AssetTypeAccountAccountOdooEnum
    {
        [EnumMember(Value = "sale")]
        DeferredRevenue = 1,

        [EnumMember(Value = "expense")]
        DeferredExpense = 2,

        [EnumMember(Value = "purchase")]
        Asset = 3,
    }

}