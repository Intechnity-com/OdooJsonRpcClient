using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Tests.Models
{
    [OdooTableName("coupon.program")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class OdooVoucherEntity : IOdooModel
    {

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("program_type")]
        public string ProgramType { get; set; }

        [JsonProperty("promo_code")]
        public string PromoCode { get; set; }

        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("create_date")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("rule_date_from")]
        public string RuleDateFrom { get; set; }

        [JsonProperty("rule_date_to")]
        public DateTime RuleDateTo { get; set; }

        [JsonProperty("discount_max_amount")]
        public double DiscountMaxAmount { get; set; }

        [JsonProperty("discount_fixed_amount")]
        public double DiscountFixedAmount { get; set; }

        [JsonProperty("discount_line_product_id")]
        public int DiscountLineProductId { get; set; }

        [JsonProperty("reward_type")]
        public string RewardType { get; set; }

        [JsonProperty("discount_type")]
        public string DiscountType { get; set; }

    }
}