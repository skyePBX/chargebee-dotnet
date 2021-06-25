using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Filters.enums;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Quote : Resource
    {
        public enum OperationTypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */

            [EnumMember(Value = "create_subscription_for_customer")]
            CreateSubscriptionForCustomer,

            [EnumMember(Value = "change_subscription")]
            ChangeSubscription,

            [EnumMember(Value = "onetime_invoice")]
            OnetimeInvoice
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "open")] Open,
            [EnumMember(Value = "accepted")] Accepted,
            [EnumMember(Value = "declined")] Declined,
            [EnumMember(Value = "invoiced")] Invoiced,
            [EnumMember(Value = "closed")] Closed
        }

        public Quote()
        {
        }

        public Quote(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Quote(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Quote(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("quotes", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static CreateSubForCustomerQuoteRequest CreateSubForCustomerQuote(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "create_subscription_quote");
            return new CreateSubForCustomerQuoteRequest(url, HttpMethod.Post);
        }

        public static EditCreateSubForCustomerQuoteRequest EditCreateSubForCustomerQuote(string id)
        {
            var url = ApiUtil.BuildUrl("quotes", CheckNull(id), "edit_create_subscription_quote");
            return new EditCreateSubForCustomerQuoteRequest(url, HttpMethod.Post);
        }

        public static CreateSubItemsForCustomerQuoteRequest CreateSubItemsForCustomerQuote(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "create_subscription_quote_for_items");
            return new CreateSubItemsForCustomerQuoteRequest(url, HttpMethod.Post);
        }

        public static UpdateSubscriptionQuoteRequest UpdateSubscriptionQuote()
        {
            var url = ApiUtil.BuildUrl("quotes", "update_subscription_quote");
            return new UpdateSubscriptionQuoteRequest(url, HttpMethod.Post);
        }

        public static EditUpdateSubscriptionQuoteRequest EditUpdateSubscriptionQuote(string id)
        {
            var url = ApiUtil.BuildUrl("quotes", CheckNull(id), "edit_update_subscription_quote");
            return new EditUpdateSubscriptionQuoteRequest(url, HttpMethod.Post);
        }

        public static UpdateSubscriptionQuoteForItemsRequest UpdateSubscriptionQuoteForItems()
        {
            var url = ApiUtil.BuildUrl("quotes", "update_subscription_quote_for_items");
            return new UpdateSubscriptionQuoteForItemsRequest(url, HttpMethod.Post);
        }

        public static CreateForOnetimeChargesRequest CreateForOnetimeCharges()
        {
            var url = ApiUtil.BuildUrl("quotes", "create_for_onetime_charges");
            return new CreateForOnetimeChargesRequest(url, HttpMethod.Post);
        }

        public static CreateForChargeItemsAndChargesRequest CreateForChargeItemsAndCharges()
        {
            var url = ApiUtil.BuildUrl("quotes", "create_for_charge_items_and_charges");
            return new CreateForChargeItemsAndChargesRequest(url, HttpMethod.Post);
        }

        public static EditOneTimeQuoteRequest EditOneTimeQuote(string id)
        {
            var url = ApiUtil.BuildUrl("quotes", CheckNull(id), "edit_one_time_quote");
            return new EditOneTimeQuoteRequest(url, HttpMethod.Post);
        }

        public static QuoteListRequest List()
        {
            var url = ApiUtil.BuildUrl("quotes");
            return new QuoteListRequest(url);
        }

        public static ListRequest QuoteLineGroupsForQuote(string id)
        {
            var url = ApiUtil.BuildUrl("quotes", CheckNull(id), "quote_line_groups");
            return new ListRequest(url);
        }

        public static ConvertRequest Convert(string id)
        {
            var url = ApiUtil.BuildUrl("quotes", CheckNull(id), "convert");
            return new ConvertRequest(url, HttpMethod.Post);
        }

        public static UpdateStatusRequest UpdateStatus(string id)
        {
            var url = ApiUtil.BuildUrl("quotes", CheckNull(id), "update_status");
            return new UpdateStatusRequest(url, HttpMethod.Post);
        }

        public static ExtendExpiryDateRequest ExtendExpiryDate(string id)
        {
            var url = ApiUtil.BuildUrl("quotes", CheckNull(id), "extend_expiry_date");
            return new ExtendExpiryDateRequest(url, HttpMethod.Post);
        }

        public static DeleteRequest Delete(string id)
        {
            var url = ApiUtil.BuildUrl("quotes", CheckNull(id), "delete");
            return new DeleteRequest(url, HttpMethod.Post);
        }

        public static PdfRequest Pdf(string id)
        {
            var url = ApiUtil.BuildUrl("quotes", CheckNull(id), "pdf");
            return new PdfRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string Name => GetValue<string>("name", false);

        public string PoNumber => GetValue<string>("po_number", false);

        public string CustomerId => GetValue<string>("customer_id");

        public string SubscriptionId => GetValue<string>("subscription_id", false);

        public string InvoiceId => GetValue<string>("invoice_id", false);

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public OperationTypeEnum OperationType => GetEnum<OperationTypeEnum>("operation_type");

        public string VatNumber => GetValue<string>("vat_number", false);

        public PriceTypeEnum PriceType => GetEnum<PriceTypeEnum>("price_type");

        public DateTime ValidTill => (DateTime) GetDateTime("valid_till");

        public DateTime Date => (DateTime) GetDateTime("date");

        public long? TotalPayable => GetValue<long?>("total_payable", false);

        public int? ChargeOnAcceptance => GetValue<int?>("charge_on_acceptance", false);

        public int SubTotal => GetValue<int>("sub_total");

        public int? Total => GetValue<int?>("total", false);

        public int? CreditsApplied => GetValue<int?>("credits_applied", false);

        public int? AmountPaid => GetValue<int?>("amount_paid", false);

        public int? AmountDue => GetValue<int?>("amount_due", false);

        public int? Version => GetValue<int?>("version", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public List<QuoteLineItem> LineItems => GetResourceList<QuoteLineItem>("line_items");

        public List<QuoteDiscount> Discounts => GetResourceList<QuoteDiscount>("discounts");

        public List<QuoteLineItemDiscount> LineItemDiscounts =>
            GetResourceList<QuoteLineItemDiscount>("line_item_discounts");

        public List<QuoteTax> Taxes => GetResourceList<QuoteTax>("taxes");

        public List<QuoteLineItemTax> LineItemTaxes => GetResourceList<QuoteLineItemTax>("line_item_taxes");

        public string CurrencyCode => GetValue<string>("currency_code");

        public JArray Notes => GetJArray("notes", false);

        public QuoteShippingAddress ShippingAddress => GetSubResource<QuoteShippingAddress>("shipping_address");

        public QuoteBillingAddress BillingAddress => GetSubResource<QuoteBillingAddress>("billing_address");

        public DateTime? ContractTermStart => GetDateTime("contract_term_start", false);

        public DateTime? ContractTermEnd => GetDateTime("contract_term_end", false);

        public int? ContractTermTerminationFee => GetValue<int?>("contract_term_termination_fee", false);

        #endregion

        #region Requests

        public class CreateSubForCustomerQuoteRequest : EntityRequest<CreateSubForCustomerQuoteRequest>
        {
            public CreateSubForCustomerQuoteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateSubForCustomerQuoteRequest Name(string name)
            {
                MParams.AddOpt("name", name);
                return this;
            }

            public CreateSubForCustomerQuoteRequest Notes(string notes)
            {
                MParams.AddOpt("notes", notes);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ExpiresAt(long expiresAt)
            {
                MParams.AddOpt("expires_at", expiresAt);
                return this;
            }

            public CreateSubForCustomerQuoteRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateSubForCustomerQuoteRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public CreateSubForCustomerQuoteRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CreateSubForCustomerQuoteRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CreateSubForCustomerQuoteRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription[id]", subscriptionId);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionPlanUnitPriceInDecimal(
                string subscriptionPlanUnitPriceInDecimal)
            {
                MParams.AddOpt("subscription[plan_unit_price_in_decimal]", subscriptionPlanUnitPriceInDecimal);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionPlanQuantityInDecimal(
                string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.Add("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionPlanUnitPrice(int subscriptionPlanUnitPrice)
            {
                MParams.AddOpt("subscription[plan_unit_price]", subscriptionPlanUnitPrice);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CreateSubForCustomerQuoteRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CreateSubForCustomerQuoteRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public CreateSubForCustomerQuoteRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CreateSubForCustomerQuoteRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CreateSubForCustomerQuoteRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CreateSubForCustomerQuoteRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CreateSubForCustomerQuoteRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CreateSubForCustomerQuoteRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public CreateSubForCustomerQuoteRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public CreateSubForCustomerQuoteRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public CreateSubForCustomerQuoteRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public CreateSubForCustomerQuoteRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public CreateSubForCustomerQuoteRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public CreateSubForCustomerQuoteRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public CreateSubForCustomerQuoteRequest EventBasedAddonOnEvent(int index,
                OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public CreateSubForCustomerQuoteRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public CreateSubForCustomerQuoteRequest EventBasedAddonChargeOn(int index,
                ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }

            public CreateSubForCustomerQuoteRequest AddonTrialEnd(int index, long addonTrialEnd)
            {
                MParams.AddOpt("addons[trial_end][" + index + "]", addonTrialEnd);
                return this;
            }
        }

        public class EditCreateSubForCustomerQuoteRequest : EntityRequest<EditCreateSubForCustomerQuoteRequest>
        {
            public EditCreateSubForCustomerQuoteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public EditCreateSubForCustomerQuoteRequest Notes(string notes)
            {
                MParams.AddOpt("notes", notes);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ExpiresAt(long expiresAt)
            {
                MParams.AddOpt("expires_at", expiresAt);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest BillingAlignmentMode(
                BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription[id]", subscriptionId);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionPlanUnitPriceInDecimal(
                string subscriptionPlanUnitPriceInDecimal)
            {
                MParams.AddOpt("subscription[plan_unit_price_in_decimal]", subscriptionPlanUnitPriceInDecimal);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionPlanQuantityInDecimal(
                string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.Add("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionPlanUnitPrice(int subscriptionPlanUnitPrice)
            {
                MParams.AddOpt("subscription[plan_unit_price]", subscriptionPlanUnitPrice);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest AddonUnitPriceInDecimal(int index,
                string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest EventBasedAddonUnitPrice(int index,
                int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest EventBasedAddonOnEvent(int index,
                OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest EventBasedAddonChargeOnce(int index,
                bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest EventBasedAddonChargeOn(int index,
                ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }

            public EditCreateSubForCustomerQuoteRequest AddonTrialEnd(int index, long addonTrialEnd)
            {
                MParams.AddOpt("addons[trial_end][" + index + "]", addonTrialEnd);
                return this;
            }
        }

        public class CreateSubItemsForCustomerQuoteRequest : EntityRequest<CreateSubItemsForCustomerQuoteRequest>
        {
            public CreateSubItemsForCustomerQuoteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateSubItemsForCustomerQuoteRequest Name(string name)
            {
                MParams.AddOpt("name", name);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest Notes(string notes)
            {
                MParams.AddOpt("notes", notes);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ExpiresAt(long expiresAt)
            {
                MParams.AddOpt("expires_at", expiresAt);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest MandatoryItemsToRemove(List<string> mandatoryItemsToRemove)
            {
                MParams.AddOpt("mandatory_items_to_remove", mandatoryItemsToRemove);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest BillingAlignmentMode(
                BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription[id]", subscriptionId);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public CreateSubItemsForCustomerQuoteRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionItemItemPriceId(int index,
                string subscriptionItemItemPriceId)
            {
                MParams.Add("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionItemQuantity(int index,
                int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionItemUnitPrice(int index,
                int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionItemBillingCycles(int index,
                int subscriptionItemBillingCycles)
            {
                MParams.AddOpt("subscription_items[billing_cycles][" + index + "]", subscriptionItemBillingCycles);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionItemTrialEnd(int index,
                long subscriptionItemTrialEnd)
            {
                MParams.AddOpt("subscription_items[trial_end][" + index + "]", subscriptionItemTrialEnd);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionItemChargeOnEvent(int index,
                ChargeOnEventEnum subscriptionItemChargeOnEvent)
            {
                MParams.AddOpt("subscription_items[charge_on_event][" + index + "]", subscriptionItemChargeOnEvent);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionItemChargeOnce(int index,
                bool subscriptionItemChargeOnce)
            {
                MParams.AddOpt("subscription_items[charge_once][" + index + "]", subscriptionItemChargeOnce);
                return this;
            }

            [Obsolete]
            public CreateSubItemsForCustomerQuoteRequest SubscriptionItemItemType(int index,
                ItemTypeEnum subscriptionItemItemType)
            {
                MParams.AddOpt("subscription_items[item_type][" + index + "]", subscriptionItemItemType);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest SubscriptionItemChargeOnOption(int index,
                ChargeOnOptionEnum subscriptionItemChargeOnOption)
            {
                MParams.AddOpt("subscription_items[charge_on_option][" + index + "]", subscriptionItemChargeOnOption);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CreateSubItemsForCustomerQuoteRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class UpdateSubscriptionQuoteRequest : EntityRequest<UpdateSubscriptionQuoteRequest>
        {
            public UpdateSubscriptionQuoteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateSubscriptionQuoteRequest Name(string name)
            {
                MParams.AddOpt("name", name);
                return this;
            }

            public UpdateSubscriptionQuoteRequest Notes(string notes)
            {
                MParams.AddOpt("notes", notes);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ExpiresAt(long expiresAt)
            {
                MParams.AddOpt("expires_at", expiresAt);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ReplaceAddonList(bool replaceAddonList)
            {
                MParams.AddOpt("replace_addon_list", replaceAddonList);
                return this;
            }

            public UpdateSubscriptionQuoteRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public UpdateSubscriptionQuoteRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ReactivateFrom(long reactivateFrom)
            {
                MParams.AddOpt("reactivate_from", reactivateFrom);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public UpdateSubscriptionQuoteRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ReplaceCouponList(bool replaceCouponList)
            {
                MParams.AddOpt("replace_coupon_list", replaceCouponList);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ForceTermReset(bool forceTermReset)
            {
                MParams.AddOpt("force_term_reset", forceTermReset);
                return this;
            }

            public UpdateSubscriptionQuoteRequest Reactivate(bool reactivate)
            {
                MParams.AddOpt("reactivate", reactivate);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionId(string subscriptionId)
            {
                MParams.Add("subscription[id]", subscriptionId);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.AddOpt("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionPlanUnitPrice(int subscriptionPlanUnitPrice)
            {
                MParams.AddOpt("subscription[plan_unit_price]", subscriptionPlanUnitPrice);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionPlanQuantityInDecimal(
                string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionPlanUnitPriceInDecimal(
                string subscriptionPlanUnitPriceInDecimal)
            {
                MParams.AddOpt("subscription[plan_unit_price_in_decimal]", subscriptionPlanUnitPriceInDecimal);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public UpdateSubscriptionQuoteRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionAutoCollection(
                AutoCollectionEnum subscriptionAutoCollection)
            {
                MParams.AddOpt("subscription[auto_collection]", subscriptionAutoCollection);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public UpdateSubscriptionQuoteRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public UpdateSubscriptionQuoteRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public UpdateSubscriptionQuoteRequest CustomerRegisteredForGst(bool customerRegisteredForGst)
            {
                MParams.AddOpt("customer[registered_for_gst]", customerRegisteredForGst);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public UpdateSubscriptionQuoteRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public UpdateSubscriptionQuoteRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public UpdateSubscriptionQuoteRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public UpdateSubscriptionQuoteRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public UpdateSubscriptionQuoteRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public UpdateSubscriptionQuoteRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public UpdateSubscriptionQuoteRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public UpdateSubscriptionQuoteRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public UpdateSubscriptionQuoteRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public UpdateSubscriptionQuoteRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public UpdateSubscriptionQuoteRequest EventBasedAddonChargeOn(int index,
                ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }

            public UpdateSubscriptionQuoteRequest EventBasedAddonOnEvent(int index, OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public UpdateSubscriptionQuoteRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public UpdateSubscriptionQuoteRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public UpdateSubscriptionQuoteRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public UpdateSubscriptionQuoteRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public UpdateSubscriptionQuoteRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public UpdateSubscriptionQuoteRequest AddonTrialEnd(int index, long addonTrialEnd)
            {
                MParams.AddOpt("addons[trial_end][" + index + "]", addonTrialEnd);
                return this;
            }
        }

        public class EditUpdateSubscriptionQuoteRequest : EntityRequest<EditUpdateSubscriptionQuoteRequest>
        {
            public EditUpdateSubscriptionQuoteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public EditUpdateSubscriptionQuoteRequest Notes(string notes)
            {
                MParams.AddOpt("notes", notes);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ExpiresAt(long expiresAt)
            {
                MParams.AddOpt("expires_at", expiresAt);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ReplaceAddonList(bool replaceAddonList)
            {
                MParams.AddOpt("replace_addon_list", replaceAddonList);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ReactivateFrom(long reactivateFrom)
            {
                MParams.AddOpt("reactivate_from", reactivateFrom);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAlignmentMode(
                BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ReplaceCouponList(bool replaceCouponList)
            {
                MParams.AddOpt("replace_coupon_list", replaceCouponList);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ForceTermReset(bool forceTermReset)
            {
                MParams.AddOpt("force_term_reset", forceTermReset);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest Reactivate(bool reactivate)
            {
                MParams.AddOpt("reactivate", reactivate);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.AddOpt("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionPlanUnitPrice(int subscriptionPlanUnitPrice)
            {
                MParams.AddOpt("subscription[plan_unit_price]", subscriptionPlanUnitPrice);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionPlanQuantityInDecimal(
                string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionPlanUnitPriceInDecimal(
                string subscriptionPlanUnitPriceInDecimal)
            {
                MParams.AddOpt("subscription[plan_unit_price_in_decimal]", subscriptionPlanUnitPriceInDecimal);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public EditUpdateSubscriptionQuoteRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionAutoCollection(
                AutoCollectionEnum subscriptionAutoCollection)
            {
                MParams.AddOpt("subscription[auto_collection]", subscriptionAutoCollection);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest CustomerRegisteredForGst(bool customerRegisteredForGst)
            {
                MParams.AddOpt("customer[registered_for_gst]", customerRegisteredForGst);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest EventBasedAddonChargeOn(int index,
                ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest EventBasedAddonOnEvent(int index,
                OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest EventBasedAddonChargeOnce(int index,
                bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public EditUpdateSubscriptionQuoteRequest AddonTrialEnd(int index, long addonTrialEnd)
            {
                MParams.AddOpt("addons[trial_end][" + index + "]", addonTrialEnd);
                return this;
            }
        }

        public class UpdateSubscriptionQuoteForItemsRequest : EntityRequest<UpdateSubscriptionQuoteForItemsRequest>
        {
            public UpdateSubscriptionQuoteForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateSubscriptionQuoteForItemsRequest Name(string name)
            {
                MParams.AddOpt("name", name);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest Notes(string notes)
            {
                MParams.AddOpt("notes", notes);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ExpiresAt(long expiresAt)
            {
                MParams.AddOpt("expires_at", expiresAt);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest MandatoryItemsToRemove(List<string> mandatoryItemsToRemove)
            {
                MParams.AddOpt("mandatory_items_to_remove", mandatoryItemsToRemove);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ReplaceItemsList(bool replaceItemsList)
            {
                MParams.AddOpt("replace_items_list", replaceItemsList);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ReactivateFrom(long reactivateFrom)
            {
                MParams.AddOpt("reactivate_from", reactivateFrom);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAlignmentMode(
                BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ReplaceCouponList(bool replaceCouponList)
            {
                MParams.AddOpt("replace_coupon_list", replaceCouponList);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ForceTermReset(bool forceTermReset)
            {
                MParams.AddOpt("force_term_reset", forceTermReset);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest Reactivate(bool reactivate)
            {
                MParams.AddOpt("reactivate", reactivate);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionId(string subscriptionId)
            {
                MParams.Add("subscription[id]", subscriptionId);
                return this;
            }

            [Obsolete]
            public UpdateSubscriptionQuoteForItemsRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public UpdateSubscriptionQuoteForItemsRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionAutoCollection(
                AutoCollectionEnum subscriptionAutoCollection)
            {
                MParams.AddOpt("subscription[auto_collection]", subscriptionAutoCollection);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest CustomerRegisteredForGst(bool customerRegisteredForGst)
            {
                MParams.AddOpt("customer[registered_for_gst]", customerRegisteredForGst);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionItemItemPriceId(int index,
                string subscriptionItemItemPriceId)
            {
                MParams.Add("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionItemQuantity(int index,
                int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionItemUnitPrice(int index,
                int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionItemBillingCycles(int index,
                int subscriptionItemBillingCycles)
            {
                MParams.AddOpt("subscription_items[billing_cycles][" + index + "]", subscriptionItemBillingCycles);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionItemTrialEnd(int index,
                long subscriptionItemTrialEnd)
            {
                MParams.AddOpt("subscription_items[trial_end][" + index + "]", subscriptionItemTrialEnd);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionItemChargeOnEvent(int index,
                ChargeOnEventEnum subscriptionItemChargeOnEvent)
            {
                MParams.AddOpt("subscription_items[charge_on_event][" + index + "]", subscriptionItemChargeOnEvent);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionItemChargeOnce(int index,
                bool subscriptionItemChargeOnce)
            {
                MParams.AddOpt("subscription_items[charge_once][" + index + "]", subscriptionItemChargeOnce);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest SubscriptionItemChargeOnOption(int index,
                ChargeOnOptionEnum subscriptionItemChargeOnOption)
            {
                MParams.AddOpt("subscription_items[charge_on_option][" + index + "]", subscriptionItemChargeOnOption);
                return this;
            }

            [Obsolete]
            public UpdateSubscriptionQuoteForItemsRequest SubscriptionItemItemType(int index,
                ItemTypeEnum subscriptionItemItemType)
            {
                MParams.AddOpt("subscription_items[item_type][" + index + "]", subscriptionItemItemType);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public UpdateSubscriptionQuoteForItemsRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class CreateForOnetimeChargesRequest : EntityRequest<CreateForOnetimeChargesRequest>
        {
            public CreateForOnetimeChargesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateForOnetimeChargesRequest Name(string name)
            {
                MParams.AddOpt("name", name);
                return this;
            }

            public CreateForOnetimeChargesRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public CreateForOnetimeChargesRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public CreateForOnetimeChargesRequest Notes(string notes)
            {
                MParams.AddOpt("notes", notes);
                return this;
            }

            public CreateForOnetimeChargesRequest ExpiresAt(long expiresAt)
            {
                MParams.AddOpt("expires_at", expiresAt);
                return this;
            }

            public CreateForOnetimeChargesRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateForOnetimeChargesRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateForOnetimeChargesRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateForOnetimeChargesRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CreateForOnetimeChargesRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CreateForOnetimeChargesRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CreateForOnetimeChargesRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CreateForOnetimeChargesRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CreateForOnetimeChargesRequest AddonServicePeriod(int index, int addonServicePeriod)
            {
                MParams.AddOpt("addons[service_period][" + index + "]", addonServicePeriod);
                return this;
            }

            public CreateForOnetimeChargesRequest ChargeAmount(int index, int chargeAmount)
            {
                MParams.AddOpt("charges[amount][" + index + "]", chargeAmount);
                return this;
            }

            public CreateForOnetimeChargesRequest ChargeAmountInDecimal(int index, string chargeAmountInDecimal)
            {
                MParams.AddOpt("charges[amount_in_decimal][" + index + "]", chargeAmountInDecimal);
                return this;
            }

            public CreateForOnetimeChargesRequest ChargeDescription(int index, string chargeDescription)
            {
                MParams.AddOpt("charges[description][" + index + "]", chargeDescription);
                return this;
            }

            public CreateForOnetimeChargesRequest ChargeAvalaraSaleType(int index,
                AvalaraSaleTypeEnum chargeAvalaraSaleType)
            {
                MParams.AddOpt("charges[avalara_sale_type][" + index + "]", chargeAvalaraSaleType);
                return this;
            }

            public CreateForOnetimeChargesRequest ChargeAvalaraTransactionType(int index,
                int chargeAvalaraTransactionType)
            {
                MParams.AddOpt("charges[avalara_transaction_type][" + index + "]", chargeAvalaraTransactionType);
                return this;
            }

            public CreateForOnetimeChargesRequest ChargeAvalaraServiceType(int index, int chargeAvalaraServiceType)
            {
                MParams.AddOpt("charges[avalara_service_type][" + index + "]", chargeAvalaraServiceType);
                return this;
            }

            public CreateForOnetimeChargesRequest ChargeServicePeriod(int index, int chargeServicePeriod)
            {
                MParams.AddOpt("charges[service_period][" + index + "]", chargeServicePeriod);
                return this;
            }
        }

        public class CreateForChargeItemsAndChargesRequest : EntityRequest<CreateForChargeItemsAndChargesRequest>
        {
            public CreateForChargeItemsAndChargesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateForChargeItemsAndChargesRequest Name(string name)
            {
                MParams.AddOpt("name", name);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest Notes(string notes)
            {
                MParams.AddOpt("notes", notes);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ExpiresAt(long expiresAt)
            {
                MParams.AddOpt("expires_at", expiresAt);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemPriceItemPriceId(int index, string itemPriceItemPriceId)
            {
                MParams.AddOpt("item_prices[item_price_id][" + index + "]", itemPriceItemPriceId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemPriceQuantity(int index, int itemPriceQuantity)
            {
                MParams.AddOpt("item_prices[quantity][" + index + "]", itemPriceQuantity);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemPriceUnitPrice(int index, int itemPriceUnitPrice)
            {
                MParams.AddOpt("item_prices[unit_price][" + index + "]", itemPriceUnitPrice);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemPriceDateFrom(int index, long itemPriceDateFrom)
            {
                MParams.AddOpt("item_prices[date_from][" + index + "]", itemPriceDateFrom);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemPriceDateTo(int index, long itemPriceDateTo)
            {
                MParams.AddOpt("item_prices[date_to][" + index + "]", itemPriceDateTo);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAmount(int index, int chargeAmount)
            {
                MParams.AddOpt("charges[amount][" + index + "]", chargeAmount);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAmountInDecimal(int index, string chargeAmountInDecimal)
            {
                MParams.AddOpt("charges[amount_in_decimal][" + index + "]", chargeAmountInDecimal);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeDescription(int index, string chargeDescription)
            {
                MParams.AddOpt("charges[description][" + index + "]", chargeDescription);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAvalaraSaleType(int index,
                AvalaraSaleTypeEnum chargeAvalaraSaleType)
            {
                MParams.AddOpt("charges[avalara_sale_type][" + index + "]", chargeAvalaraSaleType);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAvalaraTransactionType(int index,
                int chargeAvalaraTransactionType)
            {
                MParams.AddOpt("charges[avalara_transaction_type][" + index + "]", chargeAvalaraTransactionType);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAvalaraServiceType(int index,
                int chargeAvalaraServiceType)
            {
                MParams.AddOpt("charges[avalara_service_type][" + index + "]", chargeAvalaraServiceType);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeServicePeriod(int index, int chargeServicePeriod)
            {
                MParams.AddOpt("charges[service_period][" + index + "]", chargeServicePeriod);
                return this;
            }
        }

        public class EditOneTimeQuoteRequest : EntityRequest<EditOneTimeQuoteRequest>
        {
            public EditOneTimeQuoteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public EditOneTimeQuoteRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public EditOneTimeQuoteRequest Notes(string notes)
            {
                MParams.AddOpt("notes", notes);
                return this;
            }

            public EditOneTimeQuoteRequest ExpiresAt(long expiresAt)
            {
                MParams.AddOpt("expires_at", expiresAt);
                return this;
            }

            public EditOneTimeQuoteRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public EditOneTimeQuoteRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public EditOneTimeQuoteRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public EditOneTimeQuoteRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public EditOneTimeQuoteRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public EditOneTimeQuoteRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public EditOneTimeQuoteRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public EditOneTimeQuoteRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public EditOneTimeQuoteRequest AddonServicePeriod(int index, int addonServicePeriod)
            {
                MParams.AddOpt("addons[service_period][" + index + "]", addonServicePeriod);
                return this;
            }

            public EditOneTimeQuoteRequest ChargeAmount(int index, int chargeAmount)
            {
                MParams.AddOpt("charges[amount][" + index + "]", chargeAmount);
                return this;
            }

            public EditOneTimeQuoteRequest ChargeAmountInDecimal(int index, string chargeAmountInDecimal)
            {
                MParams.AddOpt("charges[amount_in_decimal][" + index + "]", chargeAmountInDecimal);
                return this;
            }

            public EditOneTimeQuoteRequest ChargeDescription(int index, string chargeDescription)
            {
                MParams.AddOpt("charges[description][" + index + "]", chargeDescription);
                return this;
            }

            public EditOneTimeQuoteRequest ChargeAvalaraSaleType(int index, AvalaraSaleTypeEnum chargeAvalaraSaleType)
            {
                MParams.AddOpt("charges[avalara_sale_type][" + index + "]", chargeAvalaraSaleType);
                return this;
            }

            public EditOneTimeQuoteRequest ChargeAvalaraTransactionType(int index, int chargeAvalaraTransactionType)
            {
                MParams.AddOpt("charges[avalara_transaction_type][" + index + "]", chargeAvalaraTransactionType);
                return this;
            }

            public EditOneTimeQuoteRequest ChargeAvalaraServiceType(int index, int chargeAvalaraServiceType)
            {
                MParams.AddOpt("charges[avalara_service_type][" + index + "]", chargeAvalaraServiceType);
                return this;
            }

            public EditOneTimeQuoteRequest ChargeServicePeriod(int index, int chargeServicePeriod)
            {
                MParams.AddOpt("charges[service_period][" + index + "]", chargeServicePeriod);
                return this;
            }
        }

        public class QuoteListRequest : ListRequestBase<QuoteListRequest>
        {
            public QuoteListRequest(string url)
                : base(url)
            {
            }

            public QuoteListRequest IncludeDeleted(bool includeDeleted)
            {
                MParams.AddOpt("include_deleted", includeDeleted);
                return this;
            }

            public StringFilter<QuoteListRequest> Id()
            {
                return new StringFilter<QuoteListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<QuoteListRequest> CustomerId()
            {
                return new StringFilter<QuoteListRequest>("customer_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<QuoteListRequest> SubscriptionId()
            {
                return new StringFilter<QuoteListRequest>("subscription_id", this).SupportsMultiOperators(true)
                    .SupportsPresenceOperator(true);
            }

            public EnumFilter<StatusEnum, QuoteListRequest> Status()
            {
                return new("status", this);
            }

            public TimestampFilter<QuoteListRequest> Date()
            {
                return new("date", this);
            }

            public TimestampFilter<QuoteListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public QuoteListRequest SortByDate(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "date");
                return this;
            }
        }

        public class ConvertRequest : EntityRequest<ConvertRequest>
        {
            public ConvertRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ConvertRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription[id]", subscriptionId);
                return this;
            }

            public ConvertRequest SubscriptionAutoCollection(AutoCollectionEnum subscriptionAutoCollection)
            {
                MParams.AddOpt("subscription[auto_collection]", subscriptionAutoCollection);
                return this;
            }

            public ConvertRequest SubscriptionPoNumber(string subscriptionPoNumber)
            {
                MParams.AddOpt("subscription[po_number]", subscriptionPoNumber);
                return this;
            }
        }

        public class UpdateStatusRequest : EntityRequest<UpdateStatusRequest>
        {
            public UpdateStatusRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateStatusRequest Status(StatusEnum status)
            {
                MParams.Add("status", status);
                return this;
            }

            public UpdateStatusRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }
        }

        public class ExtendExpiryDateRequest : EntityRequest<ExtendExpiryDateRequest>
        {
            public ExtendExpiryDateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ExtendExpiryDateRequest ValidTill(long validTill)
            {
                MParams.Add("valid_till", validTill);
                return this;
            }
        }

        public class DeleteRequest : EntityRequest<DeleteRequest>
        {
            public DeleteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public DeleteRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }
        }

        public class PdfRequest : EntityRequest<PdfRequest>
        {
            public PdfRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public PdfRequest ConsolidatedView(bool consolidatedView)
            {
                MParams.AddOpt("consolidated_view", consolidatedView);
                return this;
            }

            public PdfRequest DispositionType(DispositionTypeEnum dispositionType)
            {
                MParams.AddOpt("disposition_type", dispositionType);
                return this;
            }
        }

        #endregion

        #region Subclasses

        public class QuoteLineItem : Resource
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

        public class QuoteDiscount : Resource
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

        public class QuoteLineItemDiscount : Resource
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

        public class QuoteTax : Resource
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

        public class QuoteLineItemTax : Resource
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

        public class QuoteShippingAddress : Resource
        {
            public string FirstName()
            {
                return GetValue<string>("first_name", false);
            }

            public string LastName()
            {
                return GetValue<string>("last_name", false);
            }

            public string Email()
            {
                return GetValue<string>("email", false);
            }

            public string Company()
            {
                return GetValue<string>("company", false);
            }

            public string Phone()
            {
                return GetValue<string>("phone", false);
            }

            public string Line1()
            {
                return GetValue<string>("line1", false);
            }

            public string Line2()
            {
                return GetValue<string>("line2", false);
            }

            public string Line3()
            {
                return GetValue<string>("line3", false);
            }

            public string City()
            {
                return GetValue<string>("city", false);
            }

            public string StateCode()
            {
                return GetValue<string>("state_code", false);
            }

            public string State()
            {
                return GetValue<string>("state", false);
            }

            public string Country()
            {
                return GetValue<string>("country", false);
            }

            public string Zip()
            {
                return GetValue<string>("zip", false);
            }

            public ValidationStatusEnum? ValidationStatus()
            {
                return GetEnum<ValidationStatusEnum>("validation_status", false);
            }
        }

        public class QuoteBillingAddress : Resource
        {
            public string FirstName()
            {
                return GetValue<string>("first_name", false);
            }

            public string LastName()
            {
                return GetValue<string>("last_name", false);
            }

            public string Email()
            {
                return GetValue<string>("email", false);
            }

            public string Company()
            {
                return GetValue<string>("company", false);
            }

            public string Phone()
            {
                return GetValue<string>("phone", false);
            }

            public string Line1()
            {
                return GetValue<string>("line1", false);
            }

            public string Line2()
            {
                return GetValue<string>("line2", false);
            }

            public string Line3()
            {
                return GetValue<string>("line3", false);
            }

            public string City()
            {
                return GetValue<string>("city", false);
            }

            public string StateCode()
            {
                return GetValue<string>("state_code", false);
            }

            public string State()
            {
                return GetValue<string>("state", false);
            }

            public string Country()
            {
                return GetValue<string>("country", false);
            }

            public string Zip()
            {
                return GetValue<string>("zip", false);
            }

            public ValidationStatusEnum? ValidationStatus()
            {
                return GetEnum<ValidationStatusEnum>("validation_status", false);
            }
        }

        #endregion
    }
}