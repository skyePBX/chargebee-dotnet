using System;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class PromotionalCredit : Resource
    {
        public enum TypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "increment")] Increment,
            [EnumMember(Value = "decrement")] Decrement
        }

        public PromotionalCredit()
        {
        }

        public PromotionalCredit(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public PromotionalCredit(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public PromotionalCredit(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static AddRequest Add()
        {
            var url = ApiUtil.BuildUrl("promotional_credits", "add");
            return new AddRequest(url, HttpMethod.Post);
        }

        public static DeductRequest Deduct()
        {
            var url = ApiUtil.BuildUrl("promotional_credits", "deduct");
            return new DeductRequest(url, HttpMethod.Post);
        }

        public static SetRequest Set()
        {
            var url = ApiUtil.BuildUrl("promotional_credits", "set");
            return new SetRequest(url, HttpMethod.Post);
        }

        public static PromotionalCreditListRequest List()
        {
            var url = ApiUtil.BuildUrl("promotional_credits");
            return new PromotionalCreditListRequest(url);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("promotional_credits", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string CustomerId => GetValue<string>("customer_id");

        public TypeEnum PromotionalCreditType => GetEnum<TypeEnum>("type");

        public string AmountInDecimal => GetValue<string>("amount_in_decimal", false);

        public int Amount => GetValue<int>("amount");

        public string CurrencyCode => GetValue<string>("currency_code");

        public string Description => GetValue<string>("description");

        public CreditTypeEnum CreditType => GetEnum<CreditTypeEnum>("credit_type");

        public string Reference => GetValue<string>("reference", false);

        public int ClosingBalance => GetValue<int>("closing_balance");

        public string DoneBy => GetValue<string>("done_by", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        #endregion

        #region Requests

        public class AddRequest : EntityRequest<AddRequest>
        {
            public AddRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AddRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public AddRequest Amount(int amount)
            {
                MParams.AddOpt("amount", amount);
                return this;
            }

            public AddRequest AmountInDecimal(string amountInDecimal)
            {
                MParams.AddOpt("amount_in_decimal", amountInDecimal);
                return this;
            }

            public AddRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public AddRequest Description(string description)
            {
                MParams.Add("description", description);
                return this;
            }

            public AddRequest CreditType(CreditTypeEnum creditType)
            {
                MParams.AddOpt("credit_type", creditType);
                return this;
            }

            public AddRequest Reference(string reference)
            {
                MParams.AddOpt("reference", reference);
                return this;
            }
        }

        public class DeductRequest : EntityRequest<DeductRequest>
        {
            public DeductRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public DeductRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public DeductRequest Amount(int amount)
            {
                MParams.AddOpt("amount", amount);
                return this;
            }

            public DeductRequest AmountInDecimal(string amountInDecimal)
            {
                MParams.AddOpt("amount_in_decimal", amountInDecimal);
                return this;
            }

            public DeductRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public DeductRequest Description(string description)
            {
                MParams.Add("description", description);
                return this;
            }

            public DeductRequest CreditType(CreditTypeEnum creditType)
            {
                MParams.AddOpt("credit_type", creditType);
                return this;
            }

            public DeductRequest Reference(string reference)
            {
                MParams.AddOpt("reference", reference);
                return this;
            }
        }

        public class SetRequest : EntityRequest<SetRequest>
        {
            public SetRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public SetRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public SetRequest Amount(int amount)
            {
                MParams.AddOpt("amount", amount);
                return this;
            }

            public SetRequest AmountInDecimal(string amountInDecimal)
            {
                MParams.AddOpt("amount_in_decimal", amountInDecimal);
                return this;
            }

            public SetRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public SetRequest Description(string description)
            {
                MParams.Add("description", description);
                return this;
            }

            public SetRequest CreditType(CreditTypeEnum creditType)
            {
                MParams.AddOpt("credit_type", creditType);
                return this;
            }

            public SetRequest Reference(string reference)
            {
                MParams.AddOpt("reference", reference);
                return this;
            }
        }

        public class PromotionalCreditListRequest : ListRequestBase<PromotionalCreditListRequest>
        {
            public PromotionalCreditListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<PromotionalCreditListRequest> Id()
            {
                return new("id", this);
            }

            public TimestampFilter<PromotionalCreditListRequest> CreatedAt()
            {
                return new("created_at", this);
            }

            public EnumFilter<TypeEnum, PromotionalCreditListRequest> Type()
            {
                return new("type", this);
            }

            public StringFilter<PromotionalCreditListRequest> CustomerId()
            {
                return new("customer_id", this);
            }
        }

        #endregion

        #region Subclasses

        #endregion
    }
}