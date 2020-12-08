using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Utils;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("coupon.program")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class OdooCouponProgram : IOdooModel
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        // coupon.rule
        [JsonProperty("rule_id")]
        public long RuleId { get; set; }

        // coupon.reward
        [JsonProperty("reward_id")]
        public long RewardId { get; set; }

        [JsonProperty("sequence")]
        public int? Sequence { get; set; }

        [JsonProperty("maximum_use_number")]
        public int? MaximumUseNumber { get; set; }

        [JsonProperty("program_type")]
        public string ProgramType { get; set; }

        [JsonProperty("promo_code_usage")]
        public string PromoCodeUsage { get; set; }

        [JsonProperty("promo_code")]
        public string PromoCode { get; set; }

        [JsonProperty("promo_applicability")]
        public string PromoApplicability { get; set; }

        // coupon.coupon
        [JsonProperty("coupon_ids")]
        public long[] CouponIds { get; set; }

        [JsonProperty("coupon_count")]
        public int? CouponCount { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }

        // res.currency
        [JsonProperty("currency_id")]
        public long? CurrencyId { get; set; }

        [JsonProperty("validity_duration")]
        public int? ValidityDuration { get; set; }

        [JsonProperty("order_count")]
        public int? OrderCount { get; set; }

        // website
        [JsonProperty("website_id")]
        public long? WebsiteId { get; set; }

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

        [JsonProperty("rule_date_from")]
        public DateTime? RuleDateFrom { get; set; }

        [JsonProperty("rule_date_to")]
        public DateTime? RuleDateTo { get; set; }

        [JsonProperty("rule_partners_domain")]
        public string RulePartnersDomain { get; set; }

        [JsonProperty("rule_products_domain")]
        public string RuleProductsDomain { get; set; }

        [JsonProperty("rule_min_quantity")]
        public int? RuleMinQuantity { get; set; }

        [JsonProperty("rule_minimum_amount")]
        public double? RuleMinimumAmount { get; set; }

        [JsonProperty("rule_minimum_amount_tax_inclusion")]
        public string RuleMinimumAmountTaxInclusion { get; set; }

        [JsonProperty("reward_description")]
        public string RewardDescription { get; set; }

        // product.product
        [JsonProperty("reward_product_id")]
        public long? RewardProductId { get; set; }

        [JsonProperty("reward_product_quantity")]
        public int? RewardProductQuantity { get; set; }

        [JsonProperty("discount_type")]
        public string DiscountType { get; set; }

        [JsonProperty("discount_percentage")]
        public double? DiscountPercentage { get; set; }

        [JsonProperty("discount_apply_on")]
        public string DiscountApplyOn { get; set; }

        // product.product
        [JsonProperty("discount_specific_product_ids")]
        public long[] DiscountSpecificProductIds { get; set; }

        [JsonProperty("discount_max_amount")]
        public double? DiscountMaxAmount { get; set; }

        [JsonProperty("discount_fixed_amount")]
        public double? DiscountFixedAmount { get; set; }

        // uom.uom
        [JsonProperty("reward_product_uom_id")]
        public long? RewardProductUomId { get; set; }

        // product.product
        [JsonProperty("discount_line_product_id")]
        public long? DiscountLineProductId { get; set; }

        [JsonProperty("reward_type")]
        public string RewardType { get; set; }
    }
}
