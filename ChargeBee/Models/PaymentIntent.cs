using System;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class PaymentIntent : Resource
    {
        public enum PaymentMethodTypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "card")] Card,
            [EnumMember(Value = "ideal")] Ideal,
            [EnumMember(Value = "sofort")] Sofort,
            [EnumMember(Value = "bancontact")] Bancontact,
            [EnumMember(Value = "google_pay")] GooglePay,
            [EnumMember(Value = "dotpay")] Dotpay,
            [EnumMember(Value = "giropay")] Giropay
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "inited")] Inited,
            [EnumMember(Value = "in_progress")] InProgress,
            [EnumMember(Value = "authorized")] Authorized,
            [EnumMember(Value = "consumed")] Consumed,
            [EnumMember(Value = "expired")] Expired
        }

        public PaymentIntent()
        {
        }

        public PaymentIntent(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public PaymentIntent(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public PaymentIntent(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Subclasses

        public class PaymentIntentPaymentAttempt : Resource
        {
            public enum StatusEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "inited")] Inited,

                [EnumMember(Value = "requires_identification")]
                RequiresIdentification,

                [EnumMember(Value = "requires_challenge")]
                RequiresChallenge,

                [EnumMember(Value = "requires_redirection")]
                RequiresRedirection,
                [EnumMember(Value = "authorized")] Authorized,
                [EnumMember(Value = "refused")] Refused
            }

            public string Id()
            {
                return GetValue<string>("id", false);
            }

            public StatusEnum Status()
            {
                return GetEnum<StatusEnum>("status");
            }

            public PaymentMethodTypeEnum? PaymentMethodType()
            {
                return GetEnum<PaymentMethodTypeEnum>("payment_method_type", false);
            }

            public string IdAtGateway()
            {
                return GetValue<string>("id_at_gateway", false);
            }

            public string ErrorCode()
            {
                return GetValue<string>("error_code", false);
            }

            public string ErrorText()
            {
                return GetValue<string>("error_text", false);
            }

            public DateTime CreatedAt()
            {
                return (DateTime) GetDateTime("created_at");
            }

            public DateTime ModifiedAt()
            {
                return (DateTime) GetDateTime("modified_at");
            }
        }

        #endregion

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("payment_intents");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("payment_intents", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("payment_intents", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public string CurrencyCode => GetValue<string>("currency_code", false);

        public int Amount => GetValue<int>("amount");

        public string GatewayAccountId => GetValue<string>("gateway_account_id");

        public DateTime ExpiresAt => (DateTime) GetDateTime("expires_at");

        public string ReferenceId => GetValue<string>("reference_id", false);

        public PaymentMethodTypeEnum? PaymentMethodType => GetEnum<PaymentMethodTypeEnum>("payment_method_type", false);

        public string SuccessUrl => GetValue<string>("success_url", false);

        public string FailureUrl => GetValue<string>("failure_url", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public DateTime ModifiedAt => (DateTime) GetDateTime("modified_at");

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public string CustomerId => GetValue<string>("customer_id");

        public string Gateway => GetValue<string>("gateway", false);

        public PaymentIntentPaymentAttempt ActivePaymentAttempt =>
            GetSubResource<PaymentIntentPaymentAttempt>("active_payment_attempt");

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer_id", customerId);
                return this;
            }

            public CreateRequest Amount(int amount)
            {
                MParams.Add("amount", amount);
                return this;
            }

            public CreateRequest CurrencyCode(string currencyCode)
            {
                MParams.Add("currency_code", currencyCode);
                return this;
            }

            public CreateRequest GatewayAccountId(string gatewayAccountId)
            {
                MParams.AddOpt("gateway_account_id", gatewayAccountId);
                return this;
            }

            public CreateRequest ReferenceId(string referenceId)
            {
                MParams.AddOpt("reference_id", referenceId);
                return this;
            }

            public CreateRequest PaymentMethodType(PaymentMethodTypeEnum paymentMethodType)
            {
                MParams.AddOpt("payment_method_type", paymentMethodType);
                return this;
            }

            public CreateRequest SuccessUrl(string successUrl)
            {
                MParams.AddOpt("success_url", successUrl);
                return this;
            }

            public CreateRequest FailureUrl(string failureUrl)
            {
                MParams.AddOpt("failure_url", failureUrl);
                return this;
            }
        }

        public class UpdateRequest : EntityRequest<UpdateRequest>
        {
            public UpdateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateRequest Amount(int amount)
            {
                MParams.AddOpt("amount", amount);
                return this;
            }

            public UpdateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public UpdateRequest GatewayAccountId(string gatewayAccountId)
            {
                MParams.AddOpt("gateway_account_id", gatewayAccountId);
                return this;
            }

            public UpdateRequest PaymentMethodType(PaymentMethodTypeEnum paymentMethodType)
            {
                MParams.AddOpt("payment_method_type", paymentMethodType);
                return this;
            }

            public UpdateRequest SuccessUrl(string successUrl)
            {
                MParams.AddOpt("success_url", successUrl);
                return this;
            }

            public UpdateRequest FailureUrl(string failureUrl)
            {
                MParams.AddOpt("failure_url", failureUrl);
                return this;
            }
        }

        #endregion
    }
}