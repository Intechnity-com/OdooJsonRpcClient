using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("account.account.type")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class AccountAccountTypeOdooModel : IOdooModel
    {

        // required
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("include_initial_balance")]
        public bool? IncludeInitialBalance { get; set; }

        // required
        [JsonProperty("type")]
        public TypeAccountAccountTypeOdooEnum Type { get; set; }

        // required
        [JsonProperty("internal_group")]
        public InternalGroupAccountAccountTypeOdooEnum InternalGroup { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

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
    public enum TypeAccountAccountTypeOdooEnum
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
    public enum InternalGroupAccountAccountTypeOdooEnum
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

}