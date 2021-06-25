using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class QuotedSubscription : Resource
    {
        public enum BillingPeriodUnitEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "day")] Day,
            [EnumMember(Value = "week")] Week,
            [EnumMember(Value = "month")] Month,
            [EnumMember(Value = "year")] Year
        }

        public QuotedSubscription()
        {
        }

        public QuotedSubscription(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public QuotedSubscription(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public QuotedSubscription(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string PlanId => GetValue<string>("plan_id");

        public int PlanQuantity => GetValue<int>("plan_quantity");

        public int? PlanUnitPrice => GetValue<int?>("plan_unit_price", false);

        public int? SetupFee => GetValue<int?>("setup_fee", false);

        public int? BillingPeriod => GetValue<int?>("billing_period", false);

        public BillingPeriodUnitEnum? BillingPeriodUnit => GetEnum<BillingPeriodUnitEnum>("billing_period_unit", false);

        public DateTime? StartDate => GetDateTime("start_date", false);

        public DateTime? TrialEnd => GetDateTime("trial_end", false);

        public int? RemainingBillingCycles => GetValue<int?>("remaining_billing_cycles", false);

        public string PoNumber => GetValue<string>("po_number", false);

        public AutoCollectionEnum? AutoCollection => GetEnum<AutoCollectionEnum>("auto_collection", false);

        public List<QuotedSubscriptionAddon> Addons => GetResourceList<QuotedSubscriptionAddon>("addons");

        public List<QuotedSubscriptionEventBasedAddon> EventBasedAddons =>
            GetResourceList<QuotedSubscriptionEventBasedAddon>("event_based_addons");

        public List<QuotedSubscriptionCoupon> Coupons => GetResourceList<QuotedSubscriptionCoupon>("coupons");

        public List<QuotedSubscriptionSubscriptionItem> SubscriptionItems =>
            GetResourceList<QuotedSubscriptionSubscriptionItem>("subscription_items");

        public List<QuotedSubscriptionItemTier> ItemTiers => GetResourceList<QuotedSubscriptionItemTier>("item_tiers");

        #endregion

        #region Subclasses

        public class QuotedSubscriptionAddon : Resource
        {
            public string Id()
            {
                return GetValue<string>("id");
            }

            public int? Quantity()
            {
                return GetValue<int?>("quantity", false);
            }

            public int? UnitPrice()
            {
                return GetValue<int?>("unit_price", false);
            }

            public int? Amount()
            {
                return GetValue<int?>("amount", false);
            }

            public DateTime? TrialEnd()
            {
                return GetDateTime("trial_end", false);
            }

            public int? RemainingBillingCycles()
            {
                return GetValue<int?>("remaining_billing_cycles", false);
            }

            public string QuantityInDecimal()
            {
                return GetValue<string>("quantity_in_decimal", false);
            }

            public string UnitPriceInDecimal()
            {
                return GetValue<string>("unit_price_in_decimal", false);
            }

            public string AmountInDecimal()
            {
                return GetValue<string>("amount_in_decimal", false);
            }
        }

        public class QuotedSubscriptionEventBasedAddon : Resource
        {
            public enum OnEventEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */

                [EnumMember(Value = "subscription_creation")]
                SubscriptionCreation,

                [EnumMember(Value = "subscription_trial_start")]
                SubscriptionTrialStart,

                [EnumMember(Value = "plan_activation")]
                PlanActivation,

                [EnumMember(Value = "subscription_activation")]
                SubscriptionActivation,

                [EnumMember(Value = "contract_termination")]
                ContractTermination
            }

            public string Id()
            {
                return GetValue<string>("id");
            }

            public int Quantity()
            {
                return GetValue<int>("quantity");
            }

            public int UnitPrice()
            {
                return GetValue<int>("unit_price");
            }

            public int? ServicePeriodInDays()
            {
                return GetValue<int?>("service_period_in_days", false);
            }

            public OnEventEnum OnEvent()
            {
                return GetEnum<OnEventEnum>("on_event");
            }

            public bool ChargeOnce()
            {
                return GetValue<bool>("charge_once");
            }

            public string QuantityInDecimal()
            {
                return GetValue<string>("quantity_in_decimal", false);
            }

            public string UnitPriceInDecimal()
            {
                return GetValue<string>("unit_price_in_decimal", false);
            }
        }

        public class QuotedSubscriptionCoupon : Resource
        {
            public string CouponId()
            {
                return GetValue<string>("coupon_id");
            }

            public DateTime? ApplyTill()
            {
                return GetDateTime("apply_till", false);
            }

            public int AppliedCount()
            {
                return GetValue<int>("applied_count");
            }

            public string CouponCode()
            {
                return GetValue<string>("coupon_code", false);
            }
        }

        public class QuotedSubscriptionSubscriptionItem : Resource
        {
            public enum ChargeOnOptionEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "immediately")] Immediately,
                [EnumMember(Value = "on_event")] OnEvent
            }

            public enum ItemTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "plan")] Plan,
                [EnumMember(Value = "addon")] Addon,
                [EnumMember(Value = "charge")] Charge
            }

            public string ItemPriceId()
            {
                return GetValue<string>("item_price_id");
            }

            public ItemTypeEnum ItemType()
            {
                return GetEnum<ItemTypeEnum>("item_type");
            }

            public int? Quantity()
            {
                return GetValue<int?>("quantity", false);
            }

            public int? UnitPrice()
            {
                return GetValue<int?>("unit_price", false);
            }

            public int? Amount()
            {
                return GetValue<int?>("amount", false);
            }

            public int? FreeQuantity()
            {
                return GetValue<int?>("free_quantity", false);
            }

            public DateTime? TrialEnd()
            {
                return GetDateTime("trial_end", false);
            }

            public int? BillingCycles()
            {
                return GetValue<int?>("billing_cycles", false);
            }

            public int? ServicePeriodDays()
            {
                return GetValue<int?>("service_period_days", false);
            }

            public ChargeOnEventEnum? ChargeOnEvent()
            {
                return GetEnum<ChargeOnEventEnum>("charge_on_event", false);
            }

            public bool? ChargeOnce()
            {
                return GetValue<bool?>("charge_once", false);
            }

            public ChargeOnOptionEnum? ChargeOnOption()
            {
                return GetEnum<ChargeOnOptionEnum>("charge_on_option", false);
            }
        }

        public class QuotedSubscriptionItemTier : Resource
        {
            public string ItemPriceId()
            {
                return GetValue<string>("item_price_id");
            }

            public int StartingUnit()
            {
                return GetValue<int>("starting_unit");
            }

            public int? EndingUnit()
            {
                return GetValue<int?>("ending_unit", false);
            }

            public int Price()
            {
                return GetValue<int>("price");
            }

            [Obsolete]
            public string StartingUnitInDecimal()
            {
                return GetValue<string>("starting_unit_in_decimal", false);
            }

            [Obsolete]
            public string EndingUnitInDecimal()
            {
                return GetValue<string>("ending_unit_in_decimal", false);
            }

            [Obsolete]
            public string PriceInDecimal()
            {
                return GetValue<string>("price_in_decimal", false);
            }
        }

        #endregion
    }
}