using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class CreditNoteEstimate : Resource
    {
        public enum TypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "adjustment")] Adjustment,
            [EnumMember(Value = "refundable")] Refundable
        }

        public CreditNoteEstimate()
        {
        }

        public CreditNoteEstimate(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public CreditNoteEstimate(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public CreditNoteEstimate(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        #endregion

        #region Properties

        public string ReferenceInvoiceId => GetValue<string>("reference_invoice_id");

        public TypeEnum CreditNoteEstimateType => GetEnum<TypeEnum>("type");

        public PriceTypeEnum PriceType => GetEnum<PriceTypeEnum>("price_type");

        public string CurrencyCode => GetValue<string>("currency_code");

        public int SubTotal => GetValue<int>("sub_total");

        public int Total => GetValue<int>("total");

        public int AmountAllocated => GetValue<int>("amount_allocated");

        public int AmountAvailable => GetValue<int>("amount_available");

        public List<CreditNoteEstimateLineItem> LineItems => GetResourceList<CreditNoteEstimateLineItem>("line_items");

        public List<CreditNoteEstimateDiscount> Discounts => GetResourceList<CreditNoteEstimateDiscount>("discounts");

        public List<CreditNoteEstimateTax> Taxes => GetResourceList<CreditNoteEstimateTax>("taxes");

        public List<CreditNoteEstimateLineItemTax> LineItemTaxes =>
            GetResourceList<CreditNoteEstimateLineItemTax>("line_item_taxes");

        public List<CreditNoteEstimateLineItemDiscount> LineItemDiscounts =>
            GetResourceList<CreditNoteEstimateLineItemDiscount>("line_item_discounts");

        public List<CreditNoteEstimateLineItemTier> LineItemTiers =>
            GetResourceList<CreditNoteEstimateLineItemTier>("line_item_tiers");

        public int? RoundOffAmount => GetValue<int?>("round_off_amount", false);

        public string CustomerId => GetValue<string>("customer_id", false);

        #endregion

        #region Subclasses

        public class CreditNoteEstimateLineItem : Resource
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

        public class CreditNoteEstimateDiscount : Resource
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

        public class CreditNoteEstimateTax : Resource
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

        public class CreditNoteEstimateLineItemTax : Resource
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

        public class CreditNoteEstimateLineItemDiscount : Resource
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

        public class CreditNoteEstimateLineItemTier : Resource
        {
            public string LineItemId()
            {
                return GetValue<string>("line_item_id", false);
            }

            public int StartingUnit()
            {
                return GetValue<int>("starting_unit");
            }

            public int? EndingUnit()
            {
                return GetValue<int?>("ending_unit", false);
            }

            public int QuantityUsed()
            {
                return GetValue<int>("quantity_used");
            }

            public int UnitAmount()
            {
                return GetValue<int>("unit_amount");
            }

            public string StartingUnitInDecimal()
            {
                return GetValue<string>("starting_unit_in_decimal", false);
            }

            public string EndingUnitInDecimal()
            {
                return GetValue<string>("ending_unit_in_decimal", false);
            }

            public string QuantityUsedInDecimal()
            {
                return GetValue<string>("quantity_used_in_decimal", false);
            }

            public string UnitAmountInDecimal()
            {
                return GetValue<string>("unit_amount_in_decimal", false);
            }
        }

        #endregion
    }
}