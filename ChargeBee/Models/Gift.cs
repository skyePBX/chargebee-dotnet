using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Filters;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Gift : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "scheduled")] Scheduled,
            [EnumMember(Value = "unclaimed")] Unclaimed,
            [EnumMember(Value = "claimed")] Claimed,
            [EnumMember(Value = "cancelled")] Cancelled,
            [EnumMember(Value = "expired")] Expired
        }

        public Gift()
        {
        }

        public Gift(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Gift(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Gift(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("gifts");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static CreateForItemsRequest CreateForItems()
        {
            var url = ApiUtil.BuildUrl("gifts", "create_for_items");
            return new CreateForItemsRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("gifts", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static GiftListRequest List()
        {
            var url = ApiUtil.BuildUrl("gifts");
            return new GiftListRequest(url);
        }

        public static EntityRequest<Type> Claim(string id)
        {
            var url = ApiUtil.BuildUrl("gifts", CheckNull(id), "claim");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Cancel(string id)
        {
            var url = ApiUtil.BuildUrl("gifts", CheckNull(id), "cancel");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static UpdateGiftRequest UpdateGift(string id)
        {
            var url = ApiUtil.BuildUrl("gifts", CheckNull(id), "update_gift");
            return new UpdateGiftRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public DateTime? ScheduledAt => GetDateTime("scheduled_at", false);

        public bool AutoClaim => GetValue<bool>("auto_claim");

        public bool NoExpiry => GetValue<bool>("no_expiry");

        public DateTime? ClaimExpiryDate => GetDateTime("claim_expiry_date", false);

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public GiftGifter Gifter => GetSubResource<GiftGifter>("gifter");

        public GiftGiftReceiver GiftReceiver => GetSubResource<GiftGiftReceiver>("gift_receiver");

        public List<GiftGiftTimeline> GiftTimelines => GetResourceList<GiftGiftTimeline>("gift_timelines");

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest ScheduledAt(long scheduledAt)
            {
                MParams.AddOpt("scheduled_at", scheduledAt);
                return this;
            }

            public CreateRequest AutoClaim(bool autoClaim)
            {
                MParams.AddOpt("auto_claim", autoClaim);
                return this;
            }

            public CreateRequest NoExpiry(bool noExpiry)
            {
                MParams.AddOpt("no_expiry", noExpiry);
                return this;
            }

            public CreateRequest ClaimExpiryDate(long claimExpiryDate)
            {
                MParams.AddOpt("claim_expiry_date", claimExpiryDate);
                return this;
            }

            public CreateRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateRequest GifterCustomerId(string gifterCustomerId)
            {
                MParams.Add("gifter[customer_id]", gifterCustomerId);
                return this;
            }

            public CreateRequest GifterSignature(string gifterSignature)
            {
                MParams.Add("gifter[signature]", gifterSignature);
                return this;
            }

            public CreateRequest GifterNote(string gifterNote)
            {
                MParams.AddOpt("gifter[note]", gifterNote);
                return this;
            }

            public CreateRequest GifterPaymentSrcId(string gifterPaymentSrcId)
            {
                MParams.AddOpt("gifter[payment_src_id]", gifterPaymentSrcId);
                return this;
            }

            public CreateRequest GiftReceiverCustomerId(string giftReceiverCustomerId)
            {
                MParams.Add("gift_receiver[customer_id]", giftReceiverCustomerId);
                return this;
            }

            public CreateRequest GiftReceiverFirstName(string giftReceiverFirstName)
            {
                MParams.Add("gift_receiver[first_name]", giftReceiverFirstName);
                return this;
            }

            public CreateRequest GiftReceiverLastName(string giftReceiverLastName)
            {
                MParams.Add("gift_receiver[last_name]", giftReceiverLastName);
                return this;
            }

            public CreateRequest GiftReceiverEmail(string giftReceiverEmail)
            {
                MParams.Add("gift_receiver[email]", giftReceiverEmail);
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

            public CreateRequest SubscriptionPlanId(string subscriptionPlanId)
            {
                MParams.Add("subscription[plan_id]", subscriptionPlanId);
                return this;
            }

            public CreateRequest SubscriptionPlanQuantity(int subscriptionPlanQuantity)
            {
                MParams.AddOpt("subscription[plan_quantity]", subscriptionPlanQuantity);
                return this;
            }

            public CreateRequest SubscriptionPlanQuantityInDecimal(string subscriptionPlanQuantityInDecimal)
            {
                MParams.AddOpt("subscription[plan_quantity_in_decimal]", subscriptionPlanQuantityInDecimal);
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

            public CreateRequest AddonQuantityInDecimal(int index, string addonQuantityInDecimal)
            {
                MParams.AddOpt("addons[quantity_in_decimal][" + index + "]", addonQuantityInDecimal);
                return this;
            }
        }

        public class CreateForItemsRequest : EntityRequest<CreateForItemsRequest>
        {
            public CreateForItemsRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateForItemsRequest ScheduledAt(long scheduledAt)
            {
                MParams.AddOpt("scheduled_at", scheduledAt);
                return this;
            }

            public CreateForItemsRequest AutoClaim(bool autoClaim)
            {
                MParams.AddOpt("auto_claim", autoClaim);
                return this;
            }

            public CreateForItemsRequest NoExpiry(bool noExpiry)
            {
                MParams.AddOpt("no_expiry", noExpiry);
                return this;
            }

            public CreateForItemsRequest ClaimExpiryDate(long claimExpiryDate)
            {
                MParams.AddOpt("claim_expiry_date", claimExpiryDate);
                return this;
            }

            public CreateForItemsRequest CouponIds(List<string> couponIds)
            {
                MParams.AddOpt("coupon_ids", couponIds);
                return this;
            }

            public CreateForItemsRequest GifterCustomerId(string gifterCustomerId)
            {
                MParams.Add("gifter[customer_id]", gifterCustomerId);
                return this;
            }

            public CreateForItemsRequest GifterSignature(string gifterSignature)
            {
                MParams.Add("gifter[signature]", gifterSignature);
                return this;
            }

            public CreateForItemsRequest GifterNote(string gifterNote)
            {
                MParams.AddOpt("gifter[note]", gifterNote);
                return this;
            }

            public CreateForItemsRequest GifterPaymentSrcId(string gifterPaymentSrcId)
            {
                MParams.AddOpt("gifter[payment_src_id]", gifterPaymentSrcId);
                return this;
            }

            public CreateForItemsRequest GiftReceiverCustomerId(string giftReceiverCustomerId)
            {
                MParams.Add("gift_receiver[customer_id]", giftReceiverCustomerId);
                return this;
            }

            public CreateForItemsRequest GiftReceiverFirstName(string giftReceiverFirstName)
            {
                MParams.Add("gift_receiver[first_name]", giftReceiverFirstName);
                return this;
            }

            public CreateForItemsRequest GiftReceiverLastName(string giftReceiverLastName)
            {
                MParams.Add("gift_receiver[last_name]", giftReceiverLastName);
                return this;
            }

            public CreateForItemsRequest GiftReceiverEmail(string giftReceiverEmail)
            {
                MParams.Add("gift_receiver[email]", giftReceiverEmail);
                return this;
            }

            public CreateForItemsRequest PaymentIntentId(string paymentIntentId)
            {
                MParams.AddOpt("payment_intent[id]", paymentIntentId);
                return this;
            }

            public CreateForItemsRequest PaymentIntentGatewayAccountId(string paymentIntentGatewayAccountId)
            {
                MParams.AddOpt("payment_intent[gateway_account_id]", paymentIntentGatewayAccountId);
                return this;
            }

            public CreateForItemsRequest PaymentIntentGwToken(string paymentIntentGwToken)
            {
                MParams.AddOpt("payment_intent[gw_token]", paymentIntentGwToken);
                return this;
            }

            public CreateForItemsRequest PaymentIntentReferenceId(string paymentIntentReferenceId)
            {
                MParams.AddOpt("payment_intent[reference_id]", paymentIntentReferenceId);
                return this;
            }

            [Obsolete]
            public CreateForItemsRequest PaymentIntentGwPaymentMethodId(string paymentIntentGwPaymentMethodId)
            {
                MParams.AddOpt("payment_intent[gw_payment_method_id]", paymentIntentGwPaymentMethodId);
                return this;
            }

            public CreateForItemsRequest PaymentIntentAdditionalInfo(JToken paymentIntentAdditionalInfo)
            {
                MParams.AddOpt("payment_intent[additional_info]", paymentIntentAdditionalInfo);
                return this;
            }

            public CreateForItemsRequest ShippingAddressFirstName(string shippingAddressFirstName)
            {
                MParams.AddOpt("shipping_address[first_name]", shippingAddressFirstName);
                return this;
            }

            public CreateForItemsRequest ShippingAddressLastName(string shippingAddressLastName)
            {
                MParams.AddOpt("shipping_address[last_name]", shippingAddressLastName);
                return this;
            }

            public CreateForItemsRequest ShippingAddressEmail(string shippingAddressEmail)
            {
                MParams.AddOpt("shipping_address[email]", shippingAddressEmail);
                return this;
            }

            public CreateForItemsRequest ShippingAddressCompany(string shippingAddressCompany)
            {
                MParams.AddOpt("shipping_address[company]", shippingAddressCompany);
                return this;
            }

            public CreateForItemsRequest ShippingAddressPhone(string shippingAddressPhone)
            {
                MParams.AddOpt("shipping_address[phone]", shippingAddressPhone);
                return this;
            }

            public CreateForItemsRequest ShippingAddressLine1(string shippingAddressLine1)
            {
                MParams.AddOpt("shipping_address[line1]", shippingAddressLine1);
                return this;
            }

            public CreateForItemsRequest ShippingAddressLine2(string shippingAddressLine2)
            {
                MParams.AddOpt("shipping_address[line2]", shippingAddressLine2);
                return this;
            }

            public CreateForItemsRequest ShippingAddressLine3(string shippingAddressLine3)
            {
                MParams.AddOpt("shipping_address[line3]", shippingAddressLine3);
                return this;
            }

            public CreateForItemsRequest ShippingAddressCity(string shippingAddressCity)
            {
                MParams.AddOpt("shipping_address[city]", shippingAddressCity);
                return this;
            }

            public CreateForItemsRequest ShippingAddressStateCode(string shippingAddressStateCode)
            {
                MParams.AddOpt("shipping_address[state_code]", shippingAddressStateCode);
                return this;
            }

            public CreateForItemsRequest ShippingAddressState(string shippingAddressState)
            {
                MParams.AddOpt("shipping_address[state]", shippingAddressState);
                return this;
            }

            public CreateForItemsRequest ShippingAddressZip(string shippingAddressZip)
            {
                MParams.AddOpt("shipping_address[zip]", shippingAddressZip);
                return this;
            }

            public CreateForItemsRequest ShippingAddressCountry(string shippingAddressCountry)
            {
                MParams.AddOpt("shipping_address[country]", shippingAddressCountry);
                return this;
            }

            public CreateForItemsRequest ShippingAddressValidationStatus(
                ValidationStatusEnum shippingAddressValidationStatus)
            {
                MParams.AddOpt("shipping_address[validation_status]", shippingAddressValidationStatus);
                return this;
            }

            public CreateForItemsRequest SubscriptionItemItemPriceId(int index, string subscriptionItemItemPriceId)
            {
                MParams.AddOpt("subscription_items[item_price_id][" + index + "]", subscriptionItemItemPriceId);
                return this;
            }

            public CreateForItemsRequest SubscriptionItemQuantity(int index, int subscriptionItemQuantity)
            {
                MParams.AddOpt("subscription_items[quantity][" + index + "]", subscriptionItemQuantity);
                return this;
            }
        }

        public class GiftListRequest : ListRequestBase<GiftListRequest>
        {
            public GiftListRequest(string url)
                : base(url)
            {
            }

            public EnumFilter<StatusEnum, GiftListRequest> Status()
            {
                return new("status", this);
            }

            public StringFilter<GiftListRequest> GiftReceiverEmail()
            {
                return new("gift_receiver[email]", this);
            }

            public StringFilter<GiftListRequest> GifterCustomerId()
            {
                return new("gifter[customer_id]", this);
            }

            public StringFilter<GiftListRequest> GiftReceiverCustomerId()
            {
                return new("gift_receiver[customer_id]", this);
            }
        }

        public class UpdateGiftRequest : EntityRequest<UpdateGiftRequest>
        {
            public UpdateGiftRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateGiftRequest ScheduledAt(long scheduledAt)
            {
                MParams.Add("scheduled_at", scheduledAt);
                return this;
            }

            public UpdateGiftRequest Comment(string comment)
            {
                MParams.AddOpt("comment", comment);
                return this;
            }
        }

        #endregion

        #region Subclasses

        public class GiftGifter : Resource
        {
            public string CustomerId()
            {
                return GetValue<string>("customer_id");
            }

            public string InvoiceId()
            {
                return GetValue<string>("invoice_id", false);
            }

            public string Signature()
            {
                return GetValue<string>("signature", false);
            }

            public string Note()
            {
                return GetValue<string>("note", false);
            }
        }

        public class GiftGiftReceiver : Resource
        {
            public string CustomerId()
            {
                return GetValue<string>("customer_id");
            }

            public string SubscriptionId()
            {
                return GetValue<string>("subscription_id");
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
                return GetValue<string>("email", false);
            }
        }

        public class GiftGiftTimeline : Resource
        {
            public StatusEnum Status()
            {
                return GetEnum<StatusEnum>("status");
            }

            public DateTime? OccurredAt()
            {
                return GetDateTime("occurred_at", false);
            }
        }

        #endregion
    }
}