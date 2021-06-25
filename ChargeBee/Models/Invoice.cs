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
    public class Invoice : Resource
    {
        public enum DunningStatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "in_progress")] InProgress,
            [EnumMember(Value = "exhausted")] Exhausted,
            [EnumMember(Value = "stopped")] Stopped,
            [EnumMember(Value = "success")] Success
        }

        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "paid")] Paid,
            [EnumMember(Value = "posted")] Posted,
            [EnumMember(Value = "payment_due")] PaymentDue,
            [EnumMember(Value = "not_paid")] NotPaid,
            [EnumMember(Value = "voided")] Voided,
            [EnumMember(Value = "pending")] Pending
        }

        public Invoice()
        {
        }

        public Invoice(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Invoice(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Invoice(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("invoices");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static CreateForChargeItemsAndChargesRequest CreateForChargeItemsAndCharges()
        {
            var url = ApiUtil.BuildUrl("invoices", "create_for_charge_items_and_charges");
            return new CreateForChargeItemsAndChargesRequest(url, HttpMethod.Post);
        }

        public static ChargeRequest Charge()
        {
            var url = ApiUtil.BuildUrl("invoices", "charge");
            return new ChargeRequest(url, HttpMethod.Post);
        }

        public static ChargeAddonRequest ChargeAddon()
        {
            var url = ApiUtil.BuildUrl("invoices", "charge_addon");
            return new ChargeAddonRequest(url, HttpMethod.Post);
        }

        public static CreateForChargeItemRequest CreateForChargeItem()
        {
            var url = ApiUtil.BuildUrl("invoices", "create_for_charge_item");
            return new CreateForChargeItemRequest(url, HttpMethod.Post);
        }

        public static StopDunningRequest StopDunning(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "stop_dunning");
            return new StopDunningRequest(url, HttpMethod.Post);
        }

        public static ImportInvoiceRequest ImportInvoice()
        {
            var url = ApiUtil.BuildUrl("invoices", "import_invoice");
            return new ImportInvoiceRequest(url, HttpMethod.Post);
        }

        public static ApplyPaymentsRequest ApplyPayments(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "apply_payments");
            return new ApplyPaymentsRequest(url, HttpMethod.Post);
        }

        public static ApplyCreditsRequest ApplyCredits(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "apply_credits");
            return new ApplyCreditsRequest(url, HttpMethod.Post);
        }

        public static InvoiceListRequest List()
        {
            var url = ApiUtil.BuildUrl("invoices");
            return new InvoiceListRequest(url);
        }

        [Obsolete]
        public static ListRequest InvoicesForCustomer(string id)
        {
            var url = ApiUtil.BuildUrl("customers", CheckNull(id), "invoices");
            return new ListRequest(url);
        }

        [Obsolete]
        public static ListRequest InvoicesForSubscription(string id)
        {
            var url = ApiUtil.BuildUrl("subscriptions", CheckNull(id), "invoices");
            return new ListRequest(url);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static PdfRequest Pdf(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "pdf");
            return new PdfRequest(url, HttpMethod.Post);
        }

        public static AddChargeRequest AddCharge(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "add_charge");
            return new AddChargeRequest(url, HttpMethod.Post);
        }

        public static AddAddonChargeRequest AddAddonCharge(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "add_addon_charge");
            return new AddAddonChargeRequest(url, HttpMethod.Post);
        }

        public static AddChargeItemRequest AddChargeItem(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "add_charge_item");
            return new AddChargeItemRequest(url, HttpMethod.Post);
        }

        public static CloseRequest Close(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "close");
            return new CloseRequest(url, HttpMethod.Post);
        }

        public static CollectPaymentRequest CollectPayment(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "collect_payment");
            return new CollectPaymentRequest(url, HttpMethod.Post);
        }

        public static RecordPaymentRequest RecordPayment(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "record_payment");
            return new RecordPaymentRequest(url, HttpMethod.Post);
        }

        public static RefundRequest Refund(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "refund");
            return new RefundRequest(url, HttpMethod.Post);
        }

        public static RecordRefundRequest RecordRefund(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "record_refund");
            return new RecordRefundRequest(url, HttpMethod.Post);
        }

        public static RemovePaymentRequest RemovePayment(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "remove_payment");
            return new RemovePaymentRequest(url, HttpMethod.Post);
        }

        public static RemoveCreditNoteRequest RemoveCreditNote(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "remove_credit_note");
            return new RemoveCreditNoteRequest(url, HttpMethod.Post);
        }

        public static VoidInvoiceRequest VoidInvoice(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "void");
            return new VoidInvoiceRequest(url, HttpMethod.Post);
        }

        public static WriteOffRequest WriteOff(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "write_off");
            return new WriteOffRequest(url, HttpMethod.Post);
        }

        public static DeleteRequest Delete(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "delete");
            return new DeleteRequest(url, HttpMethod.Post);
        }

        public static UpdateDetailsRequest UpdateDetails(string id)
        {
            var url = ApiUtil.BuildUrl("invoices", CheckNull(id), "update_details");
            return new UpdateDetailsRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string PoNumber => GetValue<string>("po_number", false);

        public string CustomerId => GetValue<string>("customer_id");

        public string SubscriptionId => GetValue<string>("subscription_id", false);

        public bool Recurring => GetValue<bool>("recurring");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public string VatNumber => GetValue<string>("vat_number", false);

        public PriceTypeEnum PriceType => GetEnum<PriceTypeEnum>("price_type");

        public DateTime? Date => GetDateTime("date", false);

        public DateTime? DueDate => GetDateTime("due_date", false);

        public int? NetTermDays => GetValue<int?>("net_term_days", false);

        public string CurrencyCode => GetValue<string>("currency_code");

        public int? Total => GetValue<int?>("total", false);

        public int? AmountPaid => GetValue<int?>("amount_paid", false);

        public int? AmountAdjusted => GetValue<int?>("amount_adjusted", false);

        public int? WriteOffAmount => GetValue<int?>("write_off_amount", false);

        public int? CreditsApplied => GetValue<int?>("credits_applied", false);

        public int? AmountDue => GetValue<int?>("amount_due", false);

        public DateTime? PaidAt => GetDateTime("paid_at", false);

        public DunningStatusEnum? DunningStatus => GetEnum<DunningStatusEnum>("dunning_status", false);

        public DateTime? NextRetryAt => GetDateTime("next_retry_at", false);

        public DateTime? VoidedAt => GetDateTime("voided_at", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public int SubTotal => GetValue<int>("sub_total");

        public int? SubTotalInLocalCurrency => GetValue<int?>("sub_total_in_local_currency", false);

        public int? TotalInLocalCurrency => GetValue<int?>("total_in_local_currency", false);

        public string LocalCurrencyCode => GetValue<string>("local_currency_code", false);

        public int Tax => GetValue<int>("tax");

        public bool? FirstInvoice => GetValue<bool?>("first_invoice", false);

        public bool? HasAdvanceCharges => GetValue<bool?>("has_advance_charges", false);

        public bool TermFinalized => GetValue<bool>("term_finalized");

        public bool IsGifted => GetValue<bool>("is_gifted");

        public DateTime? ExpectedPaymentDate => GetDateTime("expected_payment_date", false);

        public int? AmountToCollect => GetValue<int?>("amount_to_collect", false);

        public int? RoundOffAmount => GetValue<int?>("round_off_amount", false);

        public List<InvoiceLineItem> LineItems => GetResourceList<InvoiceLineItem>("line_items");

        public List<InvoiceDiscount> Discounts => GetResourceList<InvoiceDiscount>("discounts");

        public List<InvoiceLineItemDiscount> LineItemDiscounts =>
            GetResourceList<InvoiceLineItemDiscount>("line_item_discounts");

        public List<InvoiceTax> Taxes => GetResourceList<InvoiceTax>("taxes");

        public List<InvoiceLineItemTax> LineItemTaxes => GetResourceList<InvoiceLineItemTax>("line_item_taxes");

        public List<InvoiceLineItemTier> LineItemTiers => GetResourceList<InvoiceLineItemTier>("line_item_tiers");

        public List<InvoiceLinkedPayment> LinkedPayments => GetResourceList<InvoiceLinkedPayment>("linked_payments");

        public List<InvoiceDunningAttempt> DunningAttempts =>
            GetResourceList<InvoiceDunningAttempt>("dunning_attempts");

        public List<InvoiceAppliedCredit> AppliedCredits => GetResourceList<InvoiceAppliedCredit>("applied_credits");

        public List<InvoiceAdjustmentCreditNote> AdjustmentCreditNotes =>
            GetResourceList<InvoiceAdjustmentCreditNote>("adjustment_credit_notes");

        public List<InvoiceIssuedCreditNote> IssuedCreditNotes =>
            GetResourceList<InvoiceIssuedCreditNote>("issued_credit_notes");

        public List<InvoiceLinkedOrder> LinkedOrders => GetResourceList<InvoiceLinkedOrder>("linked_orders");

        public List<InvoiceNote> Notes => GetResourceList<InvoiceNote>("notes");

        public InvoiceShippingAddress ShippingAddress => GetSubResource<InvoiceShippingAddress>("shipping_address");

        public InvoiceBillingAddress BillingAddress => GetSubResource<InvoiceBillingAddress>("billing_address");

        public string PaymentOwner => GetValue<string>("payment_owner", false);

        public string VoidReasonCode => GetValue<string>("void_reason_code", false);

        public bool Deleted => GetValue<bool>("deleted");

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

            public CreateRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public CreateRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateRequest InvoiceNote(string invoiceNote)
            {
                MParams.AddOpt("invoice_note", invoiceNote);
                return this;
            }

            public CreateRequest RemoveGeneralNote(bool removeGeneralNote)
            {
                MParams.AddOpt("remove_general_note", removeGeneralNote);
                return this;
            }

            public CreateRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            [Obsolete]
            public CreateRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public CreateRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateRequest AuthorizationTransactionId(string authorizationTransactionId)
            {
                MParams.AddOpt("authorization_transaction_id", authorizationTransactionId);
                return this;
            }

            public CreateRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public CreateRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public CreateRequest TokenId(string tokenId)
            {
                MParams.AddOpt("token_id", tokenId);
                return this;
            }

            public CreateRequest ReplacePrimaryPaymentSource(bool replacePrimaryPaymentSource)
            {
                MParams.AddOpt("replace_primary_payment_source", replacePrimaryPaymentSource);
                return this;
            }

            public CreateRequest RetainPaymentSource(bool retainPaymentSource)
            {
                MParams.AddOpt("retain_payment_source", retainPaymentSource);
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

            public CreateRequest AddonUnitPrice(int index, int addonUnitPrice)
            {
                MParams.AddOpt("addons[unit_price][" + index + "]", addonUnitPrice);
                return this;
            }

            public CreateRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }

            public CreateRequest AddonUnitPriceInDecimal(int index, string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addons[unit_price_in_decimal][" + index + "]", addonUnitPriceInDecimal);
                return this;
            }

            public CreateRequest AddonDateFrom(int index, long addonDateFrom)
            {
                MParams.AddOpt("addons[date_from][" + index + "]", addonDateFrom);
                return this;
            }

            public CreateRequest AddonDateTo(int index, long addonDateTo)
            {
                MParams.AddOpt("addons[date_to][" + index + "]", addonDateTo);
                return this;
            }

            public CreateRequest ChargeAmount(int index, int chargeAmount)
            {
                MParams.AddOpt("charges[amount][" + index + "]", chargeAmount);
                return this;
            }

            public CreateRequest ChargeAmountInDecimal(int index, string chargeAmountInDecimal)
            {
                MParams.AddOpt("charges[amount_in_decimal][" + index + "]", chargeAmountInDecimal);
                return this;
            }

            public CreateRequest ChargeDescription(int index, string chargeDescription)
            {
                MParams.AddOpt("charges[description][" + index + "]", chargeDescription);
                return this;
            }

            public CreateRequest ChargeAvalaraSaleType(int index, AvalaraSaleTypeEnum chargeAvalaraSaleType)
            {
                MParams.AddOpt("charges[avalara_sale_type][" + index + "]", chargeAvalaraSaleType);
                return this;
            }

            public CreateRequest ChargeAvalaraTransactionType(int index, int chargeAvalaraTransactionType)
            {
                MParams.AddOpt("charges[avalara_transaction_type][" + index + "]", chargeAvalaraTransactionType);
                return this;
            }

            public CreateRequest ChargeAvalaraServiceType(int index, int chargeAvalaraServiceType)
            {
                MParams.AddOpt("charges[avalara_service_type][" + index + "]", chargeAvalaraServiceType);
                return this;
            }

            public CreateRequest ChargeDateFrom(int index, long chargeDateFrom)
            {
                MParams.AddOpt("charges[date_from][" + index + "]", chargeDateFrom);
                return this;
            }

            public CreateRequest ChargeDateTo(int index, long chargeDateTo)
            {
                MParams.AddOpt("charges[date_to][" + index + "]", chargeDateTo);
                return this;
            }

            public CreateRequest ChargeTaxable(int index, bool chargeTaxable)
            {
                MParams.AddOpt("charges[taxable][" + index + "]", chargeTaxable);
                return this;
            }

            public CreateRequest ChargeTaxProfileId(int index, string chargeTaxProfileId)
            {
                MParams.AddOpt("charges[tax_profile_id][" + index + "]", chargeTaxProfileId);
                return this;
            }

            public CreateRequest ChargeAvalaraTaxCode(int index, string chargeAvalaraTaxCode)
            {
                MParams.AddOpt("charges[avalara_tax_code][" + index + "]", chargeAvalaraTaxCode);
                return this;
            }

            public CreateRequest ChargeTaxjarProductCode(int index, string chargeTaxjarProductCode)
            {
                MParams.AddOpt("charges[taxjar_product_code][" + index + "]", chargeTaxjarProductCode);
                return this;
            }

            public CreateRequest NotesToRemoveEntityType(int index, EntityTypeEnum notesToRemoveEntityType)
            {
                MParams.AddOpt("notes_to_remove[entity_type][" + index + "]", notesToRemoveEntityType);
                return this;
            }

            public CreateRequest NotesToRemoveEntityId(int index, string notesToRemoveEntityId)
            {
                MParams.AddOpt("notes_to_remove[entity_id][" + index + "]", notesToRemoveEntityId);
                return this;
            }
        }

        public class CreateForChargeItemsAndChargesRequest : EntityRequest<CreateForChargeItemsAndChargesRequest>
        {
            public CreateForChargeItemsAndChargesRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateForChargeItemsAndChargesRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer_id", customerId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest InvoiceNotes(string invoiceNotes)
            {
                MParams.AddOpt("invoice_notes", invoiceNotes);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            [Obsolete]
            public CreateForChargeItemsAndChargesRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest AuthorizationTransactionId(string authorizationTransactionId)
            {
                MParams.AddOpt("authorization_transaction_id", authorizationTransactionId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest AutoCollection(AutoCollectionEnum autoCollection)
            {
                MParams.AddOpt("auto_collection", autoCollection);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest TokenId(string tokenId)
            {
                MParams.AddOpt("token_id", tokenId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ReplacePrimaryPaymentSource(bool replacePrimaryPaymentSource)
            {
                MParams.AddOpt("replace_primary_payment_source", replacePrimaryPaymentSource);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest RetainPaymentSource(bool retainPaymentSource)
            {
                MParams.AddOpt("retain_payment_source", retainPaymentSource);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            [Obsolete]
            public CreateForChargeItemsAndChargesRequest CardGateway(GatewayEnum cardGateway)
            {
                MParams.AddOpt("card[gateway]", cardGateway);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardGatewayAccountId(string cardGatewayAccountId)
            {
                MParams.AddOpt("card[gateway_account_id]", cardGatewayAccountId);
                return this;
            }

            [Obsolete]
            public CreateForChargeItemsAndChargesRequest CardTmpToken(string cardTmpToken)
            {
                MParams.AddOpt("card[tmp_token]", cardTmpToken);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountGatewayAccountId(string bankAccountGatewayAccountId)
            {
                MParams.AddOpt("bank_account[gateway_account_id]", bankAccountGatewayAccountId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountIban(string bankAccountIban)
            {
                MParams.AddOpt("bank_account[iban]", bankAccountIban);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountFirstName(string bankAccountFirstName)
            {
                MParams.AddOpt("bank_account[first_name]", bankAccountFirstName);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountLastName(string bankAccountLastName)
            {
                MParams.AddOpt("bank_account[last_name]", bankAccountLastName);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountCompany(string bankAccountCompany)
            {
                MParams.AddOpt("bank_account[company]", bankAccountCompany);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountEmail(string bankAccountEmail)
            {
                MParams.AddOpt("bank_account[email]", bankAccountEmail);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountBankName(string bankAccountBankName)
            {
                MParams.AddOpt("bank_account[bank_name]", bankAccountBankName);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountAccountNumber(string bankAccountAccountNumber)
            {
                MParams.AddOpt("bank_account[account_number]", bankAccountAccountNumber);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountRoutingNumber(string bankAccountRoutingNumber)
            {
                MParams.AddOpt("bank_account[routing_number]", bankAccountRoutingNumber);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountBankCode(string bankAccountBankCode)
            {
                MParams.AddOpt("bank_account[bank_code]", bankAccountBankCode);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountAccountType(AccountTypeEnum bankAccountAccountType)
            {
                MParams.AddOpt("bank_account[account_type]", bankAccountAccountType);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountAccountHolderType(
                AccountHolderTypeEnum bankAccountAccountHolderType)
            {
                MParams.AddOpt("bank_account[account_holder_type]", bankAccountAccountHolderType);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountEcheckType(EcheckTypeEnum bankAccountEcheckType)
            {
                MParams.AddOpt("bank_account[echeck_type]", bankAccountEcheckType);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountIssuingCountry(string bankAccountIssuingCountry)
            {
                MParams.AddOpt("bank_account[issuing_country]", bankAccountIssuingCountry);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest BankAccountSwedishIdentityNumber(
                string bankAccountSwedishIdentityNumber)
            {
                MParams.AddOpt("bank_account[swedish_identity_number]", bankAccountSwedishIdentityNumber);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentMethodType(TypeEnum paymentMethodType)
            {
                MParams.AddOpt("payment_method[type]", paymentMethodType);
                return this;
            }

            [Obsolete]
            public CreateForChargeItemsAndChargesRequest PaymentMethodGateway(GatewayEnum paymentMethodGateway)
            {
                MParams.AddOpt("payment_method[gateway]", paymentMethodGateway);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentMethodGatewayAccountId(
                string paymentMethodGatewayAccountId)
            {
                MParams.AddOpt("payment_method[gateway_account_id]", paymentMethodGatewayAccountId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentMethodReferenceId(string paymentMethodReferenceId)
            {
                MParams.AddOpt("payment_method[reference_id]", paymentMethodReferenceId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentMethodTmpToken(string paymentMethodTmpToken)
            {
                MParams.AddOpt("payment_method[tmp_token]", paymentMethodTmpToken);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentMethodIssuingCountry(string paymentMethodIssuingCountry)
            {
                MParams.AddOpt("payment_method[issuing_country]", paymentMethodIssuingCountry);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardFirstName(string cardFirstName)
            {
                MParams.AddOpt("card[first_name]", cardFirstName);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardLastName(string cardLastName)
            {
                MParams.AddOpt("card[last_name]", cardLastName);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardNumber(string cardNumber)
            {
                MParams.AddOpt("card[number]", cardNumber);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardExpiryMonth(int cardExpiryMonth)
            {
                MParams.AddOpt("card[expiry_month]", cardExpiryMonth);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardExpiryYear(int cardExpiryYear)
            {
                MParams.AddOpt("card[expiry_year]", cardExpiryYear);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardCvv(string cardCvv)
            {
                MParams.AddOpt("card[cvv]", cardCvv);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardBillingAddr1(string cardBillingAddr1)
            {
                MParams.AddOpt("card[billing_addr1]", cardBillingAddr1);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardBillingAddr2(string cardBillingAddr2)
            {
                MParams.AddOpt("card[billing_addr2]", cardBillingAddr2);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardBillingCity(string cardBillingCity)
            {
                MParams.AddOpt("card[billing_city]", cardBillingCity);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardBillingStateCode(string cardBillingStateCode)
            {
                MParams.AddOpt("card[billing_state_code]", cardBillingStateCode);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardBillingState(string cardBillingState)
            {
                MParams.AddOpt("card[billing_state]", cardBillingState);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardBillingZip(string cardBillingZip)
            {
                MParams.AddOpt("card[billing_zip]", cardBillingZip);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest CardBillingCountry(string cardBillingCountry)
            {
                MParams.AddOpt("card[billing_country]", cardBillingCountry);
                return this;
            }

            [Obsolete]
            public CreateForChargeItemsAndChargesRequest CardIpAddress(string cardIpAddress)
            {
                MParams.AddOpt("card[ip_address]", cardIpAddress);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentIntentGatewayAccountId(
                string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public CreateForChargeItemsAndChargesRequest PaymentIntentGwPaymentMethodId(
                string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemPriceItemPriceId(int index, string itemPriceItemPriceId)
            {
                MParams.AddOpt("item_prices[item_price_id][" + index + "]", itemPriceItemPriceId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemPriceQuantity(int index, int itemPriceQuantity)
            {
                MParams.AddOpt("item_prices[quantity][" + index + "]", itemPriceQuantity);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemPriceUnitPrice(int index, int itemPriceUnitPrice)
            {
                MParams.AddOpt("item_prices[unit_price][" + index + "]", itemPriceUnitPrice);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemPriceDateFrom(int index, long itemPriceDateFrom)
            {
                MParams.AddOpt("item_prices[date_from][" + index + "]", itemPriceDateFrom);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemPriceDateTo(int index, long itemPriceDateTo)
            {
                MParams.AddOpt("item_prices[date_to][" + index + "]", itemPriceDateTo);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemTierItemPriceId(int index, string itemTierItemPriceId)
            {
                MParams.AddOpt("item_tiers[item_price_id][" + index + "]", itemTierItemPriceId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAmount(int index, int chargeAmount)
            {
                MParams.AddOpt("charges[amount][" + index + "]", chargeAmount);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAmountInDecimal(int index, string chargeAmountInDecimal)
            {
                MParams.AddOpt("charges[amount_in_decimal][" + index + "]", chargeAmountInDecimal);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeDescription(int index, string chargeDescription)
            {
                MParams.AddOpt("charges[description][" + index + "]", chargeDescription);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAvalaraSaleType(int index,
                AvalaraSaleTypeEnum chargeAvalaraSaleType)
            {
                MParams.AddOpt("charges[avalara_sale_type][" + index + "]", chargeAvalaraSaleType);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAvalaraTransactionType(int index,
                int chargeAvalaraTransactionType)
            {
                MParams.AddOpt("charges[avalara_transaction_type][" + index + "]", chargeAvalaraTransactionType);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAvalaraServiceType(int index,
                int chargeAvalaraServiceType)
            {
                MParams.AddOpt("charges[avalara_service_type][" + index + "]", chargeAvalaraServiceType);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeDateFrom(int index, long chargeDateFrom)
            {
                MParams.AddOpt("charges[date_from][" + index + "]", chargeDateFrom);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeDateTo(int index, long chargeDateTo)
            {
                MParams.AddOpt("charges[date_to][" + index + "]", chargeDateTo);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeTaxable(int index, bool chargeTaxable)
            {
                MParams.AddOpt("charges[taxable][" + index + "]", chargeTaxable);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeTaxProfileId(int index, string chargeTaxProfileId)
            {
                MParams.AddOpt("charges[tax_profile_id][" + index + "]", chargeTaxProfileId);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeAvalaraTaxCode(int index, string chargeAvalaraTaxCode)
            {
                MParams.AddOpt("charges[avalara_tax_code][" + index + "]", chargeAvalaraTaxCode);
                return this;
            }

            public CreateForChargeItemsAndChargesRequest ChargeTaxjarProductCode(int index,
                string chargeTaxjarProductCode)
            {
                MParams.AddOpt("charges[taxjar_product_code][" + index + "]", chargeTaxjarProductCode);
                return this;
            }
        }

        public class ChargeRequest : EntityRequest<ChargeRequest>
        {
            public ChargeRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ChargeRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer_id", customerId);
                return this;
            }

            public ChargeRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public ChargeRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public ChargeRequest Amount(int amount)
            {
                MParams.AddOpt("amount", amount);
                return this;
            }

            public ChargeRequest AmountInDecimal(string amountInDecimal)
            {
                MParams.AddOpt("amount_in_decimal", amountInDecimal);
                return this;
            }

            public ChargeRequest Description(string description)
            {
                MParams.Add("description", description);
                return this;
            }

            public ChargeRequest DateFrom(long dateFrom)
            {
                MParams.AddOpt("date_from", dateFrom);
                return this;
            }

            public ChargeRequest DateTo(long dateTo)
            {
                MParams.AddOpt("date_to", dateTo);
                return this;
            }

            public ChargeRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public ChargeRequest AvalaraSaleType(AvalaraSaleTypeEnum avalaraSaleType)
            {
                MParams.AddOpt("avalara_sale_type", avalaraSaleType);
                return this;
            }

            public ChargeRequest AvalaraTransactionType(int avalaraTransactionType)
            {
                MParams.AddOpt("avalara_transaction_type", avalaraTransactionType);
                return this;
            }

            public ChargeRequest AvalaraServiceType(int avalaraServiceType)
            {
                MParams.AddOpt("avalara_service_type", avalaraServiceType);
                return this;
            }

            public ChargeRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public ChargeRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }
        }

        public class ChargeAddonRequest : EntityRequest<ChargeAddonRequest>
        {
            public ChargeAddonRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ChargeAddonRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer_id", customerId);
                return this;
            }

            public ChargeAddonRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public ChargeAddonRequest AddonId(string addonId)
            {
                MParams.Add("addon_id", addonId);
                return this;
            }

            public ChargeAddonRequest AddonQuantity(int addonQuantity)
            {
                MParams.AddOpt("addon_quantity", addonQuantity);
                return this;
            }

            public ChargeAddonRequest AddonUnitPrice(int addonUnitPrice)
            {
                MParams.AddOpt("addon_unit_price", addonUnitPrice);
                return this;
            }

            public ChargeAddonRequest AddonQuantityInDecimal(string addonQuantityInDecimal)
            {
                MParams.AddOpt("addon_quantity_in_decimal", addonQuantityInDecimal);
                return this;
            }

            public ChargeAddonRequest AddonUnitPriceInDecimal(string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addon_unit_price_in_decimal", addonUnitPriceInDecimal);
                return this;
            }

            public ChargeAddonRequest DateFrom(long dateFrom)
            {
                MParams.AddOpt("date_from", dateFrom);
                return this;
            }

            public ChargeAddonRequest DateTo(long dateTo)
            {
                MParams.AddOpt("date_to", dateTo);
                return this;
            }

            public ChargeAddonRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public ChargeAddonRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public ChargeAddonRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }
        }

        public class CreateForChargeItemRequest : EntityRequest<CreateForChargeItemRequest>
        {
            public CreateForChargeItemRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateForChargeItemRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer_id", customerId);
                return this;
            }

            public CreateForChargeItemRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public CreateForChargeItemRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public CreateForChargeItemRequest Coupon(string coupon)
            {
                MParams.AddOpt("coupon", coupon);
                return this;
            }

            public CreateForChargeItemRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public CreateForChargeItemRequest ItemPriceItemPriceId(string itemPriceItemPriceId)
            {
                MParams.Add("item_price[item_price_id]", itemPriceItemPriceId);
                return this;
            }

            public CreateForChargeItemRequest ItemPriceQuantity(int itemPriceQuantity)
            {
                MParams.AddOpt("item_price[quantity]", itemPriceQuantity);
                return this;
            }

            public CreateForChargeItemRequest ItemPriceUnitPrice(int itemPriceUnitPrice)
            {
                MParams.AddOpt("item_price[unit_price]", itemPriceUnitPrice);
                return this;
            }

            public CreateForChargeItemRequest ItemPriceDateFrom(long itemPriceDateFrom)
            {
                MParams.AddOpt("item_price[date_from]", itemPriceDateFrom);
                return this;
            }

            public CreateForChargeItemRequest ItemPriceDateTo(long itemPriceDateTo)
            {
                MParams.AddOpt("item_price[date_to]", itemPriceDateTo);
                return this;
            }

            public CreateForChargeItemRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public CreateForChargeItemRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public CreateForChargeItemRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class StopDunningRequest : EntityRequest<StopDunningRequest>
        {
            public StopDunningRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public StopDunningRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }
        }

        public class ImportInvoiceRequest : EntityRequest<ImportInvoiceRequest>
        {
            public ImportInvoiceRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ImportInvoiceRequest Id(string id)
            {
                MParams.Add("id", id);
                return this;
            }

            public ImportInvoiceRequest CurrencyCode(string currencyCode)
            {
                MParams.AddOpt("currency_code", currencyCode);
                return this;
            }

            public ImportInvoiceRequest CustomerId(string customerId)
            {
                MParams.AddOpt("customer_id", customerId);
                return this;
            }

            public ImportInvoiceRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public ImportInvoiceRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public ImportInvoiceRequest PriceType(PriceTypeEnum priceType)
            {
                MParams.AddOpt("price_type", priceType);
                return this;
            }

            public ImportInvoiceRequest TaxOverrideReason(TaxOverrideReasonEnum taxOverrideReason)
            {
                MParams.AddOpt("tax_override_reason", taxOverrideReason);
                return this;
            }

            public ImportInvoiceRequest VatNumber(string vatNumber)
            {
                MParams.AddOpt("vat_number", vatNumber);
                return this;
            }

            public ImportInvoiceRequest Date(long date)
            {
                MParams.Add("date", date);
                return this;
            }

            public ImportInvoiceRequest Total(int total)
            {
                MParams.Add("total", total);
                return this;
            }

            public ImportInvoiceRequest RoundOff(int roundOff)
            {
                MParams.AddOpt("round_off", roundOff);
                return this;
            }

            public ImportInvoiceRequest Status(StatusEnum status)
            {
                MParams.AddOpt("status", status);
                return this;
            }

            public ImportInvoiceRequest DueDate(long dueDate)
            {
                MParams.AddOpt("due_date", dueDate);
                return this;
            }

            public ImportInvoiceRequest NetTermDays(int netTermDays)
            {
                MParams.AddOpt("net_term_days", netTermDays);
                return this;
            }

            public ImportInvoiceRequest UseForProration(bool useForProration)
            {
                MParams.AddOpt("use_for_proration", useForProration);
                return this;
            }

            public ImportInvoiceRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public ImportInvoiceRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public ImportInvoiceRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public ImportInvoiceRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public ImportInvoiceRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public ImportInvoiceRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public ImportInvoiceRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public ImportInvoiceRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public ImportInvoiceRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public ImportInvoiceRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public ImportInvoiceRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public ImportInvoiceRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public ImportInvoiceRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public ImportInvoiceRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public ImportInvoiceRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public ImportInvoiceRequest LineItemId(int index, string lineItemId)
            {
                MParams.AddOpt("line_items[id][" + index + "]", lineItemId);
                return this;
            }

            public ImportInvoiceRequest LineItemDateFrom(int index, long lineItemDateFrom)
            {
                MParams.AddOpt("line_items[date_from][" + index + "]", lineItemDateFrom);
                return this;
            }

            public ImportInvoiceRequest LineItemDateTo(int index, long lineItemDateTo)
            {
                MParams.AddOpt("line_items[date_to][" + index + "]", lineItemDateTo);
                return this;
            }

            public ImportInvoiceRequest LineItemDescription(int index, string lineItemDescription)
            {
                MParams.Add("line_items[description][" + index + "]", lineItemDescription);
                return this;
            }

            public ImportInvoiceRequest LineItemUnitAmount(int index, int lineItemUnitAmount)
            {
                MParams.AddOpt("line_items[unit_amount][" + index + "]", lineItemUnitAmount);
                return this;
            }

            public ImportInvoiceRequest LineItemQuantity(int index, int lineItemQuantity)
            {
                MParams.AddOpt("line_items[quantity][" + index + "]", lineItemQuantity);
                return this;
            }

            public ImportInvoiceRequest LineItemAmount(int index, int lineItemAmount)
            {
                MParams.AddOpt("line_items[amount][" + index + "]", lineItemAmount);
                return this;
            }

            public ImportInvoiceRequest LineItemUnitAmountInDecimal(int index, string lineItemUnitAmountInDecimal)
            {
                MParams.AddOpt("line_items[unit_amount_in_decimal][" + index + "]", lineItemUnitAmountInDecimal);
                return this;
            }

            public ImportInvoiceRequest LineItemQuantityInDecimal(int index, string lineItemQuantityInDecimal)
            {
                MParams.AddOpt("line_items[quantity_in_decimal][" + index + "]", lineItemQuantityInDecimal);
                return this;
            }

            public ImportInvoiceRequest LineItemAmountInDecimal(int index, string lineItemAmountInDecimal)
            {
                MParams.AddOpt("line_items[amount_in_decimal][" + index + "]", lineItemAmountInDecimal);
                return this;
            }

            public ImportInvoiceRequest LineItemEntityType(int index, InvoiceLineItem.EntityTypeEnum lineItemEntityType)
            {
                MParams.AddOpt("line_items[entity_type][" + index + "]", lineItemEntityType);
                return this;
            }

            public ImportInvoiceRequest LineItemEntityId(int index, string lineItemEntityId)
            {
                MParams.AddOpt("line_items[entity_id][" + index + "]", lineItemEntityId);
                return this;
            }

            public ImportInvoiceRequest LineItemItemLevelDiscount1EntityId(int index,
                string lineItemItemLevelDiscount1EntityId)
            {
                MParams.AddOpt("line_items[item_level_discount1_entity_id][" + index + "]",
                    lineItemItemLevelDiscount1EntityId);
                return this;
            }

            public ImportInvoiceRequest LineItemItemLevelDiscount1Amount(int index,
                int lineItemItemLevelDiscount1Amount)
            {
                MParams.AddOpt("line_items[item_level_discount1_amount][" + index + "]",
                    lineItemItemLevelDiscount1Amount);
                return this;
            }

            public ImportInvoiceRequest LineItemItemLevelDiscount2EntityId(int index,
                string lineItemItemLevelDiscount2EntityId)
            {
                MParams.AddOpt("line_items[item_level_discount2_entity_id][" + index + "]",
                    lineItemItemLevelDiscount2EntityId);
                return this;
            }

            public ImportInvoiceRequest LineItemItemLevelDiscount2Amount(int index,
                int lineItemItemLevelDiscount2Amount)
            {
                MParams.AddOpt("line_items[item_level_discount2_amount][" + index + "]",
                    lineItemItemLevelDiscount2Amount);
                return this;
            }

            public ImportInvoiceRequest LineItemTax1Name(int index, string lineItemTax1Name)
            {
                MParams.AddOpt("line_items[tax1_name][" + index + "]", lineItemTax1Name);
                return this;
            }

            public ImportInvoiceRequest LineItemTax1Amount(int index, int lineItemTax1Amount)
            {
                MParams.AddOpt("line_items[tax1_amount][" + index + "]", lineItemTax1Amount);
                return this;
            }

            public ImportInvoiceRequest LineItemTax2Name(int index, string lineItemTax2Name)
            {
                MParams.AddOpt("line_items[tax2_name][" + index + "]", lineItemTax2Name);
                return this;
            }

            public ImportInvoiceRequest LineItemTax2Amount(int index, int lineItemTax2Amount)
            {
                MParams.AddOpt("line_items[tax2_amount][" + index + "]", lineItemTax2Amount);
                return this;
            }

            public ImportInvoiceRequest LineItemTax3Name(int index, string lineItemTax3Name)
            {
                MParams.AddOpt("line_items[tax3_name][" + index + "]", lineItemTax3Name);
                return this;
            }

            public ImportInvoiceRequest LineItemTax3Amount(int index, int lineItemTax3Amount)
            {
                MParams.AddOpt("line_items[tax3_amount][" + index + "]", lineItemTax3Amount);
                return this;
            }

            public ImportInvoiceRequest LineItemTax4Name(int index, string lineItemTax4Name)
            {
                MParams.AddOpt("line_items[tax4_name][" + index + "]", lineItemTax4Name);
                return this;
            }

            public ImportInvoiceRequest LineItemTax4Amount(int index, int lineItemTax4Amount)
            {
                MParams.AddOpt("line_items[tax4_amount][" + index + "]", lineItemTax4Amount);
                return this;
            }

            public ImportInvoiceRequest LineItemTierLineItemId(int index, string lineItemTierLineItemId)
            {
                MParams.Add("line_item_tiers[line_item_id][" + index + "]", lineItemTierLineItemId);
                return this;
            }

            public ImportInvoiceRequest LineItemTierStartingUnit(int index, int lineItemTierStartingUnit)
            {
                MParams.AddOpt("line_item_tiers[starting_unit][" + index + "]", lineItemTierStartingUnit);
                return this;
            }

            public ImportInvoiceRequest LineItemTierEndingUnit(int index, int lineItemTierEndingUnit)
            {
                MParams.AddOpt("line_item_tiers[ending_unit][" + index + "]", lineItemTierEndingUnit);
                return this;
            }

            public ImportInvoiceRequest LineItemTierQuantityUsed(int index, int lineItemTierQuantityUsed)
            {
                MParams.AddOpt("line_item_tiers[quantity_used][" + index + "]", lineItemTierQuantityUsed);
                return this;
            }

            public ImportInvoiceRequest LineItemTierUnitAmount(int index, int lineItemTierUnitAmount)
            {
                MParams.AddOpt("line_item_tiers[unit_amount][" + index + "]", lineItemTierUnitAmount);
                return this;
            }

            public ImportInvoiceRequest LineItemTierStartingUnitInDecimal(int index,
                string lineItemTierStartingUnitInDecimal)
            {
                MParams.AddOpt("line_item_tiers[starting_unit_in_decimal][" + index + "]",
                    lineItemTierStartingUnitInDecimal);
                return this;
            }

            public ImportInvoiceRequest LineItemTierEndingUnitInDecimal(int index,
                string lineItemTierEndingUnitInDecimal)
            {
                MParams.AddOpt("line_item_tiers[ending_unit_in_decimal][" + index + "]",
                    lineItemTierEndingUnitInDecimal);
                return this;
            }

            public ImportInvoiceRequest LineItemTierQuantityUsedInDecimal(int index,
                string lineItemTierQuantityUsedInDecimal)
            {
                MParams.AddOpt("line_item_tiers[quantity_used_in_decimal][" + index + "]",
                    lineItemTierQuantityUsedInDecimal);
                return this;
            }

            public ImportInvoiceRequest LineItemTierUnitAmountInDecimal(int index,
                string lineItemTierUnitAmountInDecimal)
            {
                MParams.AddOpt("line_item_tiers[unit_amount_in_decimal][" + index + "]",
                    lineItemTierUnitAmountInDecimal);
                return this;
            }

            public ImportInvoiceRequest DiscountEntityType(int index, InvoiceDiscount.EntityTypeEnum discountEntityType)
            {
                MParams.Add("discounts[entity_type][" + index + "]", discountEntityType);
                return this;
            }

            public ImportInvoiceRequest DiscountEntityId(int index, string discountEntityId)
            {
                MParams.AddOpt("discounts[entity_id][" + index + "]", discountEntityId);
                return this;
            }

            public ImportInvoiceRequest DiscountDescription(int index, string discountDescription)
            {
                MParams.AddOpt("discounts[description][" + index + "]", discountDescription);
                return this;
            }

            public ImportInvoiceRequest DiscountAmount(int index, int discountAmount)
            {
                MParams.Add("discounts[amount][" + index + "]", discountAmount);
                return this;
            }

            public ImportInvoiceRequest TaxName(int index, string taxName)
            {
                MParams.Add("taxes[name][" + index + "]", taxName);
                return this;
            }

            public ImportInvoiceRequest TaxRate(int index, double taxRate)
            {
                MParams.Add("taxes[rate][" + index + "]", taxRate);
                return this;
            }

            public ImportInvoiceRequest TaxAmount(int index, int taxAmount)
            {
                MParams.AddOpt("taxes[amount][" + index + "]", taxAmount);
                return this;
            }

            public ImportInvoiceRequest TaxDescription(int index, string taxDescription)
            {
                MParams.AddOpt("taxes[description][" + index + "]", taxDescription);
                return this;
            }

            public ImportInvoiceRequest TaxJurisType(int index, TaxJurisTypeEnum taxJurisType)
            {
                MParams.AddOpt("taxes[juris_type][" + index + "]", taxJurisType);
                return this;
            }

            public ImportInvoiceRequest TaxJurisName(int index, string taxJurisName)
            {
                MParams.AddOpt("taxes[juris_name][" + index + "]", taxJurisName);
                return this;
            }

            public ImportInvoiceRequest TaxJurisCode(int index, string taxJurisCode)
            {
                MParams.AddOpt("taxes[juris_code][" + index + "]", taxJurisCode);
                return this;
            }

            public ImportInvoiceRequest PaymentAmount(int index, int paymentAmount)
            {
                MParams.Add("payments[amount][" + index + "]", paymentAmount);
                return this;
            }

            public ImportInvoiceRequest PaymentPaymentMethod(int index, PaymentMethodEnum paymentPaymentMethod)
            {
                MParams.Add("payments[payment_method][" + index + "]", paymentPaymentMethod);
                return this;
            }

            public ImportInvoiceRequest PaymentDate(int index, long paymentDate)
            {
                MParams.AddOpt("payments[date][" + index + "]", paymentDate);
                return this;
            }

            public ImportInvoiceRequest PaymentReferenceNumber(int index, string paymentReferenceNumber)
            {
                MParams.AddOpt("payments[reference_number][" + index + "]", paymentReferenceNumber);
                return this;
            }

            public ImportInvoiceRequest NoteEntityType(int index, InvoiceNote.EntityTypeEnum noteEntityType)
            {
                MParams.AddOpt("notes[entity_type][" + index + "]", noteEntityType);
                return this;
            }

            public ImportInvoiceRequest NoteEntityId(int index, string noteEntityId)
            {
                MParams.AddOpt("notes[entity_id][" + index + "]", noteEntityId);
                return this;
            }

            public ImportInvoiceRequest NoteNote(int index, string noteNote)
            {
                MParams.AddOpt("notes[note][" + index + "]", noteNote);
                return this;
            }
        }

        public class ApplyPaymentsRequest : EntityRequest<ApplyPaymentsRequest>
        {
            public ApplyPaymentsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ApplyPaymentsRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public ApplyPaymentsRequest TransactionId(int index, string transactionId)
            {
                MParams.AddOpt("transactions[id][" + index + "]", transactionId);
                return this;
            }
        }

        public class ApplyCreditsRequest : EntityRequest<ApplyCreditsRequest>
        {
            public ApplyCreditsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ApplyCreditsRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public ApplyCreditsRequest CreditNoteId(int index, string creditNoteId)
            {
                MParams.AddOpt("credit_notes[id][" + index + "]", creditNoteId);
                return this;
            }
        }

        public class InvoiceListRequest : ListRequestBase<InvoiceListRequest>
        {
            public InvoiceListRequest(string url)
                : base(url)
            {
            }

            [Obsolete]
            public InvoiceListRequest PaidOnAfter(long paidOnAfter)
            {
                MParams.AddOpt("paid_on_after", paidOnAfter);
                return this;
            }

            public InvoiceListRequest IncludeDeleted(bool includeDeleted)
            {
                MParams.AddOpt("include_deleted", includeDeleted);
                return this;
            }

            public StringFilter<InvoiceListRequest> Id()
            {
                return new StringFilter<InvoiceListRequest>("id", this).SupportsMultiOperators(true);
            }

            public StringFilter<InvoiceListRequest> SubscriptionId()
            {
                return new StringFilter<InvoiceListRequest>("subscription_id", this).SupportsMultiOperators(true)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<InvoiceListRequest> CustomerId()
            {
                return new StringFilter<InvoiceListRequest>("customer_id", this).SupportsMultiOperators(true);
            }

            public BooleanFilter<InvoiceListRequest> Recurring()
            {
                return new("recurring", this);
            }

            public EnumFilter<StatusEnum, InvoiceListRequest> Status()
            {
                return new("status", this);
            }

            public EnumFilter<PriceTypeEnum, InvoiceListRequest> PriceType()
            {
                return new("price_type", this);
            }

            public TimestampFilter<InvoiceListRequest> Date()
            {
                return new("date", this);
            }

            public TimestampFilter<InvoiceListRequest> PaidAt()
            {
                return new("paid_at", this);
            }

            public NumberFilter<int, InvoiceListRequest> Total()
            {
                return new("total", this);
            }

            public NumberFilter<int, InvoiceListRequest> AmountPaid()
            {
                return new("amount_paid", this);
            }

            public NumberFilter<int, InvoiceListRequest> AmountAdjusted()
            {
                return new("amount_adjusted", this);
            }

            public NumberFilter<int, InvoiceListRequest> CreditsApplied()
            {
                return new("credits_applied", this);
            }

            public NumberFilter<int, InvoiceListRequest> AmountDue()
            {
                return new("amount_due", this);
            }

            public EnumFilter<DunningStatusEnum, InvoiceListRequest> DunningStatus()
            {
                return new EnumFilter<DunningStatusEnum, InvoiceListRequest>("dunning_status", this)
                    .SupportsPresenceOperator(true);
            }

            public StringFilter<InvoiceListRequest> PaymentOwner()
            {
                return new StringFilter<InvoiceListRequest>("payment_owner", this).SupportsMultiOperators(true);
            }

            public TimestampFilter<InvoiceListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public TimestampFilter<InvoiceListRequest> VoidedAt()
            {
                return new("voided_at", this);
            }

            public StringFilter<InvoiceListRequest> VoidReasonCode()
            {
                return new StringFilter<InvoiceListRequest>("void_reason_code", this).SupportsMultiOperators(true);
            }

            public InvoiceListRequest SortByDate(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "date");
                return this;
            }

            public InvoiceListRequest SortByUpdatedAt(SortOrderEnum order)
            {
                MParams.AddOpt("sort_by[" + order.ToString().ToLower() + "]", "updated_at");
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

        public class AddChargeRequest : EntityRequest<AddChargeRequest>
        {
            public AddChargeRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AddChargeRequest Amount(int amount)
            {
                MParams.Add("amount", amount);
                return this;
            }

            public AddChargeRequest Description(string description)
            {
                MParams.Add("description", description);
                return this;
            }

            public AddChargeRequest AvalaraSaleType(AvalaraSaleTypeEnum avalaraSaleType)
            {
                MParams.AddOpt("avalara_sale_type", avalaraSaleType);
                return this;
            }

            public AddChargeRequest AvalaraTransactionType(int avalaraTransactionType)
            {
                MParams.AddOpt("avalara_transaction_type", avalaraTransactionType);
                return this;
            }

            public AddChargeRequest AvalaraServiceType(int avalaraServiceType)
            {
                MParams.AddOpt("avalara_service_type", avalaraServiceType);
                return this;
            }

            public AddChargeRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public AddChargeRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public AddChargeRequest LineItemDateFrom(long lineItemDateFrom)
            {
                MParams.AddOpt("line_item[date_from]", lineItemDateFrom);
                return this;
            }

            public AddChargeRequest LineItemDateTo(long lineItemDateTo)
            {
                MParams.AddOpt("line_item[date_to]", lineItemDateTo);
                return this;
            }
        }

        public class AddAddonChargeRequest : EntityRequest<AddAddonChargeRequest>
        {
            public AddAddonChargeRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AddAddonChargeRequest AddonId(string addonId)
            {
                MParams.Add("addon_id", addonId);
                return this;
            }

            public AddAddonChargeRequest AddonQuantity(int addonQuantity)
            {
                MParams.AddOpt("addon_quantity", addonQuantity);
                return this;
            }

            public AddAddonChargeRequest AddonUnitPrice(int addonUnitPrice)
            {
                MParams.AddOpt("addon_unit_price", addonUnitPrice);
                return this;
            }

            public AddAddonChargeRequest AddonQuantityInDecimal(string addonQuantityInDecimal)
            {
                MParams.AddOpt("addon_quantity_in_decimal", addonQuantityInDecimal);
                return this;
            }

            public AddAddonChargeRequest AddonUnitPriceInDecimal(string addonUnitPriceInDecimal)
            {
                MParams.AddOpt("addon_unit_price_in_decimal", addonUnitPriceInDecimal);
                return this;
            }

            public AddAddonChargeRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public AddAddonChargeRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public AddAddonChargeRequest LineItemDateFrom(long lineItemDateFrom)
            {
                MParams.AddOpt("line_item[date_from]", lineItemDateFrom);
                return this;
            }

            public AddAddonChargeRequest LineItemDateTo(long lineItemDateTo)
            {
                MParams.AddOpt("line_item[date_to]", lineItemDateTo);
                return this;
            }
        }

        public class AddChargeItemRequest : EntityRequest<AddChargeItemRequest>
        {
            public AddChargeItemRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public AddChargeItemRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public AddChargeItemRequest SubscriptionId(string subscriptionId)
            {
                MParams.AddOpt("subscription_id", subscriptionId);
                return this;
            }

            public AddChargeItemRequest ItemPriceItemPriceId(string itemPriceItemPriceId)
            {
                MParams.Add("item_price[item_price_id]", itemPriceItemPriceId);
                return this;
            }

            public AddChargeItemRequest ItemPriceQuantity(int itemPriceQuantity)
            {
                MParams.AddOpt("item_price[quantity]", itemPriceQuantity);
                return this;
            }

            public AddChargeItemRequest ItemPriceUnitPrice(int itemPriceUnitPrice)
            {
                MParams.AddOpt("item_price[unit_price]", itemPriceUnitPrice);
                return this;
            }

            public AddChargeItemRequest ItemPriceDateFrom(long itemPriceDateFrom)
            {
                MParams.AddOpt("item_price[date_from]", itemPriceDateFrom);
                return this;
            }

            public AddChargeItemRequest ItemPriceDateTo(long itemPriceDateTo)
            {
                MParams.AddOpt("item_price[date_to]", itemPriceDateTo);
                return this;
            }

            public AddChargeItemRequest ItemTierStartingUnit(int index, int itemTierStartingUnit)
            {
                MParams.AddOpt("item_tiers[starting_unit][" + index + "]", itemTierStartingUnit);
                return this;
            }

            public AddChargeItemRequest ItemTierEndingUnit(int index, int itemTierEndingUnit)
            {
                MParams.AddOpt("item_tiers[ending_unit][" + index + "]", itemTierEndingUnit);
                return this;
            }

            public AddChargeItemRequest ItemTierPrice(int index, int itemTierPrice)
            {
                MParams.AddOpt("item_tiers[price][" + index + "]", itemTierPrice);
                return this;
            }
        }

        public class CloseRequest : EntityRequest<CloseRequest>
        {
            public CloseRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CloseRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public CloseRequest InvoiceNote(string invoiceNote)
            {
                MParams.AddOpt("invoice_note", invoiceNote);
                return this;
            }

            public CloseRequest RemoveGeneralNote(bool removeGeneralNote)
            {
                MParams.AddOpt("remove_general_note", removeGeneralNote);
                return this;
            }

            public CloseRequest InvoiceDate(long invoiceDate)
            {
                MParams.AddOpt("invoice_date", invoiceDate);
                return this;
            }

            public CloseRequest NotesToRemoveEntityType(int index, EntityTypeEnum notesToRemoveEntityType)
            {
                MParams.AddOpt("notes_to_remove[entity_type][" + index + "]", notesToRemoveEntityType);
                return this;
            }

            public CloseRequest NotesToRemoveEntityId(int index, string notesToRemoveEntityId)
            {
                MParams.AddOpt("notes_to_remove[entity_id][" + index + "]", notesToRemoveEntityId);
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

            public CollectPaymentRequest AuthorizationTransactionId(string authorizationTransactionId)
            {
                MParams.AddOpt("authorization_transaction_id", authorizationTransactionId);
                return this;
            }

            public CollectPaymentRequest PaymentSourceId(string paymentSourceId)
            {
                MParams.AddOpt("payment_source_id", paymentSourceId);
                return this;
            }

            public CollectPaymentRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }
        }

        public class RecordPaymentRequest : EntityRequest<RecordPaymentRequest>
        {
            public RecordPaymentRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RecordPaymentRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public RecordPaymentRequest TransactionAmount(int transactionAmount)
            {
                MParams.AddOpt("transaction[amount]", transactionAmount);
                return this;
            }

            public RecordPaymentRequest TransactionPaymentMethod(PaymentMethodEnum transactionPaymentMethod)
            {
                MParams.Add("transaction[payment_method]", transactionPaymentMethod);
                return this;
            }

            public RecordPaymentRequest TransactionReferenceNumber(string transactionReferenceNumber)
            {
                MParams.AddOpt("transaction[reference_number]", transactionReferenceNumber);
                return this;
            }

            public RecordPaymentRequest TransactionIdAtGateway(string transactionIdAtGateway)
            {
                MParams.AddOpt("transaction[id_at_gateway]", transactionIdAtGateway);
                return this;
            }

            public RecordPaymentRequest TransactionStatus(Transaction.StatusEnum transactionStatus)
            {
                MParams.AddOpt("transaction[status]", transactionStatus);
                return this;
            }

            public RecordPaymentRequest TransactionDate(long transactionDate)
            {
                MParams.AddOpt("transaction[date]", transactionDate);
                return this;
            }

            public RecordPaymentRequest TransactionErrorCode(string transactionErrorCode)
            {
                MParams.AddOpt("transaction[error_code]", transactionErrorCode);
                return this;
            }

            public RecordPaymentRequest TransactionErrorText(string transactionErrorText)
            {
                MParams.AddOpt("transaction[error_text]", transactionErrorText);
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

            public RefundRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public RefundRequest CustomerNotes(string customerNotes)
            {
                MParams.AddOpt("customer_notes", customerNotes);
                return this;
            }

            public RefundRequest CreditNoteReasonCode(CreditNote.ReasonCodeEnum creditNoteReasonCode)
            {
                MParams.AddOpt("credit_note[reason_code]", creditNoteReasonCode);
                return this;
            }

            public RefundRequest CreditNoteCreateReasonCode(string creditNoteCreateReasonCode)
            {
                MParams.AddOpt("credit_note[create_reason_code]", creditNoteCreateReasonCode);
                return this;
            }
        }

        public class RecordRefundRequest : EntityRequest<RecordRefundRequest>
        {
            public RecordRefundRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RecordRefundRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public RecordRefundRequest CustomerNotes(string customerNotes)
            {
                MParams.AddOpt("customer_notes", customerNotes);
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

            public RecordRefundRequest CreditNoteReasonCode(CreditNote.ReasonCodeEnum creditNoteReasonCode)
            {
                MParams.AddOpt("credit_note[reason_code]", creditNoteReasonCode);
                return this;
            }

            public RecordRefundRequest CreditNoteCreateReasonCode(string creditNoteCreateReasonCode)
            {
                MParams.AddOpt("credit_note[create_reason_code]", creditNoteCreateReasonCode);
                return this;
            }
        }

        public class RemovePaymentRequest : EntityRequest<RemovePaymentRequest>
        {
            public RemovePaymentRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RemovePaymentRequest TransactionId(string transactionId)
            {
                MParams.Add("transaction[id]", transactionId);
                return this;
            }
        }

        public class RemoveCreditNoteRequest : EntityRequest<RemoveCreditNoteRequest>
        {
            public RemoveCreditNoteRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RemoveCreditNoteRequest CreditNoteId(string creditNoteId)
            {
                MParams.Add("credit_note[id]", creditNoteId);
                return this;
            }
        }

        public class VoidInvoiceRequest : EntityRequest<VoidInvoiceRequest>
        {
            public VoidInvoiceRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public VoidInvoiceRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public VoidInvoiceRequest VoidReasonCode(string voidReasonCode)
            {
                MParams.AddOpt("void_reason_code", voidReasonCode);
                return this;
            }
        }

        public class WriteOffRequest : EntityRequest<WriteOffRequest>
        {
            public WriteOffRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public WriteOffRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
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

            public DeleteRequest ClaimCredits(bool claimCredits)
            {
                MParams.AddOpt("claim_credits", claimCredits);
                return this;
            }
        }

        public class UpdateDetailsRequest : EntityRequest<UpdateDetailsRequest>
        {
            public UpdateDetailsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateDetailsRequest VatNumber(string vatNumber)
            {
                MParams.AddOpt("vat_number", vatNumber);
                return this;
            }

            public UpdateDetailsRequest PoNumber(string poNumber)
            {
                MParams.AddOpt("po_number", poNumber);
                return this;
            }

            public UpdateDetailsRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }

            public UpdateDetailsRequest BillingAddressFirstName(string billingAddressFirstName)
            {
                MParams.AddOpt("billing_address[first_name]", billingAddressFirstName);
                return this;
            }

            public UpdateDetailsRequest BillingAddressLastName(string billingAddressLastName)
            {
                MParams.AddOpt("billing_address[last_name]", billingAddressLastName);
                return this;
            }

            public UpdateDetailsRequest BillingAddressEmail(string billingAddressEmail)
            {
                MParams.AddOpt("billing_address[email]", billingAddressEmail);
                return this;
            }

            public UpdateDetailsRequest BillingAddressCompany(string billingAddressCompany)
            {
                MParams.AddOpt("billing_address[company]", billingAddressCompany);
                return this;
            }

            public UpdateDetailsRequest BillingAddressPhone(string billingAddressPhone)
            {
                MParams.AddOpt("billing_address[phone]", billingAddressPhone);
                return this;
            }

            public UpdateDetailsRequest BillingAddressLine1(string billingAddressLine1)
            {
                MParams.AddOpt("billing_address[line1]", billingAddressLine1);
                return this;
            }

            public UpdateDetailsRequest BillingAddressLine2(string billingAddressLine2)
            {
                MParams.AddOpt("billing_address[line2]", billingAddressLine2);
                return this;
            }

            public UpdateDetailsRequest BillingAddressLine3(string billingAddressLine3)
            {
                MParams.AddOpt("billing_address[line3]", billingAddressLine3);
                return this;
            }

            public UpdateDetailsRequest BillingAddressCity(string billingAddressCity)
            {
                MParams.AddOpt("billing_address[city]", billingAddressCity);
                return this;
            }

            public UpdateDetailsRequest BillingAddressStateCode(string billingAddressStateCode)
            {
                MParams.AddOpt("billing_address[state_code]", billingAddressStateCode);
                return this;
            }

            public UpdateDetailsRequest BillingAddressState(string billingAddressState)
            {
                MParams.AddOpt("billing_address[state]", billingAddressState);
                return this;
            }

            public UpdateDetailsRequest BillingAddressZip(string billingAddressZip)
            {
                MParams.AddOpt("billing_address[zip]", billingAddressZip);
                return this;
            }

            public UpdateDetailsRequest BillingAddressCountry(string billingAddressCountry)
            {
                MParams.AddOpt("billing_address[country]", billingAddressCountry);
                return this;
            }

            public UpdateDetailsRequest BillingAddressValidationStatus(
                ValidationStatusEnum billingAddressValidationStatus)
            {
                MParams.AddOpt("billing_address[validation_status]", billingAddressValidationStatus);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public UpdateDetailsRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }
        }

        #endregion

        #region Subclasses

        public class InvoiceLineItem : Resource
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

        public class InvoiceDiscount : Resource
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

        public class InvoiceLineItemDiscount : Resource
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

        public class InvoiceTax : Resource
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

        public class InvoiceLineItemTax : Resource
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

        public class InvoiceLineItemTier : Resource
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

        public class InvoiceLinkedPayment : Resource
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
        }

        public class InvoiceDunningAttempt : Resource
        {
            public int Attempt()
            {
                return GetValue<int>("attempt");
            }

            public string TransactionId()
            {
                return GetValue<string>("transaction_id", false);
            }

            public DunningTypeEnum DunningType()
            {
                return GetEnum<DunningTypeEnum>("dunning_type");
            }

            public DateTime? CreatedAt()
            {
                return GetDateTime("created_at", false);
            }

            public Transaction.StatusEnum? TxnStatus()
            {
                return GetEnum<Transaction.StatusEnum>("txn_status", false);
            }

            public int? TxnAmount()
            {
                return GetValue<int?>("txn_amount", false);
            }
        }

        public class InvoiceAppliedCredit : Resource
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

            public CreditNote.StatusEnum CnStatus()
            {
                return GetEnum<CreditNote.StatusEnum>("cn_status");
            }
        }

        public class InvoiceAdjustmentCreditNote : Resource
        {
            public string CnId()
            {
                return GetValue<string>("cn_id");
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
        }

        public class InvoiceIssuedCreditNote : Resource
        {
            public string CnId()
            {
                return GetValue<string>("cn_id");
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
        }

        public class InvoiceLinkedOrder : Resource
        {
            public enum OrderTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "manual")] Manual,

                [EnumMember(Value = "system_generated")]
                SystemGenerated
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

            public string Id()
            {
                return GetValue<string>("id");
            }

            public string DocumentNumber()
            {
                return GetValue<string>("document_number", false);
            }

            public StatusEnum? Status()
            {
                return GetEnum<StatusEnum>("status", false);
            }

            public OrderTypeEnum? OrderType()
            {
                return GetEnum<OrderTypeEnum>("order_type", false);
            }

            public string ReferenceId()
            {
                return GetValue<string>("reference_id", false);
            }

            public string FulfillmentStatus()
            {
                return GetValue<string>("fulfillment_status", false);
            }

            public string BatchId()
            {
                return GetValue<string>("batch_id", false);
            }

            public DateTime CreatedAt()
            {
                return (DateTime) GetDateTime("created_at");
            }
        }

        public class InvoiceNote : Resource
        {
            public enum EntityTypeEnum
            {
                UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
                dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
                [EnumMember(Value = "plan")] Plan,
                [EnumMember(Value = "addon")] Addon,
                [EnumMember(Value = "coupon")] Coupon,
                [EnumMember(Value = "subscription")] Subscription,
                [EnumMember(Value = "customer")] Customer,

                [EnumMember(Value = "plan_item_price")]
                PlanItemPrice,

                [EnumMember(Value = "addon_item_price")]
                AddonItemPrice,

                [EnumMember(Value = "charge_item_price")]
                ChargeItemPrice
            }

            public EntityTypeEnum EntityType()
            {
                return GetEnum<EntityTypeEnum>("entity_type");
            }

            public string Note()
            {
                return GetValue<string>("note");
            }

            public string EntityId()
            {
                return GetValue<string>("entity_id", false);
            }
        }

        public class InvoiceShippingAddress : Resource
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

        public class InvoiceBillingAddress : Resource
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

        #endregion
    }
}