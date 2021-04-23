using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("res.country")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class ResCountryOdooModel : IOdooModel
    {

        // required
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("address_format")]
        public string AddressFormat { get; set; }

        // ir.ui.view
        [JsonProperty("address_view_id")]
        public long? AddressViewId { get; set; }

        // res.currency
        [JsonProperty("currency_id")]
        public long? CurrencyId { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("phone_code")]
        public int? PhoneCode { get; set; }

        // res.country.group
        [JsonProperty("country_group_ids")]
        public long[] CountryGroupIds { get; set; }

        // res.country.state
        [JsonProperty("state_ids")]
        public long[] StateIds { get; set; }

        [JsonProperty("name_position")]
        public CustomerNamePositionResCountryOdooEnum? NamePosition { get; set; }

        [JsonProperty("vat_label")]
        public string VatLabel { get; set; }

        [JsonProperty("state_required")]
        public bool? StateRequired { get; set; }

        [JsonProperty("zip_required")]
        public bool? ZipRequired { get; set; }

        // required
        [JsonProperty("street_format")]
        public string StreetFormat { get; set; }

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

        [JsonProperty("x_studio_is_in_eu")]
        public bool? XStudioIsInEu { get; set; }
    }


    // Determines where the customer/company name should be placed, i.e. after or before the address.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CustomerNamePositionResCountryOdooEnum
    {
        [EnumMember(Value = "before")]
        BeforeAddress = 1,

        [EnumMember(Value = "after")]
        AfterAddress = 2,
    }
}