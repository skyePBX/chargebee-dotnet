using System.IO;
using ChargeBee.Api;
using ChargeBee.Internal;
using ChargeBee.Models.Enums;
using Newtonsoft.Json.Linq;

namespace ChargeBee.Models
{
    public class Address : Resource
    {
        public Address()
        {
        }

        public Address(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public Address(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public Address(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static RetrieveRequest Retrieve()
        {
            var url = ApiUtil.BuildUrl("addresses");
            return new RetrieveRequest(url, HttpMethod.Get);
        }

        public static UpdateRequest Update()
        {
            var url = ApiUtil.BuildUrl("addresses");
            return new UpdateRequest(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Label => GetValue<string>("label");

        public string FirstName => GetValue<string>("first_name", false);

        public string LastName => GetValue<string>("last_name", false);

        public string Email => GetValue<string>("email", false);

        public string Company => GetValue<string>("company", false);

        public string Phone => GetValue<string>("phone", false);

        public string Addr => GetValue<string>("addr", false);

        public string ExtendedAddr => GetValue<string>("extended_addr", false);

        public string ExtendedAddr2 => GetValue<string>("extended_addr2", false);

        public string City => GetValue<string>("city", false);

        public string StateCode => GetValue<string>("state_code", false);

        public string State => GetValue<string>("state", false);

        public string Country => GetValue<string>("country", false);

        public string Zip => GetValue<string>("zip", false);

        public ValidationStatusEnum? ValidationStatus => GetEnum<ValidationStatusEnum>("validation_status", false);

        public string SubscriptionId => GetValue<string>("subscription_id");

        #endregion

        #region Requests

        public class RetrieveRequest : EntityRequest<RetrieveRequest>
        {
            public RetrieveRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public RetrieveRequest SubscriptionId(string subscriptionId)
            {
                MParams.Add("subscription_id", subscriptionId);
                return this;
            }

            public RetrieveRequest Label(string label)
            {
                MParams.Add("label", label);
                return this;
            }
        }

        public class UpdateRequest : EntityRequest<UpdateRequest>
        {
            public UpdateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public UpdateRequest SubscriptionId(string subscriptionId)
            {
                MParams.Add("subscription_id", subscriptionId);
                return this;
            }

            public UpdateRequest Label(string label)
            {
                MParams.Add("label", label);
                return this;
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

            public UpdateRequest Company(string company)
            {
                MParams.AddOpt("company", company);
                return this;
            }

            public UpdateRequest Phone(string phone)
            {
                MParams.AddOpt("phone", phone);
                return this;
            }

            public UpdateRequest Addr(string addr)
            {
                MParams.AddOpt("addr", addr);
                return this;
            }

            public UpdateRequest ExtendedAddr(string extendedAddr)
            {
                MParams.AddOpt("extended_addr", extendedAddr);
                return this;
            }

            public UpdateRequest ExtendedAddr2(string extendedAddr2)
            {
                MParams.AddOpt("extended_addr2", extendedAddr2);
                return this;
            }

            public UpdateRequest City(string city)
            {
                MParams.AddOpt("city", city);
                return this;
            }

            public UpdateRequest StateCode(string stateCode)
            {
                MParams.AddOpt("state_code", stateCode);
                return this;
            }

            public UpdateRequest State(string state)
            {
                MParams.AddOpt("state", state);
                return this;
            }

            public UpdateRequest Zip(string zip)
            {
                MParams.AddOpt("zip", zip);
                return this;
            }

            public UpdateRequest Country(string country)
            {
                MParams.AddOpt("country", country);
                return this;
            }

            public UpdateRequest ValidationStatus(ValidationStatusEnum validationStatus)
            {
                MParams.AddOpt("validation_status", validationStatus);
                return this;
            }
        }

        #endregion


        #region Subclasses

        #endregion
    }
}