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
    public class PaymentSource : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "valid")] Valid,
            [EnumMember(Value = "expiring")] Expiring,
            [EnumMember(Value = "expired")] Expired,
            [EnumMember(Value = "invalid")] Invalid,

            [EnumMember(Value = "pending_verification")]
            PendingVerification
        }

        public PaymentSource()
        {
        }

        public PaymentSource(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public PaymentSource(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public PaymentSource(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateUsingTempTokenRequest CreateUsingTempToken()
        {
            var url = ApiUtil.BuildUrl("payment_sources", "create_using_temp_token");
            return new CreateUsingTempTokenRequest(url, HttpMethod.Post);
        }

        public static CreateUsingPermanentTokenRequest CreateUsingPermanentToken()
        {
            var url = ApiUtil.BuildUrl("payment_sources", "create_using_permanent_token");
            return new CreateUsingPermanentTokenRequest(url, HttpMethod.Post);
        }

        public static CreateUsingTokenRequest CreateUsingToken()
        {
            var url = ApiUtil.BuildUrl("payment_sources", "create_using_token");
            return new CreateUsingTokenRequest(url, HttpMethod.Post);
        }

        public static CreateUsingPaymentIntentRequest CreateUsingPaymentIntent()
        {
            var url = ApiUtil.BuildUrl("payment_sources", "create_using_payment_intent");
            return new CreateUsingPaymentIntentRequest(url, HttpMethod.Post);
        }

        public static CreateCardRequest CreateCard()
        {
            var url = ApiUtil.BuildUrl("payment_sources", "create_card");
            return new CreateCardRequest(url, HttpMethod.Post);
        }

        public static CreateBankAccountRequest CreateBankAccount()
        {
            var url = ApiUtil.BuildUrl("payment_sources", "create_bank_account");
            return new CreateBankAccountRequest(url, HttpMethod.Post);
        }

        public static UpdateCardRequest UpdateCard(string id)
        {
            var url = ApiUtil.BuildUrl("payment_sources", CheckNull(id), "update_card");
            return new UpdateCardRequest(url, HttpMethod.Post);
        }

        public static VerifyBankAccountRequest VerifyBankAccount(string id)
        {
            var url = ApiUtil.BuildUrl("payment_sources", CheckNull(id), "verify_bank_account");
            return new VerifyBankAccountRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("payment_sources", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static PaymentSourceListRequest List()
        {
            var url = ApiUtil.BuildUrl("payment_sources");
            return new PaymentSourceListRequest(url);
        }

        public static SwitchGatewayAccountRequest SwitchGatewayAccount(string id)
        {
            var url = ApiUtil.BuildUrl("payment_sources", CheckNull(id), "switch_gateway_account");
            return new SwitchGatewayAccountRequest(url, HttpMethod.Post);
        }

        public static ExportPaymentSourceRequest ExportPaymentSource(string id)
        {
            var url = ApiUtil.BuildUrl("payment_sources", CheckNull(id), "export_payment_source");
            return new ExportPaymentSourceRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("payment_sources", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> DeleteLocal(string id)
        {
            var url = ApiUtil.BuildUrl("payment_sources", CheckNull(id), "delete_local");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public string CustomerId => GetValue<string>("customer_id");

        public TypeEnum PaymentSourceType => GetEnum<TypeEnum>("type");

        public string ReferenceId => GetValue<string>("reference_id");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public GatewayEnum Gateway => GetEnum<GatewayEnum>("gateway");

        public string GatewayAccountId => GetValue<string>("gateway_account_id", false);

        public string IpAddress => GetValue<string>("ip_address", false);

        public string IssuingCountry => GetValue<string>("issuing_country", false);

        public PaymentSourceCard Card => GetSubResource<PaymentSourceCard>("card");

        public PaymentSourceBankAccount BankAccount => GetSubResource<PaymentSourceBankAccount>("bank_account");

        public PaymentSourceAmazonPayment AmazonPayment => GetSubResource<PaymentSourceAmazonPayment>("amazon_payment");

        public PaymentSourcePaypal Paypal => GetSubResource<PaymentSourcePaypal>("paypal");

        public bool Deleted => GetValue<bool>("deleted");

        #endregion

        #region Requests

        public class CreateUsingTempTokenRequest : EntityRequest<CreateUsingTempTokenRequest>
        {
            public CreateUsingTempTokenRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateUsingTempTokenRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public CreateUsingTempTokenRequest GatewayAccountId(string gatewayAccountId)
            {
                MParams.AddOpt("gateway_account_id", gatewayAccountId);
                return this;
            }

            public CreateUsingTempTokenRequest Type(TypeEnum type)
            {
                MParams.Add("type", type);
                return this;
            }

            public CreateUsingTempTokenRequest TmpToken(string tmpToken)
            {
                MParams.Add("tmp_token", tmpToken);
                return this;
            }

            public CreateUsingTempTokenRequest IssuingCountry(string issuingCountry)
            {
                MParams.AddOpt("issuing_country", issuingCountry);
                return this;
            }

            public CreateUsingTempTokenRequest ReplacePrimaryPaymentSource(bool replacePrimaryPaymentSource)
            {
                MParams.AddOpt("replace_primary_payment_source", replacePrimaryPaymentSource);
                return this;
            }
        }

        public class CreateUsingPermanentTokenRequest : EntityRequest<CreateUsingPermanentTokenRequest>
        {
            public CreateUsingPermanentTokenRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateUsingPermanentTokenRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public CreateUsingPermanentTokenRequest Type(TypeEnum type)
            {
                MParams.Add("type", type);
                return this;
            }

            public CreateUsingPermanentTokenRequest GatewayAccountId(string gatewayAccountId)
            {
                MParams.AddOpt("gateway_account_id", gatewayAccountId);
                return this;
            }

            public CreateUsingPermanentTokenRequest ReferenceId(string referenceId)
            {
                MParams.Add("reference_id", referenceId);
                return this;
            }

            public CreateUsingPermanentTokenRequest IssuingCountry(string issuingCountry)
            {
                MParams.AddOpt("issuing_country", issuingCountry);
                return this;
            }

            public CreateUsingPermanentTokenRequest ReplacePrimaryPaymentSource(bool replacePrimaryPaymentSource)
            {
                MParams.AddOpt("replace_primary_payment_source", replacePrimaryPaymentSource);
                return this;
            }
        }

        public class CreateUsingTokenRequest : EntityRequest<CreateUsingTokenRequest>
        {
            public CreateUsingTokenRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateUsingTokenRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public CreateUsingTokenRequest ReplacePrimaryPaymentSource(bool replacePrimaryPaymentSource)
            {
                MParams.AddOpt("replace_primary_payment_source", replacePrimaryPaymentSource);
                return this;
            }

            public CreateUsingTokenRequest TokenId(string tokenId)
            {
                MParams.Add("token_id", tokenId);
                return this;
            }
        }

        public class CreateUsingPaymentIntentRequest : EntityRequest<CreateUsingPaymentIntentRequest>
        {
            public CreateUsingPaymentIntentRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateUsingPaymentIntentRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public CreateUsingPaymentIntentRequest ReplacePrimaryPaymentSource(bool replacePrimaryPaymentSource)
            {
                MParams.AddOpt("replace_primary_payment_source", replacePrimaryPaymentSource);
                return this;
            }

            public CreateUsingPaymentIntentRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public CreateUsingPaymentIntentRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public CreateUsingPaymentIntentRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public CreateUsingPaymentIntentRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public CreateUsingPaymentIntentRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public CreateUsingPaymentIntentRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }
        }

        public class CreateCardRequest : EntityRequest<CreateCardRequest>
        {
            public CreateCardRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateCardRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public CreateCardRequest ReplacePrimaryPaymentSource(bool replacePrimaryPaymentSource)
            {
                MParams.AddOpt("replace_primary_payment_source", replacePrimaryPaymentSource);
                return this;
            }

            public CreateCardRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            public CreateCardRequest CardFirstName(string cardFirstName)
            {
                MParams.AddOpt("card[first_name]", cardFirstName);
                return this;
            }

            public CreateCardRequest CardLastName(string cardLastName)
            {
                MParams.AddOpt("card[last_name]", cardLastName);
                return this;
            }

            public CreateCardRequest CardNumber(string cardNumber)
            {
                MParams.Add("card[number]", cardNumber);
                return this;
            }

            public CreateCardRequest CardExpiryMonth(int cardExpiryMonth)
            {
                MParams.Add("card[expiry_month]", cardExpiryMonth);
                return this;
            }

            public CreateCardRequest CardExpiryYear(int cardExpiryYear)
            {
                MParams.Add("card[expiry_year]", cardExpiryYear);
                return this;
            }

            public CreateCardRequest CardCvv(string cardCvv)
            {
                MParams.AddOpt("card[cvv]", cardCvv);
                return this;
            }

            public CreateCardRequest CardBillingAddr1(string cardBillingAddr1)
            {
                MParams.AddOpt("card[billing_addr1]", cardBillingAddr1);
                return this;
            }

            public CreateCardRequest CardBillingAddr2(string cardBillingAddr2)
            {
                MParams.AddOpt("card[billing_addr2]", cardBillingAddr2);
                return this;
            }

            public CreateCardRequest CardBillingCity(string cardBillingCity)
            {
                MParams.AddOpt("card[billing_city]", cardBillingCity);
                return this;
            }

            public CreateCardRequest CardBillingStateCode(string cardBillingStateCode)
            {
                MParams.AddOpt("card[billing_state_code]", cardBillingStateCode);
                return this;
            }

            public CreateCardRequest CardBillingState(string cardBillingState)
            {
                MParams.AddOpt("card[billing_state]", cardBillingState);
                return this;
            }

            public CreateCardRequest CardBillingZip(string cardBillingZip)
            {
                MParams.AddOpt("card[billing_zip]", cardBillingZip);
                return this;
            }

            public CreateCardRequest CardBillingCountry(string cardBillingCountry)
            {
                MParams.AddOpt("card[billing_country]", cardBillingCountry);
                return this;
            }
        }

        public class CreateBankAccountRequest : EntityRequest<CreateBankAccountRequest>
        {
            public CreateBankAccountRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateBankAccountRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public CreateBankAccountRequest IssuingCountry(string issuingCountry)
            {
                MParams.AddOpt("issuing_country", issuingCountry);
                return this;
            }

            public CreateBankAccountRequest ReplacePrimaryPaymentSource(bool replacePrimaryPaymentSource)
            {
                MParams.AddOpt("replace_primary_payment_source", replacePrimaryPaymentSource);
                return this;
            }

            public CreateBankAccountRequest BankAccountGatewayAccountId(string bankAccountGatewayAccountId)
            {
                MParams.AddOpt("bank_account[gateway_account_id]", bankAccountGatewayAccountId);
                return this;
            }

            public CreateBankAccountRequest BankAccountIban(string bankAccountIban)
            {
                MParams.AddOpt("bank_account[iban]", bankAccountIban);
                return this;
            }

            public CreateBankAccountRequest BankAccountFirstName(string bankAccountFirstName)
            {
                MParams.AddOpt("bank_account[first_name]", bankAccountFirstName);
                return this;
            }

            public CreateBankAccountRequest BankAccountLastName(string bankAccountLastName)
            {
                MParams.AddOpt("bank_account[last_name]", bankAccountLastName);
                return this;
            }

            public CreateBankAccountRequest BankAccountCompany(string bankAccountCompany)
            {
                MParams.AddOpt("bank_account[company]", bankAccountCompany);
                return this;
            }

            public CreateBankAccountRequest BankAccountEmail(string bankAccountEmail)
            {
                MParams.AddOpt("bank_account[email]", bankAccountEmail);
                return this;
            }

            public CreateBankAccountRequest BankAccountBankName(string bankAccountBankName)
            {
                MParams.AddOpt("bank_account[bank_name]", bankAccountBankName);
                return this;
            }

            public CreateBankAccountRequest BankAccountAccountNumber(string bankAccountAccountNumber)
            {
                MParams.AddOpt("bank_account[account_number]", bankAccountAccountNumber);
                return this;
            }

            public CreateBankAccountRequest BankAccountRoutingNumber(string bankAccountRoutingNumber)
            {
                MParams.AddOpt("bank_account[routing_number]", bankAccountRoutingNumber);
                return this;
            }

            public CreateBankAccountRequest BankAccountBankCode(string bankAccountBankCode)
            {
                MParams.AddOpt("bank_account[bank_code]", bankAccountBankCode);
                return this;
            }

            public CreateBankAccountRequest BankAccountAccountType(AccountTypeEnum bankAccountAccountType)
            {
                MParams.AddOpt("bank_account[account_type]", bankAccountAccountType);
                return this;
            }

            public CreateBankAccountRequest BankAccountAccountHolderType(
                AccountHolderTypeEnum bankAccountAccountHolderType)
            {
                MParams.AddOpt("bank_account[account_holder_type]", bankAccountAccountHolderType);
                return this;
            }

            public CreateBankAccountRequest BankAccountEcheckType(EcheckTypeEnum bankAccountEcheckType)
            {
                MParams.AddOpt("bank_account[echeck_type]", bankAccountEcheckType);
                return this;
            }

            public CreateBankAccountRequest BankAccountSwedishIdentityNumber(string bankAccountSwedishIdentityNumber)
            {
                MParams.AddOpt("bank_account[swedish_identity_number]", bankAccountSwedishIdentityNumber);
                return this;
            }
        }

        public class UpdateCardRequest : EntityRequest<UpdateCardRequest>
        {
            public UpdateCardRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateCardRequest GatewayMetaData(JToken gatewayMetaData)
            {
                MParams.AddOpt("gateway_meta_data", gatewayMetaData);
                return this;
            }

            public UpdateCardRequest ReferenceTransaction(string referenceTransaction)
            {
                MParams.AddOpt("reference_transaction", referenceTransaction);
                return this;
            }

            public UpdateCardRequest CardFirstName(string cardFirstName)
            {
                MParams.AddOpt("card[first_name]", cardFirstName);
                return this;
            }

            public UpdateCardRequest CardLastName(string cardLastName)
            {
                MParams.AddOpt("card[last_name]", cardLastName);
                return this;
            }

            public UpdateCardRequest CardExpiryMonth(int cardExpiryMonth)
            {
                MParams.AddOpt("card[expiry_month]", cardExpiryMonth);
                return this;
            }

            public UpdateCardRequest CardExpiryYear(int cardExpiryYear)
            {
                MParams.AddOpt("card[expiry_year]", cardExpiryYear);
                return this;
            }

            public UpdateCardRequest CardBillingAddr1(string cardBillingAddr1)
            {
                MParams.AddOpt("card[billing_addr1]", cardBillingAddr1);
                return this;
            }

            public UpdateCardRequest CardBillingAddr2(string cardBillingAddr2)
            {
                MParams.AddOpt("card[billing_addr2]", cardBillingAddr2);
                return this;
            }

            public UpdateCardRequest CardBillingCity(string cardBillingCity)
            {
                MParams.AddOpt("card[billing_city]", cardBillingCity);
                return this;
            }

            public UpdateCardRequest CardBillingZip(string cardBillingZip)
            {
                MParams.AddOpt("card[billing_zip]", cardBillingZip);
                return this;
            }

            public UpdateCardRequest CardBillingStateCode(string cardBillingStateCode)
            {
                MParams.AddOpt("card[billing_state_code]", cardBillingStateCode);
                return this;
            }

            public UpdateCardRequest CardBillingState(string cardBillingState)
            {
                MParams.AddOpt("card[billing_state]", cardBillingState);
                return this;
            }

            public UpdateCardRequest CardBillingCountry(string cardBillingCountry)
            {
                MParams.AddOpt("card[billing_country]", cardBillingCountry);
                return this;
            }
        }

        public class VerifyBankAccountRequest : EntityRequest<VerifyBankAccountRequest>
        {
            public VerifyBankAccountRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public VerifyBankAccountRequest Amount1(int amount1)
            {
                MParams.Add("amount1", amount1);
                return this;
            }

            public VerifyBankAccountRequest Amount2(int amount2)
            {
                MParams.Add("amount2", amount2);
                return this;
            }
        }

        public class PaymentSourceListRequest : ListRequestBase<PaymentSourceListRequest>
        {
            public PaymentSourceListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<PaymentSourceListRequest> CustomerId()
            {
                return new StringFilter<PaymentSourceListRequest>("customer_id", this).SupportsMultiOperators(true);
            }

            public EnumFilter<TypeEnum, PaymentSourceListRequest> Type()
            {
                return new("type", this);
            }

            public EnumFilter<StatusEnum, PaymentSourceListRequest> Status()
            {
                return new("status", this);
            }

            public TimestampFilter<PaymentSourceListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public TimestampFilter<PaymentSourceListRequest> CreatedAt()
            {
                return new("created_at", this);
            }
        }

        public class SwitchGatewayAccountRequest : EntityRequest<SwitchGatewayAccountRequest>
        {
            public SwitchGatewayAccountRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public SwitchGatewayAccountRequest GatewayAccountId(string gatewayAccountId)
            {
                MParams.Add("gateway_account_id", gatewayAccountId);
                return this;
            }
        }

        public class ExportPaymentSourceRequest : EntityRequest<ExportPaymentSourceRequest>
        {
            public ExportPaymentSourceRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ExportPaymentSourceRequest GatewayAccountId(string gatewayAccountId)
            {
                MParams.Add("gateway_account_id", gatewayAccountId);
                return this;
            }
        }

        #endregion

        #region Subclasses

        public class PaymentSourceCard : Resource
        {
            public enum BrandEnum
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
                [EnumMember(Value = "other")] Other
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

            public string FirstName()
            {
                return GetValue<string>("first_name", false);
            }

            public string LastName()
            {
                return GetValue<string>("last_name", false);
            }

            public string Iin()
            {
                return GetValue<string>("iin");
            }

            public string Last4()
            {
                return GetValue<string>("last4");
            }

            public BrandEnum Brand()
            {
                return GetEnum<BrandEnum>("brand");
            }

            public FundingTypeEnum FundingType()
            {
                return GetEnum<FundingTypeEnum>("funding_type");
            }

            public int ExpiryMonth()
            {
                return GetValue<int>("expiry_month");
            }

            public int ExpiryYear()
            {
                return GetValue<int>("expiry_year");
            }

            public string BillingAddr1()
            {
                return GetValue<string>("billing_addr1", false);
            }

            public string BillingAddr2()
            {
                return GetValue<string>("billing_addr2", false);
            }

            public string BillingCity()
            {
                return GetValue<string>("billing_city", false);
            }

            public string BillingStateCode()
            {
                return GetValue<string>("billing_state_code", false);
            }

            public string BillingState()
            {
                return GetValue<string>("billing_state", false);
            }

            public string BillingCountry()
            {
                return GetValue<string>("billing_country", false);
            }

            public string BillingZip()
            {
                return GetValue<string>("billing_zip", false);
            }

            public string MaskedNumber()
            {
                return GetValue<string>("masked_number", false);
            }
        }

        public class PaymentSourceBankAccount : Resource
        {
            public enum AccountHolderTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "individual")] Individual,
                [EnumMember(Value = "company")] Company
            }

            public enum AccountTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "checking")] Checking,
                [EnumMember(Value = "savings")] Savings,

                [EnumMember(Value = "business_checking")]
                BusinessChecking
            }

            public enum EcheckTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "web")] Web,
                [EnumMember(Value = "ppd")] Ppd,
                [EnumMember(Value = "ccd")] Ccd
            }

            public string Last4()
            {
                return GetValue<string>("last4");
            }

            public string NameOnAccount()
            {
                return GetValue<string>("name_on_account", false);
            }

            public string BankName()
            {
                return GetValue<string>("bank_name", false);
            }

            public string MandateId()
            {
                return GetValue<string>("mandate_id", false);
            }

            public AccountTypeEnum? AccountType()
            {
                return GetEnum<AccountTypeEnum>("account_type", false);
            }

            public EcheckTypeEnum? EcheckType()
            {
                return GetEnum<EcheckTypeEnum>("echeck_type", false);
            }

            public AccountHolderTypeEnum? AccountHolderType()
            {
                return GetEnum<AccountHolderTypeEnum>("account_holder_type", false);
            }
        }

        public class PaymentSourceAmazonPayment : Resource
        {
            public string Email()
            {
                return GetValue<string>("email", false);
            }

            public string AgreementId()
            {
                return GetValue<string>("agreement_id", false);
            }
        }

        public class PaymentSourcePaypal : Resource
        {
            public string Email()
            {
                return GetValue<string>("email", false);
            }

            public string AgreementId()
            {
                return GetValue<string>("agreement_id", false);
            }
        }

        #endregion
    }
}