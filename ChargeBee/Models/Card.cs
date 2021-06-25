using System;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Card : Resource
    {
        public enum CardTypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "visa")] Visa,
            [EnumMember(Value = "mastercard")] Mastercard,

            [EnumMember(Value = "american_express")]
            AmericanExpress,
            [EnumMember(Value = "discover")] Discover,
            [EnumMember(Value = "jcb")] Jcb,
            [EnumMember(Value = "diners_club")] DinersClub,
            [EnumMember(Value = "other")] Other,
            [EnumMember(Value = "not_applicable")] NotApplicable
        }

        public enum FundingTypeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "credit")] Credit,
            [EnumMember(Value = "debit")] Debit,
            [EnumMember(Value = "prepaid")] Prepaid,
            [EnumMember(Value = "not_known")] NotKnown,
            [EnumMember(Value = "not_applicable")] NotApplicable
        }

        public enum PoweredByEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "ideal")] Ideal,
            [EnumMember(Value = "sofort")] Sofort,
            [EnumMember(Value = "bancontact")] Bancontact,
            [EnumMember(Value = "giropay")] Giropay,
            [EnumMember(Value = "not_applicable")] NotApplicable
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "valid")] Valid,
            [EnumMember(Value = "expiring")] Expiring,
            [EnumMember(Value = "expired")] Expired
        }

        public Card()
        {
        }

        public Card(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Card(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Card(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("cards", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static UpdateCardForCustomerRequest UpdateCardForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "credit_card");
            return new UpdateCardForCustomerRequest(url, HttpMethod.Post);
        }

        public static SwitchGatewayForCustomerRequest SwitchGatewayForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "switch_gateway");
            return new SwitchGatewayForCustomerRequest(url, HttpMethod.Post);
        }

        public static CopyCardForCustomerRequest CopyCardForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "copy_card");
            return new CopyCardForCustomerRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> DeleteCardForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "delete_card");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string PaymentSourceId => GetValue<string>("payment_source_id");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public GatewayEnum Gateway => GetEnum<GatewayEnum>("gateway");

        public string GatewayAccountId => GetValue<string>("gateway_account_id", false);

        public string RefTxId => GetValue<string>("ref_tx_id", false);

        public string FirstName => GetValue<string>("first_name", false);

        public string LastName => GetValue<string>("last_name", false);

        public string Iin => GetValue<string>("iin");

        public string Last4 => GetValue<string>("last4");

        public CardTypeEnum? CardType => GetEnum<CardTypeEnum>("card_type", false);

        public FundingTypeEnum FundingType => GetEnum<FundingTypeEnum>("funding_type");

        public int ExpiryMonth => GetValue<int>("expiry_month");

        public int ExpiryYear => GetValue<int>("expiry_year");

        public string IssuingCountry => GetValue<string>("issuing_country", false);

        public string BillingAddr1 => GetValue<string>("billing_addr1", false);

        public string BillingAddr2 => GetValue<string>("billing_addr2", false);

        public string BillingCity => GetValue<string>("billing_city", false);

        public string BillingStateCode => GetValue<string>("billing_state_code", false);

        public string BillingState => GetValue<string>("billing_state", false);

        public string BillingCountry => GetValue<string>("billing_country", false);

        public string BillingZip => GetValue<string>("billing_zip", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public string IpAddress => GetValue<string>("ip_address", false);

        public PoweredByEnum? PoweredBy => GetEnum<PoweredByEnum>("powered_by", false);

        public string CustomerId => GetValue<string>("customer_id");

        public string MaskedNumber => GetValue<string>("masked_number", false);

        #endregion

        #region Requests

        public class UpdateCardForCustomerRequest : EntityRequest<UpdateCardForCustomerRequest>
        {
            public UpdateCardForCustomerRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            [Obsolete]
            public UpdateCardForCustomerRequest Gateway(GatewayEnum gateway)
            {
                MParams.AddOpt("gateway", gateway);
                return this;
            }

            public UpdateCardForCustomerRequest GatewayAccountId(string gatewayAccountId)
            {
                MParams.AddOpt("gateway_account_id", gatewayAccountId);
                return this;
            }

            public UpdateCardForCustomerRequest TmpToken(string tmpToken)
            {
                MParams.AddOpt("tmp_token", tmpToken);
                return this;
            }

            public UpdateCardForCustomerRequest FirstName(string firstName)
            {
                MParams.AddOpt("first_name", firstName);
                return this;
            }

            public UpdateCardForCustomerRequest LastName(string lastName)
            {
                MParams.AddOpt("last_name", lastName);
                return this;
            }

            public UpdateCardForCustomerRequest Number(string number)
            {
                MParams.Add("number", number);
                return this;
            }

            public UpdateCardForCustomerRequest ExpiryMonth(int expiryMonth)
            {
                MParams.Add("expiry_month", expiryMonth);
                return this;
            }

            public UpdateCardForCustomerRequest ExpiryYear(int expiryYear)
            {
                MParams.Add("expiry_year", expiryYear);
                return this;
            }

            public UpdateCardForCustomerRequest Cvv(string cvv)
            {
                MParams.AddOpt("cvv", cvv);
                return this;
            }

            public UpdateCardForCustomerRequest BillingAddr1(string billingAddr1)
            {
                MParams.AddOpt("billing_addr1", billingAddr1);
                return this;
            }

            public UpdateCardForCustomerRequest BillingAddr2(string billingAddr2)
            {
                MParams.AddOpt("billing_addr2", billingAddr2);
                return this;
            }

            public UpdateCardForCustomerRequest BillingCity(string billingCity)
            {
                MParams.AddOpt("billing_city", billingCity);
                return this;
            }

            public UpdateCardForCustomerRequest BillingStateCode(string billingStateCode)
            {
                MParams.AddOpt("billing_state_code", billingStateCode);
                return this;
            }

            public UpdateCardForCustomerRequest BillingState(string billingState)
            {
                MParams.AddOpt("billing_state", billingState);
                return this;
            }

            public UpdateCardForCustomerRequest BillingZip(string billingZip)
            {
                MParams.AddOpt("billing_zip", billingZip);
                return this;
            }

            public UpdateCardForCustomerRequest BillingCountry(string billingCountry)
            {
                MParams.AddOpt("billing_country", billingCountry);
                return this;
            }

            [Obsolete]
            public UpdateCardForCustomerRequest IpAddress(string ipAddress)
            {
                MParams.AddOpt("ip_address", ipAddress);
                return this;
            }

            [Obsolete]
            public UpdateCardForCustomerRequest CustomerVatNumber(string customerVatNumber)
            {
                MParams.AddOpt("customer[vat_number]", customerVatNumber);
                return this;
            }
        }

        public class SwitchGatewayForCustomerRequest : EntityRequest<SwitchGatewayForCustomerRequest>
        {
            public SwitchGatewayForCustomerRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            [Obsolete]
            public SwitchGatewayForCustomerRequest Gateway(GatewayEnum gateway)
            {
                MParams.AddOpt("gateway", gateway);
                return this;
            }

            public SwitchGatewayForCustomerRequest GatewayAccountId(string gatewayAccountId)
            {
                MParams.Add("gateway_account_id", gatewayAccountId);
                return this;
            }
        }

        public class CopyCardForCustomerRequest : EntityRequest<CopyCardForCustomerRequest>
        {
            public CopyCardForCustomerRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CopyCardForCustomerRequest GatewayAccountId(string gatewayAccountId)
            {
                MParams.Add("gateway_account_id", gatewayAccountId);
                return this;
            }
        }

        #endregion

        #region Subclasses

        #endregion
    }
}