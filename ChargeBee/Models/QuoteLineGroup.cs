using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class QuoteLineGroup : Resource
    {
        public enum ChargeEventEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "immediate")] Immediate,

            [EnumMember(Value = "subscription_creation")]
            SubscriptionCreation,
            [EnumMember(Value = "trial_start")] TrialStart,

            [EnumMember(Value = "subscription_change")]
            SubscriptionChange,

            [EnumMember(Value = "subscription_renewal")]
            SubscriptionRenewal
        }

        public QuoteLineGroup()
        {
        }

        public QuoteLineGroup(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public QuoteLineGroup(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public QuoteLineGroup(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        #endregion

        #region Properties

        public int? Version => GetValue<int?>("version", false);

        public string Id => GetValue<string>("id", false);

        public int SubTotal => GetValue<int>("sub_total");

        public int? Total => GetValue<int?>("total", false);

        public int? CreditsApplied => GetValue<int?>("credits_applied", false);

        public int? AmountPaid => GetValue<int?>("amount_paid", false);

        public int? AmountDue => GetValue<int?>("amount_due", false);

        public ChargeEventEnum? ChargeEvent => GetEnum<ChargeEventEnum>("charge_event", false);

        public int? BillingCycleNumber => GetValue<int?>("billing_cycle_number", false);

        public List<QuoteLineGroupLineItem> LineItems => GetResourceList<QuoteLineGroupLineItem>("line_items");

        public List<QuoteLineGroupDiscount> Discounts => GetResourceList<QuoteLineGroupDiscount>("discounts");

        public List<QuoteLineGroupLineItemDiscount> LineItemDiscounts =>
            GetResourceList<QuoteLineGroupLineItemDiscount>("line_item_discounts");

        public List<QuoteLineGroupTax> Taxes => GetResourceList<QuoteLineGroupTax>("taxes");

        public List<QuoteLineGroupLineItemTax> LineItemTaxes =>
            GetResourceList<QuoteLineGroupLineItemTax>("line_item_taxes");

        #endregion

        #region Subclasses

        public class QuoteLineGroupLineItem : Resource
        {
            public enum EntityTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "plan_setup")] PlanSetup,
                [EnumMember(Value = "plan")] Plan,
                [EnumMember(Value = "addon")] Addon,
                [EnumMember(Value = "adhoc")] Adhoc,

                [EnumMember(Value = "plan_item_price")]
                PlanItemPrice,

                [EnumMember(Value = "addon_item_price")]
                AddonItemPrice,

                [EnumMember(Value = "charge_item_price")]
                ChargeItemPrice
            }

            public string Id()
            {
                return GetValue<string>("id", false);
            }

            public string SubscriptionId()
            {
                return GetValue<string>("subscription_id", false);
            }

            public DateTime DateFrom()
            {
                return (DateTime) GetDateTime("date_from");
            }

            public DateTime DateTo()
            {
                return (DateTime) GetDateTime("date_to");
            }

            public int UnitAmount()
            {
                return GetValue<int>("unit_amount");
            }

            public int? Quantity()
            {
                return GetValue<int?>("quantity", false);
            }

            public int? Amount()
            {
                return GetValue<int?>("amount", false);
            }

            public PricingModelEnum? PricingModel()
            {
                return GetEnum<PricingModelEnum>("pricing_model", false);
            }

            public bool IsTaxed()
            {
                return GetValue<bool>("is_taxed");
            }

            public int? TaxAmount()
            {
                return GetValue<int?>("tax_amount", false);
            }

            public double? TaxRate()
            {
                return GetValue<double?>("tax_rate", false);
            }

            public string UnitAmountInDecimal()
            {
                return GetValue<string>("unit_amount_in_decimal", false);
            }

            public string QuantityInDecimal()
            {
                return GetValue<string>("quantity_in_decimal", false);
            }

            public string AmountInDecimal()
            {
                return GetValue<string>("amount_in_decimal", false);
            }

            public int? DiscountAmount()
            {
                return GetValue<int?>("discount_amount", false);
            }

            public int? ItemLevelDiscountAmount()
            {
                return GetValue<int?>("item_level_discount_amount", false);
            }

            public string Description()
            {
                return GetValue<string>("description");
            }

            public string EntityDescription()
            {
                return GetValue<string>("entity_description");
            }

            public EntityTypeEnum EntityType()
            {
                return GetEnum<EntityTypeEnum>("entity_type");
            }

            public TaxExemptReasonEnum? TaxExemptReason()
            {
                return GetEnum<TaxExemptReasonEnum>("tax_exempt_reason", false);
            }

            public string EntityId()
            {
                return GetValue<string>("entity_id", false);
            }

            public string CustomerId()
            {
                return GetValue<string>("customer_id", false);
            }
        }

        public class QuoteLineGroupDiscount : Resource
        {
            public enum EntityTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */

                [EnumMember(Value = "item_level_coupon")]
                ItemLevelCoupon,

                [EnumMember(Value = "document_level_coupon")]
                DocumentLevelCoupon,

                [EnumMember(Value = "promotional_credits")]
                PromotionalCredits,

                [EnumMember(Value = "prorated_credits")]
                ProratedCredits
            }

            public int Amount()
            {
                return GetValue<int>("amount");
            }

            public string Description()
            {
                return GetValue<string>("description", false);
            }

            public EntityTypeEnum EntityType()
            {
                return GetEnum<EntityTypeEnum>("entity_type");
            }

            public string EntityId()
            {
                return GetValue<string>("entity_id", false);
            }
        }

        public class QuoteLineGroupLineItemDiscount : Resource
        {
            public enum DiscountTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */

                [EnumMember(Value = "item_level_coupon")]
                ItemLevelCoupon,

                [EnumMember(Value = "document_level_coupon")]
                DocumentLevelCoupon,

                [EnumMember(Value = "promotional_credits")]
                PromotionalCredits,

                [EnumMember(Value = "prorated_credits")]
                ProratedCredits
            }

            public string LineItemId()
            {
                return GetValue<string>("line_item_id");
            }

            public DiscountTypeEnum DiscountType()
            {
                return GetEnum<DiscountTypeEnum>("discount_type");
            }

            public string CouponId()
            {
                return GetValue<string>("coupon_id", false);
            }

            public int DiscountAmount()
            {
                return GetValue<int>("discount_amount");
            }
        }

        public class QuoteLineGroupTax : Resource
        {
            public string Name()
            {
                return GetValue<string>("name");
            }

            public int Amount()
            {
                return GetValue<int>("amount");
            }

            public string Description()
            {
                return GetValue<string>("description", false);
            }
        }

        public class QuoteLineGroupLineItemTax : Resource
        {
            public string LineItemId()
            {
                return GetValue<string>("line_item_id", false);
            }

            public string TaxName()
            {
                return GetValue<string>("tax_name");
            }

            public double TaxRate()
            {
                return GetValue<double>("tax_rate");
            }

            public bool? IsPartialTaxApplied()
            {
                return GetValue<bool?>("is_partial_tax_applied", false);
            }

            public bool? IsNonComplianceTax()
            {
                return GetValue<bool?>("is_non_compliance_tax", false);
            }

            public int TaxableAmount()
            {
                return GetValue<int>("taxable_amount");
            }

            public int TaxAmount()
            {
                return GetValue<int>("tax_amount");
            }

            public TaxJurisTypeEnum? TaxJurisType()
            {
                return GetEnum<TaxJurisTypeEnum>("tax_juris_type", false);
            }

            public string TaxJurisName()
            {
                return GetValue<string>("tax_juris_name", false);
            }

            public string TaxJurisCode()
            {
                return GetValue<string>("tax_juris_code", false);
            }

            public int? TaxAmountInLocalCurrency()
            {
                return GetValue<int?>("tax_amount_in_local_currency", false);
            }

            public string LocalCurrencyCode()
            {
                return GetValue<string>("local_currency_code", false);
            }
        }

        #endregion
    }
}