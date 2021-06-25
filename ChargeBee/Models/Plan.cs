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
    public class Plan : Resource
    {
        public enum AddonApplicabilityEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "all")] All,
            [EnumMember(Value = "restricted")] Restricted
        }

        [Obsolete]
        public enum ChargeModelEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "flat_fee")] FlatFee,
            [EnumMember(Value = "per_unit")] PerUnit,
            [EnumMember(Value = "tiered")] Tiered,
            [EnumMember(Value = "volume")] Volume,
            [EnumMember(Value = "stairstep")] Stairstep
        }

        public enum PeriodUnitEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "day")] Day,
            [EnumMember(Value = "week")] Week,
            [EnumMember(Value = "month")] Month,
            [EnumMember(Value = "year")] Year
        }

        public enum ShippingFrequencyPeriodUnitEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "year")] Year,
            [EnumMember(Value = "month")] Month,
            [EnumMember(Value = "week")] Week,
            [EnumMember(Value = "day")] Day
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "active")] Active,
            [EnumMember(Value = "archived")] Archived,
            [EnumMember(Value = "deleted")] Deleted
        }

        public enum TrialPeriodUnitEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "day")] Day,
            [EnumMember(Value = "month")] Month
        }

        public Plan()
        {
        }

        public Plan(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Plan(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Plan(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("plans");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("plans", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static PlanListRequest List()
        {
            var url = ApiUtil.BuildUrl("plans");
            return new PlanListRequest(url);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("plans", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("plans", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static CopyRequest Copy()
        {
            var url = ApiUtil.BuildUrl("plans", "copy");
            return new CopyRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Unarchive(string id)
        {
            var url = ApiUtil.BuildUrl("plans", CheckNull(id), "unarchive");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string Name => GetValue<string>("name");

        public string InvoiceName => GetValue<string>("invoice_name", false);

        public string Description => GetValue<string>("description", false);

        public int? Price => GetValue<int?>("price", false);

        public string CurrencyCode => GetValue<string>("currency_code");

        public int Period => GetValue<int>("period");

        public PeriodUnitEnum PeriodUnit => GetEnum<PeriodUnitEnum>("period_unit");

        public int? TrialPeriod => GetValue<int?>("trial_period", false);

        public TrialPeriodUnitEnum? TrialPeriodUnit => GetEnum<TrialPeriodUnitEnum>("trial_period_unit", false);

        public PricingModelEnum PricingModel => GetEnum<PricingModelEnum>("pricing_model");

        [Obsolete] public ChargeModelEnum ChargeModel => GetEnum<ChargeModelEnum>("charge_model");

        public int FreeQuantity => GetValue<int>("free_quantity");

        public int? SetupCost => GetValue<int?>("setup_cost", false);

        [Obsolete] public double? DowngradePenalty => GetValue<double?>("downgrade_penalty", false);

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public DateTime? ArchivedAt => GetDateTime("archived_at", false);

        public int? BillingCycles => GetValue<int?>("billing_cycles", false);

        public string RedirectUrl => GetValue<string>("redirect_url", false);

        public bool EnabledInHostedPages => GetValue<bool>("enabled_in_hosted_pages");

        public bool EnabledInPortal => GetValue<bool>("enabled_in_portal");

        public AddonApplicabilityEnum AddonApplicability => GetEnum<AddonApplicabilityEnum>("addon_applicability");

        public string TaxCode => GetValue<string>("tax_code", false);

        public string TaxjarProductCode => GetValue<string>("taxjar_product_code", false);

        public AvalaraSaleTypeEnum? AvalaraSaleType => GetEnum<AvalaraSaleTypeEnum>("avalara_sale_type", false);

        public int? AvalaraTransactionType => GetValue<int?>("avalara_transaction_type", false);

        public int? AvalaraServiceType => GetValue<int?>("avalara_service_type", false);

        public string Sku => GetValue<string>("sku", false);

        public string AccountingCode => GetValue<string>("accounting_code", false);

        public string AccountingCategory1 => GetValue<string>("accounting_category1", false);

        public string AccountingCategory2 => GetValue<string>("accounting_category2", false);

        public bool? IsShippable => GetValue<bool?>("is_shippable", false);

        public int? ShippingFrequencyPeriod => GetValue<int?>("shipping_frequency_period", false);

        public ShippingFrequencyPeriodUnitEnum? ShippingFrequencyPeriodUnit =>
            GetEnum<ShippingFrequencyPeriodUnitEnum>("shipping_frequency_period_unit", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public bool Giftable => GetValue<bool>("giftable");

        public string ClaimUrl => GetValue<string>("claim_url", false);

        public string FreeQuantityInDecimal => GetValue<string>("free_quantity_in_decimal", false);

        public string PriceInDecimal => GetValue<string>("price_in_decimal", false);

        public string InvoiceNotes => GetValue<string>("invoice_notes", false);

        public bool? Taxable => GetValue<bool?>("taxable", false);

        public string TaxProfileId => GetValue<string>("tax_profile_id", false);

        public JToken MetaData => GetJToken("meta_data", false);

        public List<PlanTier> Tiers => GetResourceList<PlanTier>("tiers");

        public List<PlanApplicableAddon> ApplicableAddons => GetResourceList<PlanApplicableAddon>("applicable_addons");

        public List<PlanAttachedAddon> AttachedAddons => GetResourceList<PlanAttachedAddon>("attached_addons");

        public List<PlanEventBasedAddon> EventBasedAddons => GetResourceList<PlanEventBasedAddon>("event_based_addons");

        public bool? ShowDescriptionInInvoices => GetValue<bool?>("show_description_in_invoices", false);

        public bool? ShowDescriptionInQuotes => GetValue<bool?>("show_description_in_quotes", false);

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest Id(string id)
            {
                MParams.Add("id", id);
                return this;
            }

            public CreateRequest Name(string name)
            {
                MParams.Add("name", name);
                return this;
            }

            public CreateRequest InvoiceName(string invoiceName)
            {
                MParams.AddOpt("invoice_name", invoiceName);
                return this;
            }

            public CreateRequest Description(string description)
            {
                MParams.AddOpt("description", description);
                return this;
            }

            public CreateRequest TrialPeriod(int trialPeriod)
            {
                MParams.AddOpt("trial_period", trialPeriod);
                return this;
            }

            public CreateRequest TrialPeriodUnit(TrialPeriodUnitEnum trialPeriodUnit)
            {
                MParams.AddOpt("trial_period_unit", trialPeriodUnit);
                return this;
            }

            public CreateRequest Period(int period)
            {
                MParams.AddOpt("period", period);
                return this;
            }

            public CreateRequest PeriodUnit(PeriodUnitEnum periodUnit)
            {
                MParams.AddOpt("period_unit", periodUnit);
                return this;
            }

            public CreateRequest SetupCost(int setupCost)
            {
                MParams.AddOpt("setup_cost", setupCost);
                return this;
            }

            public CreateRequest Price(int price)
            {
                MParams.AddOpt("price", price);
                return this;
            }

            public CreateRequest PriceInDecimal(string priceInDecimal)
            {
                MParams.AddOpt("price_in_decimal", priceInDecimal);
                return this;
            }

            public CreateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateRequest PricingModel(PricingModelEnum pricingModel)
            {
                MParams.AddOpt("pricing_model", pricingModel);
                return this;
            }

            [Obsolete]
            public CreateRequest ChargeModel(ChargeModelEnum chargeModel)
            {
                MParams.AddOpt("charge_model", chargeModel);
                return this;
            }

            public CreateRequest FreeQuantity(int freeQuantity)
            {
                MParams.AddOpt("free_quantity", freeQuantity);
                return this;
            }

            public CreateRequest FreeQuantityInDecimal(string freeQuantityInDecimal)
            {
                MParams.AddOpt("free_quantity_in_decimal", freeQuantityInDecimal);
                return this;
            }

            public CreateRequest AddonApplicability(AddonApplicabilityEnum addonApplicability)
            {
                MParams.AddOpt("addon_applicability", addonApplicability);
                return this;
            }

            [Obsolete]
            public CreateRequest DowngradePenalty(double downgradePenalty)
            {
                MParams.AddOpt("downgrade_penalty", downgradePenalty);
                return this;
            }

            public CreateRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public CreateRequest EnabledInHostedPages(bool enabledInHostedPages)
            {
                MParams.AddOpt("enabled_in_hosted_pages", enabledInHostedPages);
                return this;
            }

            public CreateRequest EnabledInPortal(bool enabledInPortal)
            {
                MParams.AddOpt("enabled_in_portal", enabledInPortal);
                return this;
            }

            public CreateRequest Taxable(bool taxable)
            {
                MParams.AddOpt("taxable", taxable);
                return this;
            }

            public CreateRequest TaxProfileId(string taxProfileId)
            {
                MParams.AddOpt("tax_profile_id", taxProfileId);
                return this;
            }

            public CreateRequest TaxCode(string taxCode)
            {
                MParams.AddOpt("tax_code", taxCode);
                return this;
            }

            public CreateRequest TaxjarProductCode(string taxjarProductCode)
            {
                MParams.AddOpt("taxjar_product_code", taxjarProductCode);
                return this;
            }

            public CreateRequest AvalaraSaleType(AvalaraSaleTypeEnum avalaraSaleType)
            {
                MParams.AddOpt("avalara_sale_type", avalaraSaleType);
                return this;
            }

            public CreateRequest AvalaraTransactionType(int avalaraTransactionType)
            {
                MParams.AddOpt("avalara_transaction_type", avalaraTransactionType);
                return this;
            }

            public CreateRequest AvalaraServiceType(int avalaraServiceType)
            {
                MParams.AddOpt("avalara_service_type", avalaraServiceType);
                return this;
            }

            public CreateRequest Sku(string sku)
            {
                MParams.AddOpt("sku", sku);
                return this;
            }

            public CreateRequest AccountingCode(string accountingCode)
            {
                MParams.AddOpt("accounting_code", accountingCode);
                return this;
            }

            public CreateRequest AccountingCategory1(string accountingCategory1)
            {
                MParams.AddOpt("accounting_category1", accountingCategory1);
                return this;
            }

            public CreateRequest AccountingCategory2(string accountingCategory2)
            {
                MParams.AddOpt("accounting_category2", accountingCategory2);
                return this;
            }

            public CreateRequest IsShippable(bool isShippable)
            {
                MParams.AddOpt("is_shippable", isShippable);
                return this;
            }

            public CreateRequest ShippingFrequencyPeriod(int shippingFrequencyPeriod)
            {
                MParams.AddOpt("shipping_frequency_period", shippingFrequencyPeriod);
                return this;
            }

            public CreateRequest ShippingFrequencyPeriodUnit(
                ShippingFrequencyPeriodUnitEnum shippingFrequencyPeriodUnit)
            {
                MParams.AddOpt("shipping_frequency_period_unit", shippingFrequencyPeriodUnit);
                return this;
            }

            public CreateRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public CreateRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public CreateRequest ShowDescriptionInInvoices(bool showDescriptionInInvoices)
            {
                MParams.AddOpt("show_description_in_invoices", showDescriptionInInvoices);
                return this;
            }

            public CreateRequest ShowDescriptionInQuotes(bool showDescriptionInQuotes)
            {
                MParams.AddOpt("show_description_in_quotes", showDescriptionInQuotes);
                return this;
            }

            public CreateRequest Giftable(bool giftable)
            {
                MParams.AddOpt("giftable", giftable);
                return this;
            }

            public CreateRequest Status(StatusEnum status)
            {
                MParams.AddOpt("status", status);
                return this;
            }

            public CreateRequest ClaimUrl(string claimUrl)
            {
                MParams.AddOpt("claim_url", claimUrl);
                return this;
            }

            public CreateRequest TierStartingUnit(int index, int tierStartingUnit)
            {
                MParams.AddOpt("tiers[starting_unit][" + index + "]", tierStartingUnit);
                return this;
            }

            public CreateRequest TierEndingUnit(int index, int tierEndingUnit)
            {
                MParams.AddOpt("tiers[ending_unit][" + index + "]", tierEndingUnit);
                return this;
            }

            public CreateRequest TierPrice(int index, int tierPrice)
            {
                MParams.AddOpt("tiers[price][" + index + "]", tierPrice);
                return this;
            }

            public CreateRequest TierStartingUnitInDecimal(int index, string tierStartingUnitInDecimal)
            {
                MParams.AddOpt("tiers[starting_unit_in_decimal][" + index + "]", tierStartingUnitInDecimal);
                return this;
            }

            public CreateRequest TierEndingUnitInDecimal(int index, string tierEndingUnitInDecimal)
            {
                MParams.AddOpt("tiers[ending_unit_in_decimal][" + index + "]", tierEndingUnitInDecimal);
                return this;
            }

            public CreateRequest TierPriceInDecimal(int index, string tierPriceInDecimal)
            {
                MParams.AddOpt("tiers[price_in_decimal][" + index + "]", tierPriceInDecimal);
                return this;
            }

            public CreateRequest ApplicableAddonId(int index, string applicableAddonId)
            {
                MParams.AddOpt("applicable_addons[id][" + index + "]", applicableAddonId);
                return this;
            }

            public CreateRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public CreateRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public CreateRequest EventBasedAddonQuantityInDecimal(int index, string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public CreateRequest EventBasedAddonOnEvent(int index, OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public CreateRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public CreateRequest AttachedAddonId(int index, string attachedAddonId)
            {
                MParams.AddOpt("attached_addons[id][" + index + "]", attachedAddonId);
                return this;
            }

            public CreateRequest AttachedAddonQuantity(int index, int attachedAddonQuantity)
            {
                MParams.AddOpt("attached_addons[quantity][" + index + "]", attachedAddonQuantity);
                return this;
            }

            public CreateRequest AttachedAddonQuantityInDecimal(int index, string attachedAddonQuantityInDecimal)
            {
                MParams.AddOpt("attached_addons[quantity_in_decimal][" + index + "]", attachedAddonQuantityInDecimal);
                return this;
            }

            public CreateRequest AttachedAddonBillingCycles(int index, int attachedAddonBillingCycles)
            {
                MParams.AddOpt("attached_addons[billing_cycles][" + index + "]", attachedAddonBillingCycles);
                return this;
            }

            public CreateRequest AttachedAddonType(int index, PlanAttachedAddon.TypeEnum attachedAddonType)
            {
                MParams.AddOpt("attached_addons[type][" + index + "]", attachedAddonType);
                return this;
            }
        }

        public class UpdateRequest : EntityRequest<UpdateRequest>
        {
            public UpdateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateRequest Name(string name)
            {
                MParams.AddOpt("name", name);
                return this;
            }

            public UpdateRequest InvoiceName(string invoiceName)
            {
                MParams.AddOpt("invoice_name", invoiceName);
                return this;
            }

            public UpdateRequest Description(string description)
            {
                MParams.AddOpt("description", description);
                return this;
            }

            public UpdateRequest TrialPeriod(int trialPeriod)
            {
                MParams.AddOpt("trial_period", trialPeriod);
                return this;
            }

            public UpdateRequest TrialPeriodUnit(TrialPeriodUnitEnum trialPeriodUnit)
            {
                MParams.AddOpt("trial_period_unit", trialPeriodUnit);
                return this;
            }

            public UpdateRequest Period(int period)
            {
                MParams.AddOpt("period", period);
                return this;
            }

            public UpdateRequest PeriodUnit(PeriodUnitEnum periodUnit)
            {
                MParams.AddOpt("period_unit", periodUnit);
                return this;
            }

            public UpdateRequest SetupCost(int setupCost)
            {
                MParams.AddOpt("setup_cost", setupCost);
                return this;
            }

            public UpdateRequest Price(int price)
            {
                MParams.AddOpt("price", price);
                return this;
            }

            public UpdateRequest PriceInDecimal(string priceInDecimal)
            {
                MParams.AddOpt("price_in_decimal", priceInDecimal);
                return this;
            }

            public UpdateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public UpdateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public UpdateRequest PricingModel(PricingModelEnum pricingModel)
            {
                MParams.AddOpt("pricing_model", pricingModel);
                return this;
            }

            [Obsolete]
            public UpdateRequest ChargeModel(ChargeModelEnum chargeModel)
            {
                MParams.AddOpt("charge_model", chargeModel);
                return this;
            }

            public UpdateRequest FreeQuantity(int freeQuantity)
            {
                MParams.AddOpt("free_quantity", freeQuantity);
                return this;
            }

            public UpdateRequest FreeQuantityInDecimal(string freeQuantityInDecimal)
            {
                MParams.AddOpt("free_quantity_in_decimal", freeQuantityInDecimal);
                return this;
            }

            public UpdateRequest AddonApplicability(AddonApplicabilityEnum addonApplicability)
            {
                MParams.AddOpt("addon_applicability", addonApplicability);
                return this;
            }

            [Obsolete]
            public UpdateRequest DowngradePenalty(double downgradePenalty)
            {
                MParams.AddOpt("downgrade_penalty", downgradePenalty);
                return this;
            }

            public UpdateRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public UpdateRequest EnabledInHostedPages(bool enabledInHostedPages)
            {
                MParams.AddOpt("enabled_in_hosted_pages", enabledInHostedPages);
                return this;
            }

            public UpdateRequest EnabledInPortal(bool enabledInPortal)
            {
                MParams.AddOpt("enabled_in_portal", enabledInPortal);
                return this;
            }

            public UpdateRequest Taxable(bool taxable)
            {
                MParams.AddOpt("taxable", taxable);
                return this;
            }

            public UpdateRequest TaxProfileId(string taxProfileId)
            {
                MParams.AddOpt("tax_profile_id", taxProfileId);
                return this;
            }

            public UpdateRequest TaxCode(string taxCode)
            {
                MParams.AddOpt("tax_code", taxCode);
                return this;
            }

            public UpdateRequest TaxjarProductCode(string taxjarProductCode)
            {
                MParams.AddOpt("taxjar_product_code", taxjarProductCode);
                return this;
            }

            public UpdateRequest AvalaraSaleType(AvalaraSaleTypeEnum avalaraSaleType)
            {
                MParams.AddOpt("avalara_sale_type", avalaraSaleType);
                return this;
            }

            public UpdateRequest AvalaraTransactionType(int avalaraTransactionType)
            {
                MParams.AddOpt("avalara_transaction_type", avalaraTransactionType);
                return this;
            }

            public UpdateRequest AvalaraServiceType(int avalaraServiceType)
            {
                MParams.AddOpt("avalara_service_type", avalaraServiceType);
                return this;
            }

            public UpdateRequest Sku(string sku)
            {
                MParams.AddOpt("sku", sku);
                return this;
            }

            public UpdateRequest AccountingCode(string accountingCode)
            {
                MParams.AddOpt("accounting_code", accountingCode);
                return this;
            }

            public UpdateRequest AccountingCategory1(string accountingCategory1)
            {
                MParams.AddOpt("accounting_category1", accountingCategory1);
                return this;
            }

            public UpdateRequest AccountingCategory2(string accountingCategory2)
            {
                MParams.AddOpt("accounting_category2", accountingCategory2);
                return this;
            }

            public UpdateRequest IsShippable(bool isShippable)
            {
                MParams.AddOpt("is_shippable", isShippable);
                return this;
            }

            public UpdateRequest ShippingFrequencyPeriod(int shippingFrequencyPeriod)
            {
                MParams.AddOpt("shipping_frequency_period", shippingFrequencyPeriod);
                return this;
            }

            public UpdateRequest ShippingFrequencyPeriodUnit(
                ShippingFrequencyPeriodUnitEnum shippingFrequencyPeriodUnit)
            {
                MParams.AddOpt("shipping_frequency_period_unit", shippingFrequencyPeriodUnit);
                return this;
            }

            public UpdateRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public UpdateRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public UpdateRequest ShowDescriptionInInvoices(bool showDescriptionInInvoices)
            {
                MParams.AddOpt("show_description_in_invoices", showDescriptionInInvoices);
                return this;
            }

            public UpdateRequest ShowDescriptionInQuotes(bool showDescriptionInQuotes)
            {
                MParams.AddOpt("show_description_in_quotes", showDescriptionInQuotes);
                return this;
            }

            public UpdateRequest TierStartingUnit(int index, int tierStartingUnit)
            {
                MParams.AddOpt("tiers[starting_unit][" + index + "]", tierStartingUnit);
                return this;
            }

            public UpdateRequest TierEndingUnit(int index, int tierEndingUnit)
            {
                MParams.AddOpt("tiers[ending_unit][" + index + "]", tierEndingUnit);
                return this;
            }

            public UpdateRequest TierPrice(int index, int tierPrice)
            {
                MParams.AddOpt("tiers[price][" + index + "]", tierPrice);
                return this;
            }

            public UpdateRequest TierStartingUnitInDecimal(int index, string tierStartingUnitInDecimal)
            {
                MParams.AddOpt("tiers[starting_unit_in_decimal][" + index + "]", tierStartingUnitInDecimal);
                return this;
            }

            public UpdateRequest TierEndingUnitInDecimal(int index, string tierEndingUnitInDecimal)
            {
                MParams.AddOpt("tiers[ending_unit_in_decimal][" + index + "]", tierEndingUnitInDecimal);
                return this;
            }

            public UpdateRequest TierPriceInDecimal(int index, string tierPriceInDecimal)
            {
                MParams.AddOpt("tiers[price_in_decimal][" + index + "]", tierPriceInDecimal);
                return this;
            }

            public UpdateRequest ApplicableAddonId(int index, string applicableAddonId)
            {
                MParams.AddOpt("applicable_addons[id][" + index + "]", applicableAddonId);
                return this;
            }

            public UpdateRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public UpdateRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public UpdateRequest EventBasedAddonQuantityInDecimal(int index, string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public UpdateRequest EventBasedAddonOnEvent(int index, OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public UpdateRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public UpdateRequest AttachedAddonId(int index, string attachedAddonId)
            {
                MParams.AddOpt("attached_addons[id][" + index + "]", attachedAddonId);
                return this;
            }

            public UpdateRequest AttachedAddonQuantity(int index, int attachedAddonQuantity)
            {
                MParams.AddOpt("attached_addons[quantity][" + index + "]", attachedAddonQuantity);
                return this;
            }

            public UpdateRequest AttachedAddonQuantityInDecimal(int index, string attachedAddonQuantityInDecimal)
            {
                MParams.AddOpt("attached_addons[quantity_in_decimal][" + index + "]", attachedAddonQuantityInDecimal);
                return this;
            }

            public UpdateRequest AttachedAddonBillingCycles(int index, int attachedAddonBillingCycles)
            {
                MParams.AddOpt("attached_addons[billing_cycles][" + index + "]", attachedAddonBillingCycles);
                return this;
            }

            public UpdateRequest AttachedAddonType(int index, PlanAttachedAddon.TypeEnum attachedAddonType)
            {
                MParams.AddOpt("attached_addons[type][" + index + "]", attachedAddonType);
                return this;
            }
        }

        public class PlanListRequest : ListRequestBase<PlanListRequest>
        {
            public PlanListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<PlanListRequest> Id()
            {
                return new StringFilter<PlanListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<PlanListRequest> Name()
            {
                return new StringFilter<PlanListRequest>("name", this).SupportsMultiOperators(true);
            }

            public NumberFilter<int, PlanListRequest> Price()
            {
                return new("price", this);
            }

            public NumberFilter<int, PlanListRequest> Period()
            {
                return new("period", this);
            }

            public EnumFilter<PeriodUnitEnum, PlanListRequest> PeriodUnit()
            {
                return new("period_unit", this);
            }

            public NumberFilter<int, PlanListRequest> TrialPeriod()
            {
                return new NumberFilter<int, PlanListRequest>("trial_period", this).SupportsPresenceOperator(true);
            }

            public EnumFilter<TrialPeriodUnitEnum, PlanListRequest> TrialPeriodUnit()
            {
                return new("trial_period_unit", this);
            }

            public EnumFilter<AddonApplicabilityEnum, PlanListRequest> AddonApplicability()
            {
                return new("addon_applicability", this);
            }

            public BooleanFilter<PlanListRequest> Giftable()
            {
                return new("giftable", this);
            }

            [Obsolete]
            public EnumFilter<ChargeModelEnum, PlanListRequest> ChargeModel()
            {
                return new("charge_model", this);
            }

            public EnumFilter<PricingModelEnum, PlanListRequest> PricingModel()
            {
                return new("pricing_model", this);
            }

            public EnumFilter<StatusEnum, PlanListRequest> Status()
            {
                return new("status", this);
            }

            public TimestampFilter<PlanListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public StringFilter<PlanListRequest> CurrencyCode()
            {
                return new StringFilter<PlanListRequest>("currency_code", this).SupportsMultiOperators(true);
            }
        }

        public class CopyRequest : EntityRequest<CopyRequest>
        {
            public CopyRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CopyRequest FromSite(string fromSite)
            {
                MParams.Add("from_site", fromSite);
                return this;
            }

            public CopyRequest IdAtFromSite(string idAtFromSite)
            {
                MParams.Add("id_at_from_site", idAtFromSite);
                return this;
            }

            public CopyRequest Id(string id)
            {
                MParams.AddOpt("id", id);
                return this;
            }

            public CopyRequest ForSiteMerging(bool forSiteMerging)
            {
                MParams.AddOpt("for_site_merging", forSiteMerging);
                return this;
            }
        }

        #endregion

        #region Subclasses

        public class PlanTier : Resource
        {
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

            public string StartingUnitInDecimal()
            {
                return GetValue<string>("starting_unit_in_decimal", false);
            }

            public string EndingUnitInDecimal()
            {
                return GetValue<string>("ending_unit_in_decimal", false);
            }

            public string PriceInDecimal()
            {
                return GetValue<string>("price_in_decimal", false);
            }
        }

        public class PlanApplicableAddon : Resource
        {
            public string Id()
            {
                return GetValue<string>("id");
            }
        }

        public class PlanAttachedAddon : Resource
        {
            public enum TypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "recommended")] Recommended,
                [EnumMember(Value = "mandatory")] Mandatory
            }

            public string Id()
            {
                return GetValue<string>("id");
            }

            public int Quantity()
            {
                return GetValue<int>("quantity");
            }

            public int? BillingCycles()
            {
                return GetValue<int?>("billing_cycles", false);
            }

            public TypeEnum AttachedAddonType()
            {
                return GetEnum<TypeEnum>("type");
            }

            public string QuantityInDecimal()
            {
                return GetValue<string>("quantity_in_decimal", false);
            }
        }

        public class PlanEventBasedAddon : Resource
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
        }

        #endregion
    }
}