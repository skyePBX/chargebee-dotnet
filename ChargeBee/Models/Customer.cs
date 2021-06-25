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
    public class Customer : Resource
    {
        public enum BillingDayOfWeekEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "sunday")] Sunday,
            [EnumMember(Value = "monday")] Monday,
            [EnumMember(Value = "tuesday")] Tuesday,
            [EnumMember(Value = "wednesday")] Wednesday,
            [EnumMember(Value = "thursday")] Thursday,
            [EnumMember(Value = "friday")] Friday,
            [EnumMember(Value = "saturday")] Saturday
        }

        [Obsolete]
        public enum CardStatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "no_card")] NoCard,
            [EnumMember(Value = "valid")] Valid,
            [EnumMember(Value = "expiring")] Expiring,
            [EnumMember(Value = "expired")] Expired,

            [EnumMember(Value = "pending_verification")]
            PendingVerification,
            [EnumMember(Value = "invalid")] Invalid
        }

        public enum FraudFlagEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "safe")] Safe,
            [EnumMember(Value = "suspicious")] Suspicious,
            [EnumMember(Value = "fraudulent")] Fraudulent
        }

        public enum PiiClearedEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "active")] Active,

            [EnumMember(Value = "scheduled_for_clear")]
            ScheduledForClear,
            [EnumMember(Value = "cleared")] Cleared
        }

        public enum VatNumberStatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "valid")] Valid,
            [EnumMember(Value = "invalid")] Invalid,
            [EnumMember(Value = "not_validated")] NotValidated,
            [EnumMember(Value = "undetermined")] Undetermined
        }

        public Customer()
        {
        }

        public Customer(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Customer(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Customer(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("customers");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static CustomerListRequest List()
        {
            var url = ApiUtil.BuildUrl("customers");
            return new CustomerListRequest(url);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static UpdateRequest Update(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id));
            return new UpdateRequest(url, HttpMethod.Post);
        }

        public static UpdatePaymentMethodRequest UpdatePaymentMethod(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "update_payment_method");
            return new UpdatePaymentMethodRequest(url, HttpMethod.Post);
        }

        public static UpdateBillingInfoRequest UpdateBillingInfo(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "update_billing_info");
            return new UpdateBillingInfoRequest(url, HttpMethod.Post);
        }

        public static ListRequest ContactsForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "contacts");
            return new ListRequest(url);
        }

        public static AssignPaymentRoleRequest AssignPaymentRole(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "assign_payment_role");
            return new AssignPaymentRoleRequest(url, HttpMethod.Post);
        }

        public static AddContactRequest AddContact(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "add_contact");
            return new AddContactRequest(url, HttpMethod.Post);
        }

        public static UpdateContactRequest UpdateContact(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "update_contact");
            return new UpdateContactRequest(url, HttpMethod.Post);
        }

        public static DeleteContactRequest DeleteContact(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "delete_contact");
            return new DeleteContactRequest(url, HttpMethod.Post);
        }

        [Obsolete]
        public static AddPromotionalCreditsRequest AddPromotionalCredits(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "add_promotional_credits");
            return new AddPromotionalCreditsRequest(url, HttpMethod.Post);
        }

        [Obsolete]
        public static DeductPromotionalCreditsRequest DeductPromotionalCredits(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "deduct_promotional_credits");
            return new DeductPromotionalCreditsRequest(url, HttpMethod.Post);
        }

        [Obsolete]
        public static SetPromotionalCreditsRequest SetPromotionalCredits(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "set_promotional_credits");
            return new SetPromotionalCreditsRequest(url, HttpMethod.Post);
        }

        public static RecordExcessPaymentRequest RecordExcessPayment(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "record_excess_payment");
            return new RecordExcessPaymentRequest(url, HttpMethod.Post);
        }

        public static CollectPaymentRequest CollectPayment(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "collect_payment");
            return new CollectPaymentRequest(url, HttpMethod.Post);
        }

        public static DeleteRequest Delete(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "delete");
            return new DeleteRequest(url, HttpMethod.Post);
        }

        public static MoveRequest Move()
        {
            var url = ApiUtil.BuildUrl("customers", "move");
            return new MoveRequest(url, HttpMethod.Post);
        }

        public static ChangeBillingDateRequest ChangeBillingDate(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "change_billing_date");
            return new ChangeBillingDateRequest(url, HttpMethod.Post);
        }

        public static MergeRequest Merge()
        {
            var url = ApiUtil.BuildUrl("customers", "merge");
            return new MergeRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> ClearPersonalData(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "clear_personal_data");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static RelationshipsRequest Relationships(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "relationships");
            return new RelationshipsRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> DeleteRelationship(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "delete_relationship");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static HierarchyRequest Hierarchy(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "hierarchy");
            return new HierarchyRequest(url, HttpMethod.Get);
        }

        public static UpdateHierarchySettingsRequest UpdateHierarchySettings(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "update_hierarchy_settings");
            return new UpdateHierarchySettingsRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string FirstName => GetValue<string>("first_name", false);

        public string LastName => GetValue<string>("last_name", false);

        public string Email => GetValue<string>("email", false);

        public string Phone => GetValue<string>("phone", false);

        public string Company => GetValue<string>("company", false);

        public string VatNumber => GetValue<string>("vat_number", false);

        public AutoCollectionEnum AutoCollection => GetEnum<AutoCollectionEnum>("auto_collection");

        public OfflinePaymentMethodEnum? OfflinePaymentMethod =>
            GetEnum<OfflinePaymentMethodEnum>("offline_payment_method", false);

        public int NetTermDays => GetValue<int>("net_term_days");

        public DateTime? VatNumberValidatedTime => GetDateTime("vat_number_validated_time", false);

        public VatNumberStatusEnum? VatNumberStatus => GetEnum<VatNumberStatusEnum>("vat_number_status", false);

        public bool AllowDirectDebit => GetValue<bool>("allow_direct_debit");

        public bool? IsLocationValid => GetValue<bool?>("is_location_valid", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public string CreatedFromIp => GetValue<string>("created_from_ip", false);

        public JArray ExemptionDetails => GetJArray("exemption_details", false);

        public TaxabilityEnum? Taxability => GetEnum<TaxabilityEnum>("taxability", false);

        public EntityCodeEnum? EntityCode => GetEnum<EntityCodeEnum>("entity_code", false);

        public string ExemptNumber => GetValue<string>("exempt_number", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public string Locale => GetValue<string>("locale", false);

        public int? BillingDate => GetValue<int?>("billing_date", false);

        public BillingDateModeEnum? BillingDateMode => GetEnum<BillingDateModeEnum>("billing_date_mode", false);

        public BillingDayOfWeekEnum? BillingDayOfWeek => GetEnum<BillingDayOfWeekEnum>("billing_day_of_week", false);

        public BillingDayOfWeekModeEnum? BillingDayOfWeekMode =>
            GetEnum<BillingDayOfWeekModeEnum>("billing_day_of_week_mode", false);

        public PiiClearedEnum? PiiCleared => GetEnum<PiiClearedEnum>("pii_cleared", false);

        public bool? AutoCloseInvoices => GetValue<bool?>("auto_close_invoices", false);

        [Obsolete] public CardStatusEnum? CardStatus => GetEnum<CardStatusEnum>("card_status", false);

        public FraudFlagEnum? FraudFlag => GetEnum<FraudFlagEnum>("fraud_flag", false);

        public string PrimaryPaymentSourceId => GetValue<string>("primary_payment_source_id", false);

        public string BackupPaymentSourceId => GetValue<string>("backup_payment_source_id", false);

        public CustomerBillingAddress BillingAddress => GetSubResource<CustomerBillingAddress>("billing_address");

        public List<CustomerReferralUrl> ReferralUrls => GetResourceList<CustomerReferralUrl>("referral_urls");

        public List<CustomerContact> Contacts => GetResourceList<CustomerContact>("contacts");

        public CustomerPaymentMethod PaymentMethod => GetSubResource<CustomerPaymentMethod>("payment_method");

        public string InvoiceNotes => GetValue<string>("invoice_notes", false);

        public string PreferredCurrencyCode => GetValue<string>("preferred_currency_code", false);

        public int PromotionalCredits => GetValue<int>("promotional_credits");

        public int UnbilledCharges => GetValue<int>("unbilled_charges");

        public int RefundableCredits => GetValue<int>("refundable_credits");

        public int ExcessPayments => GetValue<int>("excess_payments");

        public List<CustomerBalance> Balances => GetResourceList<CustomerBalance>("balances");

        public JToken MetaData => GetJToken("meta_data", false);

        public bool Deleted => GetValue<bool>("deleted");

        public bool? RegisteredForGst => GetValue<bool?>("registered_for_gst", false);

        public bool? ConsolidatedInvoicing => GetValue<bool?>("consolidated_invoicing", false);

        public CustomerTypeEnum? CustomerType => GetEnum<CustomerTypeEnum>("customer_type", false);

        public bool? BusinessCustomerWithoutVatNumber => GetValue<bool?>("business_customer_without_vat_number", false);

        public string ClientProfileId => GetValue<string>("client_profile_id", false);

        public CustomerRelationship Relationship => GetSubResource<CustomerRelationship>("relationship");

        public bool? UseDefaultHierarchySettings => GetValue<bool?>("use_default_hierarchy_settings", false);

        public CustomerParentAccountAccess ParentAccountAccess =>
            GetSubResource<CustomerParentAccountAccess>("parent_account_access");

        public CustomerChildAccountAccess ChildAccountAccess =>
            GetSubResource<CustomerChildAccountAccess>("child_account_access");

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

            public CreateRequest FirstName(string firstName)
            {
                MParams.AddOpt("first_name", firstName);
                return this;
            }

            public CreateRequest LastName(string lastName)
            {
                MParams.AddOpt("last_name", lastName);
                return this;
            }

            public CreateRequest Email(string email)
            {
                MParams.AddOpt("email", email);
                return this;
            }

            public CreateRequest PreferredCurrencyCode(string preferredCurrencyCode)
            {
                MParams.AddOpt("preferred_currency_code", preferredCurrencyCode);
                return this;
            }

            public CreateRequest Phone(string phone)
            {
                MParams.AddOpt("phone", phone);
                return this;
            }

            public CreateRequest Company(string company)
            {
                MParams.AddOpt("company", company);
                return this;
            }

            public CreateRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public CreateRequest NetTermDays(int netTermDays)
            {
                MParams.AddOpt("net_term_days", netTermDays);
                return this;
            }

            public CreateRequest AllowDirectDebit(bool allowDirectDebit)
            {
                MParams.AddOpt("allow_direct_debit", allowDirectDebit);
                return this;
            }

            public CreateRequest VatNumber(string vatNumber)
            {
                MParams.AddOpt("vat_number", vatNumber);
                return this;
            }

            public CreateRequest RegisteredForGst(bool registeredForGst)
            {
                MParams.AddOpt("registered_for_gst", registeredForGst);
                return this;
            }

            public CreateRequest Taxability(TaxabilityEnum taxability)
            {
                MParams.AddOpt("taxability", taxability);
                return this;
            }

            public CreateRequest ExemptionDetails(JArray exemptionDetails)
            {
                MParams.AddOpt("exemption_details", exemptionDetails);
                return this;
            }

            public CreateRequest CustomerType(CustomerTypeEnum customerType)
            {
                MParams.AddOpt("customer_type", customerType);
                return this;
            }

            public CreateRequest ClientProfileId(string clientProfileId)
            {
                MParams.AddOpt("client_profile_id", clientProfileId);
                return this;
            }

            public CreateRequest TaxjarExemptionCategory(TaxjarExemptionCategoryEnum taxjarExemptionCategory)
            {
                MParams.AddOpt("taxjar_exemption_category", taxjarExemptionCategory);
                return this;
            }

            public CreateRequest BusinessCustomerWithoutVatNumber(bool businessCustomerWithoutVatNumber)
            {
                MParams.AddOpt("business_customer_without_vat_number", businessCustomerWithoutVatNumber);
                return this;
            }

            public CreateRequest Locale(string locale)
            {
                MParams.AddOpt("locale", locale);
                return this;
            }

            public CreateRequest EntityCode(EntityCodeEnum entityCode)
            {
                MParams.AddOpt("entity_code", entityCode);
                return this;
            }

            public CreateRequest ExemptNumber(string exemptNumber)
            {
                MParams.AddOpt("exempt_number", exemptNumber);
                return this;
            }

            public CreateRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public CreateRequest OfflinePaymentMethod(OfflinePaymentMethodEnum offlinePaymentMethod)
            {
                MParams.AddOpt("offline_payment_method", offlinePaymentMethod);
                return this;
            }

            public CreateRequest AutoCloseInvoices(bool autoCloseInvoices)
            {
                MParams.AddOpt("auto_close_invoices", autoCloseInvoices);
                return this;
            }

            public CreateRequest ConsolidatedInvoicing(bool consolidatedInvoicing)
            {
                MParams.AddOpt("consolidated_invoicing", consolidatedInvoicing);
                return this;
            }

            public CreateRequest TokenId(string tokenId)
            {
                MParams.AddOpt("token_id", tokenId);
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
        }

        public class CustomerListRequest : ListRequestBase<CustomerListRequest>
        {
            public CustomerListRequest(string url)
                : base(url)
            {
            }

            public CustomerListRequest IncludeDeleted(bool includeDeleted)
            {
                MParams.AddOpt("include_deleted", includeDeleted);
                return this;
            }

            public StringFilter<CustomerListRequest> Id()
            {
                return new StringFilter<CustomerListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<CustomerListRequest> FirstName()
            {
                return new StringFilter<CustomerListRequest>("first_name", this).SupportsPresenceOperator(true);
            }

            public StringFilter<CustomerListRequest> LastName()
            {
                return new StringFilter<CustomerListRequest>("last_name", this).SupportsPresenceOperator(true);
            }

            public StringFilter<CustomerListRequest> Email()
            {
                return new StringFilter<CustomerListRequest>("email", this).SupportsPresenceOperator(true);
            }

            public StringFilter<CustomerListRequest> Company()
            {
                return new StringFilter<CustomerListRequest>("company", this).SupportsPresenceOperator(true);
            }

            public StringFilter<CustomerListRequest> Phone()
            {
                return new StringFilter<CustomerListRequest>("phone", this).SupportsPresenceOperator(true);
            }

            public EnumFilter<AutoCollectionEnum, CustomerListRequest> AutoCollection()
            {
                return new("auto_collection", this);
            }

            public EnumFilter<TaxabilityEnum, CustomerListRequest> Taxability()
            {
                return new("taxability", this);
            }

            public TimestampFilter<CustomerListRequest> CreatedAt()
            {
                return new("created_at", this);
            }

            public TimestampFilter<CustomerListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public EnumFilter<OfflinePaymentMethodEnum, CustomerListRequest> OfflinePaymentMethod()
            {
                return new("offline_payment_method", this);
            }

            public BooleanFilter<CustomerListRequest> AutoCloseInvoices()
            {
                return new("auto_close_invoices", this);
            }

            public CustomerListRequest SortByCreatedAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "created_at");
                return this;
            }

            public CustomerListRequest SortByUpdatedAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "updated_at");
                return this;
            }

            public StringFilter<CustomerListRequest> RelationshipParentId()
            {
                return new("relationship[parent_id]", this);
            }

            public StringFilter<CustomerListRequest> RelationshipPaymentOwnerId()
            {
                return new("relationship[payment_owner_id]", this);
            }

            public StringFilter<CustomerListRequest> RelationshipInvoiceOwnerId()
            {
                return new("relationship[invoice_owner_id]", this);
            }
        }

        public class UpdateRequest : EntityRequest<UpdateRequest>
        {
            public UpdateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateRequest FirstName(string firstName)
            {
                MParams.AddOpt("first_name", firstName);
                return this;
            }

            public UpdateRequest LastName(string lastName)
            {
                MParams.AddOpt("last_name", lastName);
                return this;
            }

            public UpdateRequest Email(string email)
            {
                MParams.AddOpt("email", email);
                return this;
            }

            public UpdateRequest PreferredCurrencyCode(string preferredCurrencyCode)
            {
                MParams.AddOpt("preferred_currency_code", preferredCurrencyCode);
                return this;
            }

            public UpdateRequest Phone(string phone)
            {
                MParams.AddOpt("phone", phone);
                return this;
            }

            public UpdateRequest Company(string company)
            {
                MParams.AddOpt("company", company);
                return this;
            }

            public UpdateRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public UpdateRequest AllowDirectDebit(bool allowDirectDebit)
            {
                MParams.AddOpt("allow_direct_debit", allowDirectDebit);
                return this;
            }

            public UpdateRequest NetTermDays(int netTermDays)
            {
                MParams.AddOpt("net_term_days", netTermDays);
                return this;
            }

            public UpdateRequest Taxability(TaxabilityEnum taxability)
            {
                MParams.AddOpt("taxability", taxability);
                return this;
            }

            public UpdateRequest ExemptionDetails(JArray exemptionDetails)
            {
                MParams.AddOpt("exemption_details", exemptionDetails);
                return this;
            }

            public UpdateRequest CustomerType(CustomerTypeEnum customerType)
            {
                MParams.AddOpt("customer_type", customerType);
                return this;
            }

            public UpdateRequest ClientProfileId(string clientProfileId)
            {
                MParams.AddOpt("client_profile_id", clientProfileId);
                return this;
            }

            public UpdateRequest TaxjarExemptionCategory(TaxjarExemptionCategoryEnum taxjarExemptionCategory)
            {
                MParams.AddOpt("taxjar_exemption_category", taxjarExemptionCategory);
                return this;
            }

            public UpdateRequest Locale(string locale)
            {
                MParams.AddOpt("locale", locale);
                return this;
            }

            public UpdateRequest EntityCode(EntityCodeEnum entityCode)
            {
                MParams.AddOpt("entity_code", entityCode);
                return this;
            }

            public UpdateRequest ExemptNumber(string exemptNumber)
            {
                MParams.AddOpt("exempt_number", exemptNumber);
                return this;
            }

            public UpdateRequest OfflinePaymentMethod(OfflinePaymentMethodEnum offlinePaymentMethod)
            {
                MParams.AddOpt("offline_payment_method", offlinePaymentMethod);
                return this;
            }

            public UpdateRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public UpdateRequest AutoCloseInvoices(bool autoCloseInvoices)
            {
                MParams.AddOpt("auto_close_invoices", autoCloseInvoices);
                return this;
            }

            public UpdateRequest MetaData(JToken metaData)
            {
                MParams.AddOpt("meta_data", metaData);
                return this;
            }

            public UpdateRequest FraudFlag(FraudFlagEnum fraudFlag)
            {
                MParams.AddOpt("fraud_flag", fraudFlag);
                return this;
            }

            public UpdateRequest ConsolidatedInvoicing(bool consolidatedInvoicing)
            {
                MParams.AddOpt("consolidated_invoicing", consolidatedInvoicing);
                return this;
            }
        }

        public class UpdatePaymentMethodRequest : EntityRequest<UpdatePaymentMethodRequest>
        {
            public UpdatePaymentMethodRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdatePaymentMethodRequest PaymentMethodType(TypeEnum paymentMethodType)
            {
                MParams.Add("payment_method[type]", paymentMethodType);
                return this;
            }

            [Obsolete]
            public UpdatePaymentMethodRequest PaymentMethodGateway(GatewayEnum paymentMethodGateway)
            {
                MParams.AddOpt("payment_method[gateway]", paymentMethodGateway);
                return this;
            }

            public UpdatePaymentMethodRequest PaymentMethodGatewayAccountId(string paymentMethodGatewayAccountId)
            {
                MParams.AddOpt("payment_method[gateway_account_id]", paymentMethodGatewayAccountId);
                return this;
            }

            public UpdatePaymentMethodRequest PaymentMethodReferenceId(string paymentMethodReferenceId)
            {
                MParams.AddOpt("payment_method[reference_id]", paymentMethodReferenceId);
                return this;
            }

            public UpdatePaymentMethodRequest PaymentMethodTmpToken(string paymentMethodTmpToken)
            {
                MParams.AddOpt("payment_method[tmp_token]", paymentMethodTmpToken);
                return this;
            }

            public UpdatePaymentMethodRequest PaymentMethodIssuingCountry(string paymentMethodIssuingCountry)
            {
                MParams.AddOpt("payment_method[issuing_country]", paymentMethodIssuingCountry);
                return this;
            }
        }

        public class UpdateBillingInfoRequest : EntityRequest<UpdateBillingInfoRequest>
        {
            public UpdateBillingInfoRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateBillingInfoRequest VatNumber(string vatNumber)
            {
                MParams.AddOpt("vat_number", vatNumber);
                return this;
            }

            public UpdateBillingInfoRequest RegisteredForGst(bool registeredForGst)
            {
                MParams.AddOpt("registered_for_gst", registeredForGst);
                return this;
            }

            public UpdateBillingInfoRequest BusinessCustomerWithoutVatNumber(bool businessCustomerWithoutVatNumber)
            {
                MParams.AddOpt("business_customer_without_vat_number", businessCustomerWithoutVatNumber);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public UpdateBillingInfoRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }
        }

        public class AssignPaymentRoleRequest : EntityRequest<AssignPaymentRoleRequest>
        {
            public AssignPaymentRoleRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AssignPaymentRoleRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.Add("payment_source_id", paymentSourceId);
                return this;
            }

            public AssignPaymentRoleRequest Role(RoleEnum role)
            {
                MParams.Add("role", role);
                return this;
            }
        }

        public class AddContactRequest : EntityRequest<AddContactRequest>
        {
            public AddContactRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AddContactRequest ContactId(string contactId)
            {
                MParams.AddOpt("contact[id]", contactId);
                return this;
            }

            public AddContactRequest ContactFirstName(string contactFirstName)
            {
                MParams.AddOpt("contact[first_name]", contactFirstName);
                return this;
            }

            public AddContactRequest ContactLastName(string contactLastName)
            {
                MParams.AddOpt("contact[last_name]", contactLastName);
                return this;
            }

            public AddContactRequest ContactEmail(string contactEmail)
            {
                MParams.Add("contact[email]", contactEmail);
                return this;
            }

            public AddContactRequest ContactPhone(string contactPhone)
            {
                MParams.AddOpt("contact[phone]", contactPhone);
                return this;
            }

            public AddContactRequest ContactLabel(string contactLabel)
            {
                MParams.AddOpt("contact[label]", contactLabel);
                return this;
            }

            public AddContactRequest ContactEnabled(bool contactEnabled)
            {
                MParams.AddOpt("contact[enabled]", contactEnabled);
                return this;
            }

            public AddContactRequest ContactSendBillingEmail(bool contactSendBillingEmail)
            {
                MParams.AddOpt("contact[send_billing_email]", contactSendBillingEmail);
                return this;
            }

            public AddContactRequest ContactSendAccountEmail(bool contactSendAccountEmail)
            {
                MParams.AddOpt("contact[send_account_email]", contactSendAccountEmail);
                return this;
            }
        }

        public class UpdateContactRequest : EntityRequest<UpdateContactRequest>
        {
            public UpdateContactRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateContactRequest ContactId(string contactId)
            {
                MParams.Add("contact[id]", contactId);
                return this;
            }

            public UpdateContactRequest ContactFirstName(string contactFirstName)
            {
                MParams.AddOpt("contact[first_name]", contactFirstName);
                return this;
            }

            public UpdateContactRequest ContactLastName(string contactLastName)
            {
                MParams.AddOpt("contact[last_name]", contactLastName);
                return this;
            }

            public UpdateContactRequest ContactEmail(string contactEmail)
            {
                MParams.AddOpt("contact[email]", contactEmail);
                return this;
            }

            public UpdateContactRequest ContactPhone(string contactPhone)
            {
                MParams.AddOpt("contact[phone]", contactPhone);
                return this;
            }

            public UpdateContactRequest ContactLabel(string contactLabel)
            {
                MParams.AddOpt("contact[label]", contactLabel);
                return this;
            }

            public UpdateContactRequest ContactEnabled(bool contactEnabled)
            {
                MParams.AddOpt("contact[enabled]", contactEnabled);
                return this;
            }

            public UpdateContactRequest ContactSendBillingEmail(bool contactSendBillingEmail)
            {
                MParams.AddOpt("contact[send_billing_email]", contactSendBillingEmail);
                return this;
            }

            public UpdateContactRequest ContactSendAccountEmail(bool contactSendAccountEmail)
            {
                MParams.AddOpt("contact[send_account_email]", contactSendAccountEmail);
                return this;
            }
        }

        public class DeleteContactRequest : EntityRequest<DeleteContactRequest>
        {
            public DeleteContactRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public DeleteContactRequest ContactId(string contactId)
            {
                MParams.Add("contact[id]", contactId);
                return this;
            }
        }

        public class AddPromotionalCreditsRequest : EntityRequest<AddPromotionalCreditsRequest>
        {
            public AddPromotionalCreditsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AddPromotionalCreditsRequest Amount(int amount)
            {
                MParams.Add("amount", amount);
                return this;
            }

            public AddPromotionalCreditsRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public AddPromotionalCreditsRequest Description(string description)
            {
                MParams.Add("description", description);
                return this;
            }

            public AddPromotionalCreditsRequest CreditType(CreditTypeEnum creditType)
            {
                MParams.AddOpt("credit_type", creditType);
                return this;
            }

            public AddPromotionalCreditsRequest Reference(string reference)
            {
                MParams.AddOpt("reference", reference);
                return this;
            }
        }

        public class DeductPromotionalCreditsRequest : EntityRequest<DeductPromotionalCreditsRequest>
        {
            public DeductPromotionalCreditsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public DeductPromotionalCreditsRequest Amount(int amount)
            {
                MParams.Add("amount", amount);
                return this;
            }

            public DeductPromotionalCreditsRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public DeductPromotionalCreditsRequest Description(string description)
            {
                MParams.Add("description", description);
                return this;
            }

            public DeductPromotionalCreditsRequest CreditType(CreditTypeEnum creditType)
            {
                MParams.AddOpt("credit_type", creditType);
                return this;
            }

            public DeductPromotionalCreditsRequest Reference(string reference)
            {
                MParams.AddOpt("reference", reference);
                return this;
            }
        }

        public class SetPromotionalCreditsRequest : EntityRequest<SetPromotionalCreditsRequest>
        {
            public SetPromotionalCreditsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public SetPromotionalCreditsRequest Amount(int amount)
            {
                MParams.Add("amount", amount);
                return this;
            }

            public SetPromotionalCreditsRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public SetPromotionalCreditsRequest Description(string description)
            {
                MParams.Add("description", description);
                return this;
            }

            public SetPromotionalCreditsRequest CreditType(CreditTypeEnum creditType)
            {
                MParams.AddOpt("credit_type", creditType);
                return this;
            }

            public SetPromotionalCreditsRequest Reference(string reference)
            {
                MParams.AddOpt("reference", reference);
                return this;
            }
        }

        public class RecordExcessPaymentRequest : EntityRequest<RecordExcessPaymentRequest>
        {
            public RecordExcessPaymentRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RecordExcessPaymentRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public RecordExcessPaymentRequest TransactionAmount(int transactionAmount)
            {
                MParams.Add("transaction[amount]", transactionAmount);
                return this;
            }

            public RecordExcessPaymentRequest TransactionCurrencyCode(string transactionCurrencyCode)
            {
                MParams.AddOpt("transaction[currency_code]", transactionCurrencyCode);
                return this;
            }

            public RecordExcessPaymentRequest TransactionDate(long transactionDate)
            {
                MParams.Add("transaction[date]", transactionDate);
                return this;
            }

            public RecordExcessPaymentRequest TransactionPaymentMethod(PaymentMethodEnum transactionPaymentMethod)
            {
                MParams.Add("transaction[payment_method]", transactionPaymentMethod);
                return this;
            }

            public RecordExcessPaymentRequest TransactionReferenceNumber(string transactionReferenceNumber)
            {
                MParams.AddOpt("transaction[reference_number]", transactionReferenceNumber);
                return this;
            }
        }

        public class CollectPaymentRequest : EntityRequest<CollectPaymentRequest>
        {
            public CollectPaymentRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CollectPaymentRequest Amount(int amount)
            {
                MParams.AddOpt("amount", amount);
                return this;
            }

            public CollectPaymentRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public CollectPaymentRequest TokenId(string tokenId)
            {
                MParams.AddOpt("token_id", tokenId);
                return this;
            }

            public CollectPaymentRequest ReplacePrimaryPaymentSource(bool replacePrimaryPaymentSource)
            {
                MParams.AddOpt("replace_primary_payment_source", replacePrimaryPaymentSource);
                return this;
            }

            public CollectPaymentRequest RetainPaymentSource(bool retainPaymentSource)
            {
                MParams.AddOpt("retain_payment_source", retainPaymentSource);
                return this;
            }

            public CollectPaymentRequest PaymentMethodType(TypeEnum paymentMethodType)
            {
                MParams.AddOpt("payment_method[type]", paymentMethodType);
                return this;
            }

            public CollectPaymentRequest PaymentMethodGatewayAccountId(string paymentMethodGatewayAccountId)
            {
                MParams.AddOpt("payment_method[gateway_account_id]", paymentMethodGatewayAccountId);
                return this;
            }

            public CollectPaymentRequest PaymentMethodReferenceId(string paymentMethodReferenceId)
            {
                MParams.AddOpt("payment_method[reference_id]", paymentMethodReferenceId);
                return this;
            }

            public CollectPaymentRequest PaymentMethodTmpToken(string paymentMethodTmpToken)
            {
                MParams.AddOpt("payment_method[tmp_token]", paymentMethodTmpToken);
                return this;
            }

            public CollectPaymentRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            public CollectPaymentRequest CardFirstName(string cardFirstName)
            {
                MParams.AddOpt("card[first_name]", cardFirstName);
                return this;
            }

            public CollectPaymentRequest CardLastName(string cardLastName)
            {
                MParams.AddOpt("card[last_name]", cardLastName);
                return this;
            }

            public CollectPaymentRequest CardNumber(string cardNumber)
            {
                MParams.AddOpt("card[number]", cardNumber);
                return this;
            }

            public CollectPaymentRequest CardExpiryMonth(int cardExpiryMonth)
            {
                MParams.AddOpt("card[expiry_month]", cardExpiryMonth);
                return this;
            }

            public CollectPaymentRequest CardExpiryYear(int cardExpiryYear)
            {
                MParams.AddOpt("card[expiry_year]", cardExpiryYear);
                return this;
            }

            public CollectPaymentRequest CardCvv(string cardCvv)
            {
                MParams.AddOpt("card[cvv]", cardCvv);
                return this;
            }

            public CollectPaymentRequest CardBillingAddr1(string cardBillingAddr1)
            {
                MParams.AddOpt("card[billing_addr1]", cardBillingAddr1);
                return this;
            }

            public CollectPaymentRequest CardBillingAddr2(string cardBillingAddr2)
            {
                MParams.AddOpt("card[billing_addr2]", cardBillingAddr2);
                return this;
            }

            public CollectPaymentRequest CardBillingCity(string cardBillingCity)
            {
                MParams.AddOpt("card[billing_city]", cardBillingCity);
                return this;
            }

            public CollectPaymentRequest CardBillingStateCode(string cardBillingStateCode)
            {
                MParams.AddOpt("card[billing_state_code]", cardBillingStateCode);
                return this;
            }

            public CollectPaymentRequest CardBillingState(string cardBillingState)
            {
                MParams.AddOpt("card[billing_state]", cardBillingState);
                return this;
            }

            public CollectPaymentRequest CardBillingZip(string cardBillingZip)
            {
                MParams.AddOpt("card[billing_zip]", cardBillingZip);
                return this;
            }

            public CollectPaymentRequest CardBillingCountry(string cardBillingCountry)
            {
                MParams.AddOpt("card[billing_country]", cardBillingCountry);
                return this;
            }

            public CollectPaymentRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public CollectPaymentRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public CollectPaymentRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            [Obsolete]
            public CollectPaymentRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public CollectPaymentRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            public CollectPaymentRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }

            public CollectPaymentRequest InvoiceAllocationInvoiceId(int index, string invoiceAllocationInvoiceId)
            {
                MParams.Add("invoice_allocations[invoice_id][" + index + "]", invoiceAllocationInvoiceId);
                return this;
            }

            public CollectPaymentRequest InvoiceAllocationAllocationAmount(int index,
                int invoiceAllocationAllocationAmount)
            {
                MParams.AddOpt("invoice_allocations[allocation_amount][" + index + "]",
                    invoiceAllocationAllocationAmount);
                return this;
            }
        }

        public class DeleteRequest : EntityRequest<DeleteRequest>
        {
            public DeleteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public DeleteRequest DeletePaymentMethod(bool deletePaymentMethod)
            {
                MParams.AddOpt("delete_payment_method", deletePaymentMethod);
                return this;
            }
        }

        public class MoveRequest : EntityRequest<MoveRequest>
        {
            public MoveRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public MoveRequest IdAtFromSite(string idAtFromSite)
            {
                MParams.Add("id_at_from_site", idAtFromSite);
                return this;
            }

            public MoveRequest FromSite(string fromSite)
            {
                MParams.Add("from_site", fromSite);
                return this;
            }
        }

        public class ChangeBillingDateRequest : EntityRequest<ChangeBillingDateRequest>
        {
            public ChangeBillingDateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ChangeBillingDateRequest BillingDate(int billingDate)
            {
                MParams.AddOpt("billing_date", billingDate);
                return this;
            }

            public ChangeBillingDateRequest BillingDateMode(BillingDateModeEnum billingDateMode)
            {
                MParams.AddOpt("billing_date_mode", billingDateMode);
                return this;
            }

            public ChangeBillingDateRequest BillingDayOfWeek(BillingDayOfWeekEnum billingDayOfWeek)
            {
                MParams.AddOpt("billing_day_of_week", billingDayOfWeek);
                return this;
            }

            public ChangeBillingDateRequest BillingDayOfWeekMode(BillingDayOfWeekModeEnum billingDayOfWeekMode)
            {
                MParams.AddOpt("billing_day_of_week_mode", billingDayOfWeekMode);
                return this;
            }
        }

        public class MergeRequest : EntityRequest<MergeRequest>
        {
            public MergeRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public MergeRequest FromCustomerId(string fromCustomerId)
            {
                MParams.Add("from_customer_id", fromCustomerId);
                return this;
            }

            public MergeRequest ToCustomerId(string toCustomerId)
            {
                MParams.Add("to_customer_id", toCustomerId);
                return this;
            }
        }

        public class RelationshipsRequest : EntityRequest<RelationshipsRequest>
        {
            public RelationshipsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RelationshipsRequest ParentId(string parentId)
            {
                MParams.AddOpt("parent_id", parentId);
                return this;
            }

            public RelationshipsRequest PaymentOwnerId(string paymentOwnerId)
            {
                MParams.AddOpt("payment_owner_id", paymentOwnerId);
                return this;
            }

            public RelationshipsRequest InvoiceOwnerId(string invoiceOwnerId)
            {
                MParams.AddOpt("invoice_owner_id", invoiceOwnerId);
                return this;
            }

            public RelationshipsRequest UseDefaultHierarchySettings(bool useDefaultHierarchySettings)
            {
                MParams.AddOpt("use_default_hierarchy_settings", useDefaultHierarchySettings);
                return this;
            }

            public RelationshipsRequest ParentAccountAccessPortalEditChildSubscriptions(
                CustomerParentAccountAccess.PortalEditChildSubscriptionsEnum
                    parentAccountAccessPortalEditChildSubscriptions)
            {
                MParams.AddOpt("parent_account_access[portal_edit_child_subscriptions]",
                    parentAccountAccessPortalEditChildSubscriptions);
                return this;
            }

            public RelationshipsRequest ParentAccountAccessPortalDownloadChildInvoices(
                CustomerParentAccountAccess.PortalDownloadChildInvoicesEnum
                    parentAccountAccessPortalDownloadChildInvoices)
            {
                MParams.AddOpt("parent_account_access[portal_download_child_invoices]",
                    parentAccountAccessPortalDownloadChildInvoices);
                return this;
            }

            public RelationshipsRequest ParentAccountAccessSendSubscriptionEmails(
                bool parentAccountAccessSendSubscriptionEmails)
            {
                MParams.AddOpt("parent_account_access[send_subscription_emails]",
                    parentAccountAccessSendSubscriptionEmails);
                return this;
            }

            public RelationshipsRequest ParentAccountAccessSendPaymentEmails(bool parentAccountAccessSendPaymentEmails)
            {
                MParams.AddOpt("parent_account_access[send_payment_emails]", parentAccountAccessSendPaymentEmails);
                return this;
            }

            public RelationshipsRequest ParentAccountAccessSendInvoiceEmails(bool parentAccountAccessSendInvoiceEmails)
            {
                MParams.AddOpt("parent_account_access[send_invoice_emails]", parentAccountAccessSendInvoiceEmails);
                return this;
            }

            public RelationshipsRequest ChildAccountAccessPortalEditSubscriptions(
                CustomerChildAccountAccess.PortalEditSubscriptionsEnum childAccountAccessPortalEditSubscriptions)
            {
                MParams.AddOpt("child_account_access[portal_edit_subscriptions]",
                    childAccountAccessPortalEditSubscriptions);
                return this;
            }

            public RelationshipsRequest ChildAccountAccessPortalDownloadInvoices(
                CustomerChildAccountAccess.PortalDownloadInvoicesEnum childAccountAccessPortalDownloadInvoices)
            {
                MParams.AddOpt("child_account_access[portal_download_invoices]",
                    childAccountAccessPortalDownloadInvoices);
                return this;
            }

            public RelationshipsRequest ChildAccountAccessSendSubscriptionEmails(
                bool childAccountAccessSendSubscriptionEmails)
            {
                MParams.AddOpt("child_account_access[send_subscription_emails]",
                    childAccountAccessSendSubscriptionEmails);
                return this;
            }

            public RelationshipsRequest ChildAccountAccessSendPaymentEmails(bool childAccountAccessSendPaymentEmails)
            {
                MParams.AddOpt("child_account_access[send_payment_emails]", childAccountAccessSendPaymentEmails);
                return this;
            }

            public RelationshipsRequest ChildAccountAccessSendInvoiceEmails(bool childAccountAccessSendInvoiceEmails)
            {
                MParams.AddOpt("child_account_access[send_invoice_emails]", childAccountAccessSendInvoiceEmails);
                return this;
            }
        }

        public class HierarchyRequest : EntityRequest<HierarchyRequest>
        {
            public HierarchyRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public HierarchyRequest HierarchyOperationType(HierarchyOperationTypeEnum hierarchyOperationType)
            {
                MParams.AddOpt("hierarchy_operation_type", hierarchyOperationType);
                return this;
            }
        }

        public class UpdateHierarchySettingsRequest : EntityRequest<UpdateHierarchySettingsRequest>
        {
            public UpdateHierarchySettingsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateHierarchySettingsRequest UseDefaultHierarchySettings(bool useDefaultHierarchySettings)
            {
                MParams.AddOpt("use_default_hierarchy_settings", useDefaultHierarchySettings);
                return this;
            }

            public UpdateHierarchySettingsRequest ParentAccountAccessPortalEditChildSubscriptions(
                CustomerParentAccountAccess.PortalEditChildSubscriptionsEnum
                    parentAccountAccessPortalEditChildSubscriptions)
            {
                MParams.AddOpt("parent_account_access[portal_edit_child_subscriptions]",
                    parentAccountAccessPortalEditChildSubscriptions);
                return this;
            }

            public UpdateHierarchySettingsRequest ParentAccountAccessPortalDownloadChildInvoices(
                CustomerParentAccountAccess.PortalDownloadChildInvoicesEnum
                    parentAccountAccessPortalDownloadChildInvoices)
            {
                MParams.AddOpt("parent_account_access[portal_download_child_invoices]",
                    parentAccountAccessPortalDownloadChildInvoices);
                return this;
            }

            public UpdateHierarchySettingsRequest ParentAccountAccessSendSubscriptionEmails(
                bool parentAccountAccessSendSubscriptionEmails)
            {
                MParams.AddOpt("parent_account_access[send_subscription_emails]",
                    parentAccountAccessSendSubscriptionEmails);
                return this;
            }

            public UpdateHierarchySettingsRequest ParentAccountAccessSendPaymentEmails(
                bool parentAccountAccessSendPaymentEmails)
            {
                MParams.AddOpt("parent_account_access[send_payment_emails]", parentAccountAccessSendPaymentEmails);
                return this;
            }

            public UpdateHierarchySettingsRequest ParentAccountAccessSendInvoiceEmails(
                bool parentAccountAccessSendInvoiceEmails)
            {
                MParams.AddOpt("parent_account_access[send_invoice_emails]", parentAccountAccessSendInvoiceEmails);
                return this;
            }

            public UpdateHierarchySettingsRequest ChildAccountAccessPortalEditSubscriptions(
                CustomerChildAccountAccess.PortalEditSubscriptionsEnum childAccountAccessPortalEditSubscriptions)
            {
                MParams.AddOpt("child_account_access[portal_edit_subscriptions]",
                    childAccountAccessPortalEditSubscriptions);
                return this;
            }

            public UpdateHierarchySettingsRequest ChildAccountAccessPortalDownloadInvoices(
                CustomerChildAccountAccess.PortalDownloadInvoicesEnum childAccountAccessPortalDownloadInvoices)
            {
                MParams.AddOpt("child_account_access[portal_download_invoices]",
                    childAccountAccessPortalDownloadInvoices);
                return this;
            }

            public UpdateHierarchySettingsRequest ChildAccountAccessSendSubscriptionEmails(
                bool childAccountAccessSendSubscriptionEmails)
            {
                MParams.AddOpt("child_account_access[send_subscription_emails]",
                    childAccountAccessSendSubscriptionEmails);
                return this;
            }

            public UpdateHierarchySettingsRequest ChildAccountAccessSendPaymentEmails(
                bool childAccountAccessSendPaymentEmails)
            {
                MParams.AddOpt("child_account_access[send_payment_emails]", childAccountAccessSendPaymentEmails);
                return this;
            }

            public UpdateHierarchySettingsRequest ChildAccountAccessSendInvoiceEmails(
                bool childAccountAccessSendInvoiceEmails)
            {
                MParams.AddOpt("child_account_access[send_invoice_emails]", childAccountAccessSendInvoiceEmails);
                return this;
            }
        }

        #endregion

        #region Subclasses

        public class CustomerBillingAddress : Resource
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

        public class CustomerReferralUrl : Resource
        {
            public string ExternalCustomerId()
            {
                return GetValue<string>("external_customer_id", false);
            }

            public string ReferralSharingUrl()
            {
                return GetValue<string>("referral_sharing_url");
            }

            public DateTime CreatedAt()
            {
                return (DateTime) GetDateTime("created_at");
            }

            public DateTime UpdatedAt()
            {
                return (DateTime) GetDateTime("updated_at");
            }

            public string ReferralCampaignId()
            {
                return GetValue<string>("referral_campaign_id");
            }

            public string ReferralAccountId()
            {
                return GetValue<string>("referral_account_id");
            }

            public string ReferralExternalCampaignId()
            {
                return GetValue<string>("referral_external_campaign_id", false);
            }

            public ReferralSystemEnum ReferralSystem()
            {
                return GetEnum<ReferralSystemEnum>("referral_system");
            }
        }

        public class CustomerContact : Resource
        {
            public string Id()
            {
                return GetValue<string>("id");
            }

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
                return GetValue<string>("email");
            }

            public string Phone()
            {
                return GetValue<string>("phone", false);
            }

            public string Label()
            {
                return GetValue<string>("label", false);
            }

            public bool Enabled()
            {
                return GetValue<bool>("enabled");
            }

            public bool SendAccountEmail()
            {
                return GetValue<bool>("send_account_email");
            }

            public bool SendBillingEmail()
            {
                return GetValue<bool>("send_billing_email");
            }
        }

        public class CustomerPaymentMethod : Resource
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

            public enum TypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "card")] Card,

                [EnumMember(Value = "paypal_express_checkout")]
                PaypalExpressCheckout,

                [EnumMember(Value = "amazon_payments")]
                AmazonPayments,
                [EnumMember(Value = "direct_debit")] DirectDebit,
                [EnumMember(Value = "generic")] Generic,
                [EnumMember(Value = "alipay")] Alipay,
                [EnumMember(Value = "unionpay")] Unionpay,
                [EnumMember(Value = "apple_pay")] ApplePay,
                [EnumMember(Value = "wechat_pay")] WechatPay,
                [EnumMember(Value = "ideal")] Ideal,
                [EnumMember(Value = "google_pay")] GooglePay,
                [EnumMember(Value = "sofort")] Sofort,
                [EnumMember(Value = "bancontact")] Bancontact,
                [EnumMember(Value = "giropay")] Giropay,
                [EnumMember(Value = "dotpay")] Dotpay
            }

            public TypeEnum PaymentMethodType()
            {
                return GetEnum<TypeEnum>("type");
            }

            public GatewayEnum Gateway()
            {
                return GetEnum<GatewayEnum>("gateway");
            }

            public string GatewayAccountId()
            {
                return GetValue<string>("gateway_account_id", false);
            }

            public StatusEnum Status()
            {
                return GetEnum<StatusEnum>("status");
            }

            public string ReferenceId()
            {
                return GetValue<string>("reference_id");
            }
        }

        public class CustomerBalance : Resource
        {
            public int PromotionalCredits()
            {
                return GetValue<int>("promotional_credits");
            }

            public int ExcessPayments()
            {
                return GetValue<int>("excess_payments");
            }

            public int RefundableCredits()
            {
                return GetValue<int>("refundable_credits");
            }

            public int UnbilledCharges()
            {
                return GetValue<int>("unbilled_charges");
            }

            public string CurrencyCode()
            {
                return GetValue<string>("currency_code");
            }

            [Obsolete]
            public string BalanceCurrencyCode()
            {
                return GetValue<string>("balance_currency_code");
            }
        }

        public class CustomerRelationship : Resource
        {
            public string ParentId()
            {
                return GetValue<string>("parent_id", false);
            }

            public string PaymentOwnerId()
            {
                return GetValue<string>("payment_owner_id");
            }

            public string InvoiceOwnerId()
            {
                return GetValue<string>("invoice_owner_id");
            }
        }

        public class CustomerParentAccountAccess : Resource
        {
            public enum PortalDownloadChildInvoicesEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "yes")] Yes,
                [EnumMember(Value = "view_only")] ViewOnly,
                [EnumMember(Value = "no")] No
            }

            public enum PortalEditChildSubscriptionsEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "yes")] Yes,
                [EnumMember(Value = "view_only")] ViewOnly,
                [EnumMember(Value = "no")] No
            }

            public PortalEditChildSubscriptionsEnum? PortalEditChildSubscriptions()
            {
                return GetEnum<PortalEditChildSubscriptionsEnum>("portal_edit_child_subscriptions", false);
            }

            public PortalDownloadChildInvoicesEnum? PortalDownloadChildInvoices()
            {
                return GetEnum<PortalDownloadChildInvoicesEnum>("portal_download_child_invoices", false);
            }

            public bool SendSubscriptionEmails()
            {
                return GetValue<bool>("send_subscription_emails");
            }

            public bool SendInvoiceEmails()
            {
                return GetValue<bool>("send_invoice_emails");
            }

            public bool SendPaymentEmails()
            {
                return GetValue<bool>("send_payment_emails");
            }
        }

        public class CustomerChildAccountAccess : Resource
        {
            public enum PortalDownloadInvoicesEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "yes")] Yes,
                [EnumMember(Value = "view_only")] ViewOnly,
                [EnumMember(Value = "no")] No
            }

            public enum PortalEditSubscriptionsEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "yes")] Yes,
                [EnumMember(Value = "view_only")] ViewOnly
            }

            public PortalEditSubscriptionsEnum? PortalEditSubscriptions()
            {
                return GetEnum<PortalEditSubscriptionsEnum>("portal_edit_subscriptions", false);
            }

            public PortalDownloadInvoicesEnum? PortalDownloadInvoices()
            {
                return GetEnum<PortalDownloadInvoicesEnum>("portal_download_invoices", false);
            }

            public bool SendSubscriptionEmails()
            {
                return GetValue<bool>("send_subscription_emails");
            }

            public bool SendInvoiceEmails()
            {
                return GetValue<bool>("send_invoice_emails");
            }

            public bool SendPaymentEmails()
            {
                return GetValue<bool>("send_payment_emails");
            }
        }

        #endregion
    }
}