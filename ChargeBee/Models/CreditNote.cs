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
    public class CreditNote : Resource
    {
        public enum ReasonCodeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "write_off")] WriteOff,

            [EnumMember(Value = "subscription_change")]
            SubscriptionChange,

            [EnumMember(Value = "subscription_cancellation")]
            SubscriptionCancellation,

            [EnumMember(Value = "subscription_pause")]
            SubscriptionPause,
            [EnumMember(Value = "chargeback")] Chargeback,

            [EnumMember(Value = "product_unsatisfactory")]
            ProductUnsatisfactory,

            [EnumMember(Value = "service_unsatisfactory")]
            ServiceUnsatisfactory,
            [EnumMember(Value = "order_change")] OrderChange,

            [EnumMember(Value = "order_cancellation")]
            OrderCancellation,
            [EnumMember(Value = "waiver")] Waiver,
            [EnumMember(Value = "other")] Other,
            [EnumMember(Value = "fraudulent")] Fraudulent
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "adjusted")] Adjusted,
            [EnumMember(Value = "refunded")] Refunded,
            [EnumMember(Value = "refund_due")] RefundDue,
            [EnumMember(Value = "voided")] Voided
        }

        public enum TypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "adjustment")] Adjustment,
            [EnumMember(Value = "refundable")] Refundable
        }

        public CreditNote()
        {
        }

        public CreditNote(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public CreditNote(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public CreditNote(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("credit_notes");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("credit_notes", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static PdfRequest Pdf(string id)
        {
            var url = ApiUtil.BuildUrl("credit_notes", CheckNull(id), "pdf");
            return new PdfRequest(url, HttpMethod.Post);
        }

        public static RefundRequest Refund(string id)
        {
            var url = ApiUtil.BuildUrl("credit_notes", CheckNull(id), "refund");
            return new RefundRequest(url, HttpMethod.Post);
        }

        public static RecordRefundRequest RecordRefund(string id)
        {
            var url = ApiUtil.BuildUrl("credit_notes", CheckNull(id), "record_refund");
            return new RecordRefundRequest(url, HttpMethod.Post);
        }

        public static VoidCreditNoteRequest VoidCreditNote(string id)
        {
            var url = ApiUtil.BuildUrl("credit_notes", CheckNull(id), "void");
            return new VoidCreditNoteRequest(url, HttpMethod.Post);
        }

        public static CreditNoteListRequest List()
        {
            var url = ApiUtil.BuildUrl("credit_notes");
            return new CreditNoteListRequest(url);
        }

        [Obsolete]
        public static ListRequest CreditNotesForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "credit_notes");
            return new ListRequest(url);
        }

        public static DeleteRequest Delete(string id)
        {
            var url = ApiUtil.BuildUrl("credit_notes", CheckNull(id), "delete");
            return new DeleteRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string CustomerId => GetValue<string>("customer_id");

        public string SubscriptionId => GetValue<string>("subscription_id", false);

        public string ReferenceInvoiceId => GetValue<string>("reference_invoice_id");

        public TypeEnum CreditNoteType => GetEnum<TypeEnum>("type");

        public ReasonCodeEnum? ReasonCode => GetEnum<ReasonCodeEnum>("reason_code", false);

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public string VatNumber => GetValue<string>("vat_number", false);

        public DateTime? Date => GetDateTime("date", false);

        public PriceTypeEnum PriceType => GetEnum<PriceTypeEnum>("price_type");

        public string CurrencyCode => GetValue<string>("currency_code");

        public int? Total => GetValue<int?>("total", false);

        public int? AmountAllocated => GetValue<int?>("amount_allocated", false);

        public int? AmountRefunded => GetValue<int?>("amount_refunded", false);

        public int? AmountAvailable => GetValue<int?>("amount_available", false);

        public DateTime? RefundedAt => GetDateTime("refunded_at", false);

        public DateTime? VoidedAt => GetDateTime("voided_at", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public int SubTotal => GetValue<int>("sub_total");

        public int? SubTotalInLocalCurrency => GetValue<int?>("sub_total_in_local_currency", false);

        public int? TotalInLocalCurrency => GetValue<int?>("total_in_local_currency", false);

        public string LocalCurrencyCode => GetValue<string>("local_currency_code", false);

        public int? RoundOffAmount => GetValue<int?>("round_off_amount", false);

        public int? FractionalCorrection => GetValue<int?>("fractional_correction", false);

        public List<CreditNoteLineItem> LineItems => GetResourceList<CreditNoteLineItem>("line_items");

        public List<CreditNoteDiscount> Discounts => GetResourceList<CreditNoteDiscount>("discounts");

        public List<CreditNoteLineItemDiscount> LineItemDiscounts =>
            GetResourceList<CreditNoteLineItemDiscount>("line_item_discounts");

        public List<CreditNoteLineItemTier> LineItemTiers => GetResourceList<CreditNoteLineItemTier>("line_item_tiers");

        public List<CreditNoteTax> Taxes => GetResourceList<CreditNoteTax>("taxes");

        public List<CreditNoteLineItemTax> LineItemTaxes => GetResourceList<CreditNoteLineItemTax>("line_item_taxes");

        public List<CreditNoteLinkedRefund> LinkedRefunds => GetResourceList<CreditNoteLinkedRefund>("linked_refunds");

        public List<CreditNoteAllocation> Allocations => GetResourceList<CreditNoteAllocation>("allocations");

        public bool Deleted => GetValue<bool>("deleted");

        public string CreateReasonCode => GetValue<string>("create_reason_code", false);

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest ReferenceInvoiceId(string referenceInvoiceId)
            {
                MParams.Add("reference_invoice_id", referenceInvoiceId);
                return this;
            }

            public CreateRequest Total(int total)
            {
                MParams.AddOpt("total", total);
                return this;
            }

            public CreateRequest Type(TypeEnum type)
            {
                MParams.Add("type", type);
                return this;
            }

            public CreateRequest ReasonCode(ReasonCodeEnum reasonCode)
            {
                MParams.AddOpt("reason_code", reasonCode);
                return this;
            }

            public CreateRequest CreateReasonCode(string createReasonCode)
            {
                MParams.AddOpt("create_reason_code", createReasonCode);
                return this;
            }

            public CreateRequest Date(long date)
            {
                MParams.AddOpt("date", date);
                return this;
            }

            public CreateRequest CustomerNotes(string customerNotes)
            {
                MParams.AddOpt("customer_notes", customerNotes);
                return this;
            }

            public CreateRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public CreateRequest LineItemReferenceLineItemId(int index, string lineItemReferenceLineItemId)
            {
                MParams.Add("line_items[reference_line_item_id][" + index + "]", lineItemReferenceLineItemId);
                return this;
            }

            public CreateRequest LineItemUnitAmount(int index, int lineItemUnitAmount)
            {
                MParams.AddOpt("line_items[unit_amount][" + index + "]", lineItemUnitAmount);
                return this;
            }

            public CreateRequest LineItemUnitAmountInDecimal(int index, string lineItemUnitAmountInDecimal)
            {
                MParams.AddOpt("line_items[unit_amount_in_decimal][" + index + "]", lineItemUnitAmountInDecimal);
                return this;
            }

            public CreateRequest LineItemQuantity(int index, int lineItemQuantity)
            {
                MParams.AddOpt("line_items[quantity][" + index + "]", lineItemQuantity);
                return this;
            }

            public CreateRequest LineItemQuantityInDecimal(int index, string lineItemQuantityInDecimal)
            {
                MParams.AddOpt("line_items[quantity_in_decimal][" + index + "]", lineItemQuantityInDecimal);
                return this;
            }

            public CreateRequest LineItemAmount(int index, int lineItemAmount)
            {
                MParams.AddOpt("line_items[amount][" + index + "]", lineItemAmount);
                return this;
            }

            public CreateRequest LineItemDateFrom(int index, long lineItemDateFrom)
            {
                MParams.AddOpt("line_items[date_from][" + index + "]", lineItemDateFrom);
                return this;
            }

            public CreateRequest LineItemDateTo(int index, long lineItemDateTo)
            {
                MParams.AddOpt("line_items[date_to][" + index + "]", lineItemDateTo);
                return this;
            }

            public CreateRequest LineItemDescription(int index, string lineItemDescription)
            {
                MParams.AddOpt("line_items[description][" + index + "]", lineItemDescription);
                return this;
            }
        }

        public class PdfRequest : EntityRequest<PdfRequest>
        {
            public PdfRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public PdfRequest DispositionType(DispositionTypeEnum dispositionType)
            {
                MParams.AddOpt("disposition_type", dispositionType);
                return this;
            }
        }

        public class RefundRequest : EntityRequest<RefundRequest>
        {
            public RefundRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RefundRequest RefundAmount(int refundAmount)
            {
                MParams.AddOpt("refund_amount", refundAmount);
                return this;
            }

            public RefundRequest CustomerNotes(string customerNotes)
            {
                MParams.AddOpt("customer_notes", customerNotes);
                return this;
            }

            public RefundRequest RefundReasonCode(string refundReasonCode)
            {
                MParams.AddOpt("refund_reason_code", refundReasonCode);
                return this;
            }
        }

        public class RecordRefundRequest : EntityRequest<RecordRefundRequest>
        {
            public RecordRefundRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RecordRefundRequest RefundReasonCode(string refundReasonCode)
            {
                MParams.AddOpt("refund_reason_code", refundReasonCode);
                return this;
            }

            public RecordRefundRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public RecordRefundRequest TransactionAmount(int transactionAmount)
            {
                MParams.AddOpt("transaction[amount]", transactionAmount);
                return this;
            }

            public RecordRefundRequest TransactionPaymentMethod(PaymentMethodEnum transactionPaymentMethod)
            {
                MParams.Add("transaction[payment_method]", transactionPaymentMethod);
                return this;
            }

            public RecordRefundRequest TransactionReferenceNumber(string transactionReferenceNumber)
            {
                MParams.AddOpt("transaction[reference_number]", transactionReferenceNumber);
                return this;
            }

            public RecordRefundRequest TransactionDate(long transactionDate)
            {
                MParams.Add("transaction[date]", transactionDate);
                return this;
            }
        }

        public class VoidCreditNoteRequest : EntityRequest<VoidCreditNoteRequest>
        {
            public VoidCreditNoteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public VoidCreditNoteRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }
        }

        public class CreditNoteListRequest : ListRequestBase<CreditNoteListRequest>
        {
            public CreditNoteListRequest(string url)
                : base(url)
            {
            }

            public CreditNoteListRequest IncludeDeleted(bool includeDeleted)
            {
                MParams.AddOpt("include_deleted", includeDeleted);
                return this;
            }

            public StringFilter<CreditNoteListRequest> Id()
            {
                return new StringFilter<CreditNoteListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<CreditNoteListRequest> CustomerId()
            {
                return new StringFilter<CreditNoteListRequest>("customer_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<CreditNoteListRequest> SubscriptionId()
            {
                return new StringFilter<CreditNoteListRequest>("subscription_id", this).SupportsMultiOperators(true)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<CreditNoteListRequest> ReferenceInvoiceId()
            {
                return new StringFilter<CreditNoteListRequest>("reference_invoice_id", this)
                    .SupportsMultiOperators(true);
            }

            public EnumFilter<TypeEnum, CreditNoteListRequest> Type()
            {
                return new("type", this);
            }

            public EnumFilter<ReasonCodeEnum, CreditNoteListRequest> ReasonCode()
            {
                return new("reason_code", this);
            }

            public StringFilter<CreditNoteListRequest> CreateReasonCode()
            {
                return new StringFilter<CreditNoteListRequest>("create_reason_code", this).SupportsMultiOperators(true);
            }

            public EnumFilter<StatusEnum, CreditNoteListRequest> Status()
            {
                return new("status", this);
            }

            public TimestampFilter<CreditNoteListRequest> Date()
            {
                return new("date", this);
            }

            public NumberFilter<int, CreditNoteListRequest> Total()
            {
                return new("total", this);
            }

            public EnumFilter<PriceTypeEnum, CreditNoteListRequest> PriceType()
            {
                return new("price_type", this);
            }

            public NumberFilter<int, CreditNoteListRequest> AmountAllocated()
            {
                return new("amount_allocated", this);
            }

            public NumberFilter<int, CreditNoteListRequest> AmountRefunded()
            {
                return new("amount_refunded", this);
            }

            public NumberFilter<int, CreditNoteListRequest> AmountAvailable()
            {
                return new("amount_available", this);
            }

            public TimestampFilter<CreditNoteListRequest> VoidedAt()
            {
                return new("voided_at", this);
            }

            public TimestampFilter<CreditNoteListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public CreditNoteListRequest SortByDate(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "date");
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

        #endregion

        #region Subclasses

        public class CreditNoteLineItem : Resource
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

        public class CreditNoteDiscount : Resource
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

        public class CreditNoteLineItemDiscount : Resource
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

        public class CreditNoteLineItemTier : Resource
        {
            public string LineItemId()
            {
                return GetValue<string>("line_item_id", false);
            }

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

        public class CreditNoteTax : Resource
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

        public class CreditNoteLineItemTax : Resource
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

        public class CreditNoteLinkedRefund : Resource
        {
            public string TxnId()
            {
                return GetValue<string>("txn_id");
            }

            public int AppliedAmount()
            {
                return GetValue<int>("applied_amount");
            }

            public DateTime AppliedAt()
            {
                return (DateTime) GetDateTime("applied_at");
            }

            public Transaction.StatusEnum? TxnStatus()
            {
                return GetEnum<Transaction.StatusEnum>("txn_status", false);
            }

            public DateTime? TxnDate()
            {
                return GetDateTime("txn_date", false);
            }

            public int? TxnAmount()
            {
                return GetValue<int?>("txn_amount", false);
            }

            public string RefundReasonCode()
            {
                return GetValue<string>("refund_reason_code", false);
            }
        }

        public class CreditNoteAllocation : Resource
        {
            public string InvoiceId()
            {
                return GetValue<string>("invoice_id");
            }

            public int AllocatedAmount()
            {
                return GetValue<int>("allocated_amount");
            }

            public DateTime AllocatedAt()
            {
                return (DateTime) GetDateTime("allocated_at");
            }

            public DateTime? InvoiceDate()
            {
                return GetDateTime("invoice_date", false);
            }

            public Invoice.StatusEnum InvoiceStatus()
            {
                return GetEnum<Invoice.StatusEnum>("invoice_status");
            }
        }

        #endregion
    }
}