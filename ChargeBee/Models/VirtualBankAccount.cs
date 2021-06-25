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
    public class VirtualBankAccount : Resource
    {
        public enum SchemeEnum
        {
            UnKnown, /*Indicates unexpected value for this enum. You can get this when there is a
            dotnet-client version incompatibility. We suggest you to upgrade to the latest version */
            [EnumMember(Value = "ach_credit")] AchCredit,
            [EnumMember(Value = "sepa_credit")] SepaCredit
        }

        public VirtualBankAccount()
        {
        }

        public VirtualBankAccount(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                JObj = JToken.Parse(reader.ReadToEnd());
                ApiVersionCheck(JObj);
            }
        }

        public VirtualBankAccount(TextReader reader)
        {
            JObj = JToken.Parse(reader.ReadToEnd());
            ApiVersionCheck(JObj);
        }

        public VirtualBankAccount(string jsonString)
        {
            JObj = JToken.Parse(jsonString);
            ApiVersionCheck(JObj);
        }

        #region Methods

        public static CreateUsingPermanentTokenRequest CreateUsingPermanentToken()
        {
            var url = ApiUtil.BuildUrl("virtual_bank_accounts", "create_using_permanent_token");
            return new CreateUsingPermanentTokenRequest(url, HttpMethod.Post);
        }

        public static CreateRequest Create()
        {
            var url = ApiUtil.BuildUrl("virtual_bank_accounts");
            return new CreateRequest(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> Retrieve(string id)
        {
            var url = ApiUtil.BuildUrl("virtual_bank_accounts", CheckNull(id));
            return new EntityRequest<Type>(url, HttpMethod.Get);
        }

        public static VirtualBankAccountListRequest List()
        {
            var url = ApiUtil.BuildUrl("virtual_bank_accounts");
            return new VirtualBankAccountListRequest(url);
        }

        public static EntityRequest<Type> Delete(string id)
        {
            var url = ApiUtil.BuildUrl("virtual_bank_accounts", CheckNull(id), "delete");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        public static EntityRequest<Type> DeleteLocal(string id)
        {
            var url = ApiUtil.BuildUrl("virtual_bank_accounts", CheckNull(id), "delete_local");
            return new EntityRequest<Type>(url, HttpMethod.Post);
        }

        #endregion

        #region Properties

        public string Id => GetValue<string>("id");

        public string CustomerId => GetValue<string>("customer_id");

        public string Email => GetValue<string>("email");

        public SchemeEnum? Scheme => GetEnum<SchemeEnum>("scheme", false);

        public string BankName => GetValue<string>("bank_name", false);

        public string AccountNumber => GetValue<string>("account_number");

        public string RoutingNumber => GetValue<string>("routing_number", false);

        public string SwiftCode => GetValue<string>("swift_code");

        public GatewayEnum Gateway => GetEnum<GatewayEnum>("gateway");

        public string GatewayAccountId => GetValue<string>("gateway_account_id");

        public long? ResourceVersion => GetValue<long?>("resource_version", false);

        public DateTime? UpdatedAt => GetDateTime("updated_at", false);

        public DateTime CreatedAt => (DateTime) GetDateTime("created_at");

        public string ReferenceId => GetValue<string>("reference_id");

        public bool Deleted => GetValue<bool>("deleted");

        #endregion

        #region Requests

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

            public CreateUsingPermanentTokenRequest ReferenceId(string referenceId)
            {
                MParams.Add("reference_id", referenceId);
                return this;
            }

            public CreateUsingPermanentTokenRequest Scheme(SchemeEnum scheme)
            {
                MParams.AddOpt("scheme", scheme);
                return this;
            }
        }

        public class CreateRequest : EntityRequest<CreateRequest>
        {
            public CreateRequest(string url, HttpMethod method)
                : base(url, method)
            {
            }

            public CreateRequest CustomerId(string customerId)
            {
                MParams.Add("customer_id", customerId);
                return this;
            }

            public CreateRequest Email(string email)
            {
                MParams.AddOpt("email", email);
                return this;
            }

            public CreateRequest Scheme(SchemeEnum scheme)
            {
                MParams.AddOpt("scheme", scheme);
                return this;
            }
        }

        public class VirtualBankAccountListRequest : ListRequestBase<VirtualBankAccountListRequest>
        {
            public VirtualBankAccountListRequest(string url)
                : base(url)
            {
            }

            public StringFilter<VirtualBankAccountListRequest> CustomerId()
            {
                return new StringFilter<VirtualBankAccountListRequest>("customer_id", this)
                    .SupportsMultiOperators(true);
            }

            public TimestampFilter<VirtualBankAccountListRequest> UpdatedAt()
            {
                return new("updated_at", this);
            }

            public TimestampFilter<VirtualBankAccountListRequest> CreatedAt()
            {
                return new("created_at", this);
            }
        }

        #endregion

        #region Subclasses

        #endregion
    }
}