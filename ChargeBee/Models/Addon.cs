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
    public class Addon : Resource
    {
        public enum ChargeTypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "recurring")] Recurring,
            [EnumMember(Value = "non_recurring")] NonRecurring
        }

        public enum PeriodUnitEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "day")] Day,
            [EnumMember(Value = "week")] Week,
            [EnumMember(Value = "month")] Month,
            [EnumMember(Value = "year")] Year,
            [EnumMember(Value = "not_applicable")] NotApplicable
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

        [Obsolete]
        public enum TypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "on_off")] OnOff,
            [EnumMember(Value = "quantity")] Quantity,
            [EnumMember(Value = "tiered")] Tiered,
            [EnumMember(Value = "volume")] Volume,
            [EnumMember(Value = "stairstep")] Stairstep
        }

        public Addon()
        {
        }

        public Addon(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Addon(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Addon(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Subclasses

        public class AddonTier : Resource
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

        #endregion

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("addons");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("addons", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static AddonListRequest List()
        {
            var url = ApiUtil.BuildUrl("addons");
            return new AddonListRequest(url);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("addons", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("addons", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static CopyRequest Copy()
        {
            var url = ApiUtil.BuildUrl("addons", "copy");
            return new CopyRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Unarchive(string id)
        {
            var url = ApiUtil.BuildUrl("addons", CheckNull(id), "unarchive");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string Name => GetValue<string>("name");

        public string InvoiceName => GetValue<string>("invoice_name", false);

        public string Description => GetValue<string>("description", false);

        public PricingModelEnum PricingModel => GetEnum<PricingModelEnum>("pricing_model");

        [Obsolete] public TypeEnum AddonType => GetEnum<TypeEnum>("type");

        public ChargeTypeEnum ChargeType => GetEnum<ChargeTypeEnum>("charge_type");

        public int? Price => GetValue<int?>("price", false);

        public string CurrencyCode => GetValue<string>("currency_code");

        public int? Period => GetValue<int?>("period", false);

        public PeriodUnitEnum PeriodUnit => GetEnum<PeriodUnitEnum>("period_unit");

        public string Unit => GetValue<string>("unit", false);

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public DateTime? ArchivedAt => GetDateTime("archived_at", false);

        public bool EnabledInPortal => GetValue<bool>("enabled_in_portal");

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

        public string PriceInDecimal => GetValue<string>("price_in_decimal", false);

        public bool? IncludedInMrr => GetValue<bool?>("included_in_mrr", false);

        public string InvoiceNotes => GetValue<string>("invoice_notes", false);

        public bool? Taxable => GetValue<bool?>("taxable", false);

        public string TaxProfileId => GetValue<string>("tax_profile_id", false);

        public JToken MetaData => GetJToken("meta_data", false);

        public List<AddonTier> Tiers => GetResourceList<AddonTier>("tiers");

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

            public CreateRequest ChargeType(ChargeTypeEnum chargeType)
            {
                MParams.Add("charge_type", chargeType);
                return this;
            }

            public CreateRequest Price(int price)
            {
                MParams.AddOpt("price", price);
                return this;
            }

            public CreateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
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

            public CreateRequest PricingModel(PricingModelEnum pricingModel)
            {
                MParams.AddOpt("pricing_model", pricingModel);
                return this;
            }

            [Obsolete]
            public CreateRequest Type(TypeEnum type)
            {
                MParams.AddOpt("type", type);
                return this;
            }

            public CreateRequest Unit(string unit)
            {
                MParams.AddOpt("unit", unit);
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

            public CreateRequest IncludedInMrr(bool includedInMrr)
            {
                MParams.AddOpt("included_in_mrr", includedInMrr);
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

            public CreateRequest PriceInDecimal(string priceInDecimal)
            {
                MParams.AddOpt("price_in_decimal", priceInDecimal);
                return this;
            }

            public CreateRequest Status(StatusEnum status)
            {
                MParams.AddOpt("status", status);
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

            public UpdateRequest ChargeType(ChargeTypeEnum chargeType)
            {
                MParams.AddOpt("charge_type", chargeType);
                return this;
            }

            public UpdateRequest Price(int price)
            {
                MParams.AddOpt("price", price);
                return this;
            }

            public UpdateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
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

            public UpdateRequest PricingModel(PricingModelEnum pricingModel)
            {
                MParams.AddOpt("pricing_model", pricingModel);
                return this;
            }

            [Obsolete]
            public UpdateRequest Type(TypeEnum type)
            {
                MParams.AddOpt("type", type);
                return this;
            }

            public UpdateRequest Unit(string unit)
            {
                MParams.AddOpt("unit", unit);
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

            public UpdateRequest IncludedInMrr(bool includedInMrr)
            {
                MParams.AddOpt("included_in_mrr", includedInMrr);
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

            public UpdateRequest PriceInDecimal(string priceInDecimal)
            {
                MParams.AddOpt("price_in_decimal", priceInDecimal);
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
        }

        public class AddonListRequest : ListRequestBase<AddonListRequest>
        {
            public AddonListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<AddonListRequest> Id()
            {
                return new StringFilter<AddonListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<AddonListRequest> Name()
            {
                return new StringFilter<AddonListRequest>("name", this).SupportsMultiOperators(true);
            }

            public EnumFilter<PricingModelEnum, AddonListRequest> PricingModel()
            {
                return new("pricing_model", this);
            }

            [Obsolete]
            public EnumFilter<TypeEnum, AddonListRequest> Type()
            {
                return new("type", this);
            }

            public EnumFilter<ChargeTypeEnum, AddonListRequest> ChargeType()
            {
                return new("charge_type", this);
            }

            public NumberFilter<int, AddonListRequest> Price()
            {
                return new("price", this);
            }

            public NumberFilter<int, AddonListRequest> Period()
            {
                return new("period", this);
            }

            public EnumFilter<PeriodUnitEnum, AddonListRequest> PeriodUnit()
            {
                return new("period_unit", this);
            }

            public EnumFilter<StatusEnum, AddonListRequest> Status()
            {
                return new("status", this);
            }

            public TimestampFilter<AddonListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public StringFilter<AddonListRequest> CurrencyCode()
            {
                return new StringFilter<AddonListRequest>("currency_code", this).SupportsMultiOperators(true);
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
    }
}