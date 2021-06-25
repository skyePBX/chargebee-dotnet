using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ChargeBee.Api;
using ChargeBee.Internal;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class PortalSession : Resource
    {
        public enum StatusEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "created")] Created,
            [EnumMember(Value = "logged_in")] LoggedIn,
            [EnumMember(Value = "logged_out")] LoggedOut,

            [EnumMember(Value = "not_yet_activated")]
            NotYetActivated,
            [EnumMember(Value = "activated")] Activated
        }

        public PortalSession()
        {
        }

        public PortalSession(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public PortalSession(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public PortalSession(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Subclasses

        public class PortalSessionLinkedCustomer : Resource
        {
            public string CustomerId()
            {
                return GetValue<string>("customer_id");
            }

            public string Email()
            {
                return GetValue<string>("email", false);
            }

            public bool HasBillingAddress()
            {
                return GetValue<bool>("has_billing_address");
            }

            public bool HasPaymentMethod()
            {
                return GetValue<bool>("has_payment_method");
            }

            public bool HasActiveSubscription()
            {
                return GetValue<bool>("has_active_subscription");
            }
        }

        #endregion

        #region Methods

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("portal_sessions");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("portal_sessions", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static EntityRequest<Type> Logout(string id)
        {
            var url = ApiUtil.BuildUrl("portal_sessions", CheckNull(id), "logout");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static ActivateRequest Activate(string id)
        {
            var url = ApiUtil.BuildUrl("portal_sessions", CheckNull(id), "activate");
            return new ActivateRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string Token => GetValue<string>("token");

        public string AccessUrl => GetValue<string>("access_url");

        public string RedirectUrl => GetValue<string>("redirect_url", false);

        public StatusEnum Status => GetEnum<StatusEnum>("status");

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public DateTime? ExpiresAt => GetDateTime("expires_at", false);

        public string CustomerId => GetValue<string>("customer_id");

        public DateTime? LoginAt => GetDateTime("login_at", false);

        public DateTime? LogoutAt => GetDateTime("logout_at", false);

        public string LoginIpaddress => GetValue<string>("login_ipaddress", false);

        public string LogoutIpaddress => GetValue<string>("logout_ipaddress", false);

        public List<PortalSessionLinkedCustomer> LinkedCustomers =>
            GetResourceList<PortalSessionLinkedCustomer>("linked_customers");

        #endregion

        #region Requests

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest RedirectUrl(string redirectUrl)
            {
                MParams.AddOpt("redirect_url", redirectUrl);
                return this;
            }

            public CreateRequest ForwardUrl(string forwardUrl)
            {
                MParams.AddOpt("forward_url", forwardUrl);
                return this;
            }

            public CreateRequest CustomerId(string customerId)
            {
                MParams.Add("customer[id]", customerId);
                return this;
            }
        }

        public class ActivateRequest : EntityRequest<ActivateRequest>
        {
            public ActivateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public ActivateRequest Token(string token)
            {
                MParams.Add("token", token);
                return this;
            }
        }

        #endregion
    }
}