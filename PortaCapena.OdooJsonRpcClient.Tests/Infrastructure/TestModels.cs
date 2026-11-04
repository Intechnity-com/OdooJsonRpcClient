using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Tests.Infrastructure
{
    [OdooTableName("test.partner")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class TestPartnerOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_company")]
        public bool IsCompany { get; set; }

        [JsonProperty("credit_limit")]
        public double? CreditLimit { get; set; }

        [JsonProperty("balance")]
        public decimal? Balance { get; set; }

        [JsonProperty("color")]
        public int? Color { get; set; }

        [JsonProperty("country_id")]
        public long? CountryId { get; set; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

        [JsonProperty("category_id")]
        public long[] CategoryId { get; set; }

        [JsonProperty("create_date")]
        public DateTime? CreateDate { get; set; }

        [JsonProperty("type")]
        public TestPartnerTypeOdooEnum? Type { get; set; }

        [JsonProperty("properties")]
        public string Properties { get; set; }

        [JsonProperty("raw")]
        public object Raw { get; set; }

        [JsonIgnore]
        public string Computed => Name?.ToUpperInvariant();
    }

    [OdooTableName("test.partner")]
    public class TestPartnerCreateModel : IOdooCreateModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public TestPartnerTypeOdooEnum? Type { get; set; }
    }

    [OdooTableName("test.country")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class TestCountryOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("currency_id")]
        public long CurrencyId { get; set; }
    }

    [OdooTableName("test.currency")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class TestCurrencyOdooModel : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    [JsonConverter(typeof(OdooModelConverter))]
    public class ModelWithoutJsonProperty : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        public string NotMapped { get; set; }
    }

    [OdooTableName("test.other")]
    public class TableModelWithoutJsonProperty : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        public string NotMapped { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TestPartnerTypeOdooEnum
    {
        [EnumMember(Value = "contact")]
        Contact = 1,

        [EnumMember(Value = "invoice")]
        InvoiceAddress = 2,
    }
}
