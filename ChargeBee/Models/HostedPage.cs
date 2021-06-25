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
    public class HostedPage : Resource
    {
        [Obsolete]
        public enum FailureReasonEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "card_error")] CardError,
            [EnumMember(Value = "server_error")] ServerError
        }

        public enum StateEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "created")] Created,
            [EnumMember(Value = "requested")] Requested,
            [EnumMember(Value = "succeeded")] Succeeded,
            [EnumMember(Value = "cancelled")] Cancelled,

            [EnumMember(Value = "failed")] [Obsolete]
            Failed,
            [EnumMember(Value = "acknowledged")] Acknowledged
        }

        public enum TypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "checkout_new")] CheckoutNew,

            [EnumMember(Value = "checkout_existing")]
            CheckoutExisting,

            [EnumMember(Value = "update_card")] [Obsolete]
            UpdateCard,

            [EnumMember(Value = "update_payment_method")]
            UpdatePaymentMethod,

            [EnumMember(Value = "manage_payment_sources")]
            ManagePaymentSources,
            [EnumMember(Value = "collect_now")] CollectNow,

            [EnumMember(Value = "extend_subscription")]
            ExtendSubscription,
            [EnumMember(Value = "checkout_gift")] CheckoutGift,
            [EnumMember(Value = "claim_gift")] ClaimGift,

            [EnumMember(Value = "checkout_one_time")]
            CheckoutOneTime
        }

        public HostedPage()
        {
        }

        public HostedPage(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public HostedPage(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public HostedPage(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Subclasses

        public class HostedPageContent : ResultBase
        {
            public HostedPageContent()
            {
            }

            internal HostedPageContent(JToken jobj)
            {
                MJobj = jobj;
            }
        }

        #endregion

        #region Methods

        public static CheckoutNewRequest CheckoutNew()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "checkout_new");
            return new CheckoutNewRequest(url, HttpMethod.Post);
        }

        public static CheckoutOneTimeRequest CheckoutOneTime()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "checkout_one_time");
            return new CheckoutOneTimeRequest(url, HttpMethod.Post);
        }

        public static CheckoutNewForItemsRequest CheckoutNewForItems()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "checkout_new_for_items");
            return new CheckoutNewForItemsRequest(url, HttpMethod.Post);
        }

        public static CheckoutExistingRequest CheckoutExisting()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "checkout_existing");
            return new CheckoutExistingRequest(url, HttpMethod.Post);
        }

        public static CheckoutExistingForItemsRequest CheckoutExistingForItems()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "checkout_existing_for_items");
            return new CheckoutExistingForItemsRequest(url, HttpMethod.Post);
        }

        [Obsolete]
        public static UpdateCardRequest UpdateCard()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "update_card");
            return new UpdateCardRequest(url, HttpMethod.Post);
        }

        public static UpdatePaymentMethodRequest UpdatePaymentMethod()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "update_payment_method");
            return new UpdatePaymentMethodRequest(url, HttpMethod.Post);
        }

        public static ManagePaymentSourcesRequest ManagePaymentSources()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "manage_payment_sources");
            return new ManagePaymentSourcesRequest(url, HttpMethod.Post);
        }

        public static CollectNowRequest CollectNow()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "collect_now");
            return new CollectNowRequest(url, HttpMethod.Post);
        }

        public static AcceptQuoteRequest AcceptQuote()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "accept_quote");
            return new AcceptQuoteRequest(url, HttpMethod.Post);
        }

        public static ExtendSubscriptionRequest ExtendSubscription()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "extend_subscription");
            return new ExtendSubscriptionRequest(url, HttpMethod.Post);
        }

        public static CheckoutGiftRequest CheckoutGift()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "checkout_gift");
            return new CheckoutGiftRequest(url, HttpMethod.Post);
        }

        public static ClaimGiftRequest ClaimGift()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "claim_gift");
            return new ClaimGiftRequest(url, HttpMethod.Post);
        }

        public static RetrieveAgreementPdfRequest RetrieveAgreementPdf()
        {
            var url = ApiUtil.BuildUrl("hosted_pages", "retrieve_agreement_pdf");
            return new RetrieveAgreementPdfRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Acknowledge(string id)
        {
            var url = ApiUtil.BuildUrl("hosted_pages", CheckNull(id), "acknowledge");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("hosted_pages", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static HostedPageListRequest List()
        {
            var url = ApiUtil.BuildUrl("hosted_pages");
            return new HostedPageListRequest(url);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id", false);

        public TypeEnum? HostedPageType => GetEnum<TypeEnum>("type", false);

        public string Url => GetValue<string>("url", false);

        public StateEnum? State => GetEnum<StateEnum>("state", false);

        [Obsolete] public FailureReasonEnum? FailureReason => GetEnum<FailureReasonEnum>("failure_reason", false);

        public string PassThruContent => GetValue<string>("pass_thru_content", false);

        public bool Embed => GetValue<bool>("embed");

        public DateTime? CreatedAt => GetDateTime("created_at", false);

        public DateTime? ExpiresAt => GetDateTime("expires_at", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public JToken CheckoutInfo => GetJToken("checkout_info", false);

        public HostedPageContent Content
        {
            get
            {
                if (GetValue<JToken>("content", false) == null) return null;
                return new HostedPageContent(GetValue<JToken>("content"));
            }
        }

        #endregion

        #region Requests

        public class CheckoutNewRequest : EntityRequest<CheckoutNewRequest>
        {
            public CheckoutNewRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CheckoutNewRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CheckoutNewRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public CheckoutNewRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CheckoutNewRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CheckoutNewRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CheckoutNewRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public CheckoutNewRequest CancelUrl(string cancelUrl)
            {
                MParams.AddOpt("cancel_url", cancelUrl);
                return this;
            }

            public CheckoutNewRequest PassThruContent(string passThruContent)
            {
                MParams.AddOpt("pass_thru_content", passThruContent);
                return this;
            }

            public CheckoutNewRequest Embed(bool embed)
            {
                MParams.AddOpt("embed", embed);
                return this;
            }

            public CheckoutNewRequest IframeMessaging(bool iframeMessaging)
            {
                MParams.AddOpt("iframe_messaging", iframeMessaging);
                return this;
            }

            public CheckoutNewRequest AllowOfflinePaymentMethods(bool allowOfflinePaymentMethods)
            {
                MParams.AddOpt("allow_offline_payment_methods", allowOfflinePaymentMethods);
                return this;
            }

            public CheckoutNewRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription[id]", subscriptionId);
                return this;
            }

            public CheckoutNewRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer[id]", customerId);
                return this;
            }

            public CheckoutNewRequest CustomerEmail(string customerEmail)
            {
                MParams.AddOpt("customer[email]", customerEmail);
                return this;
            }

            public CheckoutNewRequest CustomerFirstName(string customerFirstName)
            {
                MParams.AddOpt("customer[first_name]", customerFirstName);
                return this;
            }

            public CheckoutNewRequest CustomerLastName(string customerLastName)
            {
                MParams.AddOpt("customer[last_name]", customerLastName);
                return this;
            }

            public CheckoutNewRequest CustomerCompany(string customerCompany)
            {
                MParams.AddOpt("customer[company]", customerCompany);
                return this;
            }

            public CheckoutNewRequest CustomerTaxability(TaxabilityEnum customerTaxability)
            {
                MParams.AddOpt("customer[taxability]", customerTaxability);
                return this;
            }

            public CheckoutNewRequest CustomerLocale(string customerLocale)
            {
                MParams.AddOpt("customer[locale]", customerLocale);
                return this;
            }

            public CheckoutNewRequest CustomerPhone(string customerPhone)
            {
                MParams.AddOpt("customer[phone]", customerPhone);
                return this;
            }

            public CheckoutNewRequest SubscriptionPlanUnitPriceInDecimal(string subscriptionPlanUnitPriceInDecimal)
            {
                MParams.AddOpt("subscription[plan_unit_price_in_decimal]", subscriptionPlanUnitPriceInDecimal);
                return this;
            }

            public CheckoutNewRequest SubscriptionPlanQuantityInDecimal(string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public CheckoutNewRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.Add("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public CheckoutNewRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public CheckoutNewRequest SubscriptionPlanUnitPrice(int subscriptionPlanUnitPrice)
            {
                MParams.AddOpt("subscription[plan_unit_price]", subscriptionPlanUnitPrice);
                return this;
            }

            public CheckoutNewRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public CheckoutNewRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            public CheckoutNewRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            [Obsolete]
            public CheckoutNewRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public CheckoutNewRequest SubscriptionAutoCollection(AutoCollectionEnum subscriptionAutoCollection)
            {
                MParams.AddOpt("subscription[auto_collection]", subscriptionAutoCollection);
                return this;
            }

            public CheckoutNewRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public CheckoutNewRequest SubscriptionInvoiceNotes(string subscriptionInvoiceNotes)
            {
                MParams.AddOpt("subscription[invoice_notes]", subscriptionInvoiceNotes);
                return this;
            }

            [Obsolete]
            public CheckoutNewRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public CheckoutNewRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            public CheckoutNewRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public CheckoutNewRequest CustomerConsolidatedInvoicing(bool customerConsolidatedInvoicing)
            {
                MParams.AddOpt("customer[consolidated_invoicing]", customerConsolidatedInvoicing);
                return this;
            }

            public CheckoutNewRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public CheckoutNewRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public CheckoutNewRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public CheckoutNewRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public CheckoutNewRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public CheckoutNewRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public CheckoutNewRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public CheckoutNewRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public CheckoutNewRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public CheckoutNewRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public CheckoutNewRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public CheckoutNewRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public CheckoutNewRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public CheckoutNewRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public CheckoutNewRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CheckoutNewRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CheckoutNewRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CheckoutNewRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CheckoutNewRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CheckoutNewRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CheckoutNewRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CheckoutNewRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CheckoutNewRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CheckoutNewRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CheckoutNewRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CheckoutNewRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CheckoutNewRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CheckoutNewRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CheckoutNewRequest SubscriptionAffiliateToken(string subscriptionAffiliateToken)
            {
                MParams.AddOpt("subscription[affiliate_token]", subscriptionAffiliateToken);
                return this;
            }

            public CheckoutNewRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CheckoutNewRequest ContractTermCancellationCutoffPeriod(int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CheckoutNewRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public CheckoutNewRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CheckoutNewRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CheckoutNewRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CheckoutNewRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CheckoutNewRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CheckoutNewRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public CheckoutNewRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public CheckoutNewRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public CheckoutNewRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public CheckoutNewRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public CheckoutNewRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public CheckoutNewRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public CheckoutNewRequest EventBasedAddonOnEvent(int index, OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public CheckoutNewRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public CheckoutNewRequest EventBasedAddonChargeOn(int index, ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }
        }

        public class CheckoutOneTimeRequest : EntityRequest<CheckoutOneTimeRequest>
        {
            public CheckoutOneTimeRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CheckoutOneTimeRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CheckoutOneTimeRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CheckoutOneTimeRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public CheckoutOneTimeRequest CancelUrl(string cancelUrl)
            {
                MParams.AddOpt("cancel_url", cancelUrl);
                return this;
            }

            public CheckoutOneTimeRequest PassThruContent(string passThruContent)
            {
                MParams.AddOpt("pass_thru_content", passThruContent);
                return this;
            }

            public CheckoutOneTimeRequest Embed(bool embed)
            {
                MParams.AddOpt("embed", embed);
                return this;
            }

            public CheckoutOneTimeRequest IframeMessaging(bool iframeMessaging)
            {
                MParams.AddOpt("iframe_messaging", iframeMessaging);
                return this;
            }

            public CheckoutOneTimeRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer[id]", customerId);
                return this;
            }

            public CheckoutOneTimeRequest CustomerEmail(string customerEmail)
            {
                MParams.AddOpt("customer[email]", customerEmail);
                return this;
            }

            public CheckoutOneTimeRequest CustomerFirstName(string customerFirstName)
            {
                MParams.AddOpt("customer[first_name]", customerFirstName);
                return this;
            }

            public CheckoutOneTimeRequest CustomerLastName(string customerLastName)
            {
                MParams.AddOpt("customer[last_name]", customerLastName);
                return this;
            }

            public CheckoutOneTimeRequest CustomerCompany(string customerCompany)
            {
                MParams.AddOpt("customer[company]", customerCompany);
                return this;
            }

            public CheckoutOneTimeRequest CustomerTaxability(TaxabilityEnum customerTaxability)
            {
                MParams.AddOpt("customer[taxability]", customerTaxability);
                return this;
            }

            public CheckoutOneTimeRequest CustomerLocale(string customerLocale)
            {
                MParams.AddOpt("customer[locale]", customerLocale);
                return this;
            }

            public CheckoutOneTimeRequest CustomerPhone(string customerPhone)
            {
                MParams.AddOpt("customer[phone]", customerPhone);
                return this;
            }

            public CheckoutOneTimeRequest InvoicePoNumber(string invoicePoNumber)
            {
                MParams.AddOpt("invoice[po_number]", invoicePoNumber);
                return this;
            }

            [Obsolete]
            public CheckoutOneTimeRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public CheckoutOneTimeRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            public CheckoutOneTimeRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public CheckoutOneTimeRequest CustomerConsolidatedInvoicing(bool customerConsolidatedInvoicing)
            {
                MParams.AddOpt("customer[consolidated_invoicing]", customerConsolidatedInvoicing);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public CheckoutOneTimeRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CheckoutOneTimeRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CheckoutOneTimeRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CheckoutOneTimeRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CheckoutOneTimeRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CheckoutOneTimeRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CheckoutOneTimeRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CheckoutOneTimeRequest AddonDateFrom(int index, long addonDateFrom)
            {
                MParams.AddOpt("addons[date_from][" + index + "]", addonDateFrom);
                return this;
            }

            public CheckoutOneTimeRequest AddonDateTo(int index, long addonDateTo)
            {
                MParams.AddOpt("addons[date_to][" + index + "]", addonDateTo);
                return this;
            }

            public CheckoutOneTimeRequest ChargeAmount(int index, int chargeAmount)
            {
                MParams.AddOpt("charges[amount][" + index + "]", chargeAmount);
                return this;
            }

            public CheckoutOneTimeRequest ChargeAmountInDecimal(int index, string chargeAmountInDecimal)
            {
                MParams.AddOpt("charges[amount_in_decimal][" + index + "]", chargeAmountInDecimal);
                return this;
            }

            public CheckoutOneTimeRequest ChargeDescription(int index, string chargeDescription)
            {
                MParams.AddOpt("charges[description][" + index + "]", chargeDescription);
                return this;
            }

            public CheckoutOneTimeRequest ChargeAvalaraSaleType(int index, AvalaraSaleTypeEnum chargeAvalaraSaleType)
            {
                MParams.AddOpt("charges[avalara_sale_type][" + index + "]", chargeAvalaraSaleType);
                return this;
            }

            public CheckoutOneTimeRequest ChargeAvalaraTransactionType(int index, int chargeAvalaraTransactionType)
            {
                MParams.AddOpt("charges[avalara_transaction_type][" + index + "]", chargeAvalaraTransactionType);
                return this;
            }

            public CheckoutOneTimeRequest ChargeAvalaraServiceType(int index, int chargeAvalaraServiceType)
            {
                MParams.AddOpt("charges[avalara_service_type][" + index + "]", chargeAvalaraServiceType);
                return this;
            }

            public CheckoutOneTimeRequest ChargeDateFrom(int index, long chargeDateFrom)
            {
                MParams.AddOpt("charges[date_from][" + index + "]", chargeDateFrom);
                return this;
            }

            public CheckoutOneTimeRequest ChargeDateTo(int index, long chargeDateTo)
            {
                MParams.AddOpt("charges[date_to][" + index + "]", chargeDateTo);
                return this;
            }
        }

        public class CheckoutNewForItemsRequest : EntityRequest<CheckoutNewForItemsRequest>
        {
            public CheckoutNewForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CheckoutNewForItemsRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CheckoutNewForItemsRequest MandatoryItemsToRemove(List<string> mandatoryItemsToRemove)
            {
                MParams.AddOpt("mandatory_items_to_remove", mandatoryItemsToRemove);
                return this;
            }

            public CheckoutNewForItemsRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CheckoutNewForItemsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CheckoutNewForItemsRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public CheckoutNewForItemsRequest CancelUrl(string cancelUrl)
            {
                MParams.AddOpt("cancel_url", cancelUrl);
                return this;
            }

            public CheckoutNewForItemsRequest PassThruContent(string passThruContent)
            {
                MParams.AddOpt("pass_thru_content", passThruContent);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription[id]", subscriptionId);
                return this;
            }

            public CheckoutNewForItemsRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer[id]", customerId);
                return this;
            }

            public CheckoutNewForItemsRequest CustomerEmail(string customerEmail)
            {
                MParams.AddOpt("customer[email]", customerEmail);
                return this;
            }

            public CheckoutNewForItemsRequest CustomerFirstName(string customerFirstName)
            {
                MParams.AddOpt("customer[first_name]", customerFirstName);
                return this;
            }

            public CheckoutNewForItemsRequest CustomerLastName(string customerLastName)
            {
                MParams.AddOpt("customer[last_name]", customerLastName);
                return this;
            }

            public CheckoutNewForItemsRequest CustomerCompany(string customerCompany)
            {
                MParams.AddOpt("customer[company]", customerCompany);
                return this;
            }

            public CheckoutNewForItemsRequest CustomerTaxability(TaxabilityEnum customerTaxability)
            {
                MParams.AddOpt("customer[taxability]", customerTaxability);
                return this;
            }

            public CheckoutNewForItemsRequest CustomerLocale(string customerLocale)
            {
                MParams.AddOpt("customer[locale]", customerLocale);
                return this;
            }

            public CheckoutNewForItemsRequest CustomerPhone(string customerPhone)
            {
                MParams.AddOpt("customer[phone]", customerPhone);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public CheckoutNewForItemsRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionAutoCollection(AutoCollectionEnum subscriptionAutoCollection)
            {
                MParams.AddOpt("subscription[auto_collection]", subscriptionAutoCollection);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionInvoiceNotes(string subscriptionInvoiceNotes)
            {
                MParams.AddOpt("subscription[invoice_notes]", subscriptionInvoiceNotes);
                return this;
            }

            [Obsolete]
            public CheckoutNewForItemsRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public CheckoutNewForItemsRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            public CheckoutNewForItemsRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public CheckoutNewForItemsRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CheckoutNewForItemsRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionAffiliateToken(string subscriptionAffiliateToken)
            {
                MParams.AddOpt("subscription[affiliate_token]", subscriptionAffiliateToken);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionItemItemPriceId(int index, string subscriptionItemItemPriceId)
            {
                MParams.Add("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionItemUnitPrice(int index, int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionItemBillingCycles(int index,
                int subscriptionItemBillingCycles)
            {
                MParams.AddOpt("subscription_items[billing_cycles][" + index + "]", subscriptionItemBillingCycles);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionItemTrialEnd(int index, long subscriptionItemTrialEnd)
            {
                MParams.AddOpt("subscription_items[trial_end][" + index + "]", subscriptionItemTrialEnd);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionItemChargeOnEvent(int index,
                ChargeOnEventEnum subscriptionItemChargeOnEvent)
            {
                MParams.AddOpt("subscription_items[charge_on_event][" + index + "]", subscriptionItemChargeOnEvent);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionItemChargeOnce(int index, bool subscriptionItemChargeOnce)
            {
                MParams.AddOpt("subscription_items[charge_once][" + index + "]", subscriptionItemChargeOnce);
                return this;
            }

            [Obsolete]
            public CheckoutNewForItemsRequest SubscriptionItemItemType(int index, ItemTypeEnum subscriptionItemItemType)
            {
                MParams.AddOpt("subscription_items[item_type][" + index + "]", subscriptionItemItemType);
                return this;
            }

            public CheckoutNewForItemsRequest SubscriptionItemChargeOnOption(int index,
                ChargeOnOptionEnum subscriptionItemChargeOnOption)
            {
                MParams.AddOpt("subscription_items[charge_on_option][" + index + "]", subscriptionItemChargeOnOption);
                return this;
            }

            public CheckoutNewForItemsRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public CheckoutNewForItemsRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CheckoutNewForItemsRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CheckoutNewForItemsRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class CheckoutExistingRequest : EntityRequest<CheckoutExistingRequest>
        {
            public CheckoutExistingRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CheckoutExistingRequest ReplaceAddonList(bool replaceAddonList)
            {
                MParams.AddOpt("replace_addon_list", replaceAddonList);
                return this;
            }

            public CheckoutExistingRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public CheckoutExistingRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CheckoutExistingRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CheckoutExistingRequest ReactivateFrom(long reactivateFrom)
            {
                MParams.AddOpt("reactivate_from", reactivateFrom);
                return this;
            }

            public CheckoutExistingRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CheckoutExistingRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CheckoutExistingRequest Reactivate(bool reactivate)
            {
                MParams.AddOpt("reactivate", reactivate);
                return this;
            }

            public CheckoutExistingRequest ForceTermReset(bool forceTermReset)
            {
                MParams.AddOpt("force_term_reset", forceTermReset);
                return this;
            }

            public CheckoutExistingRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public CheckoutExistingRequest CancelUrl(string cancelUrl)
            {
                MParams.AddOpt("cancel_url", cancelUrl);
                return this;
            }

            public CheckoutExistingRequest PassThruContent(string passThruContent)
            {
                MParams.AddOpt("pass_thru_content", passThruContent);
                return this;
            }

            public CheckoutExistingRequest Embed(bool embed)
            {
                MParams.AddOpt("embed", embed);
                return this;
            }

            public CheckoutExistingRequest IframeMessaging(bool iframeMessaging)
            {
                MParams.AddOpt("iframe_messaging", iframeMessaging);
                return this;
            }

            public CheckoutExistingRequest AllowOfflinePaymentMethods(bool allowOfflinePaymentMethods)
            {
                MParams.AddOpt("allow_offline_payment_methods", allowOfflinePaymentMethods);
                return this;
            }

            public CheckoutExistingRequest SubscriptionId(string subscriptionId)
            {
                MParams.Add("subscription[id]", subscriptionId);
                return this;
            }

            public CheckoutExistingRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.AddOpt("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public CheckoutExistingRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public CheckoutExistingRequest SubscriptionPlanUnitPrice(int subscriptionPlanUnitPrice)
            {
                MParams.AddOpt("subscription[plan_unit_price]", subscriptionPlanUnitPrice);
                return this;
            }

            public CheckoutExistingRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public CheckoutExistingRequest SubscriptionPlanQuantityInDecimal(string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public CheckoutExistingRequest SubscriptionPlanUnitPriceInDecimal(string subscriptionPlanUnitPriceInDecimal)
            {
                MParams.AddOpt("subscription[plan_unit_price_in_decimal]", subscriptionPlanUnitPriceInDecimal);
                return this;
            }

            public CheckoutExistingRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public CheckoutExistingRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public CheckoutExistingRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public CheckoutExistingRequest SubscriptionAutoCollection(AutoCollectionEnum subscriptionAutoCollection)
            {
                MParams.AddOpt("subscription[auto_collection]", subscriptionAutoCollection);
                return this;
            }

            public CheckoutExistingRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public CheckoutExistingRequest SubscriptionInvoiceNotes(string subscriptionInvoiceNotes)
            {
                MParams.AddOpt("subscription[invoice_notes]", subscriptionInvoiceNotes);
                return this;
            }

            public CheckoutExistingRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            [Obsolete]
            public CheckoutExistingRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public CheckoutExistingRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            public CheckoutExistingRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CheckoutExistingRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CheckoutExistingRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public CheckoutExistingRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CheckoutExistingRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CheckoutExistingRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CheckoutExistingRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public CheckoutExistingRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public CheckoutExistingRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public CheckoutExistingRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public CheckoutExistingRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public CheckoutExistingRequest EventBasedAddonChargeOn(int index, ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }

            public CheckoutExistingRequest EventBasedAddonOnEvent(int index, OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public CheckoutExistingRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public CheckoutExistingRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CheckoutExistingRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CheckoutExistingRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public CheckoutExistingRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }
        }

        public class CheckoutExistingForItemsRequest : EntityRequest<CheckoutExistingForItemsRequest>
        {
            public CheckoutExistingForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CheckoutExistingForItemsRequest MandatoryItemsToRemove(List<string> mandatoryItemsToRemove)
            {
                MParams.AddOpt("mandatory_items_to_remove", mandatoryItemsToRemove);
                return this;
            }

            public CheckoutExistingForItemsRequest ReplaceItemsList(bool replaceItemsList)
            {
                MParams.AddOpt("replace_items_list", replaceItemsList);
                return this;
            }

            public CheckoutExistingForItemsRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CheckoutExistingForItemsRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CheckoutExistingForItemsRequest ReactivateFrom(long reactivateFrom)
            {
                MParams.AddOpt("reactivate_from", reactivateFrom);
                return this;
            }

            public CheckoutExistingForItemsRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CheckoutExistingForItemsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CheckoutExistingForItemsRequest Reactivate(bool reactivate)
            {
                MParams.AddOpt("reactivate", reactivate);
                return this;
            }

            public CheckoutExistingForItemsRequest ForceTermReset(bool forceTermReset)
            {
                MParams.AddOpt("force_term_reset", forceTermReset);
                return this;
            }

            public CheckoutExistingForItemsRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public CheckoutExistingForItemsRequest CancelUrl(string cancelUrl)
            {
                MParams.AddOpt("cancel_url", cancelUrl);
                return this;
            }

            public CheckoutExistingForItemsRequest PassThruContent(string passThruContent)
            {
                MParams.AddOpt("pass_thru_content", passThruContent);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionId(string subscriptionId)
            {
                MParams.Add("subscription[id]", subscriptionId);
                return this;
            }

            [Obsolete]
            public CheckoutExistingForItemsRequest SubscriptionSetupFee(int subscriptionSetupFee)
            {
                MParams.AddOpt("subscription[setup_fee]", subscriptionSetupFee);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionStartDate(long subscriptionStartDate)
            {
                MParams.AddOpt("subscription[start_date]", subscriptionStartDate);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionTrialEnd(long subscriptionTrialEnd)
            {
                MParams.AddOpt("subscription[trial_end]", subscriptionTrialEnd);
                return this;
            }

            [Obsolete]
            public CheckoutExistingForItemsRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionAutoCollection(
                AutoCollectionEnum subscriptionAutoCollection)
            {
                MParams.AddOpt("subscription[auto_collection]", subscriptionAutoCollection);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionOfflinePaymentMethod(
                OfflinePaymentMethodEnum subscriptionOfflinePaymentMethod)
            {
                MParams.AddOpt("subscription[offline_payment_method]", subscriptionOfflinePaymentMethod);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionInvoiceNotes(string subscriptionInvoiceNotes)
            {
                MParams.AddOpt("subscription[invoice_notes]", subscriptionInvoiceNotes);
                return this;
            }

            public CheckoutExistingForItemsRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            [Obsolete]
            public CheckoutExistingForItemsRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public CheckoutExistingForItemsRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            public CheckoutExistingForItemsRequest ContractTermActionAtTermEnd(
                ContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CheckoutExistingForItemsRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionContractTermBillingCycleOnRenewal(
                int subscriptionContractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("subscription[contract_term_billing_cycle_on_renewal]",
                    subscriptionContractTermBillingCycleOnRenewal);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionItemItemPriceId(int index,
                string subscriptionItemItemPriceId)
            {
                MParams.Add("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionItemUnitPrice(int index, int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionItemBillingCycles(int index,
                int subscriptionItemBillingCycles)
            {
                MParams.AddOpt("subscription_items[billing_cycles][" + index + "]", subscriptionItemBillingCycles);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionItemTrialEnd(int index, long subscriptionItemTrialEnd)
            {
                MParams.AddOpt("subscription_items[trial_end][" + index + "]", subscriptionItemTrialEnd);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionItemChargeOnEvent(int index,
                ChargeOnEventEnum subscriptionItemChargeOnEvent)
            {
                MParams.AddOpt("subscription_items[charge_on_event][" + index + "]", subscriptionItemChargeOnEvent);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionItemChargeOnce(int index,
                bool subscriptionItemChargeOnce)
            {
                MParams.AddOpt("subscription_items[charge_once][" + index + "]", subscriptionItemChargeOnce);
                return this;
            }

            public CheckoutExistingForItemsRequest SubscriptionItemChargeOnOption(int index,
                ChargeOnOptionEnum subscriptionItemChargeOnOption)
            {
                MParams.AddOpt("subscription_items[charge_on_option][" + index + "]", subscriptionItemChargeOnOption);
                return this;
            }

            [Obsolete]
            public CheckoutExistingForItemsRequest SubscriptionItemItemType(int index,
                ItemTypeEnum subscriptionItemItemType)
            {
                MParams.AddOpt("subscription_items[item_type][" + index + "]", subscriptionItemItemType);
                return this;
            }

            public CheckoutExistingForItemsRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public CheckoutExistingForItemsRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CheckoutExistingForItemsRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CheckoutExistingForItemsRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class UpdateCardRequest : EntityRequest<UpdateCardRequest>
        {
            public UpdateCardRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateCardRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public UpdateCardRequest CancelUrl(string cancelUrl)
            {
                MParams.AddOpt("cancel_url", cancelUrl);
                return this;
            }

            public UpdateCardRequest PassThruContent(string passThruContent)
            {
                MParams.AddOpt("pass_thru_content", passThruContent);
                return this;
            }

            public UpdateCardRequest Embed(bool embed)
            {
                MParams.AddOpt("embed", embed);
                return this;
            }

            public UpdateCardRequest IframeMessaging(bool iframeMessaging)
            {
                MParams.AddOpt("iframe_messaging", iframeMessaging);
                return this;
            }

            public UpdateCardRequest CustomerId(string customerId)
            {
                MParams.Add("customer[id]", customerId);
                return this;
            }

            [Obsolete]
            public UpdateCardRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            [Obsolete]
            public UpdateCardRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public UpdateCardRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }
        }

        public class UpdatePaymentMethodRequest : EntityRequest<UpdatePaymentMethodRequest>
        {
            public UpdatePaymentMethodRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdatePaymentMethodRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public UpdatePaymentMethodRequest CancelUrl(string cancelUrl)
            {
                MParams.AddOpt("cancel_url", cancelUrl);
                return this;
            }

            public UpdatePaymentMethodRequest PassThruContent(string passThruContent)
            {
                MParams.AddOpt("pass_thru_content", passThruContent);
                return this;
            }

            public UpdatePaymentMethodRequest Embed(bool embed)
            {
                MParams.AddOpt("embed", embed);
                return this;
            }

            public UpdatePaymentMethodRequest IframeMessaging(bool iframeMessaging)
            {
                MParams.AddOpt("iframe_messaging", iframeMessaging);
                return this;
            }

            public UpdatePaymentMethodRequest CustomerId(string customerId)
            {
                MParams.Add("customer[id]", customerId);
                return this;
            }

            [Obsolete]
            public UpdatePaymentMethodRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            [Obsolete]
            public UpdatePaymentMethodRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public UpdatePaymentMethodRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }
        }

        public class ManagePaymentSourcesRequest : EntityRequest<ManagePaymentSourcesRequest>
        {
            public ManagePaymentSourcesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ManagePaymentSourcesRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public ManagePaymentSourcesRequest CustomerId(string customerId)
            {
                MParams.Add("customer[id]", customerId);
                return this;
            }

            [Obsolete]
            public ManagePaymentSourcesRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public ManagePaymentSourcesRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }
        }

        public class CollectNowRequest : EntityRequest<CollectNowRequest>
        {
            public CollectNowRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CollectNowRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public CollectNowRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CollectNowRequest CustomerId(string customerId)
            {
                MParams.Add("customer[id]", customerId);
                return this;
            }

            [Obsolete]
            public CollectNowRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public CollectNowRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }
        }

        public class AcceptQuoteRequest : EntityRequest<AcceptQuoteRequest>
        {
            public AcceptQuoteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AcceptQuoteRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public AcceptQuoteRequest QuoteId(string quoteId)
            {
                MParams.Add("quote[id]", quoteId);
                return this;
            }
        }

        public class ExtendSubscriptionRequest : EntityRequest<ExtendSubscriptionRequest>
        {
            public ExtendSubscriptionRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ExtendSubscriptionRequest Expiry(int expiry)
            {
                MParams.AddOpt("expiry", expiry);
                return this;
            }

            public ExtendSubscriptionRequest BillingCycle(int billingCycle)
            {
                MParams.AddOpt("billing_cycle", billingCycle);
                return this;
            }

            public ExtendSubscriptionRequest SubscriptionId(string subscriptionId)
            {
                MParams.Add("subscription[id]", subscriptionId);
                return this;
            }
        }

        public class CheckoutGiftRequest : EntityRequest<CheckoutGiftRequest>
        {
            public CheckoutGiftRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CheckoutGiftRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public CheckoutGiftRequest GifterCustomerId(string gifterCustomerId)
            {
                MParams.AddOpt("gifter[customer_id]", gifterCustomerId);
                return this;
            }

            public CheckoutGiftRequest GifterLocale(string gifterLocale)
            {
                MParams.AddOpt("gifter[locale]", gifterLocale);
                return this;
            }

            public CheckoutGiftRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.Add("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public CheckoutGiftRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public CheckoutGiftRequest SubscriptionPlanQuantityInDecimal(string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
                return this;
            }

            public CheckoutGiftRequest SubscriptionCoupon(string subscriptionCoupon)
            {
                MParams.AddOpt("subscription[coupon]", subscriptionCoupon);
                return this;
            }

            public CheckoutGiftRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CheckoutGiftRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CheckoutGiftRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }
        }

        public class ClaimGiftRequest : EntityRequest<ClaimGiftRequest>
        {
            public ClaimGiftRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ClaimGiftRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public ClaimGiftRequest GiftId(string giftId)
            {
                MParams.Add("gift[id]", giftId);
                return this;
            }

            public ClaimGiftRequest CustomerLocale(string customerLocale)
            {
                MParams.AddOpt("customer[locale]", customerLocale);
                return this;
            }
        }

        public class RetrieveAgreementPdfRequest : EntityRequest<RetrieveAgreementPdfRequest>
        {
            public RetrieveAgreementPdfRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RetrieveAgreementPdfRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.Add("payment_source_id", paymentSourceId);
                return this;
            }
        }

        public class HostedPageListRequest : ListRequestBase<HostedPageListRequest>
        {
            public HostedPageListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<HostedPageListRequest> Id()
            {
                return new StringFilter<HostedPageListRequest>("id", this).SupportsMultiOperators(true);
            }

            public EnumFilter<TypeEnum, HostedPageListRequest> Type()
            {
                return new("type", this);
            }

            public EnumFilter<StateEnum, HostedPageListRequest> State()
            {
                return new("state", this);
            }

            public TimestampFilter<HostedPageListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }
        }

        #endregion
    }
}