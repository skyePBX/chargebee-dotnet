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
    public class Transaction : Resource
    {
        public enum AuthorizationReasonEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "blocking_funds")] BlockingFunds,
            [EnumMember(Value = "verification")] Verification
        }

        public enum FraudFlagEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "safe")] Safe,
            [EnumMember(Value = "suspicious")] Suspicious,
            [EnumMember(Value = "fraudulent")] Fraudulent
        }

        public enum InitiatorTypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "customer")] Customer,
            [EnumMember(Value = "merchant")] Merchant
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "in_progress")] InProgress,
            [EnumMember(Value = "success")] Success,
            [EnumMember(Value = "voided")] Voided,
            [EnumMember(Value = "failure")] Failure,
            [EnumMember(Value = "timeout")] Timeout,

            [EnumMember(Value = "needs_attention")]
            NeedsAttention
        }

        public enum TypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "authorization")] Authorization,
            [EnumMember(Value = "payment")] Payment,
            [EnumMember(Value = "refund")] Refund,

            [EnumMember(Value = "payment_reversal")]
            PaymentReversal
        }

        public Transaction()
        {
        }

        public Transaction(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Transaction(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Transaction(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateAuthorizationRequest CreateAuthorization()
        {
            var url = ApiUtil.BuildUrl("transactions", "create_authorization");
            return new CreateAuthorizationRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> VoidTransaction(string id)
        {
            var url = ApiUtil.BuildUrl("transactions", CheckNull(id), "void");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static RecordRefundRequest RecordRefund(string id)
        {
            var url = ApiUtil.BuildUrl("transactions", CheckNull(id), "record_refund");
            return new RecordRefundRequest(url, HttpMethod.Post);
        }

        public static RefundRequest Refund(string id)
        {
            var url = ApiUtil.BuildUrl("transactions", CheckNull(id), "refund");
            return new RefundRequest(url, HttpMethod.Post);
        }

        public static TransactionListRequest List()
        {
            var url = ApiUtil.BuildUrl("transactions");
            return new TransactionListRequest(url);
        }

        [Obsolete]
        public static ListRequest TransactionsForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "transactions");
            return new ListRequest(url);
        }

        [Obsolete]
        public static ListRequest TransactionsForSubscription(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "transactions");
            return new ListRequest(url);
        }

        public static ListRequest PaymentsForInvoice(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "payments");
            return new ListRequest(url);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("transactions", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static DeleteOfflineTransactionRequest DeleteOfflineTransaction(string id)
        {
            var url = ApiUtil.BuildUrl("transactions", CheckNull(id), "delete_offline_transaction");
            return new DeleteOfflineTransactionRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string CustomerId => GetValue<string>("customer_id", false);

        public string SubscriptionId => GetValue<string>("subscription_id", false);

        public string GatewayAccountId => GetValue<string>("gateway_account_id", false);

        public string PaymentSourceId => GetValue<string>("payment_source_id", false);

        public PaymentMethodEnum PaymentMethod => GetEnum<PaymentMethodEnum>("payment_method");

        public string ReferenceNumber => GetValue<string>("reference_number", false);

        public GatewayEnum Gateway => GetEnum<GatewayEnum>("gateway");

        public TypeEnum TransactionType => GetEnum<TypeEnum>("type");

        public DateTime? Date => GetDateTime("date", false);

        public DateTime? SettledAt => GetDateTime("settled_at", false);

        public string CurrencyCode => GetValue<string>("currency_code");

        public int? Amount => GetValue<int?>("amount", false);

        public string IdAtGateway => GetValue<string>("id_at_gateway", false);

        public StatusEnum? Status => GetEnum<StatusEnum>("status", false);

        public FraudFlagEnum? FraudFlag => GetEnum<FraudFlagEnum>("fraud_flag", false);

        public InitiatorTypeEnum? InitiatorType => GetEnum<InitiatorTypeEnum>("initiator_type", false);

        public bool? ThreeDSecure => GetValue<bool?>("three_d_secure", false);

        public AuthorizationReasonEnum? AuthorizationReason =>
            GetEnum<AuthorizationReasonEnum>("authorization_reason", false);

        public string ErrorCode => GetValue<string>("error_code", false);

        public string ErrorText => GetValue<string>("error_text", false);

        public DateTime? VoidedAt => GetDateTime("voided_at", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public string FraudReason => GetValue<string>("fraud_reason", false);

        public int? AmountUnused => GetValue<int?>("amount_unused", false);

        public string MaskedCardNumber => GetValue<string>("masked_card_number", false);

        public string ReferenceTransactionId => GetValue<string>("reference_transaction_id", false);

        public string RefundedTxnId => GetValue<string>("refunded_txn_id", false);

        public string ReferenceAuthorizationId => GetValue<string>("reference_authorization_id", false);

        public int? AmountCapturable => GetValue<int?>("amount_capturable", false);

        public string ReversalTransactionId => GetValue<string>("reversal_transaction_id", false);

        public List<TransactionLinkedInvoice> LinkedInvoices =>
            GetResourceList<TransactionLinkedInvoice>("linked_invoices");

        public List<TransactionLinkedCreditNote> LinkedCreditNotes =>
            GetResourceList<TransactionLinkedCreditNote>("linked_credit_notes");

        public List<TransactionLinkedRefund> LinkedRefunds =>
            GetResourceList<TransactionLinkedRefund>("linked_refunds");

        public List<TransactionLinkedPayment> LinkedPayments =>
            GetResourceList<TransactionLinkedPayment>("linked_payments");

        public bool Deleted => GetValue<bool>("deleted");

        #endregion

        #region Requests

        public class CreateAuthorizationRequest : EntityRequest<CreateAuthorizationRequest>
        {
            public CreateAuthorizationRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateAuthorizationRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public CreateAuthorizationRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public CreateAuthorizationRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateAuthorizationRequest Amount(int amount)
            {
                MParams.Add("amount", amount);
                return this;
            }
        }

        public class RecordRefundRequest : EntityRequest<RecordRefundRequest>
        {
            public RecordRefundRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RecordRefundRequest Amount(int amount)
            {
                MParams.AddOpt("amount", amount);
                return this;
            }

            public RecordRefundRequest PaymentMethod(PaymentMethodEnum paymentMethod)
            {
                MParams.Add("payment_method", paymentMethod);
                return this;
            }

            public RecordRefundRequest Date(long date)
            {
                MParams.Add("date", date);
                return this;
            }

            public RecordRefundRequest ReferenceNumber(string referenceNumber)
            {
                MParams.AddOpt("reference_number", referenceNumber);
                return this;
            }

            public RecordRefundRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }
        }

        public class RefundRequest : EntityRequest<RefundRequest>
        {
            public RefundRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RefundRequest Amount(int amount)
            {
                MParams.AddOpt("amount", amount);
                return this;
            }

            public RefundRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }
        }

        public class TransactionListRequest : ListRequestBase<TransactionListRequest>
        {
            public TransactionListRequest(string url)
                : base(url)
            {
            }

            public TransactionListRequest IncludeDeleted(bool includeDeleted)
            {
                MParams.AddOpt("include_deleted", includeDeleted);
                return this;
            }

            public StringFilter<TransactionListRequest> Id()
            {
                return new StringFilter<TransactionListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<TransactionListRequest> CustomerId()
            {
                return new StringFilter<TransactionListRequest>("customer_id", this).SupportsMultiOperators(true)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<TransactionListRequest> SubscriptionId()
            {
                return new StringFilter<TransactionListRequest>("subscription_id", this).SupportsMultiOperators(true)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<TransactionListRequest> PaymentSourceId()
            {
                return new StringFilter<TransactionListRequest>("payment_source_id", this).SupportsMultiOperators(true)
                    .SupportsPresenceOperator(true);
            }

            public EnumFilter<PaymentMethodEnum, TransactionListRequest> PaymentMethod()
            {
                return new("payment_method", this);
            }

            public EnumFilter<GatewayEnum, TransactionListRequest> Gateway()
            {
                return new("gateway", this);
            }

            public StringFilter<TransactionListRequest> GatewayAccountId()
            {
                return new StringFilter<TransactionListRequest>("gateway_account_id", this)
                    .SupportsMultiOperators(true);
            }

            public StringFilter<TransactionListRequest> IdAtGateway()
            {
                return new("id_at_gateway", this);
            }

            public StringFilter<TransactionListRequest> ReferenceNumber()
            {
                return new StringFilter<TransactionListRequest>("reference_number", this)
                    .SupportsPresenceOperator(true);
            }

            public EnumFilter<TypeEnum, TransactionListRequest> Type()
            {
                return new("type", this);
            }

            public TimestampFilter<TransactionListRequest> Date()
            {
                return new("date", this);
            }

            public NumberFilter<int, TransactionListRequest> Amount()
            {
                return new("amount", this);
            }

            public NumberFilter<int, TransactionListRequest> AmountCapturable()
            {
                return new("amount_capturable", this);
            }

            public EnumFilter<StatusEnum, TransactionListRequest> Status()
            {
                return new("status", this);
            }

            public TimestampFilter<TransactionListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public TransactionListRequest SortByDate(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "date");
                return this;
            }

            public TransactionListRequest SortByUpdatedAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "updated_at");
                return this;
            }
        }

        public class DeleteOfflineTransactionRequest : EntityRequest<DeleteOfflineTransactionRequest>
        {
            public DeleteOfflineTransactionRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public DeleteOfflineTransactionRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }
        }

        #endregion

        #region Subclasses

        public class TransactionLinkedInvoice : Resource
        {
            public string InvoiceId()
            {
                return GetValue<string>("invoice_id");
            }

            public int AppliedAmount()
            {
                return GetValue<int>("applied_amount");
            }

            public DateTime AppliedAt()
            {
                return (DateTime) GetDateTime("applied_at");
            }

            public DateTime? InvoiceDate()
            {
                return GetDateTime("invoice_date", false);
            }

            public int? InvoiceTotal()
            {
                return GetValue<int?>("invoice_total", false);
            }

            public Invoice.StatusEnum InvoiceStatus()
            {
                return GetEnum<Invoice.StatusEnum>("invoice_status");
            }
        }

        public class TransactionLinkedCreditNote : Resource
        {
            public string CnId()
            {
                return GetValue<string>("cn_id");
            }

            public int AppliedAmount()
            {
                return GetValue<int>("applied_amount");
            }

            public DateTime AppliedAt()
            {
                return (DateTime) GetDateTime("applied_at");
            }

            public CreditNote.ReasonCodeEnum? CnReasonCode()
            {
                return GetEnum<CreditNote.ReasonCodeEnum>("cn_reason_code", false);
            }

            public string CnCreateReasonCode()
            {
                return GetValue<string>("cn_create_reason_code", false);
            }

            public DateTime? CnDate()
            {
                return GetDateTime("cn_date", false);
            }

            public int? CnTotal()
            {
                return GetValue<int?>("cn_total", false);
            }

            public CreditNote.StatusEnum CnStatus()
            {
                return GetEnum<CreditNote.StatusEnum>("cn_status");
            }

            public string CnReferenceInvoiceId()
            {
                return GetValue<string>("cn_reference_invoice_id");
            }
        }

        public class TransactionLinkedRefund : Resource
        {
            public string TxnId()
            {
                return GetValue<string>("txn_id");
            }

            public StatusEnum TxnStatus()
            {
                return GetEnum<StatusEnum>("txn_status");
            }

            public DateTime TxnDate()
            {
                return (DateTime) GetDateTime("txn_date");
            }

            public int TxnAmount()
            {
                return GetValue<int>("txn_amount");
            }
        }

        public class TransactionLinkedPayment : Resource
        {
            public enum StatusEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "in_progress")] InProgress,
                [EnumMember(Value = "success")] Success,
                [EnumMember(Value = "voided")] Voided,
                [EnumMember(Value = "failure")] Failure,
                [EnumMember(Value = "timeout")] Timeout,

                [EnumMember(Value = "needs_attention")]
                NeedsAttention
            }

            public string Id()
            {
                return GetValue<string>("id");
            }

            public StatusEnum? Status()
            {
                return GetEnum<StatusEnum>("status", false);
            }

            public int? Amount()
            {
                return GetValue<int?>("amount", false);
            }

            public DateTime? Date()
            {
                return GetDateTime("date", false);
            }
        }

        #endregion
    }
}