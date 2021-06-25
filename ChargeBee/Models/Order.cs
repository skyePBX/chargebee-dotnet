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
    public class Order : Resource
    {
        public enum CancellationReasonEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */

            [EnumMember(Value = "shipping_cut_off_passed")]
            ShippingCutOffPassed,

            [EnumMember(Value = "product_unsatisfactory")]
            ProductUnsatisfactory,

            [EnumMember(Value = "third_party_cancellation")]
            ThirdPartyCancellation,

            [EnumMember(Value = "product_not_required")]
            ProductNotRequired,

            [EnumMember(Value = "delivery_date_missed")]
            DeliveryDateMissed,

            [EnumMember(Value = "alternative_found")]
            AlternativeFound,

            [EnumMember(Value = "invoice_written_off")]
            InvoiceWrittenOff,
            [EnumMember(Value = "invoice_voided")] InvoiceVoided,

            [EnumMember(Value = "fraudulent_transaction")]
            FraudulentTransaction,

            [EnumMember(Value = "payment_declined")]
            PaymentDeclined,

            [EnumMember(Value = "subscription_cancelled")]
            SubscriptionCancelled,

            [EnumMember(Value = "product_not_available")]
            ProductNotAvailable,
            [EnumMember(Value = "others")] Others
        }

        public enum OrderTypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "manual")] Manual,

            [EnumMember(Value = "system_generated")]
            SystemGenerated
        }

        public enum PaymentStatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "not_paid")] NotPaid,
            [EnumMember(Value = "paid")] Paid
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "new")] New,
            [EnumMember(Value = "processing")] Processing,
            [EnumMember(Value = "complete")] Complete,
            [EnumMember(Value = "cancelled")] Cancelled,
            [EnumMember(Value = "voided")] Voided,
            [EnumMember(Value = "queued")] Queued,

            [EnumMember(Value = "awaiting_shipment")]
            AwaitingShipment,
            [EnumMember(Value = "on_hold")] OnHold,
            [EnumMember(Value = "delivered")] Delivered,
            [EnumMember(Value = "shipped")] Shipped,

            [EnumMember(Value = "partially_delivered")]
            PartiallyDelivered,
            [EnumMember(Value = "returned")] Returned
        }

        public Order()
        {
        }

        public Order(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Order(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Order(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("orders");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("orders", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static ImportOrderRequest ImportOrder()
        {
            var url = ApiUtil.BuildUrl("orders", "import_order");
            return new ImportOrderRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> AssignOrderNumber(string id)
        {
            var url = ApiUtil.BuildUrl("orders", CheckNull(id), "assign_order_number");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static CancelRequest Cancel(string id)
        {
            var url = ApiUtil.BuildUrl("orders", CheckNull(id), "cancel");
            return new CancelRequest(url, HttpMethod.Post);
        }

        public static CreateRefundableCreditNoteRequest CreateRefundableCreditNote(string id)
        {
            var url = ApiUtil.BuildUrl("orders", CheckNull(id), "create_refundable_credit_note");
            return new CreateRefundableCreditNoteRequest(url, HttpMethod.Post);
        }

        public static ReopenRequest Reopen(string id)
        {
            var url = ApiUtil.BuildUrl("orders", CheckNull(id), "reopen");
            return new ReopenRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("orders", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("orders", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static OrderListRequest List()
        {
            var url = ApiUtil.BuildUrl("orders");
            return new OrderListRequest(url);
        }

        [Obsolete]
        public static ListRequest OrdersForInvoice(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "orders");
            return new ListRequest(url);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string DocumentNumber => GetValue<string>("document_number", false);

        public string InvoiceId => GetValue<string>("invoice_id", false);

        public string SubscriptionId => GetValue<string>("subscription_id", false);

        public string CustomerId => GetValue<string>("customer_id", false);

        public StatusEnum? Status => GetEnum<StatusEnum>("status", false);

        public CancellationReasonEnum? CancellationReason =>
            GetEnum<CancellationReasonEnum>("cancellation_reason", false);

        public PaymentStatusEnum? PaymentStatus => GetEnum<PaymentStatusEnum>("payment_status", false);

        public OrderTypeEnum? OrderType => GetEnum<OrderTypeEnum>("order_type", false);

        public PriceTypeEnum PriceType => GetEnum<PriceTypeEnum>("price_type");

        public string ReferenceId => GetValue<string>("reference_id", false);

        public string FulfillmentStatus => GetValue<string>("fulfillment_status", false);

        public DateTime? OrderDate => GetDateTime("order_date", false);

        public DateTime? ShippingDate => GetDateTime("shipping_date", false);

        public string Note => GetValue<string>("note", false);

        public string TrackingId => GetValue<string>("tracking_id", false);

        public string BatchId => GetValue<string>("batch_id", false);

        public string CreatedBy => GetValue<string>("created_by", false);

        public string ShipmentCarrier => GetValue<string>("shipment_carrier", false);

        public int? InvoiceRoundOffAmount => GetValue<int?>("invoice_round_off_amount", false);

        public int? Tax => GetValue<int?>("tax", false);

        public int? AmountPaid => GetValue<int?>("amount_paid", false);

        public int? AmountAdjusted => GetValue<int?>("amount_adjusted", false);

        public int? RefundableCreditsIssued => GetValue<int?>("refundable_credits_issued", false);

        public int? RefundableCredits => GetValue<int?>("refundable_credits", false);

        public int? RoundingAdjustement => GetValue<int?>("rounding_adjustement", false);

        public DateTime? PaidOn => GetDateTime("paid_on", false);

        public DateTime? ShippingCutOffDate => GetDateTime("shipping_cut_off_date", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public DateTime? StatusUpdateAt => GetDateTime("status_update_at", false);

        public DateTime? DeliveredAt => GetDateTime("delivered_at", false);

        public DateTime? ShippedAt => GetDateTime("shipped_at", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public DateTime? CancelledAt => GetDateTime("cancelled_at", false);

        public List<OrderOrderLineItem> OrderLineItems => GetResourceList<OrderOrderLineItem>("order_line_items");

        public OrderShippingAddress ShippingAddress => GetSubResource<OrderShippingAddress>("shipping_address");

        public OrderBillingAddress BillingAddress => GetSubResource<OrderBillingAddress>("billing_address");

        public int? Discount => GetValue<int?>("discount", false);

        public int? SubTotal => GetValue<int?>("sub_total", false);

        public int? Total => GetValue<int?>("total", false);

        public List<OrderLineItemTax> LineItemTaxes => GetResourceList<OrderLineItemTax>("line_item_taxes");

        public List<OrderLineItemDiscount> LineItemDiscounts =>
            GetResourceList<OrderLineItemDiscount>("line_item_discounts");

        public List<OrderLinkedCreditNote> LinkedCreditNotes =>
            GetResourceList<OrderLinkedCreditNote>("linked_credit_notes");

        public bool Deleted => GetValue<bool>("deleted");

        public string CurrencyCode => GetValue<string>("currency_code", false);

        public bool? IsGifted => GetValue<bool?>("is_gifted", false);

        public string GiftNote => GetValue<string>("gift_note", false);

        public string GiftId => GetValue<string>("gift_id", false);

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

            public CreateRequest InvoiceId(string invoiceId)
            {
                MParams.Add("invoice_id", invoiceId);
                return this;
            }

            public CreateRequest Status(StatusEnum status)
            {
                MParams.AddOpt("status", status);
                return this;
            }

            public CreateRequest ReferenceId(string referenceId)
            {
                MParams.AddOpt("reference_id", referenceId);
                return this;
            }

            public CreateRequest FulfillmentStatus(string fulfillmentStatus)
            {
                MParams.AddOpt("fulfillment_status", fulfillmentStatus);
                return this;
            }

            public CreateRequest Note(string note)
            {
                MParams.AddOpt("note", note);
                return this;
            }

            public CreateRequest TrackingId(string trackingId)
            {
                MParams.AddOpt("tracking_id", trackingId);
                return this;
            }

            public CreateRequest BatchId(string batchId)
            {
                MParams.AddOpt("batch_id", batchId);
                return this;
            }
        }

        public class UpdateRequest : EntityRequest<UpdateRequest>
        {
            public UpdateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateRequest ReferenceId(string referenceId)
            {
                MParams.AddOpt("reference_id", referenceId);
                return this;
            }

            public UpdateRequest BatchId(string batchId)
            {
                MParams.AddOpt("batch_id", batchId);
                return this;
            }

            public UpdateRequest Note(string note)
            {
                MParams.AddOpt("note", note);
                return this;
            }

            public UpdateRequest ShippingDate(long shippingDate)
            {
                MParams.AddOpt("shipping_date", shippingDate);
                return this;
            }

            public UpdateRequest OrderDate(long orderDate)
            {
                MParams.AddOpt("order_date", orderDate);
                return this;
            }

            public UpdateRequest CancelledAt(long cancelledAt)
            {
                MParams.AddOpt("cancelled_at", cancelledAt);
                return this;
            }

            public UpdateRequest CancellationReason(CancellationReasonEnum cancellationReason)
            {
                MParams.AddOpt("cancellation_reason", cancellationReason);
                return this;
            }

            public UpdateRequest ShippedAt(long shippedAt)
            {
                MParams.AddOpt("shipped_at", shippedAt);
                return this;
            }

            public UpdateRequest DeliveredAt(long deliveredAt)
            {
                MParams.AddOpt("delivered_at", deliveredAt);
                return this;
            }

            public UpdateRequest TrackingId(string trackingId)
            {
                MParams.AddOpt("tracking_id", trackingId);
                return this;
            }

            public UpdateRequest ShipmentCarrier(string shipmentCarrier)
            {
                MParams.AddOpt("shipment_carrier", shipmentCarrier);
                return this;
            }

            public UpdateRequest FulfillmentStatus(string fulfillmentStatus)
            {
                MParams.AddOpt("fulfillment_status", fulfillmentStatus);
                return this;
            }

            public UpdateRequest Status(StatusEnum status)
            {
                MParams.AddOpt("status", status);
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

            public UpdateRequest OrderLineItemId(int index, string orderLineItemId)
            {
                MParams.AddOpt("order_line_items[id][" + index + "]", orderLineItemId);
                return this;
            }

            public UpdateRequest OrderLineItemStatus(int index, OrderOrderLineItem.StatusEnum orderLineItemStatus)
            {
                MParams.AddOpt("order_line_items[status][" + index + "]", orderLineItemStatus);
                return this;
            }

            public UpdateRequest OrderLineItemSku(int index, string orderLineItemSku)
            {
                MParams.AddOpt("order_line_items[sku][" + index + "]", orderLineItemSku);
                return this;
            }
        }

        public class ImportOrderRequest : EntityRequest<ImportOrderRequest>
        {
            public ImportOrderRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ImportOrderRequest Id(string id)
            {
                MParams.AddOpt("id", id);
                return this;
            }

            public ImportOrderRequest DocumentNumber(string documentNumber)
            {
                MParams.AddOpt("document_number", documentNumber);
                return this;
            }

            public ImportOrderRequest InvoiceId(string invoiceId)
            {
                MParams.Add("invoice_id", invoiceId);
                return this;
            }

            public ImportOrderRequest Status(StatusEnum status)
            {
                MParams.Add("status", status);
                return this;
            }

            public ImportOrderRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public ImportOrderRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer_id", customerId);
                return this;
            }

            public ImportOrderRequest CreatedAt(long createdAt)
            {
                MParams.Add("created_at", createdAt);
                return this;
            }

            public ImportOrderRequest OrderDate(long orderDate)
            {
                MParams.Add("order_date", orderDate);
                return this;
            }

            public ImportOrderRequest ShippingDate(long shippingDate)
            {
                MParams.Add("shipping_date", shippingDate);
                return this;
            }

            public ImportOrderRequest ReferenceId(string referenceId)
            {
                MParams.AddOpt("reference_id", referenceId);
                return this;
            }

            public ImportOrderRequest FulfillmentStatus(string fulfillmentStatus)
            {
                MParams.AddOpt("fulfillment_status", fulfillmentStatus);
                return this;
            }

            public ImportOrderRequest Note(string note)
            {
                MParams.AddOpt("note", note);
                return this;
            }

            public ImportOrderRequest TrackingId(string trackingId)
            {
                MParams.AddOpt("tracking_id", trackingId);
                return this;
            }

            public ImportOrderRequest BatchId(string batchId)
            {
                MParams.AddOpt("batch_id", batchId);
                return this;
            }

            public ImportOrderRequest ShipmentCarrier(string shipmentCarrier)
            {
                MParams.AddOpt("shipment_carrier", shipmentCarrier);
                return this;
            }

            public ImportOrderRequest ShippingCutOffDate(long shippingCutOffDate)
            {
                MParams.AddOpt("shipping_cut_off_date", shippingCutOffDate);
                return this;
            }

            public ImportOrderRequest DeliveredAt(long deliveredAt)
            {
                MParams.AddOpt("delivered_at", deliveredAt);
                return this;
            }

            public ImportOrderRequest ShippedAt(long shippedAt)
            {
                MParams.AddOpt("shipped_at", shippedAt);
                return this;
            }

            public ImportOrderRequest CancelledAt(long cancelledAt)
            {
                MParams.AddOpt("cancelled_at", cancelledAt);
                return this;
            }

            public ImportOrderRequest CancellationReason(CancellationReasonEnum cancellationReason)
            {
                MParams.AddOpt("cancellation_reason", cancellationReason);
                return this;
            }

            public ImportOrderRequest RefundableCreditsIssued(int refundableCreditsIssued)
            {
                MParams.AddOpt("refundable_credits_issued", refundableCreditsIssued);
                return this;
            }

            public ImportOrderRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public ImportOrderRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public ImportOrderRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public ImportOrderRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public ImportOrderRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public ImportOrderRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public ImportOrderRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public ImportOrderRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public ImportOrderRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public ImportOrderRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public ImportOrderRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public ImportOrderRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public ImportOrderRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public ImportOrderRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public ImportOrderRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public ImportOrderRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public ImportOrderRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public ImportOrderRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public ImportOrderRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public ImportOrderRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public ImportOrderRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public ImportOrderRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public ImportOrderRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public ImportOrderRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public ImportOrderRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public ImportOrderRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public ImportOrderRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public ImportOrderRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }
        }

        public class CancelRequest : EntityRequest<CancelRequest>
        {
            public CancelRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CancelRequest CancellationReason(CancellationReasonEnum cancellationReason)
            {
                MParams.Add("cancellation_reason", cancellationReason);
                return this;
            }

            public CancelRequest CustomerNotes(string customerNotes)
            {
                MParams.AddOpt("customer_notes", customerNotes);
                return this;
            }

            public CancelRequest CancelledAt(long cancelledAt)
            {
                MParams.AddOpt("cancelled_at", cancelledAt);
                return this;
            }

            public CancelRequest CreditNoteTotal(int creditNoteTotal)
            {
                MParams.AddOpt("credit_note[total]", creditNoteTotal);
                return this;
            }
        }

        public class CreateRefundableCreditNoteRequest : EntityRequest<CreateRefundableCreditNoteRequest>
        {
            public CreateRefundableCreditNoteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRefundableCreditNoteRequest CustomerNotes(string customerNotes)
            {
                MParams.AddOpt("customer_notes", customerNotes);
                return this;
            }

            public CreateRefundableCreditNoteRequest CreditNoteReasonCode(
                CreditNote.ReasonCodeEnum creditNoteReasonCode)
            {
                MParams.Add("credit_note[reason_code]", creditNoteReasonCode);
                return this;
            }

            public CreateRefundableCreditNoteRequest CreditNoteTotal(int creditNoteTotal)
            {
                MParams.Add("credit_note[total]", creditNoteTotal);
                return this;
            }
        }

        public class ReopenRequest : EntityRequest<ReopenRequest>
        {
            public ReopenRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ReopenRequest VoidCancellationCreditNotes(bool voidCancellationCreditNotes)
            {
                MParams.AddOpt("void_cancellation_credit_notes", voidCancellationCreditNotes);
                return this;
            }
        }

        public class OrderListRequest : ListRequestBase<OrderListRequest>
        {
            public OrderListRequest(string url)
                : base(url)
            {
            }

            public OrderListRequest IncludeDeleted(bool includeDeleted)
            {
                MParams.AddOpt("include_deleted", includeDeleted);
                return this;
            }

            public OrderListRequest ExcludeDeletedCreditNotes(bool excludeDeletedCreditNotes)
            {
                MParams.AddOpt("exclude_deleted_credit_notes", excludeDeletedCreditNotes);
                return this;
            }

            public StringFilter<OrderListRequest> Id()
            {
                return new StringFilter<OrderListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<OrderListRequest> InvoiceId()
            {
                return new StringFilter<OrderListRequest>("invoice_id", this).SupportsMultiOperators(true);
            }

            public StringFilter<OrderListRequest> SubscriptionId()
            {
                return new("subscription_id", this);
            }

            public EnumFilter<StatusEnum, OrderListRequest> Status()
            {
                return new("status", this);
            }

            public TimestampFilter<OrderListRequest> ShippingDate()
            {
                return new("shipping_date", this);
            }

            public EnumFilter<OrderTypeEnum, OrderListRequest> OrderType()
            {
                return new("order_type", this);
            }

            public TimestampFilter<OrderListRequest> OrderDate()
            {
                return new("order_date", this);
            }

            public TimestampFilter<OrderListRequest> PaidOn()
            {
                return new("paid_on", this);
            }

            public TimestampFilter<OrderListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public TimestampFilter<OrderListRequest> CreatedAt()
            {
                return new("created_at", this);
            }

            public OrderListRequest SortByCreatedAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "created_at");
                return this;
            }

            public OrderListRequest SortByUpdatedAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "updated_at");
                return this;
            }
        }

        #endregion

        #region Subclasses

        public class OrderOrderLineItem : Resource
        {
            public enum EntityTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "plan_setup")] PlanSetup,
                [EnumMember(Value = "plan")] Plan,
                [EnumMember(Value = "addon")] Addon,
                [EnumMember(Value = "adhoc")] Adhoc
            }

            public enum StatusEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "queued")] Queued,

                [EnumMember(Value = "awaiting_shipment")]
                AwaitingShipment,
                [EnumMember(Value = "on_hold")] OnHold,
                [EnumMember(Value = "delivered")] Delivered,
                [EnumMember(Value = "shipped")] Shipped,

                [EnumMember(Value = "partially_delivered")]
                PartiallyDelivered,
                [EnumMember(Value = "returned")] Returned,
                [EnumMember(Value = "cancelled")] Cancelled
            }

            public string Id()
            {
                return GetValue<string>("id");
            }

            public string InvoiceId()
            {
                return GetValue<string>("invoice_id");
            }

            public string InvoiceLineItemId()
            {
                return GetValue<string>("invoice_line_item_id");
            }

            public int? UnitPrice()
            {
                return GetValue<int?>("unit_price", false);
            }

            public string Description()
            {
                return GetValue<string>("description", false);
            }

            public int? Amount()
            {
                return GetValue<int?>("amount", false);
            }

            public int? FulfillmentQuantity()
            {
                return GetValue<int?>("fulfillment_quantity", false);
            }

            public int? FulfillmentAmount()
            {
                return GetValue<int?>("fulfillment_amount", false);
            }

            public int? TaxAmount()
            {
                return GetValue<int?>("tax_amount", false);
            }

            public int? AmountPaid()
            {
                return GetValue<int?>("amount_paid", false);
            }

            public int? AmountAdjusted()
            {
                return GetValue<int?>("amount_adjusted", false);
            }

            public int? RefundableCreditsIssued()
            {
                return GetValue<int?>("refundable_credits_issued", false);
            }

            public int? RefundableCredits()
            {
                return GetValue<int?>("refundable_credits", false);
            }

            public bool IsShippable()
            {
                return GetValue<bool>("is_shippable");
            }

            public string Sku()
            {
                return GetValue<string>("sku", false);
            }

            public StatusEnum? Status()
            {
                return GetEnum<StatusEnum>("status", false);
            }

            public EntityTypeEnum EntityType()
            {
                return GetEnum<EntityTypeEnum>("entity_type");
            }

            public int? ItemLevelDiscountAmount()
            {
                return GetValue<int?>("item_level_discount_amount", false);
            }

            public int? DiscountAmount()
            {
                return GetValue<int?>("discount_amount", false);
            }

            public string EntityId()
            {
                return GetValue<string>("entity_id", false);
            }
        }

        public class OrderShippingAddress : Resource
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

        public class OrderBillingAddress : Resource
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

        public class OrderLineItemTax : Resource
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

        public class OrderLineItemDiscount : Resource
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

        public class OrderLinkedCreditNote : Resource
        {
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

            public int? Amount()
            {
                return GetValue<int?>("amount", false);
            }

            public TypeEnum LinkedCreditNoteType()
            {
                return GetEnum<TypeEnum>("type");
            }

            public string Id()
            {
                return GetValue<string>("id");
            }

            public StatusEnum Status()
            {
                return GetEnum<StatusEnum>("status");
            }

            public int? AmountAdjusted()
            {
                return GetValue<int?>("amount_adjusted", false);
            }

            public int? AmountRefunded()
            {
                return GetValue<int?>("amount_refunded", false);
            }
        }

        #endregion
    }
}