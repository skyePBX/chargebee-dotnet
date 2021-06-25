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
    public class Subscription : Resource
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

        public enum CancelReasonEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "not_paid")] NotPaid,
            [EnumMember(Value = "no_card")] NoCard,

            [EnumMember(Value = "fraud_review_failed")]
            FraudReviewFailed,

            [EnumMember(Value = "non_compliant_eu_customer")]
            NonCompliantEuCustomer,

            [EnumMember(Value = "tax_calculation_failed")]
            TaxCalculationFailed,

            [EnumMember(Value = "currency_incompatible_with_gateway")]
            CurrencyIncompatibleWithGateway,

            [EnumMember(Value = "non_compliant_customer")]
            NonCompliantCustomer
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "future")] Future,
            [EnumMember(Value = "in_trial")] InTrial,
            [EnumMember(Value = "active")] Active,
            [EnumMember(Value = "non_renewing")] NonRenewing,
            [EnumMember(Value = "paused")] Paused,
            [EnumMember(Value = "cancelled")] Cancelled
        }

        public Subscription()
        {
        }

        public Subscription(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Subscription(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Subscription(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("subscriptions");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static CreateForCustomerRequest CreateForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "subscriptions");
            return new CreateForCustomerRequest(url, HttpMethod.Post);
        }

        public static CreateWithItemsRequest CreateWithItems(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "subscription_for_items");
            return new CreateWithItemsRequest(url, HttpMethod.Post);
        }

        public static SubscriptionListRequest List()
        {
            var url = ApiUtil.BuildUrl("subscriptions");
            return new SubscriptionListRequest(url);
        }

        [Obsolete]
        public static ListRequest SubscriptionsForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "subscriptions");
            return new ListRequest(url);
        }

        public static ListRequest ContractTermsForSubscription(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "contract_terms");
            return new ListRequest(url);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static EntityRequest<Type> RetrieveWithScheduledChanges(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "retrieve_with_scheduled_changes");
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static EntityRequest<Type> RemoveScheduledChanges(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "remove_scheduled_changes");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static RemoveScheduledCancellationRequest RemoveScheduledCancellation(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "remove_scheduled_cancellation");
            return new RemoveScheduledCancellationRequest(url, HttpMethod.Post);
        }

        public static RemoveCouponsRequest RemoveCoupons(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "remove_coupons");
            return new RemoveCouponsRequest(url, HttpMethod.Post);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static UpdateForItemsRequest UpdateForItems(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "update_for_items");
            return new UpdateForItemsRequest(url, HttpMethod.Post);
        }

        public static ChangeTermEndRequest ChangeTermEnd(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "change_term_end");
            return new ChangeTermEndRequest(url, HttpMethod.Post);
        }

        public static ReactivateRequest Reactivate(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "reactivate");
            return new ReactivateRequest(url, HttpMethod.Post);
        }

        public static AddChargeAtTermEndRequest AddChargeAtTermEnd(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "add_charge_at_term_end");
            return new AddChargeAtTermEndRequest(url, HttpMethod.Post);
        }

        public static ChargeAddonAtTermEndRequest ChargeAddonAtTermEnd(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "charge_addon_at_term_end");
            return new ChargeAddonAtTermEndRequest(url, HttpMethod.Post);
        }

        public static ChargeFutureRenewalsRequest ChargeFutureRenewals(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "charge_future_renewals");
            return new ChargeFutureRenewalsRequest(url, HttpMethod.Post);
        }

        public static EditAdvanceInvoiceScheduleRequest EditAdvanceInvoiceSchedule(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "edit_advance_invoice_schedule");
            return new EditAdvanceInvoiceScheduleRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> RetrieveAdvanceInvoiceSchedule(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "retrieve_advance_invoice_schedule");
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static RemoveAdvanceInvoiceScheduleRequest RemoveAdvanceInvoiceSchedule(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "remove_advance_invoice_schedule");
            return new RemoveAdvanceInvoiceScheduleRequest(url, HttpMethod.Post);
        }

        public static RegenerateInvoiceRequest RegenerateInvoice(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "regenerate_invoice");
            return new RegenerateInvoiceRequest(url, HttpMethod.Post);
        }

        public static ImportSubscriptionRequest ImportSubscription()
        {
            var url = ApiUtil.BuildUrl("subscriptions", "import_subscription");
            return new ImportSubscriptionRequest(url, HttpMethod.Post);
        }

        public static ImportForCustomerRequest ImportForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "import_subscription");
            return new ImportForCustomerRequest(url, HttpMethod.Post);
        }

        public static ImportContractTermRequest ImportContractTerm(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "import_contract_term");
            return new ImportContractTermRequest(url, HttpMethod.Post);
        }

        public static ImportForItemsRequest ImportForItems(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "import_for_items");
            return new ImportForItemsRequest(url, HttpMethod.Post);
        }

        public static OverrideBillingProfileRequest OverrideBillingProfile(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "override_billing_profile");
            return new OverrideBillingProfileRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static PauseRequest Pause(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "pause");
            return new PauseRequest(url, HttpMethod.Post);
        }

        public static CancelRequest Cancel(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "cancel");
            return new CancelRequest(url, HttpMethod.Post);
        }

        public static CancelForItemsRequest CancelForItems(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "cancel_for_items");
            return new CancelForItemsRequest(url, HttpMethod.Post);
        }

        public static ResumeRequest Resume(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "resume");
            return new ResumeRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> RemoveScheduledPause(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "remove_scheduled_pause");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> RemoveScheduledResumption(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "remove_scheduled_resumption");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string CurrencyCode => GetValue<string>("currency_code");

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

        public string CustomerId => GetValue<string>("customer_id");

        public int? PlanAmount => GetValue<int?>("plan_amount", false);

        public int? PlanFreeQuantity => GetValue<int?>("plan_free_quantity", false);

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public DateTime? TrialStart => GetDateTime("trial_start", false);

        public DateTime? CurrentTermStart => GetDateTime("current_term_start", false);

        public DateTime? CurrentTermEnd => GetDateTime("current_term_end", false);

        public DateTime? NextBillingAt => GetDateTime("next_billing_at", false);

        public DateTime? CreatedAt => GetDateTime("created_at", false);

        public DateTime? StartedAt => GetDateTime("started_at", false);

        public DateTime? ActivatedAt => GetDateTime("activated_at", false);

        public string GiftId => GetValue<string>("gift_id", false);

        public int? ContractTermBillingCycleOnRenewal =>
            GetValue<int?>("contract_term_billing_cycle_on_renewal", false);

        public bool? OverrideRelationship => GetValue<bool?>("override_relationship", false);

        public DateTime? PauseDate => GetDateTime("pause_date", false);

        public DateTime? ResumeDate => GetDateTime("resume_date", false);

        public DateTime? CancelledAt => GetDateTime("cancelled_at", false);

        public CancelReasonEnum? CancelReason => GetEnum<CancelReasonEnum>("cancel_reason", false);

        public string AffiliateToken => GetValue<string>("affiliate_token", false);

        public string CreatedFromIp => GetValue<string>("created_from_ip", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public bool HasScheduledAdvanceInvoices => GetValue<bool>("has_scheduled_advance_invoices");

        public bool HasScheduledChanges => GetValue<bool>("has_scheduled_changes");

        public string PaymentSourceId => GetValue<string>("payment_source_id", false);

        public string PlanFreeQuantityInDecimal => GetValue<string>("plan_free_quantity_in_decimal", false);

        public string PlanQuantityInDecimal => GetValue<string>("plan_quantity_in_decimal", false);

        public string PlanUnitPriceInDecimal => GetValue<string>("plan_unit_price_in_decimal", false);

        public string PlanAmountInDecimal => GetValue<string>("plan_amount_in_decimal", false);

        public OfflinePaymentMethodEnum? OfflinePaymentMethod =>
            GetEnum<OfflinePaymentMethodEnum>("offline_payment_method", false);

        public List<SubscriptionSubscriptionItem> SubscriptionItems =>
            GetResourceList<SubscriptionSubscriptionItem>("subscription_items");

        public List<SubscriptionItemTier> ItemTiers => GetResourceList<SubscriptionItemTier>("item_tiers");

        public List<SubscriptionChargedItem> ChargedItems => GetResourceList<SubscriptionChargedItem>("charged_items");

        public int? DueInvoicesCount => GetValue<int?>("due_invoices_count", false);

        public DateTime? DueSince => GetDateTime("due_since", false);

        public int? TotalDues => GetValue<int?>("total_dues", false);

        public int? Mrr => GetValue<int?>("mrr", false);

        public decimal? ExchangeRate => GetValue<decimal?>("exchange_rate", false);

        public string BaseCurrencyCode => GetValue<string>("base_currency_code", false);

        public List<SubscriptionAddon> Addons => GetResourceList<SubscriptionAddon>("addons");

        public List<SubscriptionEventBasedAddon> EventBasedAddons =>
            GetResourceList<SubscriptionEventBasedAddon>("event_based_addons");

        public List<SubscriptionChargedEventBasedAddon> ChargedEventBasedAddons =>
            GetResourceList<SubscriptionChargedEventBasedAddon>("charged_event_based_addons");

        [Obsolete] public string Coupon => GetValue<string>("coupon", false);

        public List<SubscriptionCoupon> Coupons => GetResourceList<SubscriptionCoupon>("coupons");

        public SubscriptionShippingAddress ShippingAddress =>
            GetSubResource<SubscriptionShippingAddress>("shipping_address");

        public SubscriptionReferralInfo ReferralInfo => GetSubResource<SubscriptionReferralInfo>("referral_info");

        public string InvoiceNotes => GetValue<string>("invoice_notes", false);

        public JToken MetaData => GetJToken("meta_data", false);

        public JToken Metadata => GetJToken("metadata", false);

        public bool Deleted => GetValue<bool>("deleted");

        public SubscriptionContractTerm ContractTerm => GetSubResource<SubscriptionContractTerm>("contract_term");

        public string CancelReasonCode => GetValue<string>("cancel_reason_code", false);

        public int? FreePeriod => GetValue<int?>("free_period", false);

        public FreePeriodUnitEnum? FreePeriodUnit => GetEnum<FreePeriodUnitEnum>("free_period_unit", false);

        public bool? CreatePendingInvoices => GetValue<bool?>("create_pending_invoices", false);

        public bool? AutoCloseInvoices => GetValue<bool?>("auto_close_invoices", false);

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
                MParams.AddOpt("id", id);
                return this;
            }

            public CreateRequest PlanUnitPriceInDecimal(string planUnitPriceInDecimal)
            {
                MParams.AddOpt("plan_unit_price_in_decimal", planUnitPriceInDecimal);
                return this;
            }

            public CreateRequest PlanQuantityInDecimal(string planQuantityInDecimal)
            {
                MParams.AddOpt("plan_quantity_in_decimal", planQuantityInDecimal);
                return this;
            }

            public CreateRequest PlanId(string planId)
            {
                MParams.Add("plan_id", planId);
                return this;
            }

            public CreateRequest PlanQuantity(int planQuantity)
            {
                MParams.AddOpt("plan_quantity", planQuantity);
                return this;
            }

            public CreateRequest PlanUnitPrice(int planUnitPrice)
            {
                MParams.AddOpt("plan_unit_price", planUnitPrice);
                return this;
            }

            public CreateRequest SetupFee(int setupFee)
            {
                MParams.AddOpt("setup_fee", setupFee);
                return this;
            }

            public CreateRequest TrialEnd(long trialEnd)
            {
                MParams.AddOpt("trial_end", trialEnd);
                return this;
            }

            public CreateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public CreateRequest StartDate(long startDate)
            {
                MParams.AddOpt("start_date", startDate);
                return this;
            }

            [Obsolete]
            public CreateRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public CreateRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public CreateRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CreateRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CreateRequest OfflinePaymentMethod(OfflinePaymentMethodEnum offlinePaymentMethod)
            {
                MParams.AddOpt("offline_payment_method", offlinePaymentMethod);
                return this;
            }

            public CreateRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public CreateRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateRequest TokenId(string tokenId)
            {
                MParams.AddOpt("token_id", tokenId);
                return this;
            }

            public CreateRequest AffiliateToken(string affiliateToken)
            {
                MParams.AddOpt("affiliate_token", affiliateToken);
                return this;
            }

            [Obsolete]
            public CreateRequest CreatedFromIp(string createdFromIp)
            {
                MParams.AddOpt("created_from_ip", createdFromIp);
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

            public CreateRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public CreateRequest FreePeriod(int freePeriod)
            {
                MParams.AddOpt("free_period", freePeriod);
                return this;
            }

            public CreateRequest FreePeriodUnit(FreePeriodUnitEnum freePeriodUnit)
            {
                MParams.AddOpt("free_period_unit", freePeriodUnit);
                return this;
            }

            public CreateRequest ContractTermBillingCycleOnRenewal(int contractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("contract_term_billing_cycle_on_renewal", contractTermBillingCycleOnRenewal);
                return this;
            }

            public CreateRequest ClientProfileId(string clientProfileId)
            {
                MParams.AddOpt("client_profile_id", clientProfileId);
                return this;
            }

            public CreateRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer[id]", customerId);
                return this;
            }

            public CreateRequest CustomerEmail(string customerEmail)
            {
                MParams.AddOpt("customer[email]", customerEmail);
                return this;
            }

            public CreateRequest CustomerFirstName(string customerFirstName)
            {
                MParams.AddOpt("customer[first_name]", customerFirstName);
                return this;
            }

            public CreateRequest CustomerLastName(string customerLastName)
            {
                MParams.AddOpt("customer[last_name]", customerLastName);
                return this;
            }

            public CreateRequest CustomerCompany(string customerCompany)
            {
                MParams.AddOpt("customer[company]", customerCompany);
                return this;
            }

            public CreateRequest CustomerTaxability(TaxabilityEnum customerTaxability)
            {
                MParams.AddOpt("customer[taxability]", customerTaxability);
                return this;
            }

            public CreateRequest CustomerLocale(string customerLocale)
            {
                MParams.AddOpt("customer[locale]", customerLocale);
                return this;
            }

            public CreateRequest CustomerEntityCode(EntityCodeEnum customerEntityCode)
            {
                MParams.AddOpt("customer[entity_code]", customerEntityCode);
                return this;
            }

            public CreateRequest CustomerExemptNumber(string customerExemptNumber)
            {
                MParams.AddOpt("customer[exempt_number]", customerExemptNumber);
                return this;
            }

            public CreateRequest CustomerNetTermDays(int customerNetTermDays)
            {
                MParams.AddOpt("customer[net_term_days]", customerNetTermDays);
                return this;
            }

            public CreateRequest CustomerTaxjarExemptionCategory(
                TaxjarExemptionCategoryEnum customerTaxjarExemptionCategory)
            {
                MParams.AddOpt("customer[taxjar_exemption_category]", customerTaxjarExemptionCategory);
                return this;
            }

            public CreateRequest CustomerPhone(string customerPhone)
            {
                MParams.AddOpt("customer[phone]", customerPhone);
                return this;
            }

            public CreateRequest CustomerAutoCollection(AutoCollectionEnum customerAutoCollection)
            {
                MParams.AddOpt("customer[auto_collection]", customerAutoCollection);
                return this;
            }

            public CreateRequest CustomerOfflinePaymentMethod(OfflinePaymentMethodEnum customerOfflinePaymentMethod)
            {
                MParams.AddOpt("customer[offline_payment_method]", customerOfflinePaymentMethod);
                return this;
            }

            public CreateRequest CustomerAllowDirectDebit(bool customerAllowDirectDebit)
            {
                MParams.AddOpt("customer[allow_direct_debit]", customerAllowDirectDebit);
                return this;
            }

            public CreateRequest CustomerConsolidatedInvoicing(bool customerConsolidatedInvoicing)
            {
                MParams.AddOpt("customer[consolidated_invoicing]", customerConsolidatedInvoicing);
                return this;
            }

            [Obsolete]
            public CreateRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public CreateRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            [Obsolete]
            public CreateRequest CardTmpToken(string cardTmpToken)
            {
                MParams.AddOpt("card[tmp_token]", cardTmpToken);
                return this;
            }

            public CreateRequest BankAccountGatewayAccountId(string bankAccountGatewayAccountId)
            {
                MParams.AddOpt("bank_account[gateway_account_id]", bankAccountGatewayAccountId);
                return this;
            }

            public CreateRequest BankAccountIban(string bankAccountIban)
            {
                MParams.AddOpt("bank_account[iban]", bankAccountIban);
                return this;
            }

            public CreateRequest BankAccountFirstName(string bankAccountFirstName)
            {
                MParams.AddOpt("bank_account[first_name]", bankAccountFirstName);
                return this;
            }

            public CreateRequest BankAccountLastName(string bankAccountLastName)
            {
                MParams.AddOpt("bank_account[last_name]", bankAccountLastName);
                return this;
            }

            public CreateRequest BankAccountCompany(string bankAccountCompany)
            {
                MParams.AddOpt("bank_account[company]", bankAccountCompany);
                return this;
            }

            public CreateRequest BankAccountEmail(string bankAccountEmail)
            {
                MParams.AddOpt("bank_account[email]", bankAccountEmail);
                return this;
            }

            public CreateRequest BankAccountBankName(string bankAccountBankName)
            {
                MParams.AddOpt("bank_account[bank_name]", bankAccountBankName);
                return this;
            }

            public CreateRequest BankAccountAccountNumber(string bankAccountAccountNumber)
            {
                MParams.AddOpt("bank_account[account_number]", bankAccountAccountNumber);
                return this;
            }

            public CreateRequest BankAccountRoutingNumber(string bankAccountRoutingNumber)
            {
                MParams.AddOpt("bank_account[routing_number]", bankAccountRoutingNumber);
                return this;
            }

            public CreateRequest BankAccountBankCode(string bankAccountBankCode)
            {
                MParams.AddOpt("bank_account[bank_code]", bankAccountBankCode);
                return this;
            }

            public CreateRequest BankAccountAccountType(AccountTypeEnum bankAccountAccountType)
            {
                MParams.AddOpt("bank_account[account_type]", bankAccountAccountType);
                return this;
            }

            public CreateRequest BankAccountAccountHolderType(AccountHolderTypeEnum bankAccountAccountHolderType)
            {
                MParams.AddOpt("bank_account[account_holder_type]", bankAccountAccountHolderType);
                return this;
            }

            public CreateRequest BankAccountEcheckType(EcheckTypeEnum bankAccountEcheckType)
            {
                MParams.AddOpt("bank_account[echeck_type]", bankAccountEcheckType);
                return this;
            }

            public CreateRequest BankAccountIssuingCountry(string bankAccountIssuingCountry)
            {
                MParams.AddOpt("bank_account[issuing_country]", bankAccountIssuingCountry);
                return this;
            }

            public CreateRequest BankAccountSwedishIdentityNumber(string bankAccountSwedishIdentityNumber)
            {
                MParams.AddOpt("bank_account[swedish_identity_number]", bankAccountSwedishIdentityNumber);
                return this;
            }

            public CreateRequest PaymentMethodType(TypeEnum paymentMethodType)
            {
                MParams.AddOpt("payment_method[type]", paymentMethodType);
                return this;
            }

            [Obsolete]
            public CreateRequest PaymentMethodGateway(GatewayEnum paymentMethodGateway)
            {
                MParams.AddOpt("payment_method[gateway]", paymentMethodGateway);
                return this;
            }

            public CreateRequest PaymentMethodGatewayAccountId(string paymentMethodGatewayAccountId)
            {
                MParams.AddOpt("payment_method[gateway_account_id]", paymentMethodGatewayAccountId);
                return this;
            }

            public CreateRequest PaymentMethodReferenceId(string paymentMethodReferenceId)
            {
                MParams.AddOpt("payment_method[reference_id]", paymentMethodReferenceId);
                return this;
            }

            public CreateRequest PaymentMethodTmpToken(string paymentMethodTmpToken)
            {
                MParams.AddOpt("payment_method[tmp_token]", paymentMethodTmpToken);
                return this;
            }

            public CreateRequest PaymentMethodIssuingCountry(string paymentMethodIssuingCountry)
            {
                MParams.AddOpt("payment_method[issuing_country]", paymentMethodIssuingCountry);
                return this;
            }

            public CreateRequest CardFirstName(string cardFirstName)
            {
                MParams.AddOpt("card[first_name]", cardFirstName);
                return this;
            }

            public CreateRequest CardLastName(string cardLastName)
            {
                MParams.AddOpt("card[last_name]", cardLastName);
                return this;
            }

            public CreateRequest CardNumber(string cardNumber)
            {
                MParams.AddOpt("card[number]", cardNumber);
                return this;
            }

            public CreateRequest CardExpiryMonth(int cardExpiryMonth)
            {
                MParams.AddOpt("card[expiry_month]", cardExpiryMonth);
                return this;
            }

            public CreateRequest CardExpiryYear(int cardExpiryYear)
            {
                MParams.AddOpt("card[expiry_year]", cardExpiryYear);
                return this;
            }

            public CreateRequest CardCvv(string cardCvv)
            {
                MParams.AddOpt("card[cvv]", cardCvv);
                return this;
            }

            public CreateRequest CardBillingAddr1(string cardBillingAddr1)
            {
                MParams.AddOpt("card[billing_addr1]", cardBillingAddr1);
                return this;
            }

            public CreateRequest CardBillingAddr2(string cardBillingAddr2)
            {
                MParams.AddOpt("card[billing_addr2]", cardBillingAddr2);
                return this;
            }

            public CreateRequest CardBillingCity(string cardBillingCity)
            {
                MParams.AddOpt("card[billing_city]", cardBillingCity);
                return this;
            }

            public CreateRequest CardBillingStateCode(string cardBillingStateCode)
            {
                MParams.AddOpt("card[billing_state_code]", cardBillingStateCode);
                return this;
            }

            public CreateRequest CardBillingState(string cardBillingState)
            {
                MParams.AddOpt("card[billing_state]", cardBillingState);
                return this;
            }

            public CreateRequest CardBillingZip(string cardBillingZip)
            {
                MParams.AddOpt("card[billing_zip]", cardBillingZip);
                return this;
            }

            public CreateRequest CardBillingCountry(string cardBillingCountry)
            {
                MParams.AddOpt("card[billing_country]", cardBillingCountry);
                return this;
            }

            [Obsolete]
            public CreateRequest CardIpAddress(string cardIpAddress)
            {
                MParams.AddOpt("card[ip_address]", cardIpAddress);
                return this;
            }

            public CreateRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public CreateRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public CreateRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public CreateRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public CreateRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public CreateRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }

            public CreateRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public CreateRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public CreateRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public CreateRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public CreateRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public CreateRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public CreateRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public CreateRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public CreateRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public CreateRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public CreateRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public CreateRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public CreateRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public CreateRequest BillingAddressValidationStatus(ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public CreateRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateRequest ShippingAddressValidationStatus(ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public CreateRequest CustomerRegisteredForGst(bool customerRegisteredForGst)
            {
                MParams.AddOpt("customer[registered_for_gst]", customerRegisteredForGst);
                return this;
            }

            public CreateRequest CustomerBusinessCustomerWithoutVatNumber(bool customerBusinessCustomerWithoutVatNumber)
            {
                MParams.AddOpt("customer[business_customer_without_vat_number]",
                    customerBusinessCustomerWithoutVatNumber);
                return this;
            }

            public CreateRequest ContractTermActionAtTermEnd(
                SubscriptionContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CreateRequest ContractTermCancellationCutoffPeriod(int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CreateRequest CustomerExemptionDetails(JArray customerExemptionDetails)
            {
                MParams.AddOpt("customer[exemption_details]", customerExemptionDetails);
                return this;
            }

            public CreateRequest CustomerCustomerType(CustomerTypeEnum customerCustomerType)
            {
                MParams.AddOpt("customer[customer_type]", customerCustomerType);
                return this;
            }

            public CreateRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CreateRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CreateRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CreateRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CreateRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CreateRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
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

            public CreateRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public CreateRequest EventBasedAddonQuantityInDecimal(int index, string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public CreateRequest EventBasedAddonUnitPriceInDecimal(int index, string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public CreateRequest EventBasedAddonServicePeriodInDays(int index, int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
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

            public CreateRequest EventBasedAddonChargeOn(int index, ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }

            public CreateRequest AddonTrialEnd(int index, long addonTrialEnd)
            {
                MParams.AddOpt("addons[trial_end][" + index + "]", addonTrialEnd);
                return this;
            }
        }

        public class CreateForCustomerRequest : EntityRequest<CreateForCustomerRequest>
        {
            public CreateForCustomerRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateForCustomerRequest Id(string id)
            {
                MParams.AddOpt("id", id);
                return this;
            }

            public CreateForCustomerRequest PlanUnitPriceInDecimal(string planUnitPriceInDecimal)
            {
                MParams.AddOpt("plan_unit_price_in_decimal", planUnitPriceInDecimal);
                return this;
            }

            public CreateForCustomerRequest PlanQuantityInDecimal(string planQuantityInDecimal)
            {
                MParams.AddOpt("plan_quantity_in_decimal", planQuantityInDecimal);
                return this;
            }

            public CreateForCustomerRequest PlanId(string planId)
            {
                MParams.Add("plan_id", planId);
                return this;
            }

            public CreateForCustomerRequest PlanQuantity(int planQuantity)
            {
                MParams.AddOpt("plan_quantity", planQuantity);
                return this;
            }

            public CreateForCustomerRequest PlanUnitPrice(int planUnitPrice)
            {
                MParams.AddOpt("plan_unit_price", planUnitPrice);
                return this;
            }

            public CreateForCustomerRequest SetupFee(int setupFee)
            {
                MParams.AddOpt("setup_fee", setupFee);
                return this;
            }

            public CreateForCustomerRequest TrialEnd(long trialEnd)
            {
                MParams.AddOpt("trial_end", trialEnd);
                return this;
            }

            public CreateForCustomerRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public CreateForCustomerRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public CreateForCustomerRequest StartDate(long startDate)
            {
                MParams.AddOpt("start_date", startDate);
                return this;
            }

            [Obsolete]
            public CreateForCustomerRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public CreateForCustomerRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public CreateForCustomerRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CreateForCustomerRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CreateForCustomerRequest OfflinePaymentMethod(OfflinePaymentMethodEnum offlinePaymentMethod)
            {
                MParams.AddOpt("offline_payment_method", offlinePaymentMethod);
                return this;
            }

            public CreateForCustomerRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public CreateForCustomerRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateForCustomerRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public CreateForCustomerRequest OverrideRelationship(bool overrideRelationship)
            {
                MParams.AddOpt("override_relationship", overrideRelationship);
                return this;
            }

            public CreateForCustomerRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public CreateForCustomerRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public CreateForCustomerRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public CreateForCustomerRequest FreePeriod(int freePeriod)
            {
                MParams.AddOpt("free_period", freePeriod);
                return this;
            }

            public CreateForCustomerRequest FreePeriodUnit(FreePeriodUnitEnum freePeriodUnit)
            {
                MParams.AddOpt("free_period_unit", freePeriodUnit);
                return this;
            }

            public CreateForCustomerRequest ContractTermBillingCycleOnRenewal(int contractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("contract_term_billing_cycle_on_renewal", contractTermBillingCycleOnRenewal);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateForCustomerRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateForCustomerRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public CreateForCustomerRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public CreateForCustomerRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public CreateForCustomerRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public CreateForCustomerRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public CreateForCustomerRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }

            public CreateForCustomerRequest ContractTermActionAtTermEnd(
                SubscriptionContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CreateForCustomerRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CreateForCustomerRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public CreateForCustomerRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public CreateForCustomerRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CreateForCustomerRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CreateForCustomerRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CreateForCustomerRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public CreateForCustomerRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public CreateForCustomerRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public CreateForCustomerRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public CreateForCustomerRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public CreateForCustomerRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public CreateForCustomerRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public CreateForCustomerRequest EventBasedAddonOnEvent(int index, OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public CreateForCustomerRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public CreateForCustomerRequest EventBasedAddonChargeOn(int index, ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
                return this;
            }

            public CreateForCustomerRequest AddonTrialEnd(int index, long addonTrialEnd)
            {
                MParams.AddOpt("addons[trial_end][" + index + "]", addonTrialEnd);
                return this;
            }
        }

        public class CreateWithItemsRequest : EntityRequest<CreateWithItemsRequest>
        {
            public CreateWithItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateWithItemsRequest Id(string id)
            {
                MParams.AddOpt("id", id);
                return this;
            }

            public CreateWithItemsRequest TrialEnd(long trialEnd)
            {
                MParams.AddOpt("trial_end", trialEnd);
                return this;
            }

            public CreateWithItemsRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            [Obsolete]
            public CreateWithItemsRequest SetupFee(int setupFee)
            {
                MParams.AddOpt("setup_fee", setupFee);
                return this;
            }

            public CreateWithItemsRequest MandatoryItemsToRemove(List<string> mandatoryItemsToRemove)
            {
                MParams.AddOpt("mandatory_items_to_remove", mandatoryItemsToRemove);
                return this;
            }

            public CreateWithItemsRequest StartDate(long startDate)
            {
                MParams.AddOpt("start_date", startDate);
                return this;
            }

            [Obsolete]
            public CreateWithItemsRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public CreateWithItemsRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public CreateWithItemsRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public CreateWithItemsRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public CreateWithItemsRequest OfflinePaymentMethod(OfflinePaymentMethodEnum offlinePaymentMethod)
            {
                MParams.AddOpt("offline_payment_method", offlinePaymentMethod);
                return this;
            }

            public CreateWithItemsRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public CreateWithItemsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateWithItemsRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public CreateWithItemsRequest OverrideRelationship(bool overrideRelationship)
            {
                MParams.AddOpt("override_relationship", overrideRelationship);
                return this;
            }

            public CreateWithItemsRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public CreateWithItemsRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public CreateWithItemsRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public CreateWithItemsRequest FreePeriod(int freePeriod)
            {
                MParams.AddOpt("free_period", freePeriod);
                return this;
            }

            public CreateWithItemsRequest FreePeriodUnit(FreePeriodUnitEnum freePeriodUnit)
            {
                MParams.AddOpt("free_period_unit", freePeriodUnit);
                return this;
            }

            public CreateWithItemsRequest ContractTermBillingCycleOnRenewal(int contractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("contract_term_billing_cycle_on_renewal", contractTermBillingCycleOnRenewal);
                return this;
            }

            public CreateWithItemsRequest CreatePendingInvoices(bool createPendingInvoices)
            {
                MParams.AddOpt("create_pending_invoices", createPendingInvoices);
                return this;
            }

            public CreateWithItemsRequest AutoCloseInvoices(bool autoCloseInvoices)
            {
                MParams.AddOpt("auto_close_invoices", autoCloseInvoices);
                return this;
            }

            public CreateWithItemsRequest FirstInvoicePending(bool firstInvoicePending)
            {
                MParams.AddOpt("first_invoice_pending", firstInvoicePending);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateWithItemsRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateWithItemsRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public CreateWithItemsRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public CreateWithItemsRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public CreateWithItemsRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public CreateWithItemsRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public CreateWithItemsRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }

            public CreateWithItemsRequest ContractTermActionAtTermEnd(
                SubscriptionContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public CreateWithItemsRequest ContractTermCancellationCutoffPeriod(int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public CreateWithItemsRequest SubscriptionItemItemPriceId(int index, string subscriptionItemItemPriceId)
            {
                MParams.Add("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public CreateWithItemsRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public CreateWithItemsRequest SubscriptionItemUnitPrice(int index, int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public CreateWithItemsRequest SubscriptionItemBillingCycles(int index, int subscriptionItemBillingCycles)
            {
                MParams.AddOpt("subscription_items[billing_cycles][" + index + "]", subscriptionItemBillingCycles);
                return this;
            }

            public CreateWithItemsRequest SubscriptionItemTrialEnd(int index, long subscriptionItemTrialEnd)
            {
                MParams.AddOpt("subscription_items[trial_end][" + index + "]", subscriptionItemTrialEnd);
                return this;
            }

            public CreateWithItemsRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }

            public CreateWithItemsRequest SubscriptionItemChargeOnEvent(int index,
                ChargeOnEventEnum subscriptionItemChargeOnEvent)
            {
                MParams.AddOpt("subscription_items[charge_on_event][" + index + "]", subscriptionItemChargeOnEvent);
                return this;
            }

            public CreateWithItemsRequest SubscriptionItemChargeOnce(int index, bool subscriptionItemChargeOnce)
            {
                MParams.AddOpt("subscription_items[charge_once][" + index + "]", subscriptionItemChargeOnce);
                return this;
            }

            [Obsolete]
            public CreateWithItemsRequest SubscriptionItemItemType(int index, ItemTypeEnum subscriptionItemItemType)
            {
                MParams.AddOpt("subscription_items[item_type][" + index + "]", subscriptionItemItemType);
                return this;
            }

            public CreateWithItemsRequest SubscriptionItemChargeOnOption(int index,
                ChargeOnOptionEnum subscriptionItemChargeOnOption)
            {
                MParams.AddOpt("subscription_items[charge_on_option][" + index + "]", subscriptionItemChargeOnOption);
                return this;
            }

            public CreateWithItemsRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public CreateWithItemsRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CreateWithItemsRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CreateWithItemsRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class SubscriptionListRequest : ListRequestBase<SubscriptionListRequest>
        {
            public SubscriptionListRequest(string url)
                : base(url)
            {
            }

            public SubscriptionListRequest IncludeDeleted(bool includeDeleted)
            {
                MParams.AddOpt("include_deleted", includeDeleted);
                return this;
            }

            public StringFilter<SubscriptionListRequest> Id()
            {
                return new StringFilter<SubscriptionListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<SubscriptionListRequest> CustomerId()
            {
                return new StringFilter<SubscriptionListRequest>("customer_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<SubscriptionListRequest> PlanId()
            {
                return new StringFilter<SubscriptionListRequest>("plan_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<SubscriptionListRequest> ItemId()
            {
                return new StringFilter<SubscriptionListRequest>("item_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<SubscriptionListRequest> ItemPriceId()
            {
                return new StringFilter<SubscriptionListRequest>("item_price_id", this).SupportsMultiOperators(true);
            }

            public EnumFilter<StatusEnum, SubscriptionListRequest> Status()
            {
                return new("status", this);
            }

            public EnumFilter<CancelReasonEnum, SubscriptionListRequest> CancelReason()
            {
                return new EnumFilter<CancelReasonEnum, SubscriptionListRequest>("cancel_reason", this)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<SubscriptionListRequest> CancelReasonCode()
            {
                return new StringFilter<SubscriptionListRequest>("cancel_reason_code", this)
                    .SupportsMultiOperators(true);
            }

            public NumberFilter<int, SubscriptionListRequest> RemainingBillingCycles()
            {
                return new NumberFilter<int, SubscriptionListRequest>("remaining_billing_cycles", this)
                    .SupportsPresenceOperator(true);
            }

            public TimestampFilter<SubscriptionListRequest> CreatedAt()
            {
                return new("created_at", this);
            }

            public TimestampFilter<SubscriptionListRequest> ActivatedAt()
            {
                return new TimestampFilter<SubscriptionListRequest>("activated_at", this)
                    .SupportsPresenceOperator(true);
            }

            public TimestampFilter<SubscriptionListRequest> NextBillingAt()
            {
                return new("next_billing_at", this);
            }

            public TimestampFilter<SubscriptionListRequest> CancelledAt()
            {
                return new("cancelled_at", this);
            }

            public BooleanFilter<SubscriptionListRequest> HasScheduledChanges()
            {
                return new("has_scheduled_changes", this);
            }

            public TimestampFilter<SubscriptionListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public EnumFilter<OfflinePaymentMethodEnum, SubscriptionListRequest> OfflinePaymentMethod()
            {
                return new("offline_payment_method", this);
            }

            public BooleanFilter<SubscriptionListRequest> AutoCloseInvoices()
            {
                return new("auto_close_invoices", this);
            }

            public BooleanFilter<SubscriptionListRequest> CreatePendingInvoices()
            {
                return new("create_pending_invoices", this);
            }

            public BooleanFilter<SubscriptionListRequest> OverrideRelationship()
            {
                return new("override_relationship", this);
            }

            public SubscriptionListRequest SortByCreatedAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "created_at");
                return this;
            }

            public SubscriptionListRequest SortByUpdatedAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "updated_at");
                return this;
            }
        }

        public class RemoveScheduledCancellationRequest : EntityRequest<RemoveScheduledCancellationRequest>
        {
            public RemoveScheduledCancellationRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RemoveScheduledCancellationRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }
        }

        public class RemoveCouponsRequest : EntityRequest<RemoveCouponsRequest>
        {
            public RemoveCouponsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RemoveCouponsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }
        }

        public class UpdateRequest : EntityRequest<UpdateRequest>
        {
            public UpdateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateRequest PlanId(string planId)
            {
                MParams.AddOpt("plan_id", planId);
                return this;
            }

            public UpdateRequest PlanQuantity(int planQuantity)
            {
                MParams.AddOpt("plan_quantity", planQuantity);
                return this;
            }

            public UpdateRequest PlanUnitPrice(int planUnitPrice)
            {
                MParams.AddOpt("plan_unit_price", planUnitPrice);
                return this;
            }

            public UpdateRequest SetupFee(int setupFee)
            {
                MParams.AddOpt("setup_fee", setupFee);
                return this;
            }

            public UpdateRequest ReplaceAddonList(bool replaceAddonList)
            {
                MParams.AddOpt("replace_addon_list", replaceAddonList);
                return this;
            }

            public UpdateRequest MandatoryAddonsToRemove(List<string> mandatoryAddonsToRemove)
            {
                MParams.AddOpt("mandatory_addons_to_remove", mandatoryAddonsToRemove);
                return this;
            }

            public UpdateRequest PlanQuantityInDecimal(string planQuantityInDecimal)
            {
                MParams.AddOpt("plan_quantity_in_decimal", planQuantityInDecimal);
                return this;
            }

            public UpdateRequest PlanUnitPriceInDecimal(string planUnitPriceInDecimal)
            {
                MParams.AddOpt("plan_unit_price_in_decimal", planUnitPriceInDecimal);
                return this;
            }

            public UpdateRequest StartDate(long startDate)
            {
                MParams.AddOpt("start_date", startDate);
                return this;
            }

            public UpdateRequest TrialEnd(long trialEnd)
            {
                MParams.AddOpt("trial_end", trialEnd);
                return this;
            }

            public UpdateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            [Obsolete]
            public UpdateRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public UpdateRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public UpdateRequest ReactivateFrom(long reactivateFrom)
            {
                MParams.AddOpt("reactivate_from", reactivateFrom);
                return this;
            }

            public UpdateRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public UpdateRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public UpdateRequest OfflinePaymentMethod(OfflinePaymentMethodEnum offlinePaymentMethod)
            {
                MParams.AddOpt("offline_payment_method", offlinePaymentMethod);
                return this;
            }

            public UpdateRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public UpdateRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public UpdateRequest ReplaceCouponList(bool replaceCouponList)
            {
                MParams.AddOpt("replace_coupon_list", replaceCouponList);
                return this;
            }

            public UpdateRequest Prorate(bool prorate)
            {
                MParams.AddOpt("prorate", prorate);
                return this;
            }

            public UpdateRequest EndOfTerm(bool endOfTerm)
            {
                MParams.AddOpt("end_of_term", endOfTerm);
                return this;
            }

            public UpdateRequest ForceTermReset(bool forceTermReset)
            {
                MParams.AddOpt("force_term_reset", forceTermReset);
                return this;
            }

            public UpdateRequest Reactivate(bool reactivate)
            {
                MParams.AddOpt("reactivate", reactivate);
                return this;
            }

            public UpdateRequest TokenId(string tokenId)
            {
                MParams.AddOpt("token_id", tokenId);
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

            public UpdateRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public UpdateRequest OverrideRelationship(bool overrideRelationship)
            {
                MParams.AddOpt("override_relationship", overrideRelationship);
                return this;
            }

            public UpdateRequest ContractTermBillingCycleOnRenewal(int contractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("contract_term_billing_cycle_on_renewal", contractTermBillingCycleOnRenewal);
                return this;
            }

            public UpdateRequest FreePeriod(int freePeriod)
            {
                MParams.AddOpt("free_period", freePeriod);
                return this;
            }

            public UpdateRequest FreePeriodUnit(FreePeriodUnitEnum freePeriodUnit)
            {
                MParams.AddOpt("free_period_unit", freePeriodUnit);
                return this;
            }

            [Obsolete]
            public UpdateRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public UpdateRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            [Obsolete]
            public UpdateRequest CardTmpToken(string cardTmpToken)
            {
                MParams.AddOpt("card[tmp_token]", cardTmpToken);
                return this;
            }

            public UpdateRequest PaymentMethodType(TypeEnum paymentMethodType)
            {
                MParams.AddOpt("payment_method[type]", paymentMethodType);
                return this;
            }

            [Obsolete]
            public UpdateRequest PaymentMethodGateway(GatewayEnum paymentMethodGateway)
            {
                MParams.AddOpt("payment_method[gateway]", paymentMethodGateway);
                return this;
            }

            public UpdateRequest PaymentMethodGatewayAccountId(string paymentMethodGatewayAccountId)
            {
                MParams.AddOpt("payment_method[gateway_account_id]", paymentMethodGatewayAccountId);
                return this;
            }

            public UpdateRequest PaymentMethodReferenceId(string paymentMethodReferenceId)
            {
                MParams.AddOpt("payment_method[reference_id]", paymentMethodReferenceId);
                return this;
            }

            public UpdateRequest PaymentMethodTmpToken(string paymentMethodTmpToken)
            {
                MParams.AddOpt("payment_method[tmp_token]", paymentMethodTmpToken);
                return this;
            }

            public UpdateRequest PaymentMethodIssuingCountry(string paymentMethodIssuingCountry)
            {
                MParams.AddOpt("payment_method[issuing_country]", paymentMethodIssuingCountry);
                return this;
            }

            public UpdateRequest CardFirstName(string cardFirstName)
            {
                MParams.AddOpt("card[first_name]", cardFirstName);
                return this;
            }

            public UpdateRequest CardLastName(string cardLastName)
            {
                MParams.AddOpt("card[last_name]", cardLastName);
                return this;
            }

            public UpdateRequest CardNumber(string cardNumber)
            {
                MParams.AddOpt("card[number]", cardNumber);
                return this;
            }

            public UpdateRequest CardExpiryMonth(int cardExpiryMonth)
            {
                MParams.AddOpt("card[expiry_month]", cardExpiryMonth);
                return this;
            }

            public UpdateRequest CardExpiryYear(int cardExpiryYear)
            {
                MParams.AddOpt("card[expiry_year]", cardExpiryYear);
                return this;
            }

            public UpdateRequest CardCvv(string cardCvv)
            {
                MParams.AddOpt("card[cvv]", cardCvv);
                return this;
            }

            public UpdateRequest CardBillingAddr1(string cardBillingAddr1)
            {
                MParams.AddOpt("card[billing_addr1]", cardBillingAddr1);
                return this;
            }

            public UpdateRequest CardBillingAddr2(string cardBillingAddr2)
            {
                MParams.AddOpt("card[billing_addr2]", cardBillingAddr2);
                return this;
            }

            public UpdateRequest CardBillingCity(string cardBillingCity)
            {
                MParams.AddOpt("card[billing_city]", cardBillingCity);
                return this;
            }

            public UpdateRequest CardBillingStateCode(string cardBillingStateCode)
            {
                MParams.AddOpt("card[billing_state_code]", cardBillingStateCode);
                return this;
            }

            public UpdateRequest CardBillingState(string cardBillingState)
            {
                MParams.AddOpt("card[billing_state]", cardBillingState);
                return this;
            }

            public UpdateRequest CardBillingZip(string cardBillingZip)
            {
                MParams.AddOpt("card[billing_zip]", cardBillingZip);
                return this;
            }

            public UpdateRequest CardBillingCountry(string cardBillingCountry)
            {
                MParams.AddOpt("card[billing_country]", cardBillingCountry);
                return this;
            }

            [Obsolete]
            public UpdateRequest CardIpAddress(string cardIpAddress)
            {
                MParams.AddOpt("card[ip_address]", cardIpAddress);
                return this;
            }

            public UpdateRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public UpdateRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public UpdateRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public UpdateRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public UpdateRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public UpdateRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }

            public UpdateRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public UpdateRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public UpdateRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public UpdateRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public UpdateRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public UpdateRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public UpdateRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public UpdateRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public UpdateRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public UpdateRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public UpdateRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public UpdateRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public UpdateRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public UpdateRequest BillingAddressValidationStatus(ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public UpdateRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public UpdateRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public UpdateRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public UpdateRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public UpdateRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public UpdateRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public UpdateRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public UpdateRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public UpdateRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public UpdateRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public UpdateRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public UpdateRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public UpdateRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public UpdateRequest ShippingAddressValidationStatus(ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public UpdateRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public UpdateRequest CustomerBusinessCustomerWithoutVatNumber(bool customerBusinessCustomerWithoutVatNumber)
            {
                MParams.AddOpt("customer[business_customer_without_vat_number]",
                    customerBusinessCustomerWithoutVatNumber);
                return this;
            }

            public UpdateRequest CustomerRegisteredForGst(bool customerRegisteredForGst)
            {
                MParams.AddOpt("customer[registered_for_gst]", customerRegisteredForGst);
                return this;
            }

            public UpdateRequest ContractTermActionAtTermEnd(
                SubscriptionContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public UpdateRequest ContractTermCancellationCutoffPeriod(int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public UpdateRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public UpdateRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public UpdateRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public UpdateRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
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

            public UpdateRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public UpdateRequest EventBasedAddonServicePeriodInDays(int index, int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public UpdateRequest EventBasedAddonChargeOn(int index, ChargeOnEnum eventBasedAddonChargeOn)
            {
                MParams.AddOpt("event_based_addons[charge_on][" + index + "]", eventBasedAddonChargeOn);
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

            public UpdateRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public UpdateRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public UpdateRequest EventBasedAddonQuantityInDecimal(int index, string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public UpdateRequest EventBasedAddonUnitPriceInDecimal(int index, string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public UpdateRequest AddonTrialEnd(int index, long addonTrialEnd)
            {
                MParams.AddOpt("addons[trial_end][" + index + "]", addonTrialEnd);
                return this;
            }
        }

        public class UpdateForItemsRequest : EntityRequest<UpdateForItemsRequest>
        {
            public UpdateForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateForItemsRequest MandatoryItemsToRemove(List<string> mandatoryItemsToRemove)
            {
                MParams.AddOpt("mandatory_items_to_remove", mandatoryItemsToRemove);
                return this;
            }

            public UpdateForItemsRequest ReplaceItemsList(bool replaceItemsList)
            {
                MParams.AddOpt("replace_items_list", replaceItemsList);
                return this;
            }

            [Obsolete]
            public UpdateForItemsRequest SetupFee(int setupFee)
            {
                MParams.AddOpt("setup_fee", setupFee);
                return this;
            }

            public UpdateForItemsRequest StartDate(long startDate)
            {
                MParams.AddOpt("start_date", startDate);
                return this;
            }

            public UpdateForItemsRequest TrialEnd(long trialEnd)
            {
                MParams.AddOpt("trial_end", trialEnd);
                return this;
            }

            public UpdateForItemsRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            [Obsolete]
            public UpdateForItemsRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public UpdateForItemsRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public UpdateForItemsRequest ReactivateFrom(long reactivateFrom)
            {
                MParams.AddOpt("reactivate_from", reactivateFrom);
                return this;
            }

            public UpdateForItemsRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public UpdateForItemsRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public UpdateForItemsRequest OfflinePaymentMethod(OfflinePaymentMethodEnum offlinePaymentMethod)
            {
                MParams.AddOpt("offline_payment_method", offlinePaymentMethod);
                return this;
            }

            public UpdateForItemsRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public UpdateForItemsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public UpdateForItemsRequest ReplaceCouponList(bool replaceCouponList)
            {
                MParams.AddOpt("replace_coupon_list", replaceCouponList);
                return this;
            }

            public UpdateForItemsRequest Prorate(bool prorate)
            {
                MParams.AddOpt("prorate", prorate);
                return this;
            }

            public UpdateForItemsRequest EndOfTerm(bool endOfTerm)
            {
                MParams.AddOpt("end_of_term", endOfTerm);
                return this;
            }

            public UpdateForItemsRequest ForceTermReset(bool forceTermReset)
            {
                MParams.AddOpt("force_term_reset", forceTermReset);
                return this;
            }

            public UpdateForItemsRequest Reactivate(bool reactivate)
            {
                MParams.AddOpt("reactivate", reactivate);
                return this;
            }

            public UpdateForItemsRequest TokenId(string tokenId)
            {
                MParams.AddOpt("token_id", tokenId);
                return this;
            }

            public UpdateForItemsRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public UpdateForItemsRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public UpdateForItemsRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public UpdateForItemsRequest OverrideRelationship(bool overrideRelationship)
            {
                MParams.AddOpt("override_relationship", overrideRelationship);
                return this;
            }

            public UpdateForItemsRequest ContractTermBillingCycleOnRenewal(int contractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("contract_term_billing_cycle_on_renewal", contractTermBillingCycleOnRenewal);
                return this;
            }

            public UpdateForItemsRequest FreePeriod(int freePeriod)
            {
                MParams.AddOpt("free_period", freePeriod);
                return this;
            }

            public UpdateForItemsRequest FreePeriodUnit(FreePeriodUnitEnum freePeriodUnit)
            {
                MParams.AddOpt("free_period_unit", freePeriodUnit);
                return this;
            }

            public UpdateForItemsRequest CreatePendingInvoices(bool createPendingInvoices)
            {
                MParams.AddOpt("create_pending_invoices", createPendingInvoices);
                return this;
            }

            public UpdateForItemsRequest AutoCloseInvoices(bool autoCloseInvoices)
            {
                MParams.AddOpt("auto_close_invoices", autoCloseInvoices);
                return this;
            }

            [Obsolete]
            public UpdateForItemsRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public UpdateForItemsRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            [Obsolete]
            public UpdateForItemsRequest CardTmpToken(string cardTmpToken)
            {
                MParams.AddOpt("card[tmp_token]", cardTmpToken);
                return this;
            }

            public UpdateForItemsRequest PaymentMethodType(TypeEnum paymentMethodType)
            {
                MParams.AddOpt("payment_method[type]", paymentMethodType);
                return this;
            }

            [Obsolete]
            public UpdateForItemsRequest PaymentMethodGateway(GatewayEnum paymentMethodGateway)
            {
                MParams.AddOpt("payment_method[gateway]", paymentMethodGateway);
                return this;
            }

            public UpdateForItemsRequest PaymentMethodGatewayAccountId(string paymentMethodGatewayAccountId)
            {
                MParams.AddOpt("payment_method[gateway_account_id]", paymentMethodGatewayAccountId);
                return this;
            }

            public UpdateForItemsRequest PaymentMethodReferenceId(string paymentMethodReferenceId)
            {
                MParams.AddOpt("payment_method[reference_id]", paymentMethodReferenceId);
                return this;
            }

            public UpdateForItemsRequest PaymentMethodTmpToken(string paymentMethodTmpToken)
            {
                MParams.AddOpt("payment_method[tmp_token]", paymentMethodTmpToken);
                return this;
            }

            public UpdateForItemsRequest PaymentMethodIssuingCountry(string paymentMethodIssuingCountry)
            {
                MParams.AddOpt("payment_method[issuing_country]", paymentMethodIssuingCountry);
                return this;
            }

            public UpdateForItemsRequest CardFirstName(string cardFirstName)
            {
                MParams.AddOpt("card[first_name]", cardFirstName);
                return this;
            }

            public UpdateForItemsRequest CardLastName(string cardLastName)
            {
                MParams.AddOpt("card[last_name]", cardLastName);
                return this;
            }

            public UpdateForItemsRequest CardNumber(string cardNumber)
            {
                MParams.AddOpt("card[number]", cardNumber);
                return this;
            }

            public UpdateForItemsRequest CardExpiryMonth(int cardExpiryMonth)
            {
                MParams.AddOpt("card[expiry_month]", cardExpiryMonth);
                return this;
            }

            public UpdateForItemsRequest CardExpiryYear(int cardExpiryYear)
            {
                MParams.AddOpt("card[expiry_year]", cardExpiryYear);
                return this;
            }

            public UpdateForItemsRequest CardCvv(string cardCvv)
            {
                MParams.AddOpt("card[cvv]", cardCvv);
                return this;
            }

            public UpdateForItemsRequest CardBillingAddr1(string cardBillingAddr1)
            {
                MParams.AddOpt("card[billing_addr1]", cardBillingAddr1);
                return this;
            }

            public UpdateForItemsRequest CardBillingAddr2(string cardBillingAddr2)
            {
                MParams.AddOpt("card[billing_addr2]", cardBillingAddr2);
                return this;
            }

            public UpdateForItemsRequest CardBillingCity(string cardBillingCity)
            {
                MParams.AddOpt("card[billing_city]", cardBillingCity);
                return this;
            }

            public UpdateForItemsRequest CardBillingStateCode(string cardBillingStateCode)
            {
                MParams.AddOpt("card[billing_state_code]", cardBillingStateCode);
                return this;
            }

            public UpdateForItemsRequest CardBillingState(string cardBillingState)
            {
                MParams.AddOpt("card[billing_state]", cardBillingState);
                return this;
            }

            public UpdateForItemsRequest CardBillingZip(string cardBillingZip)
            {
                MParams.AddOpt("card[billing_zip]", cardBillingZip);
                return this;
            }

            public UpdateForItemsRequest CardBillingCountry(string cardBillingCountry)
            {
                MParams.AddOpt("card[billing_country]", cardBillingCountry);
                return this;
            }

            [Obsolete]
            public UpdateForItemsRequest CardIpAddress(string cardIpAddress)
            {
                MParams.AddOpt("card[ip_address]", cardIpAddress);
                return this;
            }

            public UpdateForItemsRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public UpdateForItemsRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public UpdateForItemsRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public UpdateForItemsRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public UpdateForItemsRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public UpdateForItemsRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }

            public UpdateForItemsRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public UpdateForItemsRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public UpdateForItemsRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public UpdateForItemsRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public UpdateForItemsRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public UpdateForItemsRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public UpdateForItemsRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public UpdateForItemsRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public UpdateForItemsRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public UpdateForItemsRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public UpdateForItemsRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public UpdateForItemsRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public UpdateForItemsRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public UpdateForItemsRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public UpdateForItemsRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public UpdateForItemsRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public UpdateForItemsRequest CustomerBusinessCustomerWithoutVatNumber(
                bool customerBusinessCustomerWithoutVatNumber)
            {
                MParams.AddOpt("customer[business_customer_without_vat_number]",
                    customerBusinessCustomerWithoutVatNumber);
                return this;
            }

            public UpdateForItemsRequest CustomerRegisteredForGst(bool customerRegisteredForGst)
            {
                MParams.AddOpt("customer[registered_for_gst]", customerRegisteredForGst);
                return this;
            }

            public UpdateForItemsRequest ContractTermActionAtTermEnd(
                SubscriptionContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public UpdateForItemsRequest ContractTermCancellationCutoffPeriod(int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public UpdateForItemsRequest SubscriptionItemItemPriceId(int index, string subscriptionItemItemPriceId)
            {
                MParams.Add("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public UpdateForItemsRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public UpdateForItemsRequest SubscriptionItemUnitPrice(int index, int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public UpdateForItemsRequest SubscriptionItemBillingCycles(int index, int subscriptionItemBillingCycles)
            {
                MParams.AddOpt("subscription_items[billing_cycles][" + index + "]", subscriptionItemBillingCycles);
                return this;
            }

            public UpdateForItemsRequest SubscriptionItemTrialEnd(int index, long subscriptionItemTrialEnd)
            {
                MParams.AddOpt("subscription_items[trial_end][" + index + "]", subscriptionItemTrialEnd);
                return this;
            }

            public UpdateForItemsRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }

            public UpdateForItemsRequest SubscriptionItemChargeOnEvent(int index,
                ChargeOnEventEnum subscriptionItemChargeOnEvent)
            {
                MParams.AddOpt("subscription_items[charge_on_event][" + index + "]", subscriptionItemChargeOnEvent);
                return this;
            }

            public UpdateForItemsRequest SubscriptionItemChargeOnce(int index, bool subscriptionItemChargeOnce)
            {
                MParams.AddOpt("subscription_items[charge_once][" + index + "]", subscriptionItemChargeOnce);
                return this;
            }

            public UpdateForItemsRequest SubscriptionItemChargeOnOption(int index,
                ChargeOnOptionEnum subscriptionItemChargeOnOption)
            {
                MParams.AddOpt("subscription_items[charge_on_option][" + index + "]", subscriptionItemChargeOnOption);
                return this;
            }

            [Obsolete]
            public UpdateForItemsRequest SubscriptionItemItemType(int index, ItemTypeEnum subscriptionItemItemType)
            {
                MParams.AddOpt("subscription_items[item_type][" + index + "]", subscriptionItemItemType);
                return this;
            }

            public UpdateForItemsRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public UpdateForItemsRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public UpdateForItemsRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public UpdateForItemsRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
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
                MParams.Add("term_ends_at", termEndsAt);
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

        public class ReactivateRequest : EntityRequest<ReactivateRequest>
        {
            public ReactivateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ReactivateRequest TrialEnd(long trialEnd)
            {
                MParams.AddOpt("trial_end", trialEnd);
                return this;
            }

            public ReactivateRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            [Obsolete]
            public ReactivateRequest TrialPeriodDays(int trialPeriodDays)
            {
                MParams.AddOpt("trial_period_days", trialPeriodDays);
                return this;
            }

            public ReactivateRequest ReactivateFrom(long reactivateFrom)
            {
                MParams.AddOpt("reactivate_from", reactivateFrom);
                return this;
            }

            public ReactivateRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public ReactivateRequest BillingAlignmentMode(BillingAlignmentModeEnum billingAlignmentMode)
            {
                MParams.AddOpt("billing_alignment_mode", billingAlignmentMode);
                return this;
            }

            public ReactivateRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public ReactivateRequest ContractTermBillingCycleOnRenewal(int contractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("contract_term_billing_cycle_on_renewal", contractTermBillingCycleOnRenewal);
                return this;
            }

            public ReactivateRequest ContractTermActionAtTermEnd(
                SubscriptionContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public ReactivateRequest ContractTermCancellationCutoffPeriod(int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public ReactivateRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public ReactivateRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public ReactivateRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public ReactivateRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public ReactivateRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public ReactivateRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }
        }

        public class AddChargeAtTermEndRequest : EntityRequest<AddChargeAtTermEndRequest>
        {
            public AddChargeAtTermEndRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AddChargeAtTermEndRequest Amount(int amount)
            {
                MParams.AddOpt("amount", amount);
                return this;
            }

            public AddChargeAtTermEndRequest Description(string description)
            {
                MParams.Add("description", description);
                return this;
            }

            public AddChargeAtTermEndRequest AmountInDecimal(string amountInDecimal)
            {
                MParams.AddOpt("amount_in_decimal", amountInDecimal);
                return this;
            }

            public AddChargeAtTermEndRequest AvalaraSaleType(AvalaraSaleTypeEnum avalaraSaleType)
            {
                MParams.AddOpt("avalara_sale_type", avalaraSaleType);
                return this;
            }

            public AddChargeAtTermEndRequest AvalaraTransactionType(int avalaraTransactionType)
            {
                MParams.AddOpt("avalara_transaction_type", avalaraTransactionType);
                return this;
            }

            public AddChargeAtTermEndRequest AvalaraServiceType(int avalaraServiceType)
            {
                MParams.AddOpt("avalara_service_type", avalaraServiceType);
                return this;
            }

            public AddChargeAtTermEndRequest DateFrom(long dateFrom)
            {
                MParams.AddOpt("date_from", dateFrom);
                return this;
            }

            public AddChargeAtTermEndRequest DateTo(long dateTo)
            {
                MParams.AddOpt("date_to", dateTo);
                return this;
            }
        }

        public class ChargeAddonAtTermEndRequest : EntityRequest<ChargeAddonAtTermEndRequest>
        {
            public ChargeAddonAtTermEndRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ChargeAddonAtTermEndRequest AddonId(string addonId)
            {
                MParams.Add("addon_id", addonId);
                return this;
            }

            public ChargeAddonAtTermEndRequest AddonQuantity(int addonQuantity)
            {
                MParams.AddOpt("addon_quantity", addonQuantity);
                return this;
            }

            public ChargeAddonAtTermEndRequest AddonUnitPrice(int addonUnitPrice)
            {
                MParams.AddOpt("addon_unit_price", addonUnitPrice);
                return this;
            }

            public ChargeAddonAtTermEndRequest AddonQuantityInDecimal(string addonQuantityInDecimal)
            {
                MParams.AddOpt("addon_quantity_in_decimal", addonQuantityInDecimal);
                return this;
            }

            public ChargeAddonAtTermEndRequest AddonUnitPriceInDecimal(string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addon_unit_price_in_decimal", addonUnitPriceInDecimal);
                return this;
            }

            public ChargeAddonAtTermEndRequest DateFrom(long dateFrom)
            {
                MParams.AddOpt("date_from", dateFrom);
                return this;
            }

            public ChargeAddonAtTermEndRequest DateTo(long dateTo)
            {
                MParams.AddOpt("date_to", dateTo);
                return this;
            }
        }

        public class ChargeFutureRenewalsRequest : EntityRequest<ChargeFutureRenewalsRequest>
        {
            public ChargeFutureRenewalsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ChargeFutureRenewalsRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public ChargeFutureRenewalsRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }

            public ChargeFutureRenewalsRequest ScheduleType(ScheduleTypeEnum scheduleType)
            {
                MParams.AddOpt("schedule_type", scheduleType);
                return this;
            }

            public ChargeFutureRenewalsRequest FixedIntervalScheduleNumberOfOccurrences(
                int fixedIntervalScheduleNumberOfOccurrences)
            {
                MParams.AddOpt("fixed_interval_schedule[number_of_occurrences]",
                    fixedIntervalScheduleNumberOfOccurrences);
                return this;
            }

            public ChargeFutureRenewalsRequest FixedIntervalScheduleDaysBeforeRenewal(
                int fixedIntervalScheduleDaysBeforeRenewal)
            {
                MParams.AddOpt("fixed_interval_schedule[days_before_renewal]", fixedIntervalScheduleDaysBeforeRenewal);
                return this;
            }

            public ChargeFutureRenewalsRequest FixedIntervalScheduleEndScheduleOn(
                EndScheduleOnEnum fixedIntervalScheduleEndScheduleOn)
            {
                MParams.AddOpt("fixed_interval_schedule[end_schedule_on]", fixedIntervalScheduleEndScheduleOn);
                return this;
            }

            public ChargeFutureRenewalsRequest FixedIntervalScheduleEndDate(long fixedIntervalScheduleEndDate)
            {
                MParams.AddOpt("fixed_interval_schedule[end_date]", fixedIntervalScheduleEndDate);
                return this;
            }

            public ChargeFutureRenewalsRequest SpecificDatesScheduleTermsToCharge(int index,
                int specificDatesScheduleTermsToCharge)
            {
                MParams.AddOpt("specific_dates_schedule[terms_to_charge][" + index + "]",
                    specificDatesScheduleTermsToCharge);
                return this;
            }

            public ChargeFutureRenewalsRequest SpecificDatesScheduleDate(int index, long specificDatesScheduleDate)
            {
                MParams.AddOpt("specific_dates_schedule[date][" + index + "]", specificDatesScheduleDate);
                return this;
            }
        }

        public class EditAdvanceInvoiceScheduleRequest : EntityRequest<EditAdvanceInvoiceScheduleRequest>
        {
            public EditAdvanceInvoiceScheduleRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public EditAdvanceInvoiceScheduleRequest TermsToCharge(int termsToCharge)
            {
                MParams.AddOpt("terms_to_charge", termsToCharge);
                return this;
            }

            public EditAdvanceInvoiceScheduleRequest ScheduleType(ScheduleTypeEnum scheduleType)
            {
                MParams.AddOpt("schedule_type", scheduleType);
                return this;
            }

            public EditAdvanceInvoiceScheduleRequest FixedIntervalScheduleNumberOfOccurrences(
                int fixedIntervalScheduleNumberOfOccurrences)
            {
                MParams.AddOpt("fixed_interval_schedule[number_of_occurrences]",
                    fixedIntervalScheduleNumberOfOccurrences);
                return this;
            }

            public EditAdvanceInvoiceScheduleRequest FixedIntervalScheduleDaysBeforeRenewal(
                int fixedIntervalScheduleDaysBeforeRenewal)
            {
                MParams.AddOpt("fixed_interval_schedule[days_before_renewal]", fixedIntervalScheduleDaysBeforeRenewal);
                return this;
            }

            public EditAdvanceInvoiceScheduleRequest FixedIntervalScheduleEndScheduleOn(
                EndScheduleOnEnum fixedIntervalScheduleEndScheduleOn)
            {
                MParams.AddOpt("fixed_interval_schedule[end_schedule_on]", fixedIntervalScheduleEndScheduleOn);
                return this;
            }

            public EditAdvanceInvoiceScheduleRequest FixedIntervalScheduleEndDate(long fixedIntervalScheduleEndDate)
            {
                MParams.AddOpt("fixed_interval_schedule[end_date]", fixedIntervalScheduleEndDate);
                return this;
            }

            public EditAdvanceInvoiceScheduleRequest SpecificDatesScheduleId(int index, string specificDatesScheduleId)
            {
                MParams.AddOpt("specific_dates_schedule[id][" + index + "]", specificDatesScheduleId);
                return this;
            }

            public EditAdvanceInvoiceScheduleRequest SpecificDatesScheduleTermsToCharge(int index,
                int specificDatesScheduleTermsToCharge)
            {
                MParams.AddOpt("specific_dates_schedule[terms_to_charge][" + index + "]",
                    specificDatesScheduleTermsToCharge);
                return this;
            }

            public EditAdvanceInvoiceScheduleRequest SpecificDatesScheduleDate(int index,
                long specificDatesScheduleDate)
            {
                MParams.AddOpt("specific_dates_schedule[date][" + index + "]", specificDatesScheduleDate);
                return this;
            }
        }

        public class RemoveAdvanceInvoiceScheduleRequest : EntityRequest<RemoveAdvanceInvoiceScheduleRequest>
        {
            public RemoveAdvanceInvoiceScheduleRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RemoveAdvanceInvoiceScheduleRequest SpecificDatesScheduleId(int index,
                string specificDatesScheduleId)
            {
                MParams.AddOpt("specific_dates_schedule[id][" + index + "]", specificDatesScheduleId);
                return this;
            }
        }

        public class RegenerateInvoiceRequest : EntityRequest<RegenerateInvoiceRequest>
        {
            public RegenerateInvoiceRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RegenerateInvoiceRequest DateFrom(long dateFrom)
            {
                MParams.AddOpt("date_from", dateFrom);
                return this;
            }

            public RegenerateInvoiceRequest DateTo(long dateTo)
            {
                MParams.AddOpt("date_to", dateTo);
                return this;
            }

            public RegenerateInvoiceRequest Prorate(bool prorate)
            {
                MParams.AddOpt("prorate", prorate);
                return this;
            }

            public RegenerateInvoiceRequest InvoiceImmediately(bool invoiceImmediately)
            {
                MParams.AddOpt("invoice_immediately", invoiceImmediately);
                return this;
            }
        }

        public class ImportSubscriptionRequest : EntityRequest<ImportSubscriptionRequest>
        {
            public ImportSubscriptionRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ImportSubscriptionRequest Id(string id)
            {
                MParams.AddOpt("id", id);
                return this;
            }

            public ImportSubscriptionRequest ClientProfileId(string clientProfileId)
            {
                MParams.AddOpt("client_profile_id", clientProfileId);
                return this;
            }

            public ImportSubscriptionRequest PlanUnitPriceInDecimal(string planUnitPriceInDecimal)
            {
                MParams.AddOpt("plan_unit_price_in_decimal", planUnitPriceInDecimal);
                return this;
            }

            public ImportSubscriptionRequest PlanQuantityInDecimal(string planQuantityInDecimal)
            {
                MParams.AddOpt("plan_quantity_in_decimal", planQuantityInDecimal);
                return this;
            }

            public ImportSubscriptionRequest PlanId(string planId)
            {
                MParams.Add("plan_id", planId);
                return this;
            }

            public ImportSubscriptionRequest PlanQuantity(int planQuantity)
            {
                MParams.AddOpt("plan_quantity", planQuantity);
                return this;
            }

            public ImportSubscriptionRequest PlanUnitPrice(int planUnitPrice)
            {
                MParams.AddOpt("plan_unit_price", planUnitPrice);
                return this;
            }

            public ImportSubscriptionRequest SetupFee(int setupFee)
            {
                MParams.AddOpt("setup_fee", setupFee);
                return this;
            }

            public ImportSubscriptionRequest TrialEnd(long trialEnd)
            {
                MParams.AddOpt("trial_end", trialEnd);
                return this;
            }

            public ImportSubscriptionRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public ImportSubscriptionRequest StartDate(long startDate)
            {
                MParams.AddOpt("start_date", startDate);
                return this;
            }

            public ImportSubscriptionRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public ImportSubscriptionRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public ImportSubscriptionRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public ImportSubscriptionRequest ContractTermBillingCycleOnRenewal(int contractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("contract_term_billing_cycle_on_renewal", contractTermBillingCycleOnRenewal);
                return this;
            }

            public ImportSubscriptionRequest Status(StatusEnum status)
            {
                MParams.Add("status", status);
                return this;
            }

            public ImportSubscriptionRequest CurrentTermEnd(long currentTermEnd)
            {
                MParams.AddOpt("current_term_end", currentTermEnd);
                return this;
            }

            public ImportSubscriptionRequest CurrentTermStart(long currentTermStart)
            {
                MParams.AddOpt("current_term_start", currentTermStart);
                return this;
            }

            public ImportSubscriptionRequest TrialStart(long trialStart)
            {
                MParams.AddOpt("trial_start", trialStart);
                return this;
            }

            public ImportSubscriptionRequest CancelledAt(long cancelledAt)
            {
                MParams.AddOpt("cancelled_at", cancelledAt);
                return this;
            }

            public ImportSubscriptionRequest StartedAt(long startedAt)
            {
                MParams.AddOpt("started_at", startedAt);
                return this;
            }

            public ImportSubscriptionRequest PauseDate(long pauseDate)
            {
                MParams.AddOpt("pause_date", pauseDate);
                return this;
            }

            public ImportSubscriptionRequest ResumeDate(long resumeDate)
            {
                MParams.AddOpt("resume_date", resumeDate);
                return this;
            }

            public ImportSubscriptionRequest CreateCurrentTermInvoice(bool createCurrentTermInvoice)
            {
                MParams.AddOpt("create_current_term_invoice", createCurrentTermInvoice);
                return this;
            }

            public ImportSubscriptionRequest AffiliateToken(string affiliateToken)
            {
                MParams.AddOpt("affiliate_token", affiliateToken);
                return this;
            }

            public ImportSubscriptionRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public ImportSubscriptionRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public ImportSubscriptionRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer[id]", customerId);
                return this;
            }

            public ImportSubscriptionRequest CustomerEmail(string customerEmail)
            {
                MParams.AddOpt("customer[email]", customerEmail);
                return this;
            }

            public ImportSubscriptionRequest CustomerFirstName(string customerFirstName)
            {
                MParams.AddOpt("customer[first_name]", customerFirstName);
                return this;
            }

            public ImportSubscriptionRequest CustomerLastName(string customerLastName)
            {
                MParams.AddOpt("customer[last_name]", customerLastName);
                return this;
            }

            public ImportSubscriptionRequest CustomerCompany(string customerCompany)
            {
                MParams.AddOpt("customer[company]", customerCompany);
                return this;
            }

            public ImportSubscriptionRequest CustomerTaxability(TaxabilityEnum customerTaxability)
            {
                MParams.AddOpt("customer[taxability]", customerTaxability);
                return this;
            }

            public ImportSubscriptionRequest CustomerLocale(string customerLocale)
            {
                MParams.AddOpt("customer[locale]", customerLocale);
                return this;
            }

            public ImportSubscriptionRequest CustomerEntityCode(EntityCodeEnum customerEntityCode)
            {
                MParams.AddOpt("customer[entity_code]", customerEntityCode);
                return this;
            }

            public ImportSubscriptionRequest CustomerExemptNumber(string customerExemptNumber)
            {
                MParams.AddOpt("customer[exempt_number]", customerExemptNumber);
                return this;
            }

            public ImportSubscriptionRequest CustomerNetTermDays(int customerNetTermDays)
            {
                MParams.AddOpt("customer[net_term_days]", customerNetTermDays);
                return this;
            }

            public ImportSubscriptionRequest CustomerTaxjarExemptionCategory(
                TaxjarExemptionCategoryEnum customerTaxjarExemptionCategory)
            {
                MParams.AddOpt("customer[taxjar_exemption_category]", customerTaxjarExemptionCategory);
                return this;
            }

            public ImportSubscriptionRequest CustomerPhone(string customerPhone)
            {
                MParams.AddOpt("customer[phone]", customerPhone);
                return this;
            }

            public ImportSubscriptionRequest CustomerCustomerType(CustomerTypeEnum customerCustomerType)
            {
                MParams.AddOpt("customer[customer_type]", customerCustomerType);
                return this;
            }

            public ImportSubscriptionRequest CustomerAutoCollection(AutoCollectionEnum customerAutoCollection)
            {
                MParams.AddOpt("customer[auto_collection]", customerAutoCollection);
                return this;
            }

            public ImportSubscriptionRequest CustomerAllowDirectDebit(bool customerAllowDirectDebit)
            {
                MParams.AddOpt("customer[allow_direct_debit]", customerAllowDirectDebit);
                return this;
            }

            public ImportSubscriptionRequest ContractTermId(string contractTermId)
            {
                MParams.AddOpt("contract_term[id]", contractTermId);
                return this;
            }

            public ImportSubscriptionRequest ContractTermCreatedAt(long contractTermCreatedAt)
            {
                MParams.AddOpt("contract_term[created_at]", contractTermCreatedAt);
                return this;
            }

            public ImportSubscriptionRequest ContractTermContractStart(long contractTermContractStart)
            {
                MParams.AddOpt("contract_term[contract_start]", contractTermContractStart);
                return this;
            }

            public ImportSubscriptionRequest ContractTermBillingCycle(int contractTermBillingCycle)
            {
                MParams.AddOpt("contract_term[billing_cycle]", contractTermBillingCycle);
                return this;
            }

            public ImportSubscriptionRequest ContractTermTotalAmountRaised(long contractTermTotalAmountRaised)
            {
                MParams.AddOpt("contract_term[total_amount_raised]", contractTermTotalAmountRaised);
                return this;
            }

            public ImportSubscriptionRequest ContractTermActionAtTermEnd(
                SubscriptionContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public ImportSubscriptionRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            [Obsolete]
            public ImportSubscriptionRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public ImportSubscriptionRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            [Obsolete]
            public ImportSubscriptionRequest CardTmpToken(string cardTmpToken)
            {
                MParams.AddOpt("card[tmp_token]", cardTmpToken);
                return this;
            }

            public ImportSubscriptionRequest PaymentMethodType(TypeEnum paymentMethodType)
            {
                MParams.AddOpt("payment_method[type]", paymentMethodType);
                return this;
            }

            [Obsolete]
            public ImportSubscriptionRequest PaymentMethodGateway(GatewayEnum paymentMethodGateway)
            {
                MParams.AddOpt("payment_method[gateway]", paymentMethodGateway);
                return this;
            }

            public ImportSubscriptionRequest PaymentMethodGatewayAccountId(string paymentMethodGatewayAccountId)
            {
                MParams.AddOpt("payment_method[gateway_account_id]", paymentMethodGatewayAccountId);
                return this;
            }

            public ImportSubscriptionRequest PaymentMethodReferenceId(string paymentMethodReferenceId)
            {
                MParams.AddOpt("payment_method[reference_id]", paymentMethodReferenceId);
                return this;
            }

            public ImportSubscriptionRequest PaymentMethodIssuingCountry(string paymentMethodIssuingCountry)
            {
                MParams.AddOpt("payment_method[issuing_country]", paymentMethodIssuingCountry);
                return this;
            }

            public ImportSubscriptionRequest CardFirstName(string cardFirstName)
            {
                MParams.AddOpt("card[first_name]", cardFirstName);
                return this;
            }

            public ImportSubscriptionRequest CardLastName(string cardLastName)
            {
                MParams.AddOpt("card[last_name]", cardLastName);
                return this;
            }

            public ImportSubscriptionRequest CardNumber(string cardNumber)
            {
                MParams.AddOpt("card[number]", cardNumber);
                return this;
            }

            public ImportSubscriptionRequest CardExpiryMonth(int cardExpiryMonth)
            {
                MParams.AddOpt("card[expiry_month]", cardExpiryMonth);
                return this;
            }

            public ImportSubscriptionRequest CardExpiryYear(int cardExpiryYear)
            {
                MParams.AddOpt("card[expiry_year]", cardExpiryYear);
                return this;
            }

            public ImportSubscriptionRequest CardCvv(string cardCvv)
            {
                MParams.AddOpt("card[cvv]", cardCvv);
                return this;
            }

            public ImportSubscriptionRequest CardBillingAddr1(string cardBillingAddr1)
            {
                MParams.AddOpt("card[billing_addr1]", cardBillingAddr1);
                return this;
            }

            public ImportSubscriptionRequest CardBillingAddr2(string cardBillingAddr2)
            {
                MParams.AddOpt("card[billing_addr2]", cardBillingAddr2);
                return this;
            }

            public ImportSubscriptionRequest CardBillingCity(string cardBillingCity)
            {
                MParams.AddOpt("card[billing_city]", cardBillingCity);
                return this;
            }

            public ImportSubscriptionRequest CardBillingStateCode(string cardBillingStateCode)
            {
                MParams.AddOpt("card[billing_state_code]", cardBillingStateCode);
                return this;
            }

            public ImportSubscriptionRequest CardBillingState(string cardBillingState)
            {
                MParams.AddOpt("card[billing_state]", cardBillingState);
                return this;
            }

            public ImportSubscriptionRequest CardBillingZip(string cardBillingZip)
            {
                MParams.AddOpt("card[billing_zip]", cardBillingZip);
                return this;
            }

            public ImportSubscriptionRequest CardBillingCountry(string cardBillingCountry)
            {
                MParams.AddOpt("card[billing_country]", cardBillingCountry);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public ImportSubscriptionRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public ImportSubscriptionRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public ImportSubscriptionRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }

            public ImportSubscriptionRequest TransactionAmount(int transactionAmount)
            {
                MParams.AddOpt("transaction[amount]", transactionAmount);
                return this;
            }

            public ImportSubscriptionRequest TransactionPaymentMethod(PaymentMethodEnum transactionPaymentMethod)
            {
                MParams.AddOpt("transaction[payment_method]", transactionPaymentMethod);
                return this;
            }

            public ImportSubscriptionRequest TransactionReferenceNumber(string transactionReferenceNumber)
            {
                MParams.AddOpt("transaction[reference_number]", transactionReferenceNumber);
                return this;
            }

            public ImportSubscriptionRequest TransactionDate(long transactionDate)
            {
                MParams.AddOpt("transaction[date]", transactionDate);
                return this;
            }

            public ImportSubscriptionRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public ImportSubscriptionRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public ImportSubscriptionRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public ImportSubscriptionRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public ImportSubscriptionRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public ImportSubscriptionRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public ImportSubscriptionRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public ImportSubscriptionRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public ImportSubscriptionRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public ImportSubscriptionRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public ImportSubscriptionRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public ImportSubscriptionRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public ImportSubscriptionRequest EventBasedAddonOnEvent(int index, OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public ImportSubscriptionRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public ImportSubscriptionRequest ChargedEventBasedAddonId(int index, string chargedEventBasedAddonId)
            {
                MParams.AddOpt("charged_event_based_addons[id][" + index + "]", chargedEventBasedAddonId);
                return this;
            }

            public ImportSubscriptionRequest ChargedEventBasedAddonLastChargedAt(int index,
                long chargedEventBasedAddonLastChargedAt)
            {
                MParams.AddOpt("charged_event_based_addons[last_charged_at][" + index + "]",
                    chargedEventBasedAddonLastChargedAt);
                return this;
            }
        }

        public class ImportForCustomerRequest : EntityRequest<ImportForCustomerRequest>
        {
            public ImportForCustomerRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ImportForCustomerRequest Id(string id)
            {
                MParams.AddOpt("id", id);
                return this;
            }

            public ImportForCustomerRequest PlanUnitPriceInDecimal(string planUnitPriceInDecimal)
            {
                MParams.AddOpt("plan_unit_price_in_decimal", planUnitPriceInDecimal);
                return this;
            }

            public ImportForCustomerRequest PlanQuantityInDecimal(string planQuantityInDecimal)
            {
                MParams.AddOpt("plan_quantity_in_decimal", planQuantityInDecimal);
                return this;
            }

            public ImportForCustomerRequest PlanId(string planId)
            {
                MParams.Add("plan_id", planId);
                return this;
            }

            public ImportForCustomerRequest PlanQuantity(int planQuantity)
            {
                MParams.AddOpt("plan_quantity", planQuantity);
                return this;
            }

            public ImportForCustomerRequest PlanUnitPrice(int planUnitPrice)
            {
                MParams.AddOpt("plan_unit_price", planUnitPrice);
                return this;
            }

            public ImportForCustomerRequest SetupFee(int setupFee)
            {
                MParams.AddOpt("setup_fee", setupFee);
                return this;
            }

            public ImportForCustomerRequest TrialEnd(long trialEnd)
            {
                MParams.AddOpt("trial_end", trialEnd);
                return this;
            }

            public ImportForCustomerRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            public ImportForCustomerRequest StartDate(long startDate)
            {
                MParams.AddOpt("start_date", startDate);
                return this;
            }

            public ImportForCustomerRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public ImportForCustomerRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public ImportForCustomerRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public ImportForCustomerRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public ImportForCustomerRequest Status(StatusEnum status)
            {
                MParams.Add("status", status);
                return this;
            }

            public ImportForCustomerRequest CurrentTermEnd(long currentTermEnd)
            {
                MParams.AddOpt("current_term_end", currentTermEnd);
                return this;
            }

            public ImportForCustomerRequest CurrentTermStart(long currentTermStart)
            {
                MParams.AddOpt("current_term_start", currentTermStart);
                return this;
            }

            public ImportForCustomerRequest TrialStart(long trialStart)
            {
                MParams.AddOpt("trial_start", trialStart);
                return this;
            }

            public ImportForCustomerRequest CancelledAt(long cancelledAt)
            {
                MParams.AddOpt("cancelled_at", cancelledAt);
                return this;
            }

            public ImportForCustomerRequest StartedAt(long startedAt)
            {
                MParams.AddOpt("started_at", startedAt);
                return this;
            }

            public ImportForCustomerRequest PauseDate(long pauseDate)
            {
                MParams.AddOpt("pause_date", pauseDate);
                return this;
            }

            public ImportForCustomerRequest ResumeDate(long resumeDate)
            {
                MParams.AddOpt("resume_date", resumeDate);
                return this;
            }

            public ImportForCustomerRequest ContractTermBillingCycleOnRenewal(int contractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("contract_term_billing_cycle_on_renewal", contractTermBillingCycleOnRenewal);
                return this;
            }

            public ImportForCustomerRequest CreateCurrentTermInvoice(bool createCurrentTermInvoice)
            {
                MParams.AddOpt("create_current_term_invoice", createCurrentTermInvoice);
                return this;
            }

            public ImportForCustomerRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public ImportForCustomerRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public ImportForCustomerRequest ContractTermId(string contractTermId)
            {
                MParams.AddOpt("contract_term[id]", contractTermId);
                return this;
            }

            public ImportForCustomerRequest ContractTermCreatedAt(long contractTermCreatedAt)
            {
                MParams.AddOpt("contract_term[created_at]", contractTermCreatedAt);
                return this;
            }

            public ImportForCustomerRequest ContractTermContractStart(long contractTermContractStart)
            {
                MParams.AddOpt("contract_term[contract_start]", contractTermContractStart);
                return this;
            }

            public ImportForCustomerRequest ContractTermBillingCycle(int contractTermBillingCycle)
            {
                MParams.AddOpt("contract_term[billing_cycle]", contractTermBillingCycle);
                return this;
            }

            public ImportForCustomerRequest ContractTermTotalAmountRaised(long contractTermTotalAmountRaised)
            {
                MParams.AddOpt("contract_term[total_amount_raised]", contractTermTotalAmountRaised);
                return this;
            }

            public ImportForCustomerRequest ContractTermActionAtTermEnd(
                SubscriptionContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public ImportForCustomerRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public ImportForCustomerRequest TransactionAmount(int transactionAmount)
            {
                MParams.AddOpt("transaction[amount]", transactionAmount);
                return this;
            }

            public ImportForCustomerRequest TransactionPaymentMethod(PaymentMethodEnum transactionPaymentMethod)
            {
                MParams.AddOpt("transaction[payment_method]", transactionPaymentMethod);
                return this;
            }

            public ImportForCustomerRequest TransactionReferenceNumber(string transactionReferenceNumber)
            {
                MParams.AddOpt("transaction[reference_number]", transactionReferenceNumber);
                return this;
            }

            public ImportForCustomerRequest TransactionDate(long transactionDate)
            {
                MParams.AddOpt("transaction[date]", transactionDate);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public ImportForCustomerRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public ImportForCustomerRequest AddonId(int index, string addonId)
            {
                MParams.AddOpt("addons[id][" + index + "]", addonId);
                return this;
            }

            public ImportForCustomerRequest AddonQuantity(int index, int addonQuantity)
            {
                MParams.AddOpt("addons[quantity][" + index + "]", addonQuantity);
                return this;
            }

            public ImportForCustomerRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public ImportForCustomerRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public ImportForCustomerRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public ImportForCustomerRequest AddonBillingCycles(int index, int addonBillingCycles)
            {
                MParams.AddOpt("addons[billing_cycles][" + index + "]", addonBillingCycles);
                return this;
            }

            public ImportForCustomerRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public ImportForCustomerRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public ImportForCustomerRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public ImportForCustomerRequest EventBasedAddonQuantityInDecimal(int index,
                string eventBasedAddonQuantityInDecimal)
            {
                MParams.AddOpt("event_based_addons[quantity_in_decimal][" + index + "]",
                    eventBasedAddonQuantityInDecimal);
                return this;
            }

            public ImportForCustomerRequest EventBasedAddonUnitPriceInDecimal(int index,
                string eventBasedAddonUnitPriceInDecimal)
            {
                MParams.AddOpt("event_based_addons[unit_price_in_decimal][" + index + "]",
                    eventBasedAddonUnitPriceInDecimal);
                return this;
            }

            public ImportForCustomerRequest EventBasedAddonServicePeriodInDays(int index,
                int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }

            public ImportForCustomerRequest EventBasedAddonOnEvent(int index, OnEventEnum eventBasedAddonOnEvent)
            {
                MParams.AddOpt("event_based_addons[on_event][" + index + "]", eventBasedAddonOnEvent);
                return this;
            }

            public ImportForCustomerRequest EventBasedAddonChargeOnce(int index, bool eventBasedAddonChargeOnce)
            {
                MParams.AddOpt("event_based_addons[charge_once][" + index + "]", eventBasedAddonChargeOnce);
                return this;
            }

            public ImportForCustomerRequest ChargedEventBasedAddonId(int index, string chargedEventBasedAddonId)
            {
                MParams.AddOpt("charged_event_based_addons[id][" + index + "]", chargedEventBasedAddonId);
                return this;
            }

            public ImportForCustomerRequest ChargedEventBasedAddonLastChargedAt(int index,
                long chargedEventBasedAddonLastChargedAt)
            {
                MParams.AddOpt("charged_event_based_addons[last_charged_at][" + index + "]",
                    chargedEventBasedAddonLastChargedAt);
                return this;
            }
        }

        public class ImportContractTermRequest : EntityRequest<ImportContractTermRequest>
        {
            public ImportContractTermRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ImportContractTermRequest ContractTermBillingCycleOnRenewal(int contractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("contract_term_billing_cycle_on_renewal", contractTermBillingCycleOnRenewal);
                return this;
            }

            public ImportContractTermRequest ContractTermId(string contractTermId)
            {
                MParams.AddOpt("contract_term[id]", contractTermId);
                return this;
            }

            public ImportContractTermRequest ContractTermCreatedAt(long contractTermCreatedAt)
            {
                MParams.AddOpt("contract_term[created_at]", contractTermCreatedAt);
                return this;
            }

            public ImportContractTermRequest ContractTermContractStart(long contractTermContractStart)
            {
                MParams.AddOpt("contract_term[contract_start]", contractTermContractStart);
                return this;
            }

            public ImportContractTermRequest ContractTermContractEnd(long contractTermContractEnd)
            {
                MParams.AddOpt("contract_term[contract_end]", contractTermContractEnd);
                return this;
            }

            public ImportContractTermRequest ContractTermStatus(SubscriptionContractTerm.StatusEnum contractTermStatus)
            {
                MParams.AddOpt("contract_term[status]", contractTermStatus);
                return this;
            }

            public ImportContractTermRequest ContractTermTotalAmountRaised(long contractTermTotalAmountRaised)
            {
                MParams.AddOpt("contract_term[total_amount_raised]", contractTermTotalAmountRaised);
                return this;
            }

            public ImportContractTermRequest ContractTermTotalContractValue(long contractTermTotalContractValue)
            {
                MParams.AddOpt("contract_term[total_contract_value]", contractTermTotalContractValue);
                return this;
            }

            public ImportContractTermRequest ContractTermBillingCycle(int contractTermBillingCycle)
            {
                MParams.AddOpt("contract_term[billing_cycle]", contractTermBillingCycle);
                return this;
            }

            public ImportContractTermRequest ContractTermActionAtTermEnd(
                SubscriptionContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public ImportContractTermRequest ContractTermCancellationCutoffPeriod(
                int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }
        }

        public class ImportForItemsRequest : EntityRequest<ImportForItemsRequest>
        {
            public ImportForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ImportForItemsRequest Id(string id)
            {
                MParams.AddOpt("id", id);
                return this;
            }

            public ImportForItemsRequest TrialEnd(long trialEnd)
            {
                MParams.AddOpt("trial_end", trialEnd);
                return this;
            }

            public ImportForItemsRequest BillingCycles(int billingCycles)
            {
                MParams.AddOpt("billing_cycles", billingCycles);
                return this;
            }

            [Obsolete]
            public ImportForItemsRequest SetupFee(int setupFee)
            {
                MParams.AddOpt("setup_fee", setupFee);
                return this;
            }

            public ImportForItemsRequest StartDate(long startDate)
            {
                MParams.AddOpt("start_date", startDate);
                return this;
            }

            public ImportForItemsRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public ImportForItemsRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public ImportForItemsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public ImportForItemsRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public ImportForItemsRequest Status(StatusEnum status)
            {
                MParams.Add("status", status);
                return this;
            }

            public ImportForItemsRequest CurrentTermEnd(long currentTermEnd)
            {
                MParams.AddOpt("current_term_end", currentTermEnd);
                return this;
            }

            public ImportForItemsRequest CurrentTermStart(long currentTermStart)
            {
                MParams.AddOpt("current_term_start", currentTermStart);
                return this;
            }

            public ImportForItemsRequest TrialStart(long trialStart)
            {
                MParams.AddOpt("trial_start", trialStart);
                return this;
            }

            public ImportForItemsRequest CancelledAt(long cancelledAt)
            {
                MParams.AddOpt("cancelled_at", cancelledAt);
                return this;
            }

            public ImportForItemsRequest StartedAt(long startedAt)
            {
                MParams.AddOpt("started_at", startedAt);
                return this;
            }

            public ImportForItemsRequest PauseDate(long pauseDate)
            {
                MParams.AddOpt("pause_date", pauseDate);
                return this;
            }

            public ImportForItemsRequest ResumeDate(long resumeDate)
            {
                MParams.AddOpt("resume_date", resumeDate);
                return this;
            }

            public ImportForItemsRequest ContractTermBillingCycleOnRenewal(int contractTermBillingCycleOnRenewal)
            {
                MParams.AddOpt("contract_term_billing_cycle_on_renewal", contractTermBillingCycleOnRenewal);
                return this;
            }

            public ImportForItemsRequest CreateCurrentTermInvoice(bool createCurrentTermInvoice)
            {
                MParams.AddOpt("create_current_term_invoice", createCurrentTermInvoice);
                return this;
            }

            public ImportForItemsRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public ImportForItemsRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public ImportForItemsRequest CreatePendingInvoices(bool createPendingInvoices)
            {
                MParams.AddOpt("create_pending_invoices", createPendingInvoices);
                return this;
            }

            public ImportForItemsRequest AutoCloseInvoices(bool autoCloseInvoices)
            {
                MParams.AddOpt("auto_close_invoices", autoCloseInvoices);
                return this;
            }

            public ImportForItemsRequest ContractTermId(string contractTermId)
            {
                MParams.AddOpt("contract_term[id]", contractTermId);
                return this;
            }

            public ImportForItemsRequest ContractTermCreatedAt(long contractTermCreatedAt)
            {
                MParams.AddOpt("contract_term[created_at]", contractTermCreatedAt);
                return this;
            }

            public ImportForItemsRequest ContractTermContractStart(long contractTermContractStart)
            {
                MParams.AddOpt("contract_term[contract_start]", contractTermContractStart);
                return this;
            }

            public ImportForItemsRequest ContractTermBillingCycle(int contractTermBillingCycle)
            {
                MParams.AddOpt("contract_term[billing_cycle]", contractTermBillingCycle);
                return this;
            }

            public ImportForItemsRequest ContractTermTotalAmountRaised(long contractTermTotalAmountRaised)
            {
                MParams.AddOpt("contract_term[total_amount_raised]", contractTermTotalAmountRaised);
                return this;
            }

            public ImportForItemsRequest ContractTermActionAtTermEnd(
                SubscriptionContractTerm.ActionAtTermEndEnum contractTermActionAtTermEnd)
            {
                MParams.AddOpt("contract_term[action_at_term_end]", contractTermActionAtTermEnd);
                return this;
            }

            public ImportForItemsRequest ContractTermCancellationCutoffPeriod(int contractTermCancellationCutoffPeriod)
            {
                MParams.AddOpt("contract_term[cancellation_cutoff_period]", contractTermCancellationCutoffPeriod);
                return this;
            }

            public ImportForItemsRequest TransactionAmount(int transactionAmount)
            {
                MParams.AddOpt("transaction[amount]", transactionAmount);
                return this;
            }

            public ImportForItemsRequest TransactionPaymentMethod(PaymentMethodEnum transactionPaymentMethod)
            {
                MParams.AddOpt("transaction[payment_method]", transactionPaymentMethod);
                return this;
            }

            public ImportForItemsRequest TransactionReferenceNumber(string transactionReferenceNumber)
            {
                MParams.AddOpt("transaction[reference_number]", transactionReferenceNumber);
                return this;
            }

            public ImportForItemsRequest TransactionDate(long transactionDate)
            {
                MParams.AddOpt("transaction[date]", transactionDate);
                return this;
            }

            public ImportForItemsRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public ImportForItemsRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public ImportForItemsRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public ImportForItemsRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public ImportForItemsRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public ImportForItemsRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public ImportForItemsRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public ImportForItemsRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public ImportForItemsRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public ImportForItemsRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public ImportForItemsRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public ImportForItemsRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public ImportForItemsRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public ImportForItemsRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public ImportForItemsRequest SubscriptionItemItemPriceId(int index, string subscriptionItemItemPriceId)
            {
                MParams.Add("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public ImportForItemsRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public ImportForItemsRequest SubscriptionItemUnitPrice(int index, int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public ImportForItemsRequest SubscriptionItemBillingCycles(int index, int subscriptionItemBillingCycles)
            {
                MParams.AddOpt("subscription_items[billing_cycles][" + index + "]", subscriptionItemBillingCycles);
                return this;
            }

            public ImportForItemsRequest SubscriptionItemTrialEnd(int index, long subscriptionItemTrialEnd)
            {
                MParams.AddOpt("subscription_items[trial_end][" + index + "]", subscriptionItemTrialEnd);
                return this;
            }

            public ImportForItemsRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }

            public ImportForItemsRequest SubscriptionItemChargeOnEvent(int index,
                ChargeOnEventEnum subscriptionItemChargeOnEvent)
            {
                MParams.AddOpt("subscription_items[charge_on_event][" + index + "]", subscriptionItemChargeOnEvent);
                return this;
            }

            public ImportForItemsRequest SubscriptionItemChargeOnce(int index, bool subscriptionItemChargeOnce)
            {
                MParams.AddOpt("subscription_items[charge_once][" + index + "]", subscriptionItemChargeOnce);
                return this;
            }

            [Obsolete]
            public ImportForItemsRequest SubscriptionItemItemType(int index, ItemTypeEnum subscriptionItemItemType)
            {
                MParams.AddOpt("subscription_items[item_type][" + index + "]", subscriptionItemItemType);
                return this;
            }

            public ImportForItemsRequest ChargedItemItemPriceId(int index, string chargedItemItemPriceId)
            {
                MParams.AddOpt("charged_items[item_price_id][" + index + "]", chargedItemItemPriceId);
                return this;
            }

            public ImportForItemsRequest ChargedItemLastChargedAt(int index, long chargedItemLastChargedAt)
            {
                MParams.AddOpt("charged_items[last_charged_at][" + index + "]", chargedItemLastChargedAt);
                return this;
            }

            public ImportForItemsRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public ImportForItemsRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public ImportForItemsRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public ImportForItemsRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class OverrideBillingProfileRequest : EntityRequest<OverrideBillingProfileRequest>
        {
            public OverrideBillingProfileRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public OverrideBillingProfileRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public OverrideBillingProfileRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }
        }

        public class PauseRequest : EntityRequest<PauseRequest>
        {
            public PauseRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public PauseRequest PauseOption(PauseOptionEnum pauseOption)
            {
                MParams.AddOpt("pause_option", pauseOption);
                return this;
            }

            public PauseRequest PauseDate(long pauseDate)
            {
                MParams.AddOpt("pause_date", pauseDate);
                return this;
            }

            public PauseRequest UnbilledChargesHandling(UnbilledChargesHandlingEnum unbilledChargesHandling)
            {
                MParams.AddOpt("unbilled_charges_handling", unbilledChargesHandling);
                return this;
            }

            public PauseRequest InvoiceDunningHandling(InvoiceDunningHandlingEnum invoiceDunningHandling)
            {
                MParams.AddOpt("invoice_dunning_handling", invoiceDunningHandling);
                return this;
            }

            public PauseRequest ResumeDate(long resumeDate)
            {
                MParams.AddOpt("resume_date", resumeDate);
                return this;
            }
        }

        public class CancelRequest : EntityRequest<CancelRequest>
        {
            public CancelRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CancelRequest EndOfTerm(bool endOfTerm)
            {
                MParams.AddOpt("end_of_term", endOfTerm);
                return this;
            }

            public CancelRequest CancelAt(long cancelAt)
            {
                MParams.AddOpt("cancel_at", cancelAt);
                return this;
            }

            public CancelRequest CreditOptionForCurrentTermCharges(
                CreditOptionForCurrentTermChargesEnum creditOptionForCurrentTermCharges)
            {
                MParams.AddOpt("credit_option_for_current_term_charges", creditOptionForCurrentTermCharges);
                return this;
            }

            public CancelRequest UnbilledChargesOption(UnbilledChargesOptionEnum unbilledChargesOption)
            {
                MParams.AddOpt("unbilled_charges_option", unbilledChargesOption);
                return this;
            }

            public CancelRequest AccountReceivablesHandling(AccountReceivablesHandlingEnum accountReceivablesHandling)
            {
                MParams.AddOpt("account_receivables_handling", accountReceivablesHandling);
                return this;
            }

            public CancelRequest RefundableCreditsHandling(RefundableCreditsHandlingEnum refundableCreditsHandling)
            {
                MParams.AddOpt("refundable_credits_handling", refundableCreditsHandling);
                return this;
            }

            public CancelRequest ContractTermCancelOption(ContractTermCancelOptionEnum contractTermCancelOption)
            {
                MParams.AddOpt("contract_term_cancel_option", contractTermCancelOption);
                return this;
            }

            public CancelRequest CancelReasonCode(string cancelReasonCode)
            {
                MParams.AddOpt("cancel_reason_code", cancelReasonCode);
                return this;
            }

            public CancelRequest EventBasedAddonId(int index, string eventBasedAddonId)
            {
                MParams.AddOpt("event_based_addons[id][" + index + "]", eventBasedAddonId);
                return this;
            }

            public CancelRequest EventBasedAddonQuantity(int index, int eventBasedAddonQuantity)
            {
                MParams.AddOpt("event_based_addons[quantity][" + index + "]", eventBasedAddonQuantity);
                return this;
            }

            public CancelRequest EventBasedAddonUnitPrice(int index, int eventBasedAddonUnitPrice)
            {
                MParams.AddOpt("event_based_addons[unit_price][" + index + "]", eventBasedAddonUnitPrice);
                return this;
            }

            public CancelRequest EventBasedAddonServicePeriodInDays(int index, int eventBasedAddonServicePeriodInDays)
            {
                MParams.AddOpt("event_based_addons[service_period_in_days][" + index + "]",
                    eventBasedAddonServicePeriodInDays);
                return this;
            }
        }

        public class CancelForItemsRequest : EntityRequest<CancelForItemsRequest>
        {
            public CancelForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CancelForItemsRequest EndOfTerm(bool endOfTerm)
            {
                MParams.AddOpt("end_of_term", endOfTerm);
                return this;
            }

            public CancelForItemsRequest CancelAt(long cancelAt)
            {
                MParams.AddOpt("cancel_at", cancelAt);
                return this;
            }

            public CancelForItemsRequest CreditOptionForCurrentTermCharges(
                CreditOptionForCurrentTermChargesEnum creditOptionForCurrentTermCharges)
            {
                MParams.AddOpt("credit_option_for_current_term_charges", creditOptionForCurrentTermCharges);
                return this;
            }

            public CancelForItemsRequest UnbilledChargesOption(UnbilledChargesOptionEnum unbilledChargesOption)
            {
                MParams.AddOpt("unbilled_charges_option", unbilledChargesOption);
                return this;
            }

            public CancelForItemsRequest AccountReceivablesHandling(
                AccountReceivablesHandlingEnum accountReceivablesHandling)
            {
                MParams.AddOpt("account_receivables_handling", accountReceivablesHandling);
                return this;
            }

            public CancelForItemsRequest RefundableCreditsHandling(
                RefundableCreditsHandlingEnum refundableCreditsHandling)
            {
                MParams.AddOpt("refundable_credits_handling", refundableCreditsHandling);
                return this;
            }

            public CancelForItemsRequest ContractTermCancelOption(ContractTermCancelOptionEnum contractTermCancelOption)
            {
                MParams.AddOpt("contract_term_cancel_option", contractTermCancelOption);
                return this;
            }

            public CancelForItemsRequest CancelReasonCode(string cancelReasonCode)
            {
                MParams.AddOpt("cancel_reason_code", cancelReasonCode);
                return this;
            }

            public CancelForItemsRequest SubscriptionItemItemPriceId(int index, string subscriptionItemItemPriceId)
            {
                MParams.AddOpt("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public CancelForItemsRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }

            public CancelForItemsRequest SubscriptionItemUnitPrice(int index, int subscriptionItemUnitPrice)
            {
                MParams.AddOpt("subscription_items[unit_price][" + index + "]", subscriptionItemUnitPrice);
                return this;
            }

            public CancelForItemsRequest SubscriptionItemServicePeriodDays(int index,
                int subscriptionItemServicePeriodDays)
            {
                MParams.AddOpt("subscription_items[service_period_days][" + index + "]",
                    subscriptionItemServicePeriodDays);
                return this;
            }
        }

        public class ResumeRequest : EntityRequest<ResumeRequest>
        {
            public ResumeRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ResumeRequest ResumeOption(ResumeOptionEnum resumeOption)
            {
                MParams.AddOpt("resume_option", resumeOption);
                return this;
            }

            public ResumeRequest ResumeDate(long resumeDate)
            {
                MParams.AddOpt("resume_date", resumeDate);
                return this;
            }

            public ResumeRequest ChargesHandling(ChargesHandlingEnum chargesHandling)
            {
                MParams.AddOpt("charges_handling", chargesHandling);
                return this;
            }

            public ResumeRequest UnpaidInvoicesHandling(UnpaidInvoicesHandlingEnum unpaidInvoicesHandling)
            {
                MParams.AddOpt("unpaid_invoices_handling", unpaidInvoicesHandling);
                return this;
            }

            public ResumeRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public ResumeRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public ResumeRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public ResumeRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public ResumeRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public ResumeRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }
        }

        #endregion

        #region Subclasses

        public class SubscriptionSubscriptionItem : Resource
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

            public string MeteredQuantity()
            {
                return GetValue<string>("metered_quantity", false);
            }

            public DateTime? LastCalculatedAt()
            {
                return GetDateTime("last_calculated_at", false);
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

        public class SubscriptionItemTier : Resource
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

        public class SubscriptionChargedItem : Resource
        {
            public string ItemPriceId()
            {
                return GetValue<string>("item_price_id");
            }

            public DateTime LastChargedAt()
            {
                return (DateTime) GetDateTime("last_charged_at");
            }
        }

        public class SubscriptionAddon : Resource
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

        public class SubscriptionEventBasedAddon : Resource
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

        public class SubscriptionChargedEventBasedAddon : Resource
        {
            public string Id()
            {
                return GetValue<string>("id");
            }

            public DateTime LastChargedAt()
            {
                return (DateTime) GetDateTime("last_charged_at");
            }
        }

        public class SubscriptionCoupon : Resource
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

        public class SubscriptionShippingAddress : Resource
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

        public class SubscriptionReferralInfo : Resource
        {
            public enum RewardStatusEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "pending")] Pending,
                [EnumMember(Value = "paid")] Paid,
                [EnumMember(Value = "invalid")] Invalid
            }

            public string ReferralCode()
            {
                return GetValue<string>("referral_code", false);
            }

            public string CouponCode()
            {
                return GetValue<string>("coupon_code", false);
            }

            public string ReferrerId()
            {
                return GetValue<string>("referrer_id", false);
            }

            public string ExternalReferenceId()
            {
                return GetValue<string>("external_reference_id", false);
            }

            public RewardStatusEnum? RewardStatus()
            {
                return GetEnum<RewardStatusEnum>("reward_status", false);
            }

            public ReferralSystemEnum? ReferralSystem()
            {
                return GetEnum<ReferralSystemEnum>("referral_system", false);
            }

            public string AccountId()
            {
                return GetValue<string>("account_id");
            }

            public string CampaignId()
            {
                return GetValue<string>("campaign_id");
            }

            public string ExternalCampaignId()
            {
                return GetValue<string>("external_campaign_id", false);
            }

            public FriendOfferTypeEnum? FriendOfferType()
            {
                return GetEnum<FriendOfferTypeEnum>("friend_offer_type", false);
            }

            public ReferrerRewardTypeEnum? ReferrerRewardType()
            {
                return GetEnum<ReferrerRewardTypeEnum>("referrer_reward_type", false);
            }

            public NotifyReferralSystemEnum? NotifyReferralSystem()
            {
                return GetEnum<NotifyReferralSystemEnum>("notify_referral_system", false);
            }

            public string DestinationUrl()
            {
                return GetValue<string>("destination_url", false);
            }

            public bool PostPurchaseWidgetEnabled()
            {
                return GetValue<bool>("post_purchase_widget_enabled");
            }
        }

        public class SubscriptionContractTerm : Resource
        {
            public enum ActionAtTermEndEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "renew")] Renew,
                [EnumMember(Value = "evergreen")] Evergreen,
                [EnumMember(Value = "cancel")] Cancel,
                [EnumMember(Value = "renew_once")] RenewOnce
            }

            public enum StatusEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "active")] Active,
                [EnumMember(Value = "completed")] Completed,
                [EnumMember(Value = "cancelled")] Cancelled,
                [EnumMember(Value = "terminated")] Terminated
            }

            public string Id()
            {
                return GetValue<string>("id");
            }

            public StatusEnum Status()
            {
                return GetEnum<StatusEnum>("status");
            }

            public DateTime ContractStart()
            {
                return (DateTime) GetDateTime("contract_start");
            }

            public DateTime ContractEnd()
            {
                return (DateTime) GetDateTime("contract_end");
            }

            public int BillingCycle()
            {
                return GetValue<int>("billing_cycle");
            }

            public ActionAtTermEndEnum ActionAtTermEnd()
            {
                return GetEnum<ActionAtTermEndEnum>("action_at_term_end");
            }

            public long TotalContractValue()
            {
                return GetValue<long>("total_contract_value");
            }

            public int? CancellationCutoffPeriod()
            {
                return GetValue<int?>("cancellation_cutoff_period", false);
            }

            public DateTime CreatedAt()
            {
                return (DateTime) GetDateTime("created_at");
            }

            public string SubscriptionId()
            {
                return GetValue<string>("subscription_id");
            }

            public int? RemainingBillingCycles()
            {
                return GetValue<int?>("remaining_billing_cycles", false);
            }
        }

        #endregion
    }
}