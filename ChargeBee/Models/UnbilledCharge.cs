using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class UnbilledCharge : Resource
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

        public UnbilledCharge()
        {
        }

        public UnbilledCharge(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public UnbilledCharge(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public UnbilledCharge(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Subclasses

        public class UnbilledChargeTier : Resource
        {
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

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("unbilled_charges");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static InvoiceUnbilledChargesRequest InvoiceUnbilledCharges()
        {
            var url = ApiUtil.BuildUrl("unbilled_charges", "invoice_unbilled_charges");
            return new InvoiceUnbilledChargesRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("unbilled_charges", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static UnbilledChargeListRequest List()
        {
            var url = ApiUtil.BuildUrl("unbilled_charges");
            return new UnbilledChargeListRequest(url);
        }

        public static InvoiceNowEstimateRequest InvoiceNowEstimate()
        {
            var url = ApiUtil.BuildUrl("unbilled_charges", "invoice_now_estimate");
            return new InvoiceNowEstimateRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id", false);

        public string CustomerId => GetValue<string>("customer_id", false);

        public string SubscriptionId => GetValue<string>("subscription_id", false);

        public DateTime? DateFrom => GetDateTime("date_from", false);

        public DateTime? DateTo => GetDateTime("date_to", false);

        public int? UnitAmount => GetValue<int?>("unit_amount", false);

        public PricingModelEnum? PricingModel => GetEnum<PricingModelEnum>("pricing_model", false);

        public int? Quantity => GetValue<int?>("quantity", false);

        public int? Amount => GetValue<int?>("amount", false);

        public string CurrencyCode => GetValue<string>("currency_code");

        public int? DiscountAmount => GetValue<int?>("discount_amount", false);

        public string Description => GetValue<string>("description", false);

        public EntityTypeEnum EntityType => GetEnum<EntityTypeEnum>("entity_type");

        public string EntityId => GetValue<string>("entity_id", false);

        public bool IsVoided => GetValue<bool>("is_voided");

        public DateTime? VoidedAt => GetDateTime("voided_at", false);

        public string UnitAmountInDecimal => GetValue<string>("unit_amount_in_decimal", false);

        public string QuantityInDecimal => GetValue<string>("quantity_in_decimal", false);

        public string AmountInDecimal => GetValue<string>("amount_in_decimal", false);

        public List<UnbilledChargeTier> Tiers => GetResourceList<UnbilledChargeTier>("tiers");

        public bool Deleted => GetValue<bool>("deleted");

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest SubscriptionId(string subscriptionId)
            {
                MParams.Add("subscription_id", subscriptionId);
                return this;
            }

            public CreateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateRequest ItemPriceItemPriceId(int index, string itemPriceItemPriceId)
            {
                MParams.AddOpt("item_prices[item_price_id][" + index + "]", itemPriceItemPriceId);
                return this;
            }

            public CreateRequest ItemPriceQuantity(int index, int itemPriceQuantity)
            {
                MParams.AddOpt("item_prices[quantity][" + index + "]", itemPriceQuantity);
                return this;
            }

            public CreateRequest ItemPriceUnitPrice(int index, int itemPriceUnitPrice)
            {
                MParams.AddOpt("item_prices[unit_price][" + index + "]", itemPriceUnitPrice);
                return this;
            }

            public CreateRequest ItemPriceDateFrom(int index, long itemPriceDateFrom)
            {
                MParams.AddOpt("item_prices[date_from][" + index + "]", itemPriceDateFrom);
                return this;
            }

            public CreateRequest ItemPriceDateTo(int index, long itemPriceDateTo)
            {
                MParams.AddOpt("item_prices[date_to][" + index + "]", itemPriceDateTo);
                return this;
            }

            public CreateRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public CreateRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CreateRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CreateRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }

            public CreateRequest ChargeAmount(int index, int chargeAmount)
            {
                MParams.AddOpt("charges[amount][" + index + "]", chargeAmount);
                return this;
            }

            public CreateRequest ChargeAmountInDecimal(int index, string chargeAmountInDecimal)
            {
                MParams.AddOpt("charges[amount_in_decimal][" + index + "]", chargeAmountInDecimal);
                return this;
            }

            public CreateRequest ChargeDescription(int index, string chargeDescription)
            {
                MParams.AddOpt("charges[description][" + index + "]", chargeDescription);
                return this;
            }

            public CreateRequest ChargeAvalaraSaleType(int index, AvalaraSaleTypeEnum chargeAvalaraSaleType)
            {
                MParams.AddOpt("charges[avalara_sale_type][" + index + "]", chargeAvalaraSaleType);
                return this;
            }

            public CreateRequest ChargeAvalaraTransactionType(int index, int chargeAvalaraTransactionType)
            {
                MParams.AddOpt("charges[avalara_transaction_type][" + index + "]", chargeAvalaraTransactionType);
                return this;
            }

            public CreateRequest ChargeAvalaraServiceType(int index, int chargeAvalaraServiceType)
            {
                MParams.AddOpt("charges[avalara_service_type][" + index + "]", chargeAvalaraServiceType);
                return this;
            }

            public CreateRequest ChargeDateFrom(int index, long chargeDateFrom)
            {
                MParams.AddOpt("charges[date_from][" + index + "]", chargeDateFrom);
                return this;
            }

            public CreateRequest ChargeDateTo(int index, long chargeDateTo)
            {
                MParams.AddOpt("charges[date_to][" + index + "]", chargeDateTo);
                return this;
            }

            public CreateRequest ChargeTaxable(int index, bool chargeTaxable)
            {
                MParams.AddOpt("charges[taxable][" + index + "]", chargeTaxable);
                return this;
            }
        }

        public class InvoiceUnbilledChargesRequest : EntityRequest<InvoiceUnbilledChargesRequest>
        {
            public InvoiceUnbilledChargesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public InvoiceUnbilledChargesRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public InvoiceUnbilledChargesRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer_id", customerId);
                return this;
            }
        }

        public class UnbilledChargeListRequest : ListRequestBase<UnbilledChargeListRequest>
        {
            public UnbilledChargeListRequest(string url)
                : base(url)
            {
            }

            public UnbilledChargeListRequest IncludeDeleted(bool includeDeleted)
            {
                MParams.AddOpt("include_deleted", includeDeleted);
                return this;
            }

            public UnbilledChargeListRequest IsVoided(bool isVoided)
            {
                MParams.AddOpt("is_voided", isVoided);
                return this;
            }

            public StringFilter<UnbilledChargeListRequest> SubscriptionId()
            {
                return new StringFilter<UnbilledChargeListRequest>("subscription_id", this).SupportsMultiOperators(true)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<UnbilledChargeListRequest> CustomerId()
            {
                return new StringFilter<UnbilledChargeListRequest>("customer_id", this).SupportsMultiOperators(true)
                    .SupportsPresenceOperator(true);
            }
        }

        public class InvoiceNowEstimateRequest : EntityRequest<InvoiceNowEstimateRequest>
        {
            public InvoiceNowEstimateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public InvoiceNowEstimateRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public InvoiceNowEstimateRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer_id", customerId);
                return this;
            }
        }

        #endregion
    }
}