using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Export : Resource
    {
        public enum MimeTypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "pdf")] Pdf,
            [EnumMember(Value = "zip")] Zip
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "in_process")] InProcess,
            [EnumMember(Value = "completed")] Completed,
            [EnumMember(Value = "failed")] Failed
        }

        public Export()
        {
        }

        public Export(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Export(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Export(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Subclasses

        public class ExportDownload : Resource
        {
            public string DownloadUrl()
            {
                return GetValue<string>("download_url");
            }

            public DateTime ValidTill()
            {
                return (DateTime) GetDateTime("valid_till");
            }
        }

        #endregion

        #region Methods

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("exports", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static RevenueRecognitionRequest RevenueRecognition()
        {
            var url = ApiUtil.BuildUrl("exports", "revenue_recognition");
            return new RevenueRecognitionRequest(url, HttpMethod.Post);
        }

        public static DeferredRevenueRequest DeferredRevenue()
        {
            var url = ApiUtil.BuildUrl("exports", "deferred_revenue");
            return new DeferredRevenueRequest(url, HttpMethod.Post);
        }

        public static PlansRequest Plans()
        {
            var url = ApiUtil.BuildUrl("exports", "plans");
            return new PlansRequest(url, HttpMethod.Post);
        }

        public static AddonsRequest Addons()
        {
            var url = ApiUtil.BuildUrl("exports", "addons");
            return new AddonsRequest(url, HttpMethod.Post);
        }

        public static CouponsRequest Coupons()
        {
            var url = ApiUtil.BuildUrl("exports", "coupons");
            return new CouponsRequest(url, HttpMethod.Post);
        }

        public static CustomersRequest Customers()
        {
            var url = ApiUtil.BuildUrl("exports", "customers");
            return new CustomersRequest(url, HttpMethod.Post);
        }

        public static SubscriptionsRequest Subscriptions()
        {
            var url = ApiUtil.BuildUrl("exports", "subscriptions");
            return new SubscriptionsRequest(url, HttpMethod.Post);
        }

        public static InvoicesRequest Invoices()
        {
            var url = ApiUtil.BuildUrl("exports", "invoices");
            return new InvoicesRequest(url, HttpMethod.Post);
        }

        public static CreditNotesRequest CreditNotes()
        {
            var url = ApiUtil.BuildUrl("exports", "credit_notes");
            return new CreditNotesRequest(url, HttpMethod.Post);
        }

        public static TransactionsRequest Transactions()
        {
            var url = ApiUtil.BuildUrl("exports", "transactions");
            return new TransactionsRequest(url, HttpMethod.Post);
        }

        public static OrdersRequest Orders()
        {
            var url = ApiUtil.BuildUrl("exports", "orders");
            return new OrdersRequest(url, HttpMethod.Post);
        }

        public static ItemFamiliesRequest ItemFamilies()
        {
            var url = ApiUtil.BuildUrl("exports", "item_families");
            return new ItemFamiliesRequest(url, HttpMethod.Post);
        }

        public static ItemsRequest Items()
        {
            var url = ApiUtil.BuildUrl("exports", "items");
            return new ItemsRequest(url, HttpMethod.Post);
        }

        public static ItemPricesRequest ItemPrices()
        {
            var url = ApiUtil.BuildUrl("exports", "item_prices");
            return new ItemPricesRequest(url, HttpMethod.Post);
        }

        public static AttachedItemsRequest AttachedItems()
        {
            var url = ApiUtil.BuildUrl("exports", "attached_items");
            return new AttachedItemsRequest(url, HttpMethod.Post);
        }

        public static DifferentialPricesRequest DifferentialPrices()
        {
            var url = ApiUtil.BuildUrl("exports", "differential_prices");
            return new DifferentialPricesRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string OperationType => GetValue<string>("operation_type");

        public MimeTypeEnum MimeType => GetEnum<MimeTypeEnum>("mime_type");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public ExportDownload Download => GetSubResource<ExportDownload>("download");

        public Export WaitForExportCompletion()
        {
            var count = 0;
            while (Status == StatusEnum.InProcess)
            {
                if (count++ > 50) throw new Exception("Export is taking too long");
                var t = Task.Factory.StartNew(() => { Task.Delay(ApiConfig.ExportSleepMillis).Wait(); });
                t.Wait();
                var req = Retrieve(Id);
                JObj = req.Request().Export.JObj;
            }

            return this;
        }

        #endregion

        #region Requests

        public class RevenueRecognitionRequest : EntityRequest<RevenueRecognitionRequest>
        {
            public RevenueRecognitionRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RevenueRecognitionRequest ReportBy(ReportByEnum reportBy)
            {
                MParams.Add("report_by", reportBy);
                return this;
            }

            public RevenueRecognitionRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public RevenueRecognitionRequest ReportFromMonth(int reportFromMonth)
            {
                MParams.Add("report_from_month", reportFromMonth);
                return this;
            }

            public RevenueRecognitionRequest ReportFromYear(int reportFromYear)
            {
                MParams.Add("report_from_year", reportFromYear);
                return this;
            }

            public RevenueRecognitionRequest ReportToMonth(int reportToMonth)
            {
                MParams.Add("report_to_month", reportToMonth);
                return this;
            }

            public RevenueRecognitionRequest ReportToYear(int reportToYear)
            {
                MParams.Add("report_to_year", reportToYear);
                return this;
            }

            public RevenueRecognitionRequest IncludeDiscounts(bool includeDiscounts)
            {
                MParams.AddOpt("include_discounts", includeDiscounts);
                return this;
            }

            public StringFilter<RevenueRecognitionRequest> PaymentOwner()
            {
                return new StringFilter<RevenueRecognitionRequest>("payment_owner", this).SupportsMultiOperators(true);
            }

            public StringFilter<RevenueRecognitionRequest> ItemId()
            {
                return new StringFilter<RevenueRecognitionRequest>("item_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<RevenueRecognitionRequest> ItemPriceId()
            {
                return new StringFilter<RevenueRecognitionRequest>("item_price_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<RevenueRecognitionRequest> CancelReasonCode()
            {
                return new StringFilter<RevenueRecognitionRequest>("cancel_reason_code", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<RevenueRecognitionRequest> InvoiceId()
            {
                return new StringFilter<RevenueRecognitionRequest>("invoice[id]", this).SupportsMultiOperators(true);
            }

            public BooleanFilter<RevenueRecognitionRequest> InvoiceRecurring()
            {
                return new("invoice[recurring]", this);
            }

            public EnumFilter<Invoice.StatusEnum, RevenueRecognitionRequest> InvoiceStatus()
            {
                return new("invoice[status]", this);
            }

            public EnumFilter<PriceTypeEnum, RevenueRecognitionRequest> InvoicePriceType()
            {
                return new("invoice[price_type]", this);
            }

            public TimestampFilter<RevenueRecognitionRequest> InvoiceDate()
            {
                return new("invoice[date]", this);
            }

            public TimestampFilter<RevenueRecognitionRequest> InvoicePaidAt()
            {
                return new("invoice[paid_at]", this);
            }

            public NumberFilter<int, RevenueRecognitionRequest> InvoiceTotal()
            {
                return new("invoice[total]", this);
            }

            public NumberFilter<int, RevenueRecognitionRequest> InvoiceAmountPaid()
            {
                return new("invoice[amount_paid]", this);
            }

            public NumberFilter<int, RevenueRecognitionRequest> InvoiceAmountAdjusted()
            {
                return new("invoice[amount_adjusted]", this);
            }

            public NumberFilter<int, RevenueRecognitionRequest> InvoiceCreditsApplied()
            {
                return new("invoice[credits_applied]", this);
            }

            public NumberFilter<int, RevenueRecognitionRequest> InvoiceAmountDue()
            {
                return new("invoice[amount_due]", this);
            }

            public EnumFilter<Invoice.DunningStatusEnum, RevenueRecognitionRequest> InvoiceDunningStatus()
            {
                return new EnumFilter<Invoice.DunningStatusEnum, RevenueRecognitionRequest>("invoice[dunning_status]",
                    this).SupportsPresenceOperator(true);
            }

            public TimestampFilter<RevenueRecognitionRequest> InvoiceUpdatedAt()
            {
                return new("invoice[updated_at]", this);
            }

            public StringFilter<RevenueRecognitionRequest> SubscriptionId()
            {
                return new StringFilter<RevenueRecognitionRequest>("subscription[id]", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<RevenueRecognitionRequest> SubscriptionCustomerId()
            {
                return new StringFilter<RevenueRecognitionRequest>("subscription[customer_id]", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<RevenueRecognitionRequest> SubscriptionPlanId()
            {
                return new StringFilter<RevenueRecognitionRequest>("subscription[plan_id]", this)
                    .SupportsMultiOperators(true);
            }

            public EnumFilter<Subscription.StatusEnum, RevenueRecognitionRequest> SubscriptionStatus()
            {
                return new("subscription[status]", this);
            }

            public EnumFilter<Subscription.CancelReasonEnum, RevenueRecognitionRequest> SubscriptionCancelReason()
            {
                return new EnumFilter<Subscription.CancelReasonEnum, RevenueRecognitionRequest>(
                    "subscription[cancel_reason]", this).SupportsPresenceOperator(true);
            }

            public NumberFilter<int, RevenueRecognitionRequest> SubscriptionRemainingBillingCycles()
            {
                return new NumberFilter<int, RevenueRecognitionRequest>("subscription[remaining_billing_cycles]", this)
                    .SupportsPresenceOperator(true);
            }

            public TimestampFilter<RevenueRecognitionRequest> SubscriptionCreatedAt()
            {
                return new("subscription[created_at]", this);
            }

            public TimestampFilter<RevenueRecognitionRequest> SubscriptionActivatedAt()
            {
                return new TimestampFilter<RevenueRecognitionRequest>("subscription[activated_at]", this)
                    .SupportsPresenceOperator(true);
            }

            public TimestampFilter<RevenueRecognitionRequest> SubscriptionNextBillingAt()
            {
                return new("subscription[next_billing_at]", this);
            }

            public TimestampFilter<RevenueRecognitionRequest> SubscriptionCancelledAt()
            {
                return new("subscription[cancelled_at]", this);
            }

            public BooleanFilter<RevenueRecognitionRequest> SubscriptionHasScheduledChanges()
            {
                return new("subscription[has_scheduled_changes]", this);
            }

            public TimestampFilter<RevenueRecognitionRequest> SubscriptionUpdatedAt()
            {
                return new("subscription[updated_at]", this);
            }

            public EnumFilter<OfflinePaymentMethodEnum, RevenueRecognitionRequest> SubscriptionOfflinePaymentMethod()
            {
                return new("subscription[offline_payment_method]", this);
            }

            public BooleanFilter<RevenueRecognitionRequest> SubscriptionAutoCloseInvoices()
            {
                return new("subscription[auto_close_invoices]", this);
            }

            public StringFilter<RevenueRecognitionRequest> CustomerId()
            {
                return new StringFilter<RevenueRecognitionRequest>("customer[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<RevenueRecognitionRequest> CustomerFirstName()
            {
                return new StringFilter<RevenueRecognitionRequest>("customer[first_name]", this)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<RevenueRecognitionRequest> CustomerLastName()
            {
                return new StringFilter<RevenueRecognitionRequest>("customer[last_name]", this)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<RevenueRecognitionRequest> CustomerEmail()
            {
                return new StringFilter<RevenueRecognitionRequest>("customer[email]", this)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<RevenueRecognitionRequest> CustomerCompany()
            {
                return new StringFilter<RevenueRecognitionRequest>("customer[company]", this)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<RevenueRecognitionRequest> CustomerPhone()
            {
                return new StringFilter<RevenueRecognitionRequest>("customer[phone]", this)
                    .SupportsPresenceOperator(true);
            }

            public EnumFilter<AutoCollectionEnum, RevenueRecognitionRequest> CustomerAutoCollection()
            {
                return new("customer[auto_collection]", this);
            }

            public EnumFilter<TaxabilityEnum, RevenueRecognitionRequest> CustomerTaxability()
            {
                return new("customer[taxability]", this);
            }

            public TimestampFilter<RevenueRecognitionRequest> CustomerCreatedAt()
            {
                return new("customer[created_at]", this);
            }

            public TimestampFilter<RevenueRecognitionRequest> CustomerUpdatedAt()
            {
                return new("customer[updated_at]", this);
            }

            public StringFilter<RevenueRecognitionRequest> RelationshipParentId()
            {
                return new("relationship[parent_id]", this);
            }

            public StringFilter<RevenueRecognitionRequest> RelationshipPaymentOwnerId()
            {
                return new("relationship[payment_owner_id]", this);
            }

            public StringFilter<RevenueRecognitionRequest> RelationshipInvoiceOwnerId()
            {
                return new("relationship[invoice_owner_id]", this);
            }

            public EnumFilter<OfflinePaymentMethodEnum, RevenueRecognitionRequest> CustomerOfflinePaymentMethod()
            {
                return new("customer[offline_payment_method]", this);
            }

            public BooleanFilter<RevenueRecognitionRequest> CustomerAutoCloseInvoices()
            {
                return new("customer[auto_close_invoices]", this);
            }
        }

        public class DeferredRevenueRequest : EntityRequest<DeferredRevenueRequest>
        {
            public DeferredRevenueRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public DeferredRevenueRequest ReportBy(ReportByEnum reportBy)
            {
                MParams.Add("report_by", reportBy);
                return this;
            }

            public DeferredRevenueRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public DeferredRevenueRequest ReportFromMonth(int reportFromMonth)
            {
                MParams.Add("report_from_month", reportFromMonth);
                return this;
            }

            public DeferredRevenueRequest ReportFromYear(int reportFromYear)
            {
                MParams.Add("report_from_year", reportFromYear);
                return this;
            }

            public DeferredRevenueRequest ReportToMonth(int reportToMonth)
            {
                MParams.Add("report_to_month", reportToMonth);
                return this;
            }

            public DeferredRevenueRequest ReportToYear(int reportToYear)
            {
                MParams.Add("report_to_year", reportToYear);
                return this;
            }

            public DeferredRevenueRequest IncludeDiscounts(bool includeDiscounts)
            {
                MParams.AddOpt("include_discounts", includeDiscounts);
                return this;
            }

            public StringFilter<DeferredRevenueRequest> PaymentOwner()
            {
                return new StringFilter<DeferredRevenueRequest>("payment_owner", this).SupportsMultiOperators(true);
            }

            public StringFilter<DeferredRevenueRequest> ItemId()
            {
                return new StringFilter<DeferredRevenueRequest>("item_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<DeferredRevenueRequest> ItemPriceId()
            {
                return new StringFilter<DeferredRevenueRequest>("item_price_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<DeferredRevenueRequest> CancelReasonCode()
            {
                return new StringFilter<DeferredRevenueRequest>("cancel_reason_code", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<DeferredRevenueRequest> InvoiceId()
            {
                return new StringFilter<DeferredRevenueRequest>("invoice[id]", this).SupportsMultiOperators(true);
            }

            public BooleanFilter<DeferredRevenueRequest> InvoiceRecurring()
            {
                return new("invoice[recurring]", this);
            }

            public EnumFilter<Invoice.StatusEnum, DeferredRevenueRequest> InvoiceStatus()
            {
                return new("invoice[status]", this);
            }

            public EnumFilter<PriceTypeEnum, DeferredRevenueRequest> InvoicePriceType()
            {
                return new("invoice[price_type]", this);
            }

            public TimestampFilter<DeferredRevenueRequest> InvoiceDate()
            {
                return new("invoice[date]", this);
            }

            public TimestampFilter<DeferredRevenueRequest> InvoicePaidAt()
            {
                return new("invoice[paid_at]", this);
            }

            public NumberFilter<int, DeferredRevenueRequest> InvoiceTotal()
            {
                return new("invoice[total]", this);
            }

            public NumberFilter<int, DeferredRevenueRequest> InvoiceAmountPaid()
            {
                return new("invoice[amount_paid]", this);
            }

            public NumberFilter<int, DeferredRevenueRequest> InvoiceAmountAdjusted()
            {
                return new("invoice[amount_adjusted]", this);
            }

            public NumberFilter<int, DeferredRevenueRequest> InvoiceCreditsApplied()
            {
                return new("invoice[credits_applied]", this);
            }

            public NumberFilter<int, DeferredRevenueRequest> InvoiceAmountDue()
            {
                return new("invoice[amount_due]", this);
            }

            public EnumFilter<Invoice.DunningStatusEnum, DeferredRevenueRequest> InvoiceDunningStatus()
            {
                return new EnumFilter<Invoice.DunningStatusEnum, DeferredRevenueRequest>("invoice[dunning_status]",
                    this).SupportsPresenceOperator(true);
            }

            public TimestampFilter<DeferredRevenueRequest> InvoiceUpdatedAt()
            {
                return new("invoice[updated_at]", this);
            }

            public StringFilter<DeferredRevenueRequest> SubscriptionId()
            {
                return new StringFilter<DeferredRevenueRequest>("subscription[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<DeferredRevenueRequest> SubscriptionCustomerId()
            {
                return new StringFilter<DeferredRevenueRequest>("subscription[customer_id]", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<DeferredRevenueRequest> SubscriptionPlanId()
            {
                return new StringFilter<DeferredRevenueRequest>("subscription[plan_id]", this)
                    .SupportsMultiOperators(true);
            }

            public EnumFilter<Subscription.StatusEnum, DeferredRevenueRequest> SubscriptionStatus()
            {
                return new("subscription[status]", this);
            }

            public EnumFilter<Subscription.CancelReasonEnum, DeferredRevenueRequest> SubscriptionCancelReason()
            {
                return new EnumFilter<Subscription.CancelReasonEnum, DeferredRevenueRequest>(
                    "subscription[cancel_reason]", this).SupportsPresenceOperator(true);
            }

            public NumberFilter<int, DeferredRevenueRequest> SubscriptionRemainingBillingCycles()
            {
                return new NumberFilter<int, DeferredRevenueRequest>("subscription[remaining_billing_cycles]", this)
                    .SupportsPresenceOperator(true);
            }

            public TimestampFilter<DeferredRevenueRequest> SubscriptionCreatedAt()
            {
                return new("subscription[created_at]", this);
            }

            public TimestampFilter<DeferredRevenueRequest> SubscriptionActivatedAt()
            {
                return new TimestampFilter<DeferredRevenueRequest>("subscription[activated_at]", this)
                    .SupportsPresenceOperator(true);
            }

            public TimestampFilter<DeferredRevenueRequest> SubscriptionNextBillingAt()
            {
                return new("subscription[next_billing_at]", this);
            }

            public TimestampFilter<DeferredRevenueRequest> SubscriptionCancelledAt()
            {
                return new("subscription[cancelled_at]", this);
            }

            public BooleanFilter<DeferredRevenueRequest> SubscriptionHasScheduledChanges()
            {
                return new("subscription[has_scheduled_changes]", this);
            }

            public TimestampFilter<DeferredRevenueRequest> SubscriptionUpdatedAt()
            {
                return new("subscription[updated_at]", this);
            }

            public EnumFilter<OfflinePaymentMethodEnum, DeferredRevenueRequest> SubscriptionOfflinePaymentMethod()
            {
                return new("subscription[offline_payment_method]", this);
            }

            public BooleanFilter<DeferredRevenueRequest> SubscriptionAutoCloseInvoices()
            {
                return new("subscription[auto_close_invoices]", this);
            }

            public StringFilter<DeferredRevenueRequest> CustomerId()
            {
                return new StringFilter<DeferredRevenueRequest>("customer[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<DeferredRevenueRequest> CustomerFirstName()
            {
                return new StringFilter<DeferredRevenueRequest>("customer[first_name]", this)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<DeferredRevenueRequest> CustomerLastName()
            {
                return new StringFilter<DeferredRevenueRequest>("customer[last_name]", this)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<DeferredRevenueRequest> CustomerEmail()
            {
                return new StringFilter<DeferredRevenueRequest>("customer[email]", this).SupportsPresenceOperator(true);
            }

            public StringFilter<DeferredRevenueRequest> CustomerCompany()
            {
                return new StringFilter<DeferredRevenueRequest>("customer[company]", this)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<DeferredRevenueRequest> CustomerPhone()
            {
                return new StringFilter<DeferredRevenueRequest>("customer[phone]", this).SupportsPresenceOperator(true);
            }

            public EnumFilter<AutoCollectionEnum, DeferredRevenueRequest> CustomerAutoCollection()
            {
                return new("customer[auto_collection]", this);
            }

            public EnumFilter<TaxabilityEnum, DeferredRevenueRequest> CustomerTaxability()
            {
                return new("customer[taxability]", this);
            }

            public TimestampFilter<DeferredRevenueRequest> CustomerCreatedAt()
            {
                return new("customer[created_at]", this);
            }

            public TimestampFilter<DeferredRevenueRequest> CustomerUpdatedAt()
            {
                return new("customer[updated_at]", this);
            }

            public StringFilter<DeferredRevenueRequest> RelationshipParentId()
            {
                return new("relationship[parent_id]", this);
            }

            public StringFilter<DeferredRevenueRequest> RelationshipPaymentOwnerId()
            {
                return new("relationship[payment_owner_id]", this);
            }

            public StringFilter<DeferredRevenueRequest> RelationshipInvoiceOwnerId()
            {
                return new("relationship[invoice_owner_id]", this);
            }

            public EnumFilter<OfflinePaymentMethodEnum, DeferredRevenueRequest> CustomerOfflinePaymentMethod()
            {
                return new("customer[offline_payment_method]", this);
            }

            public BooleanFilter<DeferredRevenueRequest> CustomerAutoCloseInvoices()
            {
                return new("customer[auto_close_invoices]", this);
            }
        }

        public class PlansRequest : EntityRequest<PlansRequest>
        {
            public PlansRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<PlansRequest> CurrencyCode()
            {
                return new StringFilter<PlansRequest>("currency_code", this).SupportsMultiOperators(true);
            }

            public StringFilter<PlansRequest> PlanId()
            {
                return new StringFilter<PlansRequest>("plan[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<PlansRequest> PlanName()
            {
                return new StringFilter<PlansRequest>("plan[name]", this).SupportsMultiOperators(true);
            }

            public NumberFilter<int, PlansRequest> PlanPrice()
            {
                return new("plan[price]", this);
            }

            public NumberFilter<int, PlansRequest> PlanPeriod()
            {
                return new("plan[period]", this);
            }

            public EnumFilter<Plan.PeriodUnitEnum, PlansRequest> PlanPeriodUnit()
            {
                return new("plan[period_unit]", this);
            }

            public NumberFilter<int, PlansRequest> PlanTrialPeriod()
            {
                return new NumberFilter<int, PlansRequest>("plan[trial_period]", this).SupportsPresenceOperator(true);
            }

            public EnumFilter<Plan.TrialPeriodUnitEnum, PlansRequest> PlanTrialPeriodUnit()
            {
                return new("plan[trial_period_unit]", this);
            }

            public EnumFilter<Plan.AddonApplicabilityEnum, PlansRequest> PlanAddonApplicability()
            {
                return new("plan[addon_applicability]", this);
            }

            public BooleanFilter<PlansRequest> PlanGiftable()
            {
                return new("plan[giftable]", this);
            }

            public EnumFilter<Plan.StatusEnum, PlansRequest> PlanStatus()
            {
                return new("plan[status]", this);
            }

            public TimestampFilter<PlansRequest> PlanUpdatedAt()
            {
                return new("plan[updated_at]", this);
            }
        }

        public class AddonsRequest : EntityRequest<AddonsRequest>
        {
            public AddonsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<AddonsRequest> CurrencyCode()
            {
                return new StringFilter<AddonsRequest>("currency_code", this).SupportsMultiOperators(true);
            }

            public StringFilter<AddonsRequest> AddonId()
            {
                return new StringFilter<AddonsRequest>("addon[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<AddonsRequest> AddonName()
            {
                return new StringFilter<AddonsRequest>("addon[name]", this).SupportsMultiOperators(true);
            }

            public EnumFilter<Addon.ChargeTypeEnum, AddonsRequest> AddonChargeType()
            {
                return new("addon[charge_type]", this);
            }

            public NumberFilter<int, AddonsRequest> AddonPrice()
            {
                return new("addon[price]", this);
            }

            public NumberFilter<int, AddonsRequest> AddonPeriod()
            {
                return new("addon[period]", this);
            }

            public EnumFilter<Addon.PeriodUnitEnum, AddonsRequest> AddonPeriodUnit()
            {
                return new("addon[period_unit]", this);
            }

            public EnumFilter<Addon.StatusEnum, AddonsRequest> AddonStatus()
            {
                return new("addon[status]", this);
            }

            public TimestampFilter<AddonsRequest> AddonUpdatedAt()
            {
                return new("addon[updated_at]", this);
            }
        }

        public class CouponsRequest : EntityRequest<CouponsRequest>
        {
            public CouponsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<CouponsRequest> CurrencyCode()
            {
                return new StringFilter<CouponsRequest>("currency_code", this).SupportsMultiOperators(true);
            }

            public StringFilter<CouponsRequest> CouponId()
            {
                return new StringFilter<CouponsRequest>("coupon[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<CouponsRequest> CouponName()
            {
                return new StringFilter<CouponsRequest>("coupon[name]", this).SupportsMultiOperators(true);
            }

            public EnumFilter<Coupon.DiscountTypeEnum, CouponsRequest> CouponDiscountType()
            {
                return new("coupon[discount_type]", this);
            }

            public EnumFilter<Coupon.DurationTypeEnum, CouponsRequest> CouponDurationType()
            {
                return new("coupon[duration_type]", this);
            }

            public EnumFilter<Coupon.StatusEnum, CouponsRequest> CouponStatus()
            {
                return new("coupon[status]", this);
            }

            public EnumFilter<Coupon.ApplyOnEnum, CouponsRequest> CouponApplyOn()
            {
                return new("coupon[apply_on]", this);
            }

            public TimestampFilter<CouponsRequest> CouponCreatedAt()
            {
                return new("coupon[created_at]", this);
            }

            public TimestampFilter<CouponsRequest> CouponUpdatedAt()
            {
                return new("coupon[updated_at]", this);
            }
        }

        public class CustomersRequest : EntityRequest<CustomersRequest>
        {
            public CustomersRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<CustomersRequest> CustomerId()
            {
                return new StringFilter<CustomersRequest>("customer[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<CustomersRequest> CustomerFirstName()
            {
                return new StringFilter<CustomersRequest>("customer[first_name]", this).SupportsPresenceOperator(true);
            }

            public StringFilter<CustomersRequest> CustomerLastName()
            {
                return new StringFilter<CustomersRequest>("customer[last_name]", this).SupportsPresenceOperator(true);
            }

            public StringFilter<CustomersRequest> CustomerEmail()
            {
                return new StringFilter<CustomersRequest>("customer[email]", this).SupportsPresenceOperator(true);
            }

            public StringFilter<CustomersRequest> CustomerCompany()
            {
                return new StringFilter<CustomersRequest>("customer[company]", this).SupportsPresenceOperator(true);
            }

            public StringFilter<CustomersRequest> CustomerPhone()
            {
                return new StringFilter<CustomersRequest>("customer[phone]", this).SupportsPresenceOperator(true);
            }

            public EnumFilter<AutoCollectionEnum, CustomersRequest> CustomerAutoCollection()
            {
                return new("customer[auto_collection]", this);
            }

            public EnumFilter<TaxabilityEnum, CustomersRequest> CustomerTaxability()
            {
                return new("customer[taxability]", this);
            }

            public TimestampFilter<CustomersRequest> CustomerCreatedAt()
            {
                return new("customer[created_at]", this);
            }

            public TimestampFilter<CustomersRequest> CustomerUpdatedAt()
            {
                return new("customer[updated_at]", this);
            }

            public StringFilter<CustomersRequest> RelationshipParentId()
            {
                return new("relationship[parent_id]", this);
            }

            public StringFilter<CustomersRequest> RelationshipPaymentOwnerId()
            {
                return new("relationship[payment_owner_id]", this);
            }

            public StringFilter<CustomersRequest> RelationshipInvoiceOwnerId()
            {
                return new("relationship[invoice_owner_id]", this);
            }

            public EnumFilter<OfflinePaymentMethodEnum, CustomersRequest> CustomerOfflinePaymentMethod()
            {
                return new("customer[offline_payment_method]", this);
            }

            public BooleanFilter<CustomersRequest> CustomerAutoCloseInvoices()
            {
                return new("customer[auto_close_invoices]", this);
            }
        }

        public class SubscriptionsRequest : EntityRequest<SubscriptionsRequest>
        {
            public SubscriptionsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<SubscriptionsRequest> ItemId()
            {
                return new StringFilter<SubscriptionsRequest>("item_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<SubscriptionsRequest> ItemPriceId()
            {
                return new StringFilter<SubscriptionsRequest>("item_price_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<SubscriptionsRequest> CancelReasonCode()
            {
                return new StringFilter<SubscriptionsRequest>("cancel_reason_code", this).SupportsMultiOperators(true);
            }

            public StringFilter<SubscriptionsRequest> SubscriptionId()
            {
                return new StringFilter<SubscriptionsRequest>("subscription[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<SubscriptionsRequest> SubscriptionCustomerId()
            {
                return new StringFilter<SubscriptionsRequest>("subscription[customer_id]", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<SubscriptionsRequest> SubscriptionPlanId()
            {
                return new StringFilter<SubscriptionsRequest>("subscription[plan_id]", this)
                    .SupportsMultiOperators(true);
            }

            public EnumFilter<Subscription.StatusEnum, SubscriptionsRequest> SubscriptionStatus()
            {
                return new("subscription[status]", this);
            }

            public EnumFilter<Subscription.CancelReasonEnum, SubscriptionsRequest> SubscriptionCancelReason()
            {
                return new EnumFilter<Subscription.CancelReasonEnum, SubscriptionsRequest>(
                    "subscription[cancel_reason]", this).SupportsPresenceOperator(true);
            }

            public NumberFilter<int, SubscriptionsRequest> SubscriptionRemainingBillingCycles()
            {
                return new NumberFilter<int, SubscriptionsRequest>("subscription[remaining_billing_cycles]", this)
                    .SupportsPresenceOperator(true);
            }

            public TimestampFilter<SubscriptionsRequest> SubscriptionCreatedAt()
            {
                return new("subscription[created_at]", this);
            }

            public TimestampFilter<SubscriptionsRequest> SubscriptionActivatedAt()
            {
                return new TimestampFilter<SubscriptionsRequest>("subscription[activated_at]", this)
                    .SupportsPresenceOperator(true);
            }

            public TimestampFilter<SubscriptionsRequest> SubscriptionNextBillingAt()
            {
                return new("subscription[next_billing_at]", this);
            }

            public TimestampFilter<SubscriptionsRequest> SubscriptionCancelledAt()
            {
                return new("subscription[cancelled_at]", this);
            }

            public BooleanFilter<SubscriptionsRequest> SubscriptionHasScheduledChanges()
            {
                return new("subscription[has_scheduled_changes]", this);
            }

            public TimestampFilter<SubscriptionsRequest> SubscriptionUpdatedAt()
            {
                return new("subscription[updated_at]", this);
            }

            public EnumFilter<OfflinePaymentMethodEnum, SubscriptionsRequest> SubscriptionOfflinePaymentMethod()
            {
                return new("subscription[offline_payment_method]", this);
            }

            public BooleanFilter<SubscriptionsRequest> SubscriptionAutoCloseInvoices()
            {
                return new("subscription[auto_close_invoices]", this);
            }
        }

        public class InvoicesRequest : EntityRequest<InvoicesRequest>
        {
            public InvoicesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<InvoicesRequest> PaymentOwner()
            {
                return new StringFilter<InvoicesRequest>("payment_owner", this).SupportsMultiOperators(true);
            }

            public StringFilter<InvoicesRequest> InvoiceId()
            {
                return new StringFilter<InvoicesRequest>("invoice[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<InvoicesRequest> InvoiceSubscriptionId()
            {
                return new StringFilter<InvoicesRequest>("invoice[subscription_id]", this).SupportsMultiOperators(true)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<InvoicesRequest> InvoiceCustomerId()
            {
                return new StringFilter<InvoicesRequest>("invoice[customer_id]", this).SupportsMultiOperators(true);
            }

            public BooleanFilter<InvoicesRequest> InvoiceRecurring()
            {
                return new("invoice[recurring]", this);
            }

            public EnumFilter<Invoice.StatusEnum, InvoicesRequest> InvoiceStatus()
            {
                return new("invoice[status]", this);
            }

            public EnumFilter<PriceTypeEnum, InvoicesRequest> InvoicePriceType()
            {
                return new("invoice[price_type]", this);
            }

            public TimestampFilter<InvoicesRequest> InvoiceDate()
            {
                return new("invoice[date]", this);
            }

            public TimestampFilter<InvoicesRequest> InvoicePaidAt()
            {
                return new("invoice[paid_at]", this);
            }

            public NumberFilter<int, InvoicesRequest> InvoiceTotal()
            {
                return new("invoice[total]", this);
            }

            public NumberFilter<int, InvoicesRequest> InvoiceAmountPaid()
            {
                return new("invoice[amount_paid]", this);
            }

            public NumberFilter<int, InvoicesRequest> InvoiceAmountAdjusted()
            {
                return new("invoice[amount_adjusted]", this);
            }

            public NumberFilter<int, InvoicesRequest> InvoiceCreditsApplied()
            {
                return new("invoice[credits_applied]", this);
            }

            public NumberFilter<int, InvoicesRequest> InvoiceAmountDue()
            {
                return new("invoice[amount_due]", this);
            }

            public EnumFilter<Invoice.DunningStatusEnum, InvoicesRequest> InvoiceDunningStatus()
            {
                return new EnumFilter<Invoice.DunningStatusEnum, InvoicesRequest>("invoice[dunning_status]", this)
                    .SupportsPresenceOperator(true);
            }

            public TimestampFilter<InvoicesRequest> InvoiceUpdatedAt()
            {
                return new("invoice[updated_at]", this);
            }
        }

        public class CreditNotesRequest : EntityRequest<CreditNotesRequest>
        {
            public CreditNotesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<CreditNotesRequest> CreditNoteId()
            {
                return new StringFilter<CreditNotesRequest>("credit_note[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<CreditNotesRequest> CreditNoteCustomerId()
            {
                return new StringFilter<CreditNotesRequest>("credit_note[customer_id]", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<CreditNotesRequest> CreditNoteSubscriptionId()
            {
                return new StringFilter<CreditNotesRequest>("credit_note[subscription_id]", this)
                    .SupportsMultiOperators(true).SupportsPresenceOperator(true);
            }

            public StringFilter<CreditNotesRequest> CreditNoteReferenceInvoiceId()
            {
                return new StringFilter<CreditNotesRequest>("credit_note[reference_invoice_id]", this)
                    .SupportsMultiOperators(true);
            }

            public EnumFilter<CreditNote.TypeEnum, CreditNotesRequest> CreditNoteType()
            {
                return new("credit_note[type]", this);
            }

            public EnumFilter<CreditNote.ReasonCodeEnum, CreditNotesRequest> CreditNoteReasonCode()
            {
                return new("credit_note[reason_code]", this);
            }

            public StringFilter<CreditNotesRequest> CreditNoteCreateReasonCode()
            {
                return new StringFilter<CreditNotesRequest>("credit_note[create_reason_code]", this)
                    .SupportsMultiOperators(true);
            }

            public EnumFilter<CreditNote.StatusEnum, CreditNotesRequest> CreditNoteStatus()
            {
                return new("credit_note[status]", this);
            }

            public TimestampFilter<CreditNotesRequest> CreditNoteDate()
            {
                return new("credit_note[date]", this);
            }

            public NumberFilter<int, CreditNotesRequest> CreditNoteTotal()
            {
                return new("credit_note[total]", this);
            }

            public EnumFilter<PriceTypeEnum, CreditNotesRequest> CreditNotePriceType()
            {
                return new("credit_note[price_type]", this);
            }

            public NumberFilter<int, CreditNotesRequest> CreditNoteAmountAllocated()
            {
                return new("credit_note[amount_allocated]", this);
            }

            public NumberFilter<int, CreditNotesRequest> CreditNoteAmountRefunded()
            {
                return new("credit_note[amount_refunded]", this);
            }

            public NumberFilter<int, CreditNotesRequest> CreditNoteAmountAvailable()
            {
                return new("credit_note[amount_available]", this);
            }

            public TimestampFilter<CreditNotesRequest> CreditNoteVoidedAt()
            {
                return new("credit_note[voided_at]", this);
            }

            public TimestampFilter<CreditNotesRequest> CreditNoteUpdatedAt()
            {
                return new("credit_note[updated_at]", this);
            }
        }

        public class TransactionsRequest : EntityRequest<TransactionsRequest>
        {
            public TransactionsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<TransactionsRequest> TransactionId()
            {
                return new StringFilter<TransactionsRequest>("transaction[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<TransactionsRequest> TransactionCustomerId()
            {
                return new StringFilter<TransactionsRequest>("transaction[customer_id]", this)
                    .SupportsMultiOperators(true).SupportsPresenceOperator(true);
            }

            public StringFilter<TransactionsRequest> TransactionSubscriptionId()
            {
                return new StringFilter<TransactionsRequest>("transaction[subscription_id]", this)
                    .SupportsMultiOperators(true).SupportsPresenceOperator(true);
            }

            public StringFilter<TransactionsRequest> TransactionPaymentSourceId()
            {
                return new StringFilter<TransactionsRequest>("transaction[payment_source_id]", this)
                    .SupportsMultiOperators(true).SupportsPresenceOperator(true);
            }

            public EnumFilter<PaymentMethodEnum, TransactionsRequest> TransactionPaymentMethod()
            {
                return new("transaction[payment_method]", this);
            }

            public EnumFilter<GatewayEnum, TransactionsRequest> TransactionGateway()
            {
                return new("transaction[gateway]", this);
            }

            public StringFilter<TransactionsRequest> TransactionGatewayAccountId()
            {
                return new StringFilter<TransactionsRequest>("transaction[gateway_account_id]", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<TransactionsRequest> TransactionIdAtGateway()
            {
                return new("transaction[id_at_gateway]", this);
            }

            public StringFilter<TransactionsRequest> TransactionReferenceNumber()
            {
                return new StringFilter<TransactionsRequest>("transaction[reference_number]", this)
                    .SupportsPresenceOperator(true);
            }

            public EnumFilter<Transaction.TypeEnum, TransactionsRequest> TransactionType()
            {
                return new("transaction[type]", this);
            }

            public TimestampFilter<TransactionsRequest> TransactionDate()
            {
                return new("transaction[date]", this);
            }

            public NumberFilter<int, TransactionsRequest> TransactionAmount()
            {
                return new("transaction[amount]", this);
            }

            public NumberFilter<int, TransactionsRequest> TransactionAmountCapturable()
            {
                return new("transaction[amount_capturable]", this);
            }

            public EnumFilter<Transaction.StatusEnum, TransactionsRequest> TransactionStatus()
            {
                return new("transaction[status]", this);
            }

            public TimestampFilter<TransactionsRequest> TransactionUpdatedAt()
            {
                return new("transaction[updated_at]", this);
            }
        }

        public class OrdersRequest : EntityRequest<OrdersRequest>
        {
            public OrdersRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public NumberFilter<int, OrdersRequest> Total()
            {
                return new("total", this);
            }

            public StringFilter<OrdersRequest> OrderId()
            {
                return new StringFilter<OrdersRequest>("order[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<OrdersRequest> OrderSubscriptionId()
            {
                return new StringFilter<OrdersRequest>("order[subscription_id]", this).SupportsMultiOperators(true)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<OrdersRequest> OrderCustomerId()
            {
                return new StringFilter<OrdersRequest>("order[customer_id]", this).SupportsMultiOperators(true);
            }

            public EnumFilter<Order.StatusEnum, OrdersRequest> OrderStatus()
            {
                return new("order[status]", this);
            }

            public EnumFilter<PriceTypeEnum, OrdersRequest> OrderPriceType()
            {
                return new("order[price_type]", this);
            }

            public TimestampFilter<OrdersRequest> OrderOrderDate()
            {
                return new("order[order_date]", this);
            }

            public TimestampFilter<OrdersRequest> OrderShippingDate()
            {
                return new("order[shipping_date]", this);
            }

            public TimestampFilter<OrdersRequest> OrderShippedAt()
            {
                return new("order[shipped_at]", this);
            }

            public TimestampFilter<OrdersRequest> OrderDeliveredAt()
            {
                return new("order[delivered_at]", this);
            }

            public TimestampFilter<OrdersRequest> OrderCancelledAt()
            {
                return new("order[cancelled_at]", this);
            }

            public NumberFilter<int, OrdersRequest> OrderAmountPaid()
            {
                return new("order[amount_paid]", this);
            }

            public NumberFilter<int, OrdersRequest> OrderRefundableCredits()
            {
                return new("order[refundable_credits]", this);
            }

            public NumberFilter<int, OrdersRequest> OrderRefundableCreditsIssued()
            {
                return new("order[refundable_credits_issued]", this);
            }

            public TimestampFilter<OrdersRequest> OrderUpdatedAt()
            {
                return new("order[updated_at]", this);
            }
        }

        public class ItemFamiliesRequest : EntityRequest<ItemFamiliesRequest>
        {
            public ItemFamiliesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<ItemFamiliesRequest> ItemFamilyId()
            {
                return new StringFilter<ItemFamiliesRequest>("item_family[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<ItemFamiliesRequest> ItemFamilyName()
            {
                return new("item_family[name]", this);
            }
        }

        public class ItemsRequest : EntityRequest<ItemsRequest>
        {
            public ItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<ItemsRequest> ItemId()
            {
                return new StringFilter<ItemsRequest>("item[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<ItemsRequest> ItemItemFamilyId()
            {
                return new StringFilter<ItemsRequest>("item[item_family_id]", this).SupportsMultiOperators(true);
            }

            public EnumFilter<Item.TypeEnum, ItemsRequest> ItemType()
            {
                return new("item[type]", this);
            }

            public StringFilter<ItemsRequest> ItemName()
            {
                return new("item[name]", this);
            }

            public EnumFilter<Item.ItemApplicabilityEnum, ItemsRequest> ItemItemApplicability()
            {
                return new("item[item_applicability]", this);
            }

            public EnumFilter<Item.StatusEnum, ItemsRequest> ItemStatus()
            {
                return new("item[status]", this);
            }

            public BooleanFilter<ItemsRequest> ItemIsGiftable()
            {
                return new("item[is_giftable]", this);
            }

            public TimestampFilter<ItemsRequest> ItemUpdatedAt()
            {
                return new("item[updated_at]", this);
            }

            public BooleanFilter<ItemsRequest> ItemEnabledForCheckout()
            {
                return new("item[enabled_for_checkout]", this);
            }

            public BooleanFilter<ItemsRequest> ItemEnabledInPortal()
            {
                return new("item[enabled_in_portal]", this);
            }

            public BooleanFilter<ItemsRequest> ItemMetered()
            {
                return new("item[metered]", this);
            }

            public EnumFilter<Item.UsageCalculationEnum, ItemsRequest> ItemUsageCalculation()
            {
                return new("item[usage_calculation]", this);
            }
        }

        public class ItemPricesRequest : EntityRequest<ItemPricesRequest>
        {
            public ItemPricesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<ItemPricesRequest> ItemFamilyId()
            {
                return new StringFilter<ItemPricesRequest>("item_family_id", this).SupportsMultiOperators(true);
            }

            public EnumFilter<ItemTypeEnum, ItemPricesRequest> ItemType()
            {
                return new("item_type", this);
            }

            public StringFilter<ItemPricesRequest> CurrencyCode()
            {
                return new StringFilter<ItemPricesRequest>("currency_code", this).SupportsMultiOperators(true);
            }

            public StringFilter<ItemPricesRequest> ItemPriceId()
            {
                return new StringFilter<ItemPricesRequest>("item_price[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<ItemPricesRequest> ItemPriceName()
            {
                return new StringFilter<ItemPricesRequest>("item_price[name]", this).SupportsMultiOperators(true);
            }

            public EnumFilter<PricingModelEnum, ItemPricesRequest> ItemPricePricingModel()
            {
                return new("item_price[pricing_model]", this);
            }

            public StringFilter<ItemPricesRequest> ItemPriceItemId()
            {
                return new StringFilter<ItemPricesRequest>("item_price[item_id]", this).SupportsMultiOperators(true);
            }

            public NumberFilter<int, ItemPricesRequest> ItemPriceTrialPeriod()
            {
                return new("item_price[trial_period]", this);
            }

            public EnumFilter<ItemPrice.TrialPeriodUnitEnum, ItemPricesRequest> ItemPriceTrialPeriodUnit()
            {
                return new("item_price[trial_period_unit]", this);
            }

            public EnumFilter<ItemPrice.StatusEnum, ItemPricesRequest> ItemPriceStatus()
            {
                return new("item_price[status]", this);
            }

            public TimestampFilter<ItemPricesRequest> ItemPriceUpdatedAt()
            {
                return new("item_price[updated_at]", this);
            }

            public EnumFilter<ItemPrice.PeriodUnitEnum, ItemPricesRequest> ItemPricePeriodUnit()
            {
                return new("item_price[period_unit]", this);
            }

            public NumberFilter<int, ItemPricesRequest> ItemPricePeriod()
            {
                return new("item_price[period]", this);
            }
        }

        public class AttachedItemsRequest : EntityRequest<AttachedItemsRequest>
        {
            public AttachedItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public EnumFilter<ItemTypeEnum, AttachedItemsRequest> ItemType()
            {
                return new("item_type", this);
            }

            public StringFilter<AttachedItemsRequest> AttachedItemId()
            {
                return new StringFilter<AttachedItemsRequest>("attached_item[id]", this).SupportsMultiOperators(true);
            }

            public StringFilter<AttachedItemsRequest> AttachedItemItemId()
            {
                return new StringFilter<AttachedItemsRequest>("attached_item[item_id]", this)
                    .SupportsMultiOperators(true);
            }

            public EnumFilter<AttachedItem.TypeEnum, AttachedItemsRequest> AttachedItemType()
            {
                return new("attached_item[type]", this);
            }

            public EnumFilter<ChargeOnEventEnum, AttachedItemsRequest> AttachedItemChargeOnEvent()
            {
                return new("attached_item[charge_on_event]", this);
            }

            public StringFilter<AttachedItemsRequest> AttachedItemParentItemId()
            {
                return new StringFilter<AttachedItemsRequest>("attached_item[parent_item_id]", this)
                    .SupportsMultiOperators(true);
            }
        }

        public class DifferentialPricesRequest : EntityRequest<DifferentialPricesRequest>
        {
            public DifferentialPricesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StringFilter<DifferentialPricesRequest> ItemId()
            {
                return new StringFilter<DifferentialPricesRequest>("item_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<DifferentialPricesRequest> DifferentialPriceItemPriceId()
            {
                return new StringFilter<DifferentialPricesRequest>("differential_price[item_price_id]", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<DifferentialPricesRequest> DifferentialPriceId()
            {
                return new StringFilter<DifferentialPricesRequest>("differential_price[id]", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<DifferentialPricesRequest> DifferentialPriceParentItemId()
            {
                return new StringFilter<DifferentialPricesRequest>("differential_price[parent_item_id]", this)
                    .SupportsMultiOperators(true);
            }
        }

        #endregion
    }
}