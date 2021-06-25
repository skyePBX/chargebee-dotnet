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
    public class ItemPrice : Resource
    {
        public enum PeriodUnitEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "day")] Day,
            [EnumMember(Value = "week")] Week,
            [EnumMember(Value = "month")] Month,
            [EnumMember(Value = "year")] Year
        }

        public enum ShippingPeriodUnitEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "day")] Day,
            [EnumMember(Value = "week")] Week,
            [EnumMember(Value = "month")] Month,
            [EnumMember(Value = "year")] Year
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

        public ItemPrice()
        {
        }

        public ItemPrice(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public ItemPrice(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public ItemPrice(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("item_prices");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("item_prices", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("item_prices", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static ItemPriceListRequest List()
        {
            var url = ApiUtil.BuildUrl("item_prices");
            return new ItemPriceListRequest(url);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("item_prices", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string Name => GetValue<string>("name");

        public string ItemFamilyId => GetValue<string>("item_family_id", false);

        public string ItemId => GetValue<string>("item_id", false);

        public string Description => GetValue<string>("description", false);

        public StatusEnum? Status => GetEnum<StatusEnum>("status", false);

        public string ExternalName => GetValue<string>("external_name", false);

        public PricingModelEnum PricingModel => GetEnum<PricingModelEnum>("pricing_model");

        public int? Price => GetValue<int?>("price", false);

        public int? Period => GetValue<int?>("period", false);

        public string CurrencyCode => GetValue<string>("currency_code");

        public PeriodUnitEnum? PeriodUnit => GetEnum<PeriodUnitEnum>("period_unit", false);

        public int? TrialPeriod => GetValue<int?>("trial_period", false);

        public TrialPeriodUnitEnum? TrialPeriodUnit => GetEnum<TrialPeriodUnitEnum>("trial_period_unit", false);

        public int? ShippingPeriod => GetValue<int?>("shipping_period", false);

        public ShippingPeriodUnitEnum? ShippingPeriodUnit =>
            GetEnum<ShippingPeriodUnitEnum>("shipping_period_unit", false);

        public int? BillingCycles => GetValue<int?>("billing_cycles", false);

        public int FreeQuantity => GetValue<int>("free_quantity");

        [Obsolete] public string FreeQuantityInDecimal => GetValue<string>("free_quantity_in_decimal", false);

        [Obsolete] public string PriceInDecimal => GetValue<string>("price_in_decimal", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public string InvoiceNotes => GetValue<string>("invoice_notes", false);

        public List<ItemPriceTier> Tiers => GetResourceList<ItemPriceTier>("tiers");

        public bool? IsTaxable => GetValue<bool?>("is_taxable", false);

        public ItemPriceTaxDetail TaxDetail => GetSubResource<ItemPriceTaxDetail>("tax_detail");

        public ItemPriceAccountingDetail AccountingDetail =>
            GetSubResource<ItemPriceAccountingDetail>("accounting_detail");

        public JToken Metadata => GetJToken("metadata", false);

        public ItemTypeEnum? ItemType => GetEnum<ItemTypeEnum>("item_type", false);

        [Obsolete] public bool? Archivable => GetValue<bool?>("archivable", false);

        [Obsolete] public string ParentItemId => GetValue<string>("parent_item_id", false);

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

            public CreateRequest Description(string description)
            {
                MParams.AddOpt("description", description);
                return this;
            }

            public CreateRequest ItemId(string itemId)
            {
                MParams.Add("item_id", itemId);
                return this;
            }

            public CreateRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public CreateRequest ExternalName(string externalName)
            {
                MParams.AddOpt("external_name", externalName);
                return this;
            }

            public CreateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateRequest IsTaxable(bool isTaxable)
            {
                MParams.AddOpt("is_taxable", isTaxable);
                return this;
            }

            public CreateRequest FreeQuantity(int freeQuantity)
            {
                MParams.AddOpt("free_quantity", freeQuantity);
                return this;
            }

            public CreateRequest Metadata(JToken metadata)
            {
                MParams.AddOpt("metadata", metadata);
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

            public CreateRequest PricingModel(PricingModelEnum pricingModel)
            {
                MParams.AddOpt("pricing_model", pricingModel);
                return this;
            }

            public CreateRequest Price(int price)
            {
                MParams.AddOpt("price", price);
                return this;
            }

            public CreateRequest PeriodUnit(PeriodUnitEnum periodUnit)
            {
                MParams.AddOpt("period_unit", periodUnit);
                return this;
            }

            public CreateRequest Period(int period)
            {
                MParams.AddOpt("period", period);
                return this;
            }

            public CreateRequest TrialPeriodUnit(TrialPeriodUnitEnum trialPeriodUnit)
            {
                MParams.AddOpt("trial_period_unit", trialPeriodUnit);
                return this;
            }

            public CreateRequest TrialPeriod(int trialPeriod)
            {
                MParams.AddOpt("trial_period", trialPeriod);
                return this;
            }

            public CreateRequest ShippingPeriod(int shippingPeriod)
            {
                MParams.AddOpt("shipping_period", shippingPeriod);
                return this;
            }

            public CreateRequest ShippingPeriodUnit(ShippingPeriodUnitEnum shippingPeriodUnit)
            {
                MParams.AddOpt("shipping_period_unit", shippingPeriodUnit);
                return this;
            }

            public CreateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateRequest TaxDetailTaxProfileId(string taxDetailTaxProfileId)
            {
                MParams.AddOpt("tax_detail[tax_profile_id]", taxDetailTaxProfileId);
                return this;
            }

            public CreateRequest TaxDetailAvalaraTaxCode(string taxDetailAvalaraTaxCode)
            {
                MParams.AddOpt("tax_detail[avalara_tax_code]", taxDetailAvalaraTaxCode);
                return this;
            }

            public CreateRequest TaxDetailAvalaraSaleType(AvalaraSaleTypeEnum taxDetailAvalaraSaleType)
            {
                MParams.AddOpt("tax_detail[avalara_sale_type]", taxDetailAvalaraSaleType);
                return this;
            }

            public CreateRequest TaxDetailAvalaraTransactionType(int taxDetailAvalaraTransactionType)
            {
                MParams.AddOpt("tax_detail[avalara_transaction_type]", taxDetailAvalaraTransactionType);
                return this;
            }

            public CreateRequest TaxDetailAvalaraServiceType(int taxDetailAvalaraServiceType)
            {
                MParams.AddOpt("tax_detail[avalara_service_type]", taxDetailAvalaraServiceType);
                return this;
            }

            public CreateRequest TaxDetailTaxjarProductCode(string taxDetailTaxjarProductCode)
            {
                MParams.AddOpt("tax_detail[taxjar_product_code]", taxDetailTaxjarProductCode);
                return this;
            }

            public CreateRequest AccountingDetailSku(string accountingDetailSku)
            {
                MParams.AddOpt("accounting_detail[sku]", accountingDetailSku);
                return this;
            }

            public CreateRequest AccountingDetailAccountingCode(string accountingDetailAccountingCode)
            {
                MParams.AddOpt("accounting_detail[accounting_code]", accountingDetailAccountingCode);
                return this;
            }

            public CreateRequest AccountingDetailAccountingCategory1(string accountingDetailAccountingCategory1)
            {
                MParams.AddOpt("accounting_detail[accounting_category1]", accountingDetailAccountingCategory1);
                return this;
            }

            public CreateRequest AccountingDetailAccountingCategory2(string accountingDetailAccountingCategory2)
            {
                MParams.AddOpt("accounting_detail[accounting_category2]", accountingDetailAccountingCategory2);
                return this;
            }

            public CreateRequest AccountingDetailAccountingCategory3(string accountingDetailAccountingCategory3)
            {
                MParams.AddOpt("accounting_detail[accounting_category3]", accountingDetailAccountingCategory3);
                return this;
            }

            public CreateRequest AccountingDetailAccountingCategory4(string accountingDetailAccountingCategory4)
            {
                MParams.AddOpt("accounting_detail[accounting_category4]", accountingDetailAccountingCategory4);
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

            public UpdateRequest Description(string description)
            {
                MParams.AddOpt("description", description);
                return this;
            }

            public UpdateRequest Status(StatusEnum status)
            {
                MParams.AddOpt("status", status);
                return this;
            }

            public UpdateRequest ExternalName(string externalName)
            {
                MParams.AddOpt("external_name", externalName);
                return this;
            }

            public UpdateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public UpdateRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public UpdateRequest IsTaxable(bool isTaxable)
            {
                MParams.AddOpt("is_taxable", isTaxable);
                return this;
            }

            public UpdateRequest FreeQuantity(int freeQuantity)
            {
                MParams.AddOpt("free_quantity", freeQuantity);
                return this;
            }

            public UpdateRequest Metadata(JToken metadata)
            {
                MParams.AddOpt("metadata", metadata);
                return this;
            }

            public UpdateRequest PricingModel(PricingModelEnum pricingModel)
            {
                MParams.AddOpt("pricing_model", pricingModel);
                return this;
            }

            public UpdateRequest Price(int price)
            {
                MParams.AddOpt("price", price);
                return this;
            }

            public UpdateRequest PeriodUnit(PeriodUnitEnum periodUnit)
            {
                MParams.AddOpt("period_unit", periodUnit);
                return this;
            }

            public UpdateRequest Period(int period)
            {
                MParams.AddOpt("period", period);
                return this;
            }

            public UpdateRequest TrialPeriodUnit(TrialPeriodUnitEnum trialPeriodUnit)
            {
                MParams.AddOpt("trial_period_unit", trialPeriodUnit);
                return this;
            }

            public UpdateRequest TrialPeriod(int trialPeriod)
            {
                MParams.AddOpt("trial_period", trialPeriod);
                return this;
            }

            public UpdateRequest ShippingPeriod(int shippingPeriod)
            {
                MParams.AddOpt("shipping_period", shippingPeriod);
                return this;
            }

            public UpdateRequest ShippingPeriodUnit(ShippingPeriodUnitEnum shippingPeriodUnit)
            {
                MParams.AddOpt("shipping_period_unit", shippingPeriodUnit);
                return this;
            }

            public UpdateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
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

            public UpdateRequest TaxDetailTaxProfileId(string taxDetailTaxProfileId)
            {
                MParams.AddOpt("tax_detail[tax_profile_id]", taxDetailTaxProfileId);
                return this;
            }

            public UpdateRequest TaxDetailAvalaraTaxCode(string taxDetailAvalaraTaxCode)
            {
                MParams.AddOpt("tax_detail[avalara_tax_code]", taxDetailAvalaraTaxCode);
                return this;
            }

            public UpdateRequest TaxDetailAvalaraSaleType(AvalaraSaleTypeEnum taxDetailAvalaraSaleType)
            {
                MParams.AddOpt("tax_detail[avalara_sale_type]", taxDetailAvalaraSaleType);
                return this;
            }

            public UpdateRequest TaxDetailAvalaraTransactionType(int taxDetailAvalaraTransactionType)
            {
                MParams.AddOpt("tax_detail[avalara_transaction_type]", taxDetailAvalaraTransactionType);
                return this;
            }

            public UpdateRequest TaxDetailAvalaraServiceType(int taxDetailAvalaraServiceType)
            {
                MParams.AddOpt("tax_detail[avalara_service_type]", taxDetailAvalaraServiceType);
                return this;
            }

            public UpdateRequest TaxDetailTaxjarProductCode(string taxDetailTaxjarProductCode)
            {
                MParams.AddOpt("tax_detail[taxjar_product_code]", taxDetailTaxjarProductCode);
                return this;
            }

            public UpdateRequest AccountingDetailSku(string accountingDetailSku)
            {
                MParams.AddOpt("accounting_detail[sku]", accountingDetailSku);
                return this;
            }

            public UpdateRequest AccountingDetailAccountingCode(string accountingDetailAccountingCode)
            {
                MParams.AddOpt("accounting_detail[accounting_code]", accountingDetailAccountingCode);
                return this;
            }

            public UpdateRequest AccountingDetailAccountingCategory1(string accountingDetailAccountingCategory1)
            {
                MParams.AddOpt("accounting_detail[accounting_category1]", accountingDetailAccountingCategory1);
                return this;
            }

            public UpdateRequest AccountingDetailAccountingCategory2(string accountingDetailAccountingCategory2)
            {
                MParams.AddOpt("accounting_detail[accounting_category2]", accountingDetailAccountingCategory2);
                return this;
            }

            public UpdateRequest AccountingDetailAccountingCategory3(string accountingDetailAccountingCategory3)
            {
                MParams.AddOpt("accounting_detail[accounting_category3]", accountingDetailAccountingCategory3);
                return this;
            }

            public UpdateRequest AccountingDetailAccountingCategory4(string accountingDetailAccountingCategory4)
            {
                MParams.AddOpt("accounting_detail[accounting_category4]", accountingDetailAccountingCategory4);
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
        }

        public class ItemPriceListRequest : ListRequestBase<ItemPriceListRequest>
        {
            public ItemPriceListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<ItemPriceListRequest> Id()
            {
                return new StringFilter<ItemPriceListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<ItemPriceListRequest> Name()
            {
                return new StringFilter<ItemPriceListRequest>("name", this).SupportsMultiOperators(true);
            }

            public EnumFilter<PricingModelEnum, ItemPriceListRequest> PricingModel()
            {
                return new("pricing_model", this);
            }

            public StringFilter<ItemPriceListRequest> ItemId()
            {
                return new StringFilter<ItemPriceListRequest>("item_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<ItemPriceListRequest> ItemFamilyId()
            {
                return new StringFilter<ItemPriceListRequest>("item_family_id", this).SupportsMultiOperators(true);
            }

            public EnumFilter<ItemTypeEnum, ItemPriceListRequest> ItemType()
            {
                return new("item_type", this);
            }

            public StringFilter<ItemPriceListRequest> CurrencyCode()
            {
                return new StringFilter<ItemPriceListRequest>("currency_code", this).SupportsMultiOperators(true);
            }

            public NumberFilter<int, ItemPriceListRequest> TrialPeriod()
            {
                return new("trial_period", this);
            }

            public EnumFilter<TrialPeriodUnitEnum, ItemPriceListRequest> TrialPeriodUnit()
            {
                return new("trial_period_unit", this);
            }

            public EnumFilter<StatusEnum, ItemPriceListRequest> Status()
            {
                return new("status", this);
            }

            public TimestampFilter<ItemPriceListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public EnumFilter<PeriodUnitEnum, ItemPriceListRequest> PeriodUnit()
            {
                return new("period_unit", this);
            }

            public NumberFilter<int, ItemPriceListRequest> Period()
            {
                return new("period", this);
            }
        }

        #endregion

        #region Subclasses

        public class ItemPriceTier : Resource
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

        public class ItemPriceTaxDetail : Resource
        {
            public string TaxProfileId()
            {
                return GetValue<string>("tax_profile_id", false);
            }

            public AvalaraSaleTypeEnum? AvalaraSaleType()
            {
                return GetEnum<AvalaraSaleTypeEnum>("avalara_sale_type", false);
            }

            public int? AvalaraTransactionType()
            {
                return GetValue<int?>("avalara_transaction_type", false);
            }

            public int? AvalaraServiceType()
            {
                return GetValue<int?>("avalara_service_type", false);
            }

            public string AvalaraTaxCode()
            {
                return GetValue<string>("avalara_tax_code", false);
            }

            public string TaxjarProductCode()
            {
                return GetValue<string>("taxjar_product_code", false);
            }
        }

        public class ItemPriceAccountingDetail : Resource
        {
            public string Sku()
            {
                return GetValue<string>("sku", false);
            }

            public string AccountingCode()
            {
                return GetValue<string>("accounting_code", false);
            }

            public string AccountingCategory1()
            {
                return GetValue<string>("accounting_category1", false);
            }

            public string AccountingCategory2()
            {
                return GetValue<string>("accounting_category2", false);
            }
        }

        #endregion
    }
}