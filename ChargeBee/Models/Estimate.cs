using System;
using System.Collections.Generic;
using System.IO;
using ChargeBee.Api;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Estimate : Resource
    {
        public Estimate()
        {
        }

        public Estimate(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Estimate(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Estimate(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateSubscriptionRequest CreateSubscription()
        {
            var url = ApiUtil.BuildUrl("estimates", "create_subscription");
            return new CreateSubscriptionRequest(url, HttpMethod.Post);
        }

        public static CreateSubItemEstimateRequest CreateSubItemEstimate()
        {
            var url = ApiUtil.BuildUrl("estimates", "create_subscription_for_items");
            return new CreateSubItemEstimateRequest(url, HttpMethod.Post);
        }

        public static CreateSubForCustomerEstimateRequest CreateSubForCustomerEstimate(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "create_subscription_estimate");
            return new CreateSubForCustomerEstimateRequest(url, HttpMethod.Get);
        }

        public static CreateSubItemForCustomerEstimateRequest CreateSubItemForCustomerEstimate(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "create_subscription_for_items_estimate");
            return new CreateSubItemForCustomerEstimateRequest(url, HttpMethod.Post);
        }

        public static UpdateSubscriptionRequest UpdateSubscription()
        {
            var url = ApiUtil.BuildUrl("estimates", "update_subscription");
            return new UpdateSubscriptionRequest(url, HttpMethod.Post);
        }

        public static UpdateSubscriptionForItemsRequest UpdateSubscriptionForItems()
        {
            var url = ApiUtil.BuildUrl("estimates", "update_subscription_for_items");
            return new UpdateSubscriptionForItemsRequest(url, HttpMethod.Post);
        }

        public static RenewalEstimateRequest RenewalEstimate(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "renewal_estimate");
            return new RenewalEstimateRequest(url, HttpMethod.Get);
        }

        public static AdvanceInvoiceEstimateRequest AdvanceInvoiceEstimate(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "advance_invoice_estimate");
            return new AdvanceInvoiceEstimateRequest(url, HttpMethod.Post);
        }

        public static RegenerateInvoiceEstimateRequest RegenerateInvoiceEstimate(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "regenerate_invoice_estimate");
            return new RegenerateInvoiceEstimateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> UpcomingInvoicesEstimate(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "upcoming_invoices_estimate");
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static ChangeTermEndRequest ChangeTermEnd(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "change_term_end_estimate");
            return new ChangeTermEndRequest(url, HttpMethod.Post);
        }

        public static CancelSubscriptionRequest CancelSubscription(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "cancel_subscription_estimate");
            return new CancelSubscriptionRequest(url, HttpMethod.Post);
        }

        public static CancelSubscriptionForItemsRequest CancelSubscriptionForItems(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "cancel_subscription_for_items_estimate");
            return new CancelSubscriptionForItemsRequest(url, HttpMethod.Post);
        }

        public static PauseSubscriptionRequest PauseSubscription(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "pause_subscription_estimate");
            return new PauseSubscriptionRequest(url, HttpMethod.Post);
        }

        public static ResumeSubscriptionRequest ResumeSubscription(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "resume_subscription_estimate");
            return new ResumeSubscriptionRequest(url, HttpMethod.Post);
        }

        public static GiftSubscriptionRequest GiftSubscription()
        {
            var url = ApiUtil.BuildUrl("estimates", "gift_subscription");
            return new GiftSubscriptionRequest(url, HttpMethod.Post);
        }

        public static GiftSubscriptionForItemsRequest GiftSubscriptionForItems()
        {
            var url = ApiUtil.BuildUrl("estimates", "gift_subscription_for_items");
            return new GiftSubscriptionForItemsRequest(url, HttpMethod.Post);
        }

        public static CreateInvoiceRequest CreateInvoice()
        {
            var url = ApiUtil.BuildUrl("estimates", "create_invoice");
            return new CreateInvoiceRequest(url, HttpMethod.Post);
        }

        public static CreateInvoiceForItemsRequest CreateInvoiceForItems()
        {
            var url = ApiUtil.BuildUrl("estimates", "create_invoice_for_items");
            return new CreateInvoiceForItemsRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public SubscriptionEstimate SubscriptionEstimate =>
            GetSubResource<SubscriptionEstimate>("subscription_estimate");

        public InvoiceEstimate InvoiceEstimate => GetSubResource<InvoiceEstimate>("invoice_estimate");

        public List<InvoiceEstimate> InvoiceEstimates => GetResourceList<InvoiceEstimate>("invoice_estimates");

        public InvoiceEstimate NextInvoiceEstimate => GetSubResource<InvoiceEstimate>("next_invoice_estimate");

        public List<CreditNoteEstimate> CreditNoteEstimates =>
            GetResourceList<CreditNoteEstimate>("credit_note_estimates");

        public List<UnbilledCharge> UnbilledChargeEstimates =>
            GetResourceList<UnbilledCharge>("unbilled_charge_estimates");

        #endregion

        #region Requests

        public class CreateSubscriptionRequest : EntityRequest<CreateSubscriptionRequest>
        {
            public CreateSubscriptionRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateSubscriptionRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateSubscriptionRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public CreateSubscriptionRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CreateSubscriptionRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CreateSubscriptionRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateSubscriptionRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public CreateSubscriptionRequest ClientProfileId(string clientProfileId)
            {
                MParams.AddOpt("client_profile_id", clientProfileId);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription[id]", subscriptionId);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionPlanUnitPriceInDecimal(
                string subscriptionPlanUnitPriceInDecimal)
            {
                MParams.AddOpt("subscription[plan_unit_price_in_decimal]", subscriptionPlanUnitPriceInDecimal);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionPlanQuantityInDecimal(string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.Add("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionPlanUnitPrice(int subscriptionPlanUnitPrice)
            {
                MParams.AddOpt("subscription[plan_unit_price]", subscriptionPlanUnitPrice);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            [Obsolete]
            public CreateSubscriptionRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public CreateSubscriptionRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public CreateSubscriptionRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public CreateSubscriptionRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public CreateSubscriptionRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public CreateSubscriptionRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public CreateSubscriptionRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public CreateSubscriptionRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public CreateSubscriptionRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public CreateSubscriptionRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateSubscriptionRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateSubscriptionRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateSubscriptionRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateSubscriptionRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateSubscriptionRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateSubscriptionRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateSubscriptionRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateSubscriptionRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public CreateSubscriptionRequest CustomerRegisteredForGst(bool customerRegisteredForGst)
            {
                MParams.AddOpt("customer[registered_for_gst]", customerRegisteredForGst);
                return this;
            }

            public CreateSubscriptionRequest CustomerTaxability(TaxabilityEnum customerTaxability)
            {
                MParams.AddOpt("customer[taxability]", customerTaxability);
                return this;
            }

            public CreateSubscriptionRequest CustomerEntityCode(EntityCodeEnum customerEntityCode)
            {
                MParams.AddOpt("customer[entity_code]", customerEntityCode);
                return this;
            }

            public CreateSubscriptionRequest CustomerExemptNumber(string customerExemptNumber)
            {
                MParams.AddOpt("customer[exempt_number]", customerExemptNumber);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionFreePeriod(int subscriptionFreePeriod)
            {
                MParams.AddOpt("subscription[free_period]", subscriptionFreePeriod);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionFreePeriodUnit(FreePeriodUnitEnum subscriptionFreePeriodUnit)
            {
                MParams.AddOpt("subscription[free_period_unit]", subscriptionFreePeriodUnit);
                return this;
            }

            public CreateSubscriptionRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CreateSubscriptionRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CreateSubscriptionRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public CreateSubscriptionRequest CustomerExemptionDetails(JArray customerExemptionDetails)
            {
                MParams.AddOpt("customer[exemption_details]", customerExemptionDetails);
                return this;
            }

            public CreateSubscriptionRequest CustomerCustomerType(CustomerTypeEnum customerCustomerType)
            {
                MParams.AddOpt("customer[customer_type]", customerCustomerType);
                return this;
            }

            public CreateSubscriptionRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CreateSubscriptionRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CreateSubscriptionRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CreateSubscriptionRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CreateSubscriptionRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CreateSubscriptionRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public CreateSubscriptionRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public CreateSubscriptionRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public CreateSubscriptionRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public CreateSubscriptionRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public CreateSubscriptionRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public CreateSubscriptionRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public CreateSubscriptionRequest EventBasedAddonOnEvent(int index, OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public CreateSubscriptionRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public CreateSubscriptionRequest EventBasedAddonChargeOn(int index, ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }

            public CreateSubscriptionRequest AddonTrialEnd(int index, long addonTrialEnd)
            {
                MParams.AddOpt("addons[trial_end][" + index + "]", addonTrialEnd);
                return this;
            }
        }

        public class CreateSubItemEstimateRequest : EntityRequest<CreateSubItemEstimateRequest>
        {
            public CreateSubItemEstimateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateSubItemEstimateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateSubItemEstimateRequest MandatoryItemsToRemove(List<string> mandatoryItemsToRemove)
            {
                MParams.AddOpt("mandatory_items_to_remove", mandatoryItemsToRemove);
                return this;
            }

            public CreateSubItemEstimateRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CreateSubItemEstimateRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CreateSubItemEstimateRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateSubItemEstimateRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public CreateSubItemEstimateRequest ClientProfileId(string clientProfileId)
            {
                MParams.AddOpt("client_profile_id", clientProfileId);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription[id]", subscriptionId);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public CreateSubItemEstimateRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            [Obsolete]
            public CreateSubItemEstimateRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public CreateSubItemEstimateRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public CreateSubItemEstimateRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public CreateSubItemEstimateRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public CreateSubItemEstimateRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public CreateSubItemEstimateRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public CreateSubItemEstimateRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public CreateSubItemEstimateRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public CreateSubItemEstimateRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public CreateSubItemEstimateRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateSubItemEstimateRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateSubItemEstimateRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateSubItemEstimateRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateSubItemEstimateRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateSubItemEstimateRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateSubItemEstimateRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateSubItemEstimateRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateSubItemEstimateRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public CreateSubItemEstimateRequest CustomerRegisteredForGst(bool customerRegisteredForGst)
            {
                MParams.AddOpt("customer[registered_for_gst]", customerRegisteredForGst);
                return this;
            }

            public CreateSubItemEstimateRequest CustomerTaxability(TaxabilityEnum customerTaxability)
            {
                MParams.AddOpt("customer[taxability]", customerTaxability);
                return this;
            }

            public CreateSubItemEstimateRequest CustomerEntityCode(EntityCodeEnum customerEntityCode)
            {
                MParams.AddOpt("customer[entity_code]", customerEntityCode);
                return this;
            }

            public CreateSubItemEstimateRequest CustomerExemptNumber(string customerExemptNumber)
            {
                MParams.AddOpt("customer[exempt_number]", customerExemptNumber);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionFreePeriod(int subscriptionFreePeriod)
            {
                MParams.AddOpt("subscription[free_period]", subscriptionFreePeriod);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionFreePeriodUnit(
                FreePeriodUnitEnum subscriptionFreePeriodUnit)
            {
                MParams.AddOpt("subscription[free_period_unit]", subscriptionFreePeriodUnit);
                return this;
            }

            public CreateSubItemEstimateRequest CustomerExemptionDetails(JArray customerExemptionDetails)
            {
                MParams.AddOpt("customer[exemption_details]", customerExemptionDetails);
                return this;
            }

            public CreateSubItemEstimateRequest CustomerCustomerType(CustomerTypeEnum customerCustomerType)
            {
                MParams.AddOpt("customer[customer_type]", customerCustomerType);
                return this;
            }

            public CreateSubItemEstimateRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CreateSubItemEstimateRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionItemItemPriceId(int index,
                string subscriptionItemItemPriceId)
            {
                MParams.Add("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionItemUnitPrice(int index, int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionItemBillingCycles(int index,
                int subscriptionItemBillingCycles)
            {
                MParams.AddOpt("subscription_items[billing_cycles][" + index + "]", subscriptionItemBillingCycles);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionItemTrialEnd(int index, long subscriptionItemTrialEnd)
            {
                MParams.AddOpt("subscription_items[trial_end][" + index + "]", subscriptionItemTrialEnd);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionItemChargeOnEvent(int index,
                ChargeOnEventEnum subscriptionItemChargeOnEvent)
            {
                MParams.AddOpt("subscription_items[charge_on_event][" + index + "]", subscriptionItemChargeOnEvent);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionItemChargeOnce(int index, bool subscriptionItemChargeOnce)
            {
                MParams.AddOpt("subscription_items[charge_once][" + index + "]", subscriptionItemChargeOnce);
                return this;
            }

            [Obsolete]
            public CreateSubItemEstimateRequest SubscriptionItemItemType(int index,
                ItemTypeEnum subscriptionItemItemType)
            {
                MParams.AddOpt("subscription_items[item_type][" + index + "]", subscriptionItemItemType);
                return this;
            }

            public CreateSubItemEstimateRequest SubscriptionItemChargeOnOption(int index,
                ChargeOnOptionEnum subscriptionItemChargeOnOption)
            {
                MParams.AddOpt("subscription_items[charge_on_option][" + index + "]", subscriptionItemChargeOnOption);
                return this;
            }

            public CreateSubItemEstimateRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public CreateSubItemEstimateRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CreateSubItemEstimateRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CreateSubItemEstimateRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class CreateSubForCustomerEstimateRequest : EntityRequest<CreateSubForCustomerEstimateRequest>
        {
            public CreateSubForCustomerEstimateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateSubForCustomerEstimateRequest UseExistingBalances(bool useExistingBalances)
            {
                MParams.AddOpt("use_existing_balances", useExistingBalances);
                return this;
            }

            public CreateSubForCustomerEstimateRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public CreateSubForCustomerEstimateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateSubForCustomerEstimateRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public CreateSubForCustomerEstimateRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CreateSubForCustomerEstimateRequest BillingAlignmentMode(
                BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CreateSubForCustomerEstimateRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription[id]", subscriptionId);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionPlanUnitPriceInDecimal(
                string subscriptionPlanUnitPriceInDecimal)
            {
                MParams.AddOpt("subscription[plan_unit_price_in_decimal]", subscriptionPlanUnitPriceInDecimal);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionPlanQuantityInDecimal(
                string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.Add("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionPlanUnitPrice(int subscriptionPlanUnitPrice)
            {
                MParams.AddOpt("subscription[plan_unit_price]", subscriptionPlanUnitPrice);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public CreateSubForCustomerEstimateRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateSubForCustomerEstimateRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateSubForCustomerEstimateRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateSubForCustomerEstimateRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateSubForCustomerEstimateRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateSubForCustomerEstimateRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateSubForCustomerEstimateRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateSubForCustomerEstimateRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionFreePeriod(int subscriptionFreePeriod)
            {
                MParams.AddOpt("subscription[free_period]", subscriptionFreePeriod);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionFreePeriodUnit(
                FreePeriodUnitEnum subscriptionFreePeriodUnit)
            {
                MParams.AddOpt("subscription[free_period_unit]", subscriptionFreePeriodUnit);
                return this;
            }

            public CreateSubForCustomerEstimateRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CreateSubForCustomerEstimateRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CreateSubForCustomerEstimateRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public CreateSubForCustomerEstimateRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CreateSubForCustomerEstimateRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CreateSubForCustomerEstimateRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CreateSubForCustomerEstimateRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CreateSubForCustomerEstimateRequest AddonUnitPriceInDecimal(int index,
                string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CreateSubForCustomerEstimateRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public CreateSubForCustomerEstimateRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public CreateSubForCustomerEstimateRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public CreateSubForCustomerEstimateRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public CreateSubForCustomerEstimateRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public CreateSubForCustomerEstimateRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public CreateSubForCustomerEstimateRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public CreateSubForCustomerEstimateRequest EventBasedAddonOnEvent(int index,
                OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public CreateSubForCustomerEstimateRequest EventBasedAddonChargeOnce(int index,
                bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public CreateSubForCustomerEstimateRequest EventBasedAddonChargeOn(int index,
                ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }

            public CreateSubForCustomerEstimateRequest AddonTrialEnd(int index, long addonTrialEnd)
            {
                MParams.AddOpt("addons[trial_end][" + index + "]", addonTrialEnd);
                return this;
            }
        }

        public class CreateSubItemForCustomerEstimateRequest : EntityRequest<CreateSubItemForCustomerEstimateRequest>
        {
            public CreateSubItemForCustomerEstimateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateSubItemForCustomerEstimateRequest UseExistingBalances(bool useExistingBalances)
            {
                MParams.AddOpt("use_existing_balances", useExistingBalances);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest MandatoryItemsToRemove(List<string> mandatoryItemsToRemove)
            {
                MParams.AddOpt("mandatory_items_to_remove", mandatoryItemsToRemove);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest BillingAlignmentMode(
                BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription[id]", subscriptionId);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public CreateSubItemForCustomerEstimateRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionFreePeriod(int subscriptionFreePeriod)
            {
                MParams.AddOpt("subscription[free_period]", subscriptionFreePeriod);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionFreePeriodUnit(
                FreePeriodUnitEnum subscriptionFreePeriodUnit)
            {
                MParams.AddOpt("subscription[free_period_unit]", subscriptionFreePeriodUnit);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionItemItemPriceId(int index,
                string subscriptionItemItemPriceId)
            {
                MParams.Add("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionItemQuantity(int index,
                int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionItemUnitPrice(int index,
                int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionItemBillingCycles(int index,
                int subscriptionItemBillingCycles)
            {
                MParams.AddOpt("subscription_items[billing_cycles][" + index + "]", subscriptionItemBillingCycles);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionItemTrialEnd(int index,
                long subscriptionItemTrialEnd)
            {
                MParams.AddOpt("subscription_items[trial_end][" + index + "]", subscriptionItemTrialEnd);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionItemChargeOnEvent(int index,
                ChargeOnEventEnum subscriptionItemChargeOnEvent)
            {
                MParams.AddOpt("subscription_items[charge_on_event][" + index + "]", subscriptionItemChargeOnEvent);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionItemChargeOnce(int index,
                bool subscriptionItemChargeOnce)
            {
                MParams.AddOpt("subscription_items[charge_once][" + index + "]", subscriptionItemChargeOnce);
                return this;
            }

            [Obsolete]
            public CreateSubItemForCustomerEstimateRequest SubscriptionItemItemType(int index,
                ItemTypeEnum subscriptionItemItemType)
            {
                MParams.AddOpt("subscription_items[item_type][" + index + "]", subscriptionItemItemType);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest SubscriptionItemChargeOnOption(int index,
                ChargeOnOptionEnum subscriptionItemChargeOnOption)
            {
                MParams.AddOpt("subscription_items[charge_on_option][" + index + "]", subscriptionItemChargeOnOption);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CreateSubItemForCustomerEstimateRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class UpdateSubscriptionRequest : EntityRequest<UpdateSubscriptionRequest>
        {
            public UpdateSubscriptionRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateSubscriptionRequest ReplaceAddonList(bool replaceAddonList)
            {
                MParams.AddOpt("replace_addon_list", replaceAddonList);
                return this;
            }

            public UpdateSubscriptionRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public UpdateSubscriptionRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public UpdateSubscriptionRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public UpdateSubscriptionRequest ReactivateFrom(long reactivateFrom)
            {
                MParams.AddOpt("reactivate_from", reactivateFrom);
                return this;
            }

            public UpdateSubscriptionRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public UpdateSubscriptionRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public UpdateSubscriptionRequest ReplaceCouponList(bool replaceCouponList)
            {
                MParams.AddOpt("replace_coupon_list", replaceCouponList);
                return this;
            }

            public UpdateSubscriptionRequest Prorate(bool prorate)
            {
                MParams.AddOpt("prorate", prorate);
                return this;
            }

            public UpdateSubscriptionRequest EndOfTerm(bool endOfTerm)
            {
                MParams.AddOpt("end_of_term", endOfTerm);
                return this;
            }

            public UpdateSubscriptionRequest ForceTermReset(bool forceTermReset)
            {
                MParams.AddOpt("force_term_reset", forceTermReset);
                return this;
            }

            public UpdateSubscriptionRequest Reactivate(bool reactivate)
            {
                MParams.AddOpt("reactivate", reactivate);
                return this;
            }

            public UpdateSubscriptionRequest IncludeDelayedCharges(bool includeDelayedCharges)
            {
                MParams.AddOpt("include_delayed_charges", includeDelayedCharges);
                return this;
            }

            public UpdateSubscriptionRequest UseExistingBalances(bool useExistingBalances)
            {
                MParams.AddOpt("use_existing_balances", useExistingBalances);
                return this;
            }

            public UpdateSubscriptionRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionId(string subscriptionId)
            {
                MParams.Add("subscription[id]", subscriptionId);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.AddOpt("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionPlanUnitPrice(int subscriptionPlanUnitPrice)
            {
                MParams.AddOpt("subscription[plan_unit_price]", subscriptionPlanUnitPrice);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionPlanQuantityInDecimal(string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionPlanUnitPriceInDecimal(
                string subscriptionPlanUnitPriceInDecimal)
            {
                MParams.AddOpt("subscription[plan_unit_price_in_decimal]", subscriptionPlanUnitPriceInDecimal);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public UpdateSubscriptionRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionAutoCollection(AutoCollectionEnum subscriptionAutoCollection)
            {
                MParams.AddOpt("subscription[auto_collection]", subscriptionAutoCollection);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public UpdateSubscriptionRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public UpdateSubscriptionRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public UpdateSubscriptionRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public UpdateSubscriptionRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public UpdateSubscriptionRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public UpdateSubscriptionRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public UpdateSubscriptionRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public UpdateSubscriptionRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public UpdateSubscriptionRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public UpdateSubscriptionRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public UpdateSubscriptionRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public UpdateSubscriptionRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public UpdateSubscriptionRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public UpdateSubscriptionRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public UpdateSubscriptionRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public UpdateSubscriptionRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public UpdateSubscriptionRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public UpdateSubscriptionRequest CustomerRegisteredForGst(bool customerRegisteredForGst)
            {
                MParams.AddOpt("customer[registered_for_gst]", customerRegisteredForGst);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionFreePeriod(int subscriptionFreePeriod)
            {
                MParams.AddOpt("subscription[free_period]", subscriptionFreePeriod);
                return this;
            }

            public UpdateSubscriptionRequest SubscriptionFreePeriodUnit(FreePeriodUnitEnum subscriptionFreePeriodUnit)
            {
                MParams.AddOpt("subscription[free_period_unit]", subscriptionFreePeriodUnit);
                return this;
            }

            [Obsolete]
            public UpdateSubscriptionRequest CustomerTaxability(TaxabilityEnum customerTaxability)
            {
                MParams.AddOpt("customer[taxability]", customerTaxability);
                return this;
            }

            public UpdateSubscriptionRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public UpdateSubscriptionRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public UpdateSubscriptionRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public UpdateSubscriptionRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public UpdateSubscriptionRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public UpdateSubscriptionRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public UpdateSubscriptionRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public UpdateSubscriptionRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public UpdateSubscriptionRequest EventBasedAddonChargeOn(int index, ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }

            public UpdateSubscriptionRequest EventBasedAddonOnEvent(int index, OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public UpdateSubscriptionRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public UpdateSubscriptionRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public UpdateSubscriptionRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public UpdateSubscriptionRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public UpdateSubscriptionRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public UpdateSubscriptionRequest AddonTrialEnd(int index, long addonTrialEnd)
            {
                MParams.AddOpt("addons[trial_end][" + index + "]", addonTrialEnd);
                return this;
            }
        }

        public class UpdateSubscriptionForItemsRequest : EntityRequest<UpdateSubscriptionForItemsRequest>
        {
            public UpdateSubscriptionForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateSubscriptionForItemsRequest MandatoryItemsToRemove(List<string> mandatoryItemsToRemove)
            {
                MParams.AddOpt("mandatory_items_to_remove", mandatoryItemsToRemove);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ReplaceItemsList(bool replaceItemsList)
            {
                MParams.AddOpt("replace_items_list", replaceItemsList);
                return this;
            }

            public UpdateSubscriptionForItemsRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public UpdateSubscriptionForItemsRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ReactivateFrom(long reactivateFrom)
            {
                MParams.AddOpt("reactivate_from", reactivateFrom);
                return this;
            }

            public UpdateSubscriptionForItemsRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public UpdateSubscriptionForItemsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ReplaceCouponList(bool replaceCouponList)
            {
                MParams.AddOpt("replace_coupon_list", replaceCouponList);
                return this;
            }

            public UpdateSubscriptionForItemsRequest Prorate(bool prorate)
            {
                MParams.AddOpt("prorate", prorate);
                return this;
            }

            public UpdateSubscriptionForItemsRequest EndOfTerm(bool endOfTerm)
            {
                MParams.AddOpt("end_of_term", endOfTerm);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ForceTermReset(bool forceTermReset)
            {
                MParams.AddOpt("force_term_reset", forceTermReset);
                return this;
            }

            public UpdateSubscriptionForItemsRequest Reactivate(bool reactivate)
            {
                MParams.AddOpt("reactivate", reactivate);
                return this;
            }

            public UpdateSubscriptionForItemsRequest IncludeDelayedCharges(bool includeDelayedCharges)
            {
                MParams.AddOpt("include_delayed_charges", includeDelayedCharges);
                return this;
            }

            public UpdateSubscriptionForItemsRequest UseExistingBalances(bool useExistingBalances)
            {
                MParams.AddOpt("use_existing_balances", useExistingBalances);
                return this;
            }

            public UpdateSubscriptionForItemsRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionId(string subscriptionId)
            {
                MParams.Add("subscription[id]", subscriptionId);
                return this;
            }

            [Obsolete]
            public UpdateSubscriptionForItemsRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public UpdateSubscriptionForItemsRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionAutoCollection(
                AutoCollectionEnum subscriptionAutoCollection)
            {
                MParams.AddOpt("subscription[auto_collection]", subscriptionAutoCollection);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public UpdateSubscriptionForItemsRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public UpdateSubscriptionForItemsRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public UpdateSubscriptionForItemsRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public UpdateSubscriptionForItemsRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public UpdateSubscriptionForItemsRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public UpdateSubscriptionForItemsRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public UpdateSubscriptionForItemsRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public UpdateSubscriptionForItemsRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public UpdateSubscriptionForItemsRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public UpdateSubscriptionForItemsRequest CustomerRegisteredForGst(bool customerRegisteredForGst)
            {
                MParams.AddOpt("customer[registered_for_gst]", customerRegisteredForGst);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionFreePeriod(int subscriptionFreePeriod)
            {
                MParams.AddOpt("subscription[free_period]", subscriptionFreePeriod);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionFreePeriodUnit(
                FreePeriodUnitEnum subscriptionFreePeriodUnit)
            {
                MParams.AddOpt("subscription[free_period_unit]", subscriptionFreePeriodUnit);
                return this;
            }

            [Obsolete]
            public UpdateSubscriptionForItemsRequest CustomerTaxability(TaxabilityEnum customerTaxability)
            {
                MParams.AddOpt("customer[taxability]", customerTaxability);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionItemItemPriceId(int index,
                string subscriptionItemItemPriceId)
            {
                MParams.Add("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionItemUnitPrice(int index, int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionItemBillingCycles(int index,
                int subscriptionItemBillingCycles)
            {
                MParams.AddOpt("subscription_items[billing_cycles][" + index + "]", subscriptionItemBillingCycles);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionItemTrialEnd(int index, long subscriptionItemTrialEnd)
            {
                MParams.AddOpt("subscription_items[trial_end][" + index + "]", subscriptionItemTrialEnd);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionItemChargeOnEvent(int index,
                ChargeOnEventEnum subscriptionItemChargeOnEvent)
            {
                MParams.AddOpt("subscription_items[charge_on_event][" + index + "]", subscriptionItemChargeOnEvent);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionItemChargeOnce(int index,
                bool subscriptionItemChargeOnce)
            {
                MParams.AddOpt("subscription_items[charge_once][" + index + "]", subscriptionItemChargeOnce);
                return this;
            }

            public UpdateSubscriptionForItemsRequest SubscriptionItemChargeOnOption(int index,
                ChargeOnOptionEnum subscriptionItemChargeOnOption)
            {
                MParams.AddOpt("subscription_items[charge_on_option][" + index + "]", subscriptionItemChargeOnOption);
                return this;
            }

            [Obsolete]
            public UpdateSubscriptionForItemsRequest SubscriptionItemItemType(int index,
                ItemTypeEnum subscriptionItemItemType)
            {
                MParams.AddOpt("subscription_items[item_type][" + index + "]", subscriptionItemItemType);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public UpdateSubscriptionForItemsRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class RenewalEstimateRequest : EntityRequest<RenewalEstimateRequest>
        {
            public RenewalEstimateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RenewalEstimateRequest IncludeDelayedCharges(bool includeDelayedCharges)
            {
                MParams.AddOpt("include_delayed_charges", includeDelayedCharges);
                return this;
            }

            public RenewalEstimateRequest UseExistingBalances(bool useExistingBalances)
            {
                MParams.AddOpt("use_existing_balances", useExistingBalances);
                return this;
            }

            public RenewalEstimateRequest IgnoreScheduledCancellation(bool ignoreScheduledCancellation)
            {
                MParams.AddOpt("ignore_scheduled_cancellation", ignoreScheduledCancellation);
                return this;
            }

            public RenewalEstimateRequest IgnoreScheduledChanges(bool ignoreScheduledChanges)
            {
                MParams.AddOpt("ignore_scheduled_changes", ignoreScheduledChanges);
                return this;
            }
        }

        public class AdvanceInvoiceEstimateRequest : EntityRequest<AdvanceInvoiceEstimateRequest>
        {
            public AdvanceInvoiceEstimateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AdvanceInvoiceEstimateRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public AdvanceInvoiceEstimateRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public AdvanceInvoiceEstimateRequest ScheduleType(ScheduleTypeEnum scheduleType)
            {
                MParams.AddOpt("schedule_type", scheduleType);
                return this;
            }

            public AdvanceInvoiceEstimateRequest FixedIntervalScheduleNumberOfOccurrences(
                int fixedIntervalScheduleNumberOfOccurrences)
            {
                MParams.AddOpt("fixed_interval_schedule[number_of_occurrences]",
                    fixedIntervalScheduleNumberOfOccurrences);
                return this;
            }

            public AdvanceInvoiceEstimateRequest FixedIntervalScheduleDaysBeforeRenewal(
                int fixedIntervalScheduleDaysBeforeRenewal)
            {
                MParams.AddOpt("fixed_interval_schedule[days_before_renewal]", fixedIntervalScheduleDaysBeforeRenewal);
                return this;
            }

            public AdvanceInvoiceEstimateRequest FixedIntervalScheduleEndScheduleOn(
                EndScheduleOnEnum fixedIntervalScheduleEndScheduleOn)
            {
                MParams.AddOpt("fixed_interval_schedule[end_schedule_on]", fixedIntervalScheduleEndScheduleOn);
                return this;
            }

            public AdvanceInvoiceEstimateRequest FixedIntervalScheduleEndDate(long fixedIntervalScheduleEndDate)
            {
                MParams.AddOpt("fixed_interval_schedule[end_date]", fixedIntervalScheduleEndDate);
                return this;
            }

            public AdvanceInvoiceEstimateRequest SpecificDatesScheduleTermsToCharge(int index,
                int specificDatesScheduleTermsToCharge)
            {
                MParams.AddOpt("specific_dates_schedule[terms_to_charge][" + index + "]",
                    specificDatesScheduleTermsToCharge);
                return this;
            }

            public AdvanceInvoiceEstimateRequest SpecificDatesScheduleDate(int index, long specificDatesScheduleDate)
            {
                MParams.AddOpt("specific_dates_schedule[date][" + index + "]", specificDatesScheduleDate);
                return this;
            }
        }

        public class RegenerateInvoiceEstimateRequest : EntityRequest<RegenerateInvoiceEstimateRequest>
        {
            public RegenerateInvoiceEstimateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RegenerateInvoiceEstimateRequest DateFrom(long dateFrom)
            {
                MParams.AddOpt("date_from", dateFrom);
                return this;
            }

            public RegenerateInvoiceEstimateRequest DateTo(long dateTo)
            {
                MParams.AddOpt("date_to", dateTo);
                return this;
            }

            public RegenerateInvoiceEstimateRequest Prorate(bool prorate)
            {
                MParams.AddOpt("prorate", prorate);
                return this;
            }

            public RegenerateInvoiceEstimateRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }
        }

        public class ChangeTermEndRequest : EntityRequest<ChangeTermEndRequest>
        {
            public ChangeTermEndRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ChangeTermEndRequest TermEndsAt(long termEndsAt)
            {
                MParams.AddOpt("term_ends_at", termEndsAt);
                return this;
            }

            public ChangeTermEndRequest Prorate(bool prorate)
            {
                MParams.AddOpt("prorate", prorate);
                return this;
            }

            public ChangeTermEndRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }
        }

        public class CancelSubscriptionRequest : EntityRequest<CancelSubscriptionRequest>
        {
            public CancelSubscriptionRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CancelSubscriptionRequest EndOfTerm(bool endOfTerm)
            {
                MParams.AddOpt("end_of_term", endOfTerm);
                return this;
            }

            public CancelSubscriptionRequest CancelAt(long cancelAt)
            {
                MParams.AddOpt("cancel_at", cancelAt);
                return this;
            }

            public CancelSubscriptionRequest CreditOptionForCurrentTermCharges(
                CreditOptionForCurrentTermChargesEnum creditOptionForCurrentTermCharges)
            {
                MParams.AddOpt("credit_option_for_current_term_charges", creditOptionForCurrentTermCharges);
                return this;
            }

            public CancelSubscriptionRequest UnbilledChargesOption(UnbilledChargesOptionEnum unbilledChargesOption)
            {
                MParams.AddOpt("unbilled_charges_option", unbilledChargesOption);
                return this;
            }

            public CancelSubscriptionRequest AccountReceivablesHandling(
                AccountReceivablesHandlingEnum accountReceivablesHandling)
            {
                MParams.AddOpt("account_receivables_handling", accountReceivablesHandling);
                return this;
            }

            public CancelSubscriptionRequest RefundableCreditsHandling(
                RefundableCreditsHandlingEnum refundableCreditsHandling)
            {
                MParams.AddOpt("refundable_credits_handling", refundableCreditsHandling);
                return this;
            }

            public CancelSubscriptionRequest ContractTermCancelOption(
                ContractTermCancelOptionEnum contractTermCancelOption)
            {
                MParams.AddOpt("contract_term_cancel_option", contractTermCancelOption);
                return this;
            }

            public CancelSubscriptionRequest CancelReasonCode(string cancelReasonCode)
            {
                MParams.AddOpt("cancel_reason_code", cancelReasonCode);
                return this;
            }

            public CancelSubscriptionRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public CancelSubscriptionRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public CancelSubscriptionRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public CancelSubscriptionRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }
        }

        public class CancelSubscriptionForItemsRequest : EntityRequest<CancelSubscriptionForItemsRequest>
        {
            public CancelSubscriptionForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CancelSubscriptionForItemsRequest EndOfTerm(bool endOfTerm)
            {
                MParams.AddOpt("end_of_term", endOfTerm);
                return this;
            }

            public CancelSubscriptionForItemsRequest CancelAt(long cancelAt)
            {
                MParams.AddOpt("cancel_at", cancelAt);
                return this;
            }

            public CancelSubscriptionForItemsRequest CreditOptionForCurrentTermCharges(
                CreditOptionForCurrentTermChargesEnum creditOptionForCurrentTermCharges)
            {
                MParams.AddOpt("credit_option_for_current_term_charges", creditOptionForCurrentTermCharges);
                return this;
            }

            public CancelSubscriptionForItemsRequest UnbilledChargesOption(
                UnbilledChargesOptionEnum unbilledChargesOption)
            {
                MParams.AddOpt("unbilled_charges_option", unbilledChargesOption);
                return this;
            }

            public CancelSubscriptionForItemsRequest AccountReceivablesHandling(
                AccountReceivablesHandlingEnum accountReceivablesHandling)
            {
                MParams.AddOpt("account_receivables_handling", accountReceivablesHandling);
                return this;
            }

            public CancelSubscriptionForItemsRequest RefundableCreditsHandling(
                RefundableCreditsHandlingEnum refundableCreditsHandling)
            {
                MParams.AddOpt("refundable_credits_handling", refundableCreditsHandling);
                return this;
            }

            public CancelSubscriptionForItemsRequest ContractTermCancelOption(
                ContractTermCancelOptionEnum contractTermCancelOption)
            {
                MParams.AddOpt("contract_term_cancel_option", contractTermCancelOption);
                return this;
            }

            public CancelSubscriptionForItemsRequest CancelReasonCode(string cancelReasonCode)
            {
                MParams.AddOpt("cancel_reason_code", cancelReasonCode);
                return this;
            }

            public CancelSubscriptionForItemsRequest SubscriptionItemItemPriceId(int index,
                string subscriptionItemItemPriceId)
            {
                MParams.AddOpt("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public CancelSubscriptionForItemsRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public CancelSubscriptionForItemsRequest SubscriptionItemUnitPrice(int index, int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public CancelSubscriptionForItemsRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }
        }

        public class PauseSubscriptionRequest : EntityRequest<PauseSubscriptionRequest>
        {
            public PauseSubscriptionRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public PauseSubscriptionRequest PauseOption(PauseOptionEnum pauseOption)
            {
                MParams.AddOpt("pause_option", pauseOption);
                return this;
            }

            public PauseSubscriptionRequest UnbilledChargesHandling(UnbilledChargesHandlingEnum unbilledChargesHandling)
            {
                MParams.AddOpt("unbilled_charges_handling", unbilledChargesHandling);
                return this;
            }

            public PauseSubscriptionRequest SubscriptionPauseDate(long subscriptionPauseDate)
            {
                MParams.AddOpt("subscription[pause_date]", subscriptionPauseDate);
                return this;
            }

            public PauseSubscriptionRequest SubscriptionResumeDate(long subscriptionResumeDate)
            {
                MParams.AddOpt("subscription[resume_date]", subscriptionResumeDate);
                return this;
            }
        }

        public class ResumeSubscriptionRequest : EntityRequest<ResumeSubscriptionRequest>
        {
            public ResumeSubscriptionRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ResumeSubscriptionRequest ResumeOption(ResumeOptionEnum resumeOption)
            {
                MParams.AddOpt("resume_option", resumeOption);
                return this;
            }

            public ResumeSubscriptionRequest ChargesHandling(ChargesHandlingEnum chargesHandling)
            {
                MParams.AddOpt("charges_handling", chargesHandling);
                return this;
            }

            public ResumeSubscriptionRequest SubscriptionResumeDate(long subscriptionResumeDate)
            {
                MParams.AddOpt("subscription[resume_date]", subscriptionResumeDate);
                return this;
            }
        }

        public class GiftSubscriptionRequest : EntityRequest<GiftSubscriptionRequest>
        {
            public GiftSubscriptionRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public GiftSubscriptionRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public GiftSubscriptionRequest GiftScheduledAt(long giftScheduledAt)
            {
                MParams.AddOpt("gift[scheduled_at]", giftScheduledAt);
                return this;
            }

            public GiftSubscriptionRequest GiftAutoClaim(bool giftAutoClaim)
            {
                MParams.AddOpt("gift[auto_claim]", giftAutoClaim);
                return this;
            }

            public GiftSubscriptionRequest GiftNoExpiry(bool giftNoExpiry)
            {
                MParams.AddOpt("gift[no_expiry]", giftNoExpiry);
                return this;
            }

            public GiftSubscriptionRequest GiftClaimExpiryDate(long giftClaimExpiryDate)
            {
                MParams.AddOpt("gift[claim_expiry_date]", giftClaimExpiryDate);
                return this;
            }

            public GiftSubscriptionRequest GifterCustomerId(string gifterCustomerId)
            {
                MParams.Add("gifter[customer_id]", gifterCustomerId);
                return this;
            }

            public GiftSubscriptionRequest GifterSignature(string gifterSignature)
            {
                MParams.Add("gifter[signature]", gifterSignature);
                return this;
            }

            public GiftSubscriptionRequest GifterNote(string gifterNote)
            {
                MParams.AddOpt("gifter[note]", gifterNote);
                return this;
            }

            public GiftSubscriptionRequest GifterPaymentSrcId(string gifterPaymentSrcId)
            {
                MParams.AddOpt("gifter[payment_src_id]", gifterPaymentSrcId);
                return this;
            }

            public GiftSubscriptionRequest GiftReceiverCustomerId(string giftReceiverCustomerId)
            {
                MParams.Add("gift_receiver[customer_id]", giftReceiverCustomerId);
                return this;
            }

            public GiftSubscriptionRequest GiftReceiverFirstName(string giftReceiverFirstName)
            {
                MParams.Add("gift_receiver[first_name]", giftReceiverFirstName);
                return this;
            }

            public GiftSubscriptionRequest GiftReceiverLastName(string giftReceiverLastName)
            {
                MParams.Add("gift_receiver[last_name]", giftReceiverLastName);
                return this;
            }

            public GiftSubscriptionRequest GiftReceiverEmail(string giftReceiverEmail)
            {
                MParams.Add("gift_receiver[email]", giftReceiverEmail);
                return this;
            }

            public GiftSubscriptionRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public GiftSubscriptionRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public GiftSubscriptionRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public GiftSubscriptionRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public GiftSubscriptionRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public GiftSubscriptionRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public GiftSubscriptionRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public GiftSubscriptionRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.Add("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public GiftSubscriptionRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public GiftSubscriptionRequest SubscriptionPlanQuantityInDecimal(string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public GiftSubscriptionRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public GiftSubscriptionRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public GiftSubscriptionRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }
        }

        public class GiftSubscriptionForItemsRequest : EntityRequest<GiftSubscriptionForItemsRequest>
        {
            public GiftSubscriptionForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public GiftSubscriptionForItemsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public GiftSubscriptionForItemsRequest GiftScheduledAt(long giftScheduledAt)
            {
                MParams.AddOpt("gift[scheduled_at]", giftScheduledAt);
                return this;
            }

            public GiftSubscriptionForItemsRequest GiftAutoClaim(bool giftAutoClaim)
            {
                MParams.AddOpt("gift[auto_claim]", giftAutoClaim);
                return this;
            }

            public GiftSubscriptionForItemsRequest GiftNoExpiry(bool giftNoExpiry)
            {
                MParams.AddOpt("gift[no_expiry]", giftNoExpiry);
                return this;
            }

            public GiftSubscriptionForItemsRequest GiftClaimExpiryDate(long giftClaimExpiryDate)
            {
                MParams.AddOpt("gift[claim_expiry_date]", giftClaimExpiryDate);
                return this;
            }

            public GiftSubscriptionForItemsRequest GifterCustomerId(string gifterCustomerId)
            {
                MParams.Add("gifter[customer_id]", gifterCustomerId);
                return this;
            }

            public GiftSubscriptionForItemsRequest GifterSignature(string gifterSignature)
            {
                MParams.Add("gifter[signature]", gifterSignature);
                return this;
            }

            public GiftSubscriptionForItemsRequest GifterNote(string gifterNote)
            {
                MParams.AddOpt("gifter[note]", gifterNote);
                return this;
            }

            public GiftSubscriptionForItemsRequest GifterPaymentSrcId(string gifterPaymentSrcId)
            {
                MParams.AddOpt("gifter[payment_src_id]", gifterPaymentSrcId);
                return this;
            }

            public GiftSubscriptionForItemsRequest GiftReceiverCustomerId(string giftReceiverCustomerId)
            {
                MParams.Add("gift_receiver[customer_id]", giftReceiverCustomerId);
                return this;
            }

            public GiftSubscriptionForItemsRequest GiftReceiverFirstName(string giftReceiverFirstName)
            {
                MParams.Add("gift_receiver[first_name]", giftReceiverFirstName);
                return this;
            }

            public GiftSubscriptionForItemsRequest GiftReceiverLastName(string giftReceiverLastName)
            {
                MParams.Add("gift_receiver[last_name]", giftReceiverLastName);
                return this;
            }

            public GiftSubscriptionForItemsRequest GiftReceiverEmail(string giftReceiverEmail)
            {
                MParams.Add("gift_receiver[email]", giftReceiverEmail);
                return this;
            }

            public GiftSubscriptionForItemsRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public GiftSubscriptionForItemsRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public GiftSubscriptionForItemsRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public GiftSubscriptionForItemsRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public GiftSubscriptionForItemsRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public GiftSubscriptionForItemsRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public GiftSubscriptionForItemsRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public GiftSubscriptionForItemsRequest SubscriptionItemItemPriceId(int index,
                string subscriptionItemItemPriceId)
            {
                MParams.AddOpt("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public GiftSubscriptionForItemsRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }
        }

        public class CreateInvoiceRequest : EntityRequest<CreateInvoiceRequest>
        {
            public CreateInvoiceRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateInvoiceRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateInvoiceRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            [Obsolete]
            public CreateInvoiceRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public CreateInvoiceRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateInvoiceRequest AuthorizationTransactionId(string authorizationTransactionId)
            {
                MParams.AddOpt("authorization_transaction_id", authorizationTransactionId);
                return this;
            }

            public CreateInvoiceRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public CreateInvoiceRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public CreateInvoiceRequest InvoiceCustomerId(string invoiceCustomerId)
            {
                MParams.AddOpt("invoice[customer_id]", invoiceCustomerId);
                return this;
            }

            public CreateInvoiceRequest InvoiceSubscriptionId(string invoiceSubscriptionId)
            {
                MParams.AddOpt("invoice[subscription_id]", invoiceSubscriptionId);
                return this;
            }

            public CreateInvoiceRequest InvoicePoNumber(string invoicePoNumber)
            {
                MParams.AddOpt("invoice[po_number]", invoicePoNumber);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateInvoiceRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateInvoiceRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CreateInvoiceRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CreateInvoiceRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CreateInvoiceRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CreateInvoiceRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CreateInvoiceRequest AddonDateFrom(int index, long addonDateFrom)
            {
                MParams.AddOpt("addons[date_from][" + index + "]", addonDateFrom);
                return this;
            }

            public CreateInvoiceRequest AddonDateTo(int index, long addonDateTo)
            {
                MParams.AddOpt("addons[date_to][" + index + "]", addonDateTo);
                return this;
            }

            public CreateInvoiceRequest ChargeAmount(int index, int chargeAmount)
            {
                MParams.AddOpt("charges[amount][" + index + "]", chargeAmount);
                return this;
            }

            public CreateInvoiceRequest ChargeAmountInDecimal(int index, string chargeAmountInDecimal)
            {
                MParams.AddOpt("charges[amount_in_decimal][" + index + "]", chargeAmountInDecimal);
                return this;
            }

            public CreateInvoiceRequest ChargeDescription(int index, string chargeDescription)
            {
                MParams.AddOpt("charges[description][" + index + "]", chargeDescription);
                return this;
            }

            public CreateInvoiceRequest ChargeAvalaraSaleType(int index, AvalaraSaleTypeEnum chargeAvalaraSaleType)
            {
                MParams.AddOpt("charges[avalara_sale_type][" + index + "]", chargeAvalaraSaleType);
                return this;
            }

            public CreateInvoiceRequest ChargeAvalaraTransactionType(int index, int chargeAvalaraTransactionType)
            {
                MParams.AddOpt("charges[avalara_transaction_type][" + index + "]", chargeAvalaraTransactionType);
                return this;
            }

            public CreateInvoiceRequest ChargeAvalaraServiceType(int index, int chargeAvalaraServiceType)
            {
                MParams.AddOpt("charges[avalara_service_type][" + index + "]", chargeAvalaraServiceType);
                return this;
            }

            public CreateInvoiceRequest ChargeDateFrom(int index, long chargeDateFrom)
            {
                MParams.AddOpt("charges[date_from][" + index + "]", chargeDateFrom);
                return this;
            }

            public CreateInvoiceRequest ChargeDateTo(int index, long chargeDateTo)
            {
                MParams.AddOpt("charges[date_to][" + index + "]", chargeDateTo);
                return this;
            }

            public CreateInvoiceRequest ChargeTaxable(int index, bool chargeTaxable)
            {
                MParams.AddOpt("charges[taxable][" + index + "]", chargeTaxable);
                return this;
            }

            public CreateInvoiceRequest ChargeTaxProfileId(int index, string chargeTaxProfileId)
            {
                MParams.AddOpt("charges[tax_profile_id][" + index + "]", chargeTaxProfileId);
                return this;
            }

            public CreateInvoiceRequest ChargeAvalaraTaxCode(int index, string chargeAvalaraTaxCode)
            {
                MParams.AddOpt("charges[avalara_tax_code][" + index + "]", chargeAvalaraTaxCode);
                return this;
            }

            public CreateInvoiceRequest ChargeTaxjarProductCode(int index, string chargeTaxjarProductCode)
            {
                MParams.AddOpt("charges[taxjar_product_code][" + index + "]", chargeTaxjarProductCode);
                return this;
            }
        }

        public class CreateInvoiceForItemsRequest : EntityRequest<CreateInvoiceForItemsRequest>
        {
            public CreateInvoiceForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateInvoiceForItemsRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateInvoiceForItemsRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public CreateInvoiceForItemsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateInvoiceForItemsRequest AuthorizationTransactionId(string authorizationTransactionId)
            {
                MParams.AddOpt("authorization_transaction_id", authorizationTransactionId);
                return this;
            }

            public CreateInvoiceForItemsRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public CreateInvoiceForItemsRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public CreateInvoiceForItemsRequest InvoiceCustomerId(string invoiceCustomerId)
            {
                MParams.Add("invoice[customer_id]", invoiceCustomerId);
                return this;
            }

            public CreateInvoiceForItemsRequest InvoiceSubscriptionId(string invoiceSubscriptionId)
            {
                MParams.AddOpt("invoice[subscription_id]", invoiceSubscriptionId);
                return this;
            }

            public CreateInvoiceForItemsRequest InvoicePoNumber(string invoicePoNumber)
            {
                MParams.AddOpt("invoice[po_number]", invoicePoNumber);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateInvoiceForItemsRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateInvoiceForItemsRequest ItemPriceItemPriceId(int index, string itemPriceItemPriceId)
            {
                MParams.AddOpt("item_prices[item_price_id][" + index + "]", itemPriceItemPriceId);
                return this;
            }

            public CreateInvoiceForItemsRequest ItemPriceQuantity(int index, int itemPriceQuantity)
            {
                MParams.AddOpt("item_prices[quantity][" + index + "]", itemPriceQuantity);
                return this;
            }

            public CreateInvoiceForItemsRequest ItemPriceUnitPrice(int index, int itemPriceUnitPrice)
            {
                MParams.AddOpt("item_prices[unit_price][" + index + "]", itemPriceUnitPrice);
                return this;
            }

            public CreateInvoiceForItemsRequest ItemPriceDateFrom(int index, long itemPriceDateFrom)
            {
                MParams.AddOpt("item_prices[date_from][" + index + "]", itemPriceDateFrom);
                return this;
            }

            public CreateInvoiceForItemsRequest ItemPriceDateTo(int index, long itemPriceDateTo)
            {
                MParams.AddOpt("item_prices[date_to][" + index + "]", itemPriceDateTo);
                return this;
            }

            public CreateInvoiceForItemsRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public CreateInvoiceForItemsRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CreateInvoiceForItemsRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CreateInvoiceForItemsRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }

            public CreateInvoiceForItemsRequest ChargeAmount(int index, int chargeAmount)
            {
                MParams.AddOpt("charges[amount][" + index + "]", chargeAmount);
                return this;
            }

            public CreateInvoiceForItemsRequest ChargeAmountInDecimal(int index, string chargeAmountInDecimal)
            {
                MParams.AddOpt("charges[amount_in_decimal][" + index + "]", chargeAmountInDecimal);
                return this;
            }

            public CreateInvoiceForItemsRequest ChargeDescription(int index, string chargeDescription)
            {
                MParams.AddOpt("charges[description][" + index + "]", chargeDescription);
                return this;
            }

            public CreateInvoiceForItemsRequest ChargeAvalaraSaleType(int index,
                AvalaraSaleTypeEnum chargeAvalaraSaleType)
            {
                MParams.AddOpt("charges[avalara_sale_type][" + index + "]", chargeAvalaraSaleType);
                return this;
            }

            public CreateInvoiceForItemsRequest ChargeAvalaraTransactionType(int index,
                int chargeAvalaraTransactionType)
            {
                MParams.AddOpt("charges[avalara_transaction_type][" + index + "]", chargeAvalaraTransactionType);
                return this;
            }

            public CreateInvoiceForItemsRequest ChargeAvalaraServiceType(int index, int chargeAvalaraServiceType)
            {
                MParams.AddOpt("charges[avalara_service_type][" + index + "]", chargeAvalaraServiceType);
                return this;
            }

            public CreateInvoiceForItemsRequest ChargeDateFrom(int index, long chargeDateFrom)
            {
                MParams.AddOpt("charges[date_from][" + index + "]", chargeDateFrom);
                return this;
            }

            public CreateInvoiceForItemsRequest ChargeDateTo(int index, long chargeDateTo)
            {
                MParams.AddOpt("charges[date_to][" + index + "]", chargeDateTo);
                return this;
            }
        }

        #endregion


        #region Subclasses

        #endregion
    }
}